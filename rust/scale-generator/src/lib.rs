#[derive(Debug)]
pub enum Error {
    InvalidTonic(String),
    InvalidIntervals(String),
}

pub struct Scale(Vec<String>);

impl Scale {
    pub fn new(tonic: &str, intervals: &str) -> Result<Scale, Error> {
        let sharp_tonics = ["C", "G", "D", "A", "E", "B", "F#", "a", "e", "b", "f#", "c#", "g#", "d#"];
        let flat_tonics = ["F", "Bb", "Eb", "Ab", "Db", "Gb", "d", "g", "c", "f", "bb", "eb"];
        if !sharp_tonics.contains(&tonic) && !flat_tonics.contains(&tonic) {
            return Err(Error::InvalidTonic(tonic.to_string()));
        } 
        else if intervals.chars().any(|c| !['m', 'M', 'A'].contains(&c)) {
            return Err(Error::InvalidIntervals(intervals.to_string()));
        }
        let mut filter_pattern = intervals.chars().fold(vec![], |mut acc, c| match c {
            'm' => {acc.extend(vec![true]); acc},
            'M' => {acc.extend(vec![true, false]); acc},
            'A' => {acc.extend(vec![true, false, false]); acc},
            _   => acc,
        });
        filter_pattern.push(true);
        let tones = if sharp_tonics.contains(&tonic) {
                ["A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"]
            } else {
                ["A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab"]
        };
        let invariant = Scale::get_invariant_tonic(&tonic);
        Ok(
            Scale(
                tones
                    .iter()
                    .cycle()
                    .skip_while(|&c| *c != invariant)
                    .zip(filter_pattern.iter())
                    .filter(|(_, &b)| b)
                    .map(|(&t, _)| t.to_string())
                    .collect::<Vec<String>>()
                )
            )

    }

    fn get_invariant_tonic(tonic: &str) -> String {
        if tonic.len() == 1 {
            tonic.to_ascii_uppercase().to_string()
        } else {
            let mut iter= tonic.chars();
            let mut invariant = String::new();
            invariant.push(iter.next().unwrap().to_ascii_uppercase());
            invariant.push(iter.next().unwrap());
            invariant
        }
    }

    pub fn chromatic(tonic: &str) -> Result<Scale, Error> {
        let intervals = "m".repeat(12);
        Scale::new(tonic, &intervals)
    }

    pub fn enumerate(&self) -> Vec<String> {
        self.0.clone()
    }
}
