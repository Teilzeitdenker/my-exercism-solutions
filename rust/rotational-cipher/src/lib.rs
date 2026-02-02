pub fn rotate(input: &str, key: i8) -> String {
    let shift_by = |c: char| {
        if c.is_ascii_alphabetic() {
            let zero = if c.is_ascii_uppercase() {'A'} else {'a'};
            (remainder(c as i8 - zero as i8 + key, 26) + zero as u8) as char
        } else {c}
    };
    input.chars().map(shift_by).collect()
}

fn remainder(a: i8, rhs: i8) -> u8 {
    let r = a % rhs;
    if r < 0 { (r + rhs.abs()) as u8 } else { r as u8 }
}
