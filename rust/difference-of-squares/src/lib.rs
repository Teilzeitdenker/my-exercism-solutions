pub fn square_of_sum(n: u32) -> u32 {
    let sum_to_n = (n*(n+1))/2;
    sum_to_n * sum_to_n
}

pub fn sum_of_squares(n: u32) -> u32 {
    (n*(n+1)*(2*n+1))/6
}

pub fn difference(n: u32) -> u32 {
    square_of_sum(n) - sum_of_squares(n)
}
