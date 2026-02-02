#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    IncompleteNumber,
    Overflow,
}

/// Convert a list of numbers to a stream of bytes encoded with variable length encoding.
pub fn to_bytes(values: &[u32]) -> Vec<u8> {
    let mut bytes = Vec::new();
    for i in 0..values.len() {
        let mut intermediate = Vec::new();
        let mut value = values[i];
        let last_entry = (value & 0x7F) as u8; // get rid of any continuation bit for the last entry
        intermediate.push(last_entry);
        value = value >> 7;
        while value > 0 {
            intermediate.push((value | 0x80) as u8); // guarantee continuation bit to be set 
            value = value >> 7;
        }
        intermediate.reverse();
        bytes.append(&mut intermediate);
    }
    return bytes;
}

/// Given a stream of bytes, extract all numbers which are encoded in there.
pub fn from_bytes(bytes: &[u8]) -> Result<Vec<u32>, Error> {
    
    if bytes.is_empty() || // no input
        (bytes[bytes.len() - 1] & 0x80) != 0 // last byte has its continuation bit set
        { return Err(Error::IncompleteNumber); }
    
    let mut numbers = Vec::new();
    let mut i: usize = 0;
    while i < bytes.len() {
        let mut count: usize = 0; //counts the number of bytes that carry a continuation bit
        let mut intermediate = Vec::new();
        while (bytes[i] & 0x80) != 0 {
            intermediate.push((bytes[i] & 0x7F) as u32); // get rid of the continuation bits and cast to u32
            count += 1;
            i += 1;
        }
        intermediate.push(bytes[i] as u32);
        i += 1; // indexer i now points to the first byte of the next number or behind the last entry of the slice

        if count > 4 || // for count > 4 overflow is guaranteed, 
            (count == 4 && intermediate[0].checked_mul(2_u32.pow(28)) == None) 
            // for count == 4 the first byte will be bit-shifted by 4*7 bits, i.e. multiplied with 2^28, so use checked_mul
            { return Err(Error::Overflow); }
        
        numbers.push(intermediate.iter().enumerate().map(|(j, el)| el << 7*(count - j)).sum());
    }
    Ok(numbers)
}
