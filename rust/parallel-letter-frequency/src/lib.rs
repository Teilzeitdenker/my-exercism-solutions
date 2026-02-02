use std::collections::HashMap;
use std::sync::Mutex;
use rayon::prelude::*;

pub fn frequency(input: &[&str], _worker_count: usize) -> HashMap<char, usize> {
    let acc: Mutex<HashMap<char, usize>> = Mutex::new(HashMap::new());
    input.par_iter()
        .map(|&text| process_one_text(text))
        .for_each(|m| {
            let mut acc = acc.lock().unwrap();
            for (key, value) in m {
                (*acc).entry(key).and_modify(|v| *v += value).or_insert(value);
    }
        });
    let acc = acc.lock().unwrap();
    acc.to_owned()
}

fn process_one_text(input: &str) -> HashMap<char, usize> {
    let mut map = HashMap::new();
    for c in input.chars().filter(|c| c.is_alphabetic()) {
        if let Some(c) = c.to_lowercase().next() {
            (*map.entry(c).or_insert(0)) += 1;  
        }
    }
    map
}