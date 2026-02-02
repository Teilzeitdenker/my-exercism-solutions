use std::collections::BTreeMap;

pub fn transform(h: &BTreeMap<i32, Vec<char>>) -> BTreeMap<char, i32> {
    let mut result: BTreeMap<char, i32> = BTreeMap::new();
    for key in h.keys() {
        for c in &h[key] {
            // since char and i32 are Copy this is no problem
            result.insert(get_lower(c), *key);
        }
    }
    result
}
fn get_lower(c: &char) -> char {
    c.to_string().to_lowercase().chars().next().unwrap()
}
