use fancy_regex::{Regex, Captures};
use lazy_static::lazy_static;

lazy_static! {
    // usual Rust regex crate does NOT support back referencing! So use fancy-regex instead!
    static ref CLUSTER: Regex 
        = Regex::new(r"(\D)\1+").unwrap();
    static ref GROUP: Regex
        = Regex::new(r"(\d+)(\D)").unwrap();
}

pub fn encode(source: &str) -> String {
    CLUSTER.replace_all(source, |caps: &Captures| {
        format!("{}{}",(&caps[0]).len(), &caps[1])
    }).to_string()
}

pub fn decode(source: &str) -> String {
    GROUP.replace_all(source, |caps: &Captures| {
        format!("{}", caps[2].repeat(caps[1].parse::<usize>().unwrap()))
    }).to_string()
}
