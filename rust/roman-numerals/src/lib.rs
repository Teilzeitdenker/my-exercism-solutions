use std::fmt::{Display, Formatter, Result};
pub struct Roman(u32);

const ROMAN_LETTERS: [(&'static str, &'static str); 4] = [("I", "V"), ("X", "L"), ("C", "D"), ("M", "?")];

fn format_digit(f: &mut Formatter, digit: u32, pos: usize) -> Result {
    assert!(digit < 10);
    let (a, b) = ROMAN_LETTERS[pos];
    match digit {
        n if n < 4 => write!(f, "{}", str::repeat(a, n as usize)),
        4               => write!(f, "{a}{b}"),
        r if r < 9 => write!(f, "{b}{}", str::repeat(a, r as usize - 5)),
        9               => write!(f, "{a}{}", ROMAN_LETTERS[pos + 1].0),
        _               => unreachable!(),
    }
}

impl Display for Roman {
    fn fmt(&self, f: &mut Formatter<'_>) -> Result {
        let mut digits_reverse: Vec<u32> = vec![]; 
        let mut num = self.0;
        loop {
            digits_reverse.push(num % 10);
            num /= 10;
            if num == 0 { break; }
        }
        digits_reverse
            .iter()
            .enumerate()
            .rfold(Ok(()), |acc, (pos, &digit)| {format_digit(f, digit, pos)?; acc})
    }
}

impl From<u32> for Roman {
    fn from(num: u32) -> Self {
        if num >= 4000 {panic!("The number {} is too big to be converted into a roman numeral", num)}
        Self(num)
    }
}
