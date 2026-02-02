pub fn abbreviate(phrase: &str) -> String {
    let delimiters = [' ', '-', '_'];
    phrase.split(&delimiters).map(|part| get_relevant_chars(part)).collect::<String>()
}
fn get_relevant_chars(word: &str) -> String {
    let mut result = match word.chars().next() {
        None => "".to_string(),
        Some(c) => c.to_string().to_uppercase()
     };
     if word.chars().any(|c| c.is_alphabetic() && c.is_lowercase()) {
        let more_chars = word.chars().skip(1).filter(|c| c.is_alphabetic() && c.is_uppercase()).collect::<String>();
        result += &more_chars;
     }
     result
}