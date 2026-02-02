use std::collections::HashSet;

fn is_anagram(w1: &str, w2: &str) -> bool {
    if w1.to_lowercase() == w2.to_lowercase() {
        return false;
    }
    let mut w1: Vec<char> = w1.to_lowercase().chars().collect();
    let mut w2: Vec<char> = w2.to_lowercase().chars().collect();
    w1.sort();
    w2.sort();
    return w1 == w2;
}

pub fn anagrams_for<'a>(word: &str, possible_anagrams: &[&'a str]) -> HashSet<&'a str> {
    let mut result: HashSet<&'a str> = HashSet::new();
    for &cand in possible_anagrams {
        if is_anagram(cand, word) {
            result.insert(cand);
        }
    }
    result
}
