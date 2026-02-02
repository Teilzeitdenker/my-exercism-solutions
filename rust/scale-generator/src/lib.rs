#[derive(Debug)]
pub enum Error {
    InvalidTonic(String),
    InvalidIntervals(String),
}

pub struct Scale(Vec<String>);

const SHARPS: [&str; 14] = ["C", "G", "D", "A", "E", "B", "F#", "a", "e", "b", "f#", "c#", "g#", "d#"];
const FLATS: [&str; 12] = ["F", "Bb", "Eb", "Ab", "Db", "Gb", "d", "g", "c", "f", "bb", "eb"];
const SHARP_CHROMATIC: [&str; 12] = ["A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"];
const FLAT_CHROMATIC: [&str; 12] = ["A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab"];

impl Scale {
    pub fn new(tonic: &str, intervals: &str) -> Result<Scale, Error> {
        let tones = if SHARPS.contains(&tonic) { SHARP_CHROMATIC }  else if FLATS.contains(&tonic) { FLAT_CHROMATIC } else { return Err(Error::InvalidTonic(tonic.to_string())) };
        let mut filter_pattern: Vec<bool> = Vec::new();
        for interval in intervals.chars() {
            filter_pattern.extend(
                match interval {
                    'm' => vec![true],
                    'M' => vec![true, false],
                    'A' => vec![true, false, false],
                    _   => return Err(Error::InvalidIntervals(intervals.to_string())),
                })
        }
        filter_pattern.push(true);  // don't forget the last tone
        Ok(Scale(tones.iter()
                    .cycle().skip_while(|&c| *c.to_ascii_uppercase() != tonic.to_ascii_uppercase()) // use this trick to save a function that uppercases tonic
                    .zip(filter_pattern.iter())
                    .filter_map(|(&t, &b)| if b {Some(t.to_string())} else {None}) 
                    .collect()))
    }
    pub fn chromatic(tonic: &str) -> Result<Scale, Error> {
        let intervals = "m".repeat(12);
        Scale::new(tonic, &intervals)
    }
    pub fn enumerate(&self) -> Vec<String> {
        self.0.clone()
    }
}
