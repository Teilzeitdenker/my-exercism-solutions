pub fn brackets_are_balanced(input: &str) -> bool {
    let opened = vec!['(', '[', '{'];
    let closed = vec![')', ']', '}'];
    let mut brackets: Vec<char> = Vec::new();
    for c in input.chars() {
        if !c.is_ascii_punctuation() {
            continue;
        }
        if opened.contains(&c) {
            brackets.push(c);
        }
        if closed.contains(&c) {
            if brackets.len() == 0 {
                return false;
            }
            let open_bracket = brackets.pop().unwrap();
            let open_bracket_index = opened.iter().position(|&r| r == open_bracket).unwrap();
            let closed_bracket_index = closed.iter().position(|&r| r == c).unwrap();
            if open_bracket_index != closed_bracket_index {
                return false;
            }
        }
    }
    brackets.len() == 0
}
