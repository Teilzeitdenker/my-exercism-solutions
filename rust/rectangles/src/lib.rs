use itertools::Itertools;
pub fn count(lines: &[&str]) -> u32 {
    if lines.len() == 0 || lines.iter().all(|&line| !line.contains('+')) { return 0; }
    // just to be sure, using an .unwrap() below, so lines have to be ascii...
    assert!(lines.iter().all(|line| line.len() == lines[0].len() && line.is_ascii()));
    // a data structure that collects all column index pairs of '+' (the corner_pairs of possible rectangles) line by line
    let corner_pairs: Vec<Vec<(usize, usize)>> = 
        lines.iter().map(|&line| {
            let indices = line.char_indices().filter_map(|(i, c)| if c == '+' {Some(i)} else {None}).collect::<Vec<_>>();
            indices.iter().combinations(2).map(|comb| (*comb[0], *comb[1])).collect()
        }).collect();
    let horizontal = ['+', '-'];
    let vertical = ['+', '|'];
    // a closure that validates the edges of a rectangle whose corners are given by the indices of rows and columns
    let check_rectangle = |row1: usize, row2: usize, col1: usize, col2: usize| -> bool {
        lines[row1][(col1 + 1)..col2].chars().all(|c| horizontal.contains(&c)) && // top edge
        lines[row2][(col1 + 1)..col2].chars().all(|c| horizontal.contains(&c)) && // bottom edge
        lines[(row1 + 1)..row2].iter().all(|&row| // left and right edge
            vertical.contains(&row.chars().nth(col1).unwrap()) && vertical.contains(&row.chars().nth(col2).unwrap()))
    };
    // for a given pair in row1 this closure looks through the rows beneath for the exact same pair 
    // and counts how many of these candidate rectangles are valid
    let count_rectangles_for_pair = |pair: &(usize, usize), row1: usize| -> u32 {
        corner_pairs.iter().enumerate().skip(row1 + 1).filter_map(|(row2, pairs)| 
            if pairs.contains(pair) && check_rectangle(row1, row2, pair.0, pair.1) {Some(1)} else {None}).sum()
    };
    // calculate the number of rectangles for every single pair in corner_pairs and subsequently sum them all up
    corner_pairs.iter().enumerate().map(|(row1, pairs)| 
            pairs.iter().map(|pair| count_rectangles_for_pair(pair, row1)).sum::<u32>()).sum()
}

