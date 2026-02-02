pub struct Item {
    pub weight: u32,
    pub value: u32,
}

pub fn maximum_value(mx: u32, items: &[Item]) -> u32 {
    let mut dp = vec![0; mx as usize + 1];
    for item in items {
        let (w, v) = (item.weight, item.value);
        for j in (w as usize ..=mx as usize).rev() {
            dp[j] = (dp[j - w as usize] + v).max(dp[j]);
        }
    }
    return dp[mx as usize];
}
