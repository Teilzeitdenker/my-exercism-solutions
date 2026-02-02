use regex::Regex;
use lazy_static::lazy_static;
use std::collections::HashMap;

lazy_static! {
    static ref WORD: Regex 
        = Regex::new(r"\w+('\w+)*").expect("error parsing regex");
}

/// Count occurrences of words.
pub fn word_count(words: &str) -> HashMap<String, u32> {
    let mut frequencies: HashMap<String, u32> = HashMap::new();
    for match_ in WORD.find_iter(&words.to_lowercase()) {
        *frequencies.entry(match_.as_str().to_string()).or_insert(0) += 1;
    }
    frequencies
}
