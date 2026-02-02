pub fn annotate(minefield: &[&str]) -> Vec<String> {
    let mine_locations = get_mine_locations(minefield);
    minefield
        .iter()
        .enumerate()
        .map(|(i, &line)| 
            line
                .char_indices()
                .map(|(j, c)| 
                    if c == ' ' {count_neighbouring_mines(&(i as i32, j as i32), &mine_locations)} 
                    else {c})
                .collect())
        .collect()
}

fn get_mine_locations(minefield: &[&str]) -> Vec<(i32, i32)> {
    minefield
        .iter()
        .enumerate()
        .flat_map(|(i, &line)| 
            line
                .char_indices()
                .filter(|(_, c)| *c == '*')
                .map(move |(j, _)| (i as i32, j as i32)))
        .collect()
}

fn count_neighbouring_mines(location: &(i32, i32), mine_locations: &[(i32, i32)]) -> char {
    let (i, j) = location.clone();
    let neighbouring_fields = vec![(i-1, j-1), (i-1, j), (i-1, j+1), (i, j-1), (i, j+1), (i+1, j-1), (i+1, j), (i+1, j+1)];
    match neighbouring_fields.iter().filter(|&loc| mine_locations.contains(loc)).count() as u8 {
        0 => ' ',
        n => ('0' as u8 + n) as char
    }
}