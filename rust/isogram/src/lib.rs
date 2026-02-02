use std::collections::HashSet;

pub fn check(candidate: &str) -> bool {
    let mut different_letters = HashSet::new();
    for c in candidate.chars().filter(|c| c.is_alphabetic()) {
        different_letters.insert(c.to_ascii_lowercase());
    }
    candidate.chars().filter(|c| c.is_alphabetic()).count() == different_letters.len()
}
