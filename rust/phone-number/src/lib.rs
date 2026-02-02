use lazy_static::lazy_static;
use regex::Regex;

lazy_static! {
    static ref NODIGIT: Regex = Regex::new(r"[^[:digit:]]").unwrap();
    static ref PUNCTUATION: Regex = Regex::new(r"[\s|\-|\.|\(|\)|\+]").unwrap();
}

pub fn number(user_number: &str) -> Option<String> {
    validate_input(user_number).and_then(check_length).and_then(check_area_code).and_then(check_exchange_code)
}

fn validate_input(input: &str) -> Option<String> {
    let only_digits = PUNCTUATION.replace_all(input, "");
    println!("{}", only_digits);
    match NODIGIT.is_match(&only_digits) {
        true => None,
        false => Some(only_digits.to_string())
    }
}

fn check_length(input: String) -> Option<String> {
    match input.len() {
        11 => if input.starts_with("1") { Some(input[1..].to_string()) } else { None },
        10 => Some(input),
        _  => None
    }
}

fn check_area_code(input: String) -> Option<String> {
    match input.chars().nth(0) {
        Some('0') => None,
        Some('1') => None,
        _   => Some(input)
    }
}

fn check_exchange_code(input: String) -> Option<String> {
    match input.chars().nth(3) {
        Some('0') => None,
        Some('1') => None,
        _   => Some(input)
    }
}