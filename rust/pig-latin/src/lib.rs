use regex::Regex;
use lazy_static::lazy_static;

lazy_static! {
    static ref RULE_1: Regex 
        = Regex::new(r"^([aeiou]|x[^aeiou]|y[^aeiou])").unwrap();
    static ref RULE24: Regex
        = Regex::new(r"^(?P<cluster>[^aeiou][^aeiouy]*)(?P<rest>.*)$").unwrap();
    static ref RULE_3: Regex
        = Regex::new(r"^(?P<cluster>[^aeiou]*qu)(?P<rest>.*)$").unwrap();
}


pub fn translate(input: &str) -> String {
    if input.contains(" ") {
        input.split(" ").map(|w| translate(w)).collect::<Vec<String>>().join(" ")
    } else {
        if RULE_1.is_match(input) {
            input.to_string() + "ay"
        } else if RULE_3.is_match(input) {
            RULE_3.replace_all(input, "$rest$cluster").to_string() + "ay"
        } else if RULE24.is_match(input) {
            RULE24.replace_all(input, "$rest$cluster").to_string() + "ay"
        }  else {
            input.to_string()
        }
    }
}
