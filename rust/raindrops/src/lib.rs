use std::collections::BTreeMap;
pub fn raindrops(n: u32) -> String {
    let mut sounds = BTreeMap::new();
    sounds.insert(3, "Pling");
    sounds.insert(5, "Plang");
    sounds.insert(7, "Plong");

    let result = sounds.iter()
                            .filter(|(&key, _ )| n % key == 0)
                            .map(|( _ , &value)| value.to_string())
                            .collect::<Vec<_>>()
                            .join("");
    if result.is_empty() {
        return n.to_string();
    }                        
    result
}
