pub fn egg_count(display_value: u32) -> usize {
    if display_value == 0 { return 0; }
    (display_value & 1) as usize + egg_count(display_value >> 1)
}
