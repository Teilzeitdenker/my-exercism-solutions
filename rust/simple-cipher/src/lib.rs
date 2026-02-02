use rand::distributions::{Uniform, Distribution};

pub fn encode(key: &str, s: &str) -> Option<String> {
    do_encode(key, s, true)
}

pub fn decode(key: &str, s: &str) -> Option<String> {
    do_encode(key, s, false)
}

fn do_encode(key: &str, s: &str, encode: bool) -> Option<String> {
    if key.chars().any(|c| !c.is_ascii_lowercase()) || key.is_empty() { None }
    else {
        let sign_function = if encode {i32::saturating_add} else {i32::saturating_sub};
        let affine_encode = |(c, k)| int_to_letter( sign_function(letter_to_int(c), letter_to_int(k)) );
        Some(s.chars().zip(key.chars().cycle()).map(affine_encode).collect())
    }
}

pub fn encode_random(s: &str) -> (String, String) {
    let mut key = String::with_capacity(100);
    let between: Uniform<u8> = Uniform::from(0..26);
    let mut rng = rand::thread_rng();
    for _ in 0..100 {
        key.push((between.sample(&mut rng) + 'a' as u8) as char);
    }
    match encode(&key, s) {
        Some(res) => (key, res),
        None => (key, "".to_string())
    }
}

fn letter_to_int(c: char) -> i32 {
    ((c as u32) - ('a' as u32)) as i32
}

fn int_to_letter(n: i32) -> char {
    ((n.rem_euclid(26) + 'a' as i32) as u8) as char
}