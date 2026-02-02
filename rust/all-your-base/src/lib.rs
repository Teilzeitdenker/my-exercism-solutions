#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    InvalidInputBase,
    InvalidOutputBase,
    InvalidDigit(u32),
}

///
/// Convert a number between two bases.
///
/// A number is any slice of digits.
/// A digit is any unsigned integer (e.g. u8, u16, u32, u64, or usize).
/// Bases are specified as unsigned integers.
///
/// Return an `Err(.)` if the conversion is impossible.
/// The tests do not test for specific values inside the `Err(.)`.
///
///
/// You are allowed to change the function signature as long as all test still pass.
///
///
/// Example:
/// Input
///   number: &[4, 2]
///   from_base: 10
///   to_base: 2
/// Result
///   Ok(vec![1, 0, 1, 0, 1, 0])
///
/// The example corresponds to converting the number 42 from decimal
/// which is equivalent to 101010 in binary.
///
///
/// Notes:
///  * The empty slice ( "[]" ) is equal to the number 0.
///  * Never output leading 0 digits, unless the input number is 0, in which the output must be `[0]`.
///    However, your function must be able to process input with leading 0 digits.
///
pub fn convert(number: &[u32], from_base: u32, to_base: u32) -> Result<Vec<u32>, Error> {
    if from_base < 2 {
        return Err(Error::InvalidInputBase);
    }
    if to_base < 2 {
        return Err(Error::InvalidOutputBase);
    }
    match number.iter().find(|&d| *d >= from_base) {
        Some(d) => return Err(Error::InvalidDigit(*d)),
        None => { }
    }
    if number.iter().sum::<u32>() == 0 {
        return Ok(vec![0]);
    }
    let number = number.iter().rev().enumerate().map(|(i, &d)| d * u32::pow(from_base, i as u32)).sum::<u32>();
    let exponent = ((number as f64).log2() / (to_base as f64).log2()).floor() as u32 + 1;
    Ok((0..exponent).into_iter().rev().map(|i| (number / u32::pow(to_base, i)) % to_base).collect())
}
