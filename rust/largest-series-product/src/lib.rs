#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    SpanTooLong,
    InvalidDigit(char),
}

pub fn lsp(string_digits: &str, span: usize) -> Result<u64, Error> {
    use Error::*;
    for ch in string_digits.chars() {
        if ch.to_string().parse::<u8>().is_err() {
            return Err(InvalidDigit(ch))
        }
    }
    if span > string_digits.len() { Err(SpanTooLong) }
    else if span == 0 { Ok(1) }
    else {
        let digits = string_digits.as_bytes().iter().map(|&c| (c - b'0') as u64).collect::<Vec<_>>();
        Ok(digits.windows(span).map(|w| w.iter().product()).max().unwrap())
    }
}

