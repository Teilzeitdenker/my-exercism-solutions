pub fn is_leap_year(y: u64) -> bool {
    match (y % 4, y % 100, y % 400) {
        (_, _, 0) => true,
        (_, 0, _) => false,
        (0, _, _) => true,
        _ => false
    }
}
