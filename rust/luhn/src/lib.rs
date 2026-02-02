/// Check a Luhn checksum.
pub fn is_valid(code: &str) -> bool {
    if code.chars().filter(|c| !c.is_whitespace()).any(|c| !c.is_digit(10)) {
        return false;
    }
    let digits: Vec<char> = code
        .chars()
        .filter(|c| c.is_digit(10))
        .collect();
    if  digits.len() <= 1 {
        return false;
    }
    digits
        .iter()
        .map(|&c| char::to_digit(c, 10).unwrap())
        .rev()
        .enumerate()
        .map(|(ind, dig)| if ind % 2 == 1 { luhn_double(dig) } else { dig })
        .sum::<u32>() % 10 == 0
}

fn luhn_double(n: u32) -> u32 {
    if 2 * n > 9 {
        return 2 * n - 9;
    }
    2 * n
}