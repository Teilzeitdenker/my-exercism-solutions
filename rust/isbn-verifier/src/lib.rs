use regex::Regex;
use lazy_static::lazy_static;

lazy_static! {
    static ref VALID_ISBN: Regex = Regex::new(r"^(\d-?){9}(\d|X)$").unwrap();
}

/// Determines whether the supplied string is a valid ISBN number
pub fn is_valid_isbn(isbn: &str) -> bool {
    if !VALID_ISBN.is_match(isbn) {return false;}
    isbn
        .replace("-", "")
        .chars()
        .rev()
        .enumerate()
        .fold(0, checksum)
        .rem_euclid(11) == 0
}

fn checksum(accum: usize, tuple: (usize, char)) -> usize {
    match tuple {
        (0, 'X') => accum + 10,
        (n, c) => accum + (n + 1)*(c as usize - '0' as usize),
    }
}
