use std::collections::HashMap;

pub struct CodonsInfo<'a> {
    pairs: HashMap<&'a str, &'a str>,
}

impl<'a> CodonsInfo<'a> {
    pub fn name_for(&'a self, codon: &str) -> Option<&'a str> {
       self.pairs.get(&codon).cloned()
    }
    pub fn of_rna(&'a self, rna: &str) -> Option<Vec<&'a str>> {
        if rna.len() % 3 != 0 
        || rna.chars()
              .zip(rna.chars().skip(1).zip(rna.chars().skip(2)))
              .step_by(3)
              .map(|(a, (b, c))| CodonsInfo::name_for(self, &format!("{}{}{}", a, b, c)))
              .take_while(|&opt| opt != Some("stop codon"))
              .any(|opt| opt.is_none()) {
            return None;
        }
        else {
            Some(rna.chars()
                    .zip(rna.chars().skip(1).zip(rna.chars().skip(2)))
                    .step_by(3)
                    .filter_map(|(a, (b, c))| CodonsInfo::name_for(self, &format!("{}{}{}", a, b, c)))
                    .take_while(|&opt| opt != "stop codon")
                    .collect())
        }
    }
}

// hier wird das CodonsInfo-Objekt erzeugt, da muss ich die Liste aus dem Tests-File einfach reinkopieren
pub fn parse<'a>(pairs: Vec<(&'a str, &'a str)>) -> CodonsInfo<'a> {
    let mut new_pairs: HashMap<&str, &str> = HashMap::new();
    for (codon, protein) in pairs.iter() {
        new_pairs.insert(codon, protein);
    }
    CodonsInfo { pairs: new_pairs }
}
