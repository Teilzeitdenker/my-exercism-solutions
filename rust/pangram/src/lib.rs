use std::collections::HashSet;
/// Determine whether a sentence is a pangram.
pub fn is_pangram(sentence: &str) -> bool {
    let mut letters: HashSet<char> = HashSet::new();
    sentence
        .to_lowercase()
        .chars()
        .filter(|c| c.is_ascii_alphabetic())
        .map(|c| letters.insert(c))
        .filter(|b|*b == true)
        .count() == 26
}
