use std::vec;

pub struct PascalsTriangle(Vec<Vec<u32>>);

impl PascalsTriangle {
    pub fn new(row_count: u32) -> Self {
        match row_count {
            0 => PascalsTriangle(vec![]),
            1 => PascalsTriangle(vec![vec![1]]),
            n => {
                let PascalsTriangle(mut before) =  PascalsTriangle::new(n - 1);
                let mut last_row: Vec<u32>= vec![1];
                for item in before.last().unwrap().windows(2).map(|v| v.iter().sum()) {
                    last_row.push(item);
                }
                last_row.push(1);
                before.push(last_row);
                PascalsTriangle(before)
            }      
        }
    }

    pub fn rows(&self) -> Vec<Vec<u32>> {
        self.0.clone()
    }
}
