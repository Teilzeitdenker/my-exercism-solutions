pub struct Item {
    pub weight: u32,
    pub value: u32,
}

pub fn maximum_value(max_weight: u32, items: &[Item]) -> u32 {
    let mut dp = vec![0; max_weight as usize + 1];
    let mut prv = vec![0; max_weight as usize + 1];
    for item in items {
        if item.weight <= max_weight {
            prv.clone_from_slice(&dp);
            for (dp, &p) in dp[item.weight as usize..].iter_mut().zip(prv.iter()) {
                *dp = (p + item.value).max(*dp);
            }
        }
    }
    return dp[max_weight as usize];
}
