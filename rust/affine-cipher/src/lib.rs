/// While the problem description indicates a return status of 1 should be returned on errors,
/// it is much more common to return a `Result`, so we provide an error type for the result here.
#[derive(Debug, Eq, PartialEq)]
pub enum AffineCipherError {
    NotCoprime(i32),
}

/// Encodes the plaintext using the affine cipher with key (`a`, `b`). Note that, rather than
/// returning a return code, the more common convention in Rust is to return a `Result`.
pub fn encode(plaintext: &str, a: i32, b: i32) -> Result<String, AffineCipherError> {
    if !is_valid(a) {
        Err(AffineCipherError::NotCoprime(a))
    } else {
        let affine_encode = |c: char| {if c.is_ascii_digit() {c} else {int_to_letter(a * letter_to_int(c) + b)}};
        Ok(plaintext
            .chars()
            .filter(|c| c.is_alphanumeric())
            .map(|c| c.to_ascii_lowercase())
            .map(affine_encode)
            .collect::<Vec<_>>()
            .chunks(5)
            .map(|v| v.iter().collect::<String>())
            .collect::<Vec<_>>()
            .join(" "))
    }
}

/// Decodes the ciphertext using the affine cipher with key (`a`, `b`). Note that, rather than
/// returning a return code, the more common convention in Rust is to return a `Result`.
pub fn decode(ciphertext: &str, a: i32, b: i32) -> Result<String, AffineCipherError> {
    if !is_valid(a) {
        Err(AffineCipherError::NotCoprime(a))
    } else {
        let affine_decode = |c: char| {if c.is_ascii_digit() {c} else {int_to_letter(mod_inv(a) * (letter_to_int(c) - b))}};
        Ok(ciphertext
            .chars()
            .filter(|c| c.is_alphanumeric())
            .map(|c| c.to_ascii_lowercase())
            .map(affine_decode)
            .collect::<String>())
    }
}

fn is_valid(a: i32) -> bool {
    let amod = a % 26;
    [1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25].contains(&amod)
}

fn mod_inv(a: i32) -> i32 {
    *[1, 3, 5, 7, 9, 11, 15, 17, 19, 21, 23, 25].iter().filter(|&cand| (cand * a) % 26 == 1).last().unwrap()
}

fn letter_to_int(c: char) -> i32 {
    ((c as u32) - ('a' as u32)) as i32
}

fn int_to_letter(n: i32) -> char {
    if n < 0 {
        int_to_letter(n + 26)
    } else {
        (((n as u32 % 26) + ('a' as u32)) as u8) as char
    }
}
