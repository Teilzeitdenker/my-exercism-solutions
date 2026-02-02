// This stub file contains items that aren't used yet; feel free to remove this module attribute
// to enable stricter warnings.
#![allow(unused)]

use std::collections::HashMap;

pub fn can_construct_note(magazine: &[&str], note: &[&str]) -> bool {
    let mut mag_words: HashMap<&str, u32>= HashMap::with_capacity(note.len());
    for &word in magazine {
        let counter = mag_words.entry(word).or_insert(0);
        *counter += 1;
    }
    for &word in note {
        match mag_words.get_mut(word) {
            None => return false,
            Some(num) => { if *num > 1 {
                *num -= 1;
                } else {
                mag_words.remove(word);
                }
            }
        }
    }
    true
}
