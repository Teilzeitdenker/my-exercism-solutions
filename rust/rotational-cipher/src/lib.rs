pub fn rotate(input: &str, key: i8) -> String {
    let shift_by = |c: char| {
        if c.is_ascii_alphabetic() {
            let zero = if c.is_ascii_uppercase() {'A'} else {'a'};
            ((c as i8 - zero as i8 + key).rem_euclid(26) as u8 + zero as u8) as char
        } else {c}
    };
    input.chars().map(shift_by).collect()
}
