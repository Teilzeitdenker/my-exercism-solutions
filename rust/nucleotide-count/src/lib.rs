use std::collections::HashMap;
const NUCLEOTIDES: [char; 4] = ['A', 'G', 'T', 'C'];
pub fn count(nucleotide: char, dna: &str) -> Result<usize, char> {
    if !NUCLEOTIDES.contains(&nucleotide) {
        return Err(nucleotide);
    }
    match dna.chars().find(|c| !NUCLEOTIDES.contains(&c)) {
        Some(c) => return Err(c),
        None => {},
    }
    Ok(dna.chars().filter(|c| c == &nucleotide).count())
}

pub fn nucleotide_counts(dna: &str) -> Result<HashMap<char, usize>, char> {
    match dna.chars().find(|c| !NUCLEOTIDES.contains(&c)) {
        Some(c) => return Err(c),
        None => {},
    }
    Ok(NUCLEOTIDES.iter().map(|c| (*c, count(*c, dna).unwrap())).collect())
}
