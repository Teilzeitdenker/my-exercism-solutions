use std::collections::HashMap;

/// "Encipher" with the Atbash cipher.
pub fn encode(plain: &str) -> String {
    let cipher_map = ('a'..='z').zip(('a'..='z').rev()).collect::<HashMap<char, char>>();
    let atbash_encode = |c: char| {if c.is_ascii_digit() {c} else {cipher_map[&c]}};
    plain
        .to_ascii_lowercase()
        .chars()
        .filter(|c| c.is_alphanumeric())
        .map(atbash_encode).collect::<Vec<_>>()
        .chunks(5)
        .map(|v| v.iter().collect::<String>())
        .collect::<Vec<_>>()
        .join(" ")
}

/// "Decipher" with the Atbash cipher.
pub fn decode(cipher: &str) -> String {
    encode(cipher).chars().filter(|c| c.is_alphanumeric()).collect()
}
