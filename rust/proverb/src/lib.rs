pub fn build_proverb(list: &[&str]) -> String {
    if list.len() == 0 {
        return "".to_string();
    }
    let last = std::iter::once(format!("And all for the want of a {}.", list[0]));
    list.iter()
        .zip(list.iter().skip(1))
        .map(|(a, b)| format!("For want of a {} the {} was lost.", *a, *b))
        .chain(last)
        .collect::<Vec<_>>()
        .join("\n")
}
