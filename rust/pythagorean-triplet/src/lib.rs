use std::collections::HashSet;

pub fn find(sum: u32) -> HashSet<[u32; 3]> {
    let mut result: HashSet<[u32; 3]> = HashSet::new();
    for a in 1..=(sum/3) {
        for b in (a + 1)..=((sum - a)/2) {
            if a * a + b * b == (sum - a - b) * (sum - a - b) {
                result.insert([a, b, sum - a - b]);
            }
        }
    } 
    result
}
