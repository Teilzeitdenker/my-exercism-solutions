// could save both matrix and transposed matrix in the struct, in order to get O(1) for both queries
pub struct Matrix(Vec<Vec<u32>>);

impl Matrix {
    pub fn new(input: &str) -> Self {
        Matrix(input.lines()
            .map(|line| line.split_ascii_whitespace()
                .map(|it| it.parse::<u32>().unwrap())
                .collect())
            .collect())
    }

    // fn transpose<T>(v: &Vec<Vec<T>>) -> Vec<Vec<T>> where T: Clone {
    //     if v.is_empty() { return vec![vec![]]; }
    //     (0..v[0].len())
    //         .map(|i| v.iter()
    //             .map(|inner| inner[i].clone())
    //             .collect())
    //         .collect()
    // }

    pub fn row(&self, row_no: usize) -> Option<Vec<u32>> {
        self.0.get(row_no - 1).cloned()
    }

    // pub fn column(&self, col_no: usize) -> Option<Vec<u32>> {
    //     Self::transpose(&self.0).get(col_no - 1).cloned()
    // }

    // still O(n), but doesn't build up a the transposed matrix every time called
    pub fn column(&self, col_no: usize) -> Option<Vec<u32>> {
        if self.0.first().is_some() && self.0[0].get(col_no - 1).is_some() {
            Some(self.0.iter().map(|row| row[col_no - 1].clone()).collect())
        } else { None }
    }
}
