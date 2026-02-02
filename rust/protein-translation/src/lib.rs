use std::collections::HashMap;
use std::str;

pub struct CodonsInfo<'a> {
    pairs: HashMap<&'a str, &'a str>,
}

impl<'a> CodonsInfo<'a> {
    pub fn name_for(&'a self, codon: &str) -> Option<&'a str> {
       self.pairs.get(codon).cloned()
    }
    pub fn of_rna(&'a self, rna: &str) -> Option<Vec<&'a str>> {
        rna.as_bytes()
           .chunks(3)
           .map(str::from_utf8)
           .map(|codon_result| CodonsInfo::name_for(self, codon_result.unwrap()))
           .take_while(|&codon| codon != Some("stop codon"))
           .collect()
        
        
        // This doesn't work. Either the "test_invalid_length" fails 
        // (because the zipping doesn't "see", if there are 1 or 2 nucleotides left at the end)
        // and when I put the length check in front, the "test_valid_stopped_rna" fails
        
        // if rna.len() % 3 != 0 {return None;}
        // rna.chars()
        //     .zip(rna.chars().skip(1).zip(rna.chars().skip(2)))
        //     .step_by(3)
        //     .map(|(a, (b, c))| CodonsInfo::name_for(self, &format!("{}{}{}", a, b, c)))
        //     .take_while(|&opt| opt != Some(STOP))
        //     .collect()
        
    }
}

// hier wird das CodonsInfo-Objekt erzeugt, da muss ich die Liste aus dem Tests-File einfach reinkopieren
pub fn parse<'a>(pairs: Vec<(&'a str, &'a str)>) -> CodonsInfo<'a> {
    CodonsInfo { pairs: pairs.iter().cloned().collect() }
}
