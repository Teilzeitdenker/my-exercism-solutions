pub fn annotate(garden: &[&str]) -> Vec<String> {
    let flower_locations = get_flower_locations(garden);
    garden
        .iter()
        .enumerate()
        .map(|(i, &line)|
            line
                .char_indices()
                .map(|(j, c)|
                    if c == ' ' {count_neighbouring_flowers(&(i as i32, j as i32), &flower_locations)}
                    else {c})
                .collect())
        .collect()
}

fn get_flower_locations(garden: &[&str]) -> Vec<(i32, i32)> {
    garden
        .iter()
        .enumerate()
        .flat_map(|(i, &line)|
            line
                .char_indices()
                .filter(|(_, c)| *c == '*')
                .map(move |(j, _)| (i as i32, j as i32)))
        .collect()
}

fn count_neighbouring_flowers(location: &(i32, i32), flower_locations: &[(i32, i32)]) -> char {
    let (i, j) = *location;
    let neighbouring_fields = [(i-1, j-1), (i-1, j), (i-1, j+1), (i, j-1), (i, j+1), (i+1, j-1), (i+1, j), (i+1, j+1)];
    match neighbouring_fields.iter().filter(|&loc| flower_locations.contains(loc)).count() as u8 {
        0 => ' ',
        n => (b'0' + n) as char
    }
}