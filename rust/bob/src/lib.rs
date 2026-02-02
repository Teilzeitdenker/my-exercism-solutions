pub fn reply(message: &str) -> &str {
    let s = message.trim();
    if s.len() == 0 {
        return "Fine. Be that way!";
    }
    match (is_question(s), is_yelled(s)) {
        (true, true) => "Calm down, I know what I'm doing!",
        (true,  _  ) => "Sure.",
        ( _  , true) => "Whoa, chill out!",
        _            => "Whatever.",
    }
}

fn is_question(s: &str) -> bool {
    if s.len() == 0 {
        return false;
    }
    s.ends_with('?')
}

fn contains_letters(s: &str) -> bool {
    s.chars().any(char::is_alphabetic)
}

fn is_yelled(s: &str) -> bool {
    if contains_letters(s) {
        return s.to_uppercase() == s;
    }
    false
}
