pub fn find_saddle_points(input: &[Vec<u64>]) -> Vec<(usize, usize)> {
    let num_rows = input.len();
    let num_columns = input[0].len();
    let is_saddle_point = |(i, j): (usize, usize)| -> bool {
        let value = input[i][j];
        (0..num_columns).all(|k| input[i][k] <= value)
        && (0..num_rows).all(|k| input[k][j] >= value)
    };
    (0..num_rows).flat_map(|i| {
        (0..num_columns)
            .map(move |j| (i, j))
            .filter(|(i, j)| is_saddle_point((*i, *j)))
    })
    .collect()
}

