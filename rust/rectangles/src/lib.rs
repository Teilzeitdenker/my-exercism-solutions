pub fn count(lines: &[&str]) -> u32 {
    if lines.len() == 0 || lines.iter().all(|&line| !line.contains('+')) { return 0; }
    // a data structure that collects all column index pairs of '+' (the corner_pairs of possible rectangles) line by line
    let corner_pairs: Vec<Vec<(usize, usize)>> = 
        lines.iter().map(|&line| {
            let indices = line.char_indices().filter_map(|(i, c)| if c == '+' {Some(i)} else {None}).collect::<Vec<_>>();
            match indices.len() {
                0 => Vec::new(),
                _ => { // this part could possibly be done with the .combinations(2) function in the itertools crate
                    let mut pairs = Vec::with_capacity( (indices.len() * (indices.len() - 1)) / 2);
                    for i in 0..(indices.len() - 1) {
                        for j in (i + 1)..indices.len() {
                            pairs.push((indices[i], indices[j]));
                        }
                    }
                    pairs
                }
            }
        }).collect();
    let horizontal = ['+', '-'];
    let vertical = ['+', '|'];
    // a closure that validates the edges of a rectangle whose corners are given by the indices of rows and columns
    let check_rectangle = |row1: usize, row2: usize, col1: usize, col2: usize| -> bool {
        lines[row1][(col1 + 1)..col2].chars().all(|c| horizontal.contains(&c)) &&
        lines[row2][(col1 + 1)..col2].chars().all(|c| horizontal.contains(&c)) &&
        lines[(row1 + 1)..row2].iter().all(|&row| vertical.contains(&row.chars().nth(col1).unwrap()) && vertical.contains(&row.chars().nth(col2).unwrap()))
    };
    // for a given pair in row1 this closure looks through the rows beneath for the exact same pair and counts how many of these candidate rectangles are valid
    let count_rectangles_for_pair = |pair: &(usize, usize), row1: usize| -> u32 {
        corner_pairs.iter().enumerate().skip(row1 + 1).filter_map(|(row2, pairs)| 
            if pairs.contains(pair) && check_rectangle(row1, row2, pair.0, pair.1) {Some(1)} else {None}).sum()
    };
    // calculate the number of rectangles for every single pair in corner_pairs and subsequently sum them all up
    corner_pairs
        .iter()
        .enumerate()
        .map(|(row1, pairs)| 
            pairs
                .iter()
                .map(|pair| count_rectangles_for_pair(pair, row1))
                .sum::<u32>()
        ).sum()
}

