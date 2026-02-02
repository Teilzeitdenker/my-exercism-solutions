pub fn series(digits: &str, len: usize) -> Vec<String> {
    let size = digits.len();
    if len == 0 {
        return vec!["".to_string(); size + 1];
    }
    else if len > size {
        return vec![];
    }
    else {
        let mut res = vec![];
        for i in 0..(size - len + 1) {
            res.push(digits[i..(i + len)].to_string());
        }
        res
    }
}
