pub fn sum_of_multiples(limit: u32, factors: &[u32]) -> u32 {
    (1..limit).filter(|i| is_multiple(i, factors)).sum()
}

fn is_multiple(n: &u32, factors: &[u32]) -> bool {
    factors.iter().any(|cand| *cand != 0 && n % cand == 0)
}
