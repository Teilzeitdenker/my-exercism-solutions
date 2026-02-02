use std::sync::LazyLock;

const NUMBERS : [&str; 11] = ["no", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"];
static CAPITALIZED_NUMBERS : LazyLock<Vec<String>> = LazyLock::new(||
    NUMBERS.iter().map(|s| capitalize(s)).collect()
);

fn capitalize(s: &str) -> String {
    let mut chars = s.chars();
    match chars.next() {
        None => String::new(),
        Some(first) => first.to_uppercase().collect::<String>() + chars.as_str(),
    }
}

fn get_bottle_text(n: u32, cap: bool) -> String {
    let number_word: &str = if cap { &CAPITALIZED_NUMBERS[n as usize] } else { NUMBERS[n as usize] };
    format!("{0} green bottle{1}", number_word, if n == 1 {""} else {"s"})
}

fn verse(n: u32) -> String {
    let start : String = format!("{} hanging on the wall,\n", get_bottle_text(n, true));
    let end : String = format!(
        "And if one green bottle should accidentally fall,\n\
        There'll be {} hanging on the wall.",
        get_bottle_text(n - 1, false));
    format!("{}{}{}", start, start, end)
}

pub fn recite(start_bottles: u32, take_down: u32) -> String {
    (start_bottles - take_down + 1 ..= start_bottles)
    .rev()
    .map(verse)
    .collect::<Vec<_>>() 
    .join("\n\n")
}
