#[derive(Debug, PartialEq, Eq)]
pub enum Classification {
    Abundant,
    Perfect,
    Deficient,
}

fn aliquot(num: u64) -> u64 {
    (1..=num/2).filter(|a| num % a == 0).sum()
}

pub fn classify(num: u64) -> Option<Classification> {
    use self::Classification::*;
    if num == 0 {return None;}
    match aliquot(num) {
        a if a > num => Some(Abundant),
        a if a == num => Some(Perfect),
        _ => Some(Deficient),
    }
}
