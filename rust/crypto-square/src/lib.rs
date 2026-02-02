pub fn encrypt(input: &str) -> String {
    if input.len() == 0 { return String::new();}
    let mut normalized: String = input.to_lowercase().chars().filter(|c| c.is_alphanumeric()).collect();
    let l = (normalized.len() as f32).sqrt().ceil() as usize;
    let k = normalized.len() % l;
    if k != 0 {
        normalized.extend(vec![' '; l - k]);
    }
    let chars_to_chunk = normalized.chars().collect::<Vec<_>>();
    let rows = chars_to_chunk.chunks(l).collect::<Vec<_>>();
    (0..l).map(|n| rows.iter().map(|&row| row[n]).collect::<String>()).collect::<Vec<_>>().join(" ")
}
