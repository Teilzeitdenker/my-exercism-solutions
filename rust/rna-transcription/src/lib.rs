#[derive(Debug, PartialEq, Eq)]
pub struct Dna(String);

#[derive(Debug, PartialEq, Eq)]
pub struct Rna(String);

impl Dna {
    const VALID_DNA: [char; 4] = ['A', 'C', 'G', 'T'];
    pub fn new(dna: &str) -> Result<Dna, usize> {
        for (i, c) in dna.chars().enumerate() {
            if !Dna::VALID_DNA.contains(&c) {
                return Err(i);
            }
        }
        Ok(Dna(dna.to_string()))
    }

    fn translate(c: char) -> char {
        match c {
            'A' => 'U',
            'C' => 'G',
            'G' => 'C',
            'T' => 'A',
            _   => panic!("Invalid DNA letter in private translate-function!")
        }
    }
    pub fn into_rna(self) -> Rna {
        Rna::new(&self.0.chars().map(|c| Dna::translate(c)).collect::<String>()).unwrap()
    }
}

impl Rna {
    const VALID_RNA: [char; 4] = ['A', 'C', 'G', 'U'];
    pub fn new(rna: &str) -> Result<Rna, usize> {
        for (i, c) in rna.chars().enumerate() {
            if !Rna::VALID_RNA.contains(&c) {
                return Err(i);
            }
        }
        Ok(Rna(rna.to_string()))
    }
}
