use std::fmt::{Display, Formatter, Result};

pub struct Roman(u32);

impl Display for Roman {
    fn fmt(&self, f: &mut Formatter<'_>) -> Result {
        let roman_letters:Vec<(&str, &str)> = vec![("I", "V"), ("X", "L"), ("C", "D"), ("M", "?")];
        
        let mut digits: Vec<u32> = vec![];
        let mut value = self.0;
        loop {
            digits.push(value % 10);
            value /= 10;
            if value == 0 { break; }
        }
        let res = digits
            .iter()
            .enumerate()
            .map(|(ind, &digit)| {
                    let (a, b) = roman_letters[ind];
                    match digit {
                        n if n < 4 => str::repeat(a, n as usize),
                        4 => a.to_string() + b,
                        r if r < 9 => b.to_string() + str::repeat(a, r as usize - 5).as_str(),
                        9 => a.to_string() + roman_letters[ind + 1].0,
                        _ => panic!("Not a digit!"),
                    }
                })
            .rfold("".to_string(), |acc, el| acc + &el);
        write!(f, "{}", res)
    }
}

impl From<u32> for Roman {
    fn from(num: u32) -> Self {
        Roman(num)
    }
}
