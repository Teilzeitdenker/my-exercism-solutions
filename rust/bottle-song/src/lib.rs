const CAPITALIZED_NUMBERS : [&str; 11] = ["No", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"];

fn get_bottle_text(n: u32, cap: bool) -> String {
    format!("{0} green bottle{1}", if cap { CAPITALIZED_NUMBERS[n as usize].to_string() } else { CAPITALIZED_NUMBERS[n as usize].to_lowercase() } , if n == 1 {""} else {"s"})
}

fn verse(n: u32) -> String {
    let start : String = format!("{} hanging on the wall,\n", get_bottle_text(n, true));
    let ending : String = format!("And if one green bottle should accidentally fall,\nThere'll be {} hanging on the wall.", get_bottle_text(n - 1, false));
    format!("{}{}{}", start, start, ending)
}

pub fn recite(start_bottles: u32, take_down: u32) -> String {
    (start_bottles-take_down+1..=start_bottles).rev().map(verse).collect::<Vec<_>>().join("\n\n")
}
