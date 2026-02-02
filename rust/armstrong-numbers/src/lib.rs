pub fn is_armstrong_number(num: usize) -> bool {
    let dig_vec: Vec<usize> = x(num);
    let exponent: u32 = dig_vec.len() as u32;
    let dig_pow_sum = dig_vec
        .iter()
        .map(|dig| usize::pow(*dig,exponent))
        .fold(0, |acc, val| acc + val);
    num == dig_pow_sum
}

fn x(n: usize) -> Vec<usize> {
    fn x_inner(n: usize, xs: &mut Vec<usize>) {
        if n >= 10 {
            x_inner(n / 10, xs);
        }
        xs.push(n % 10);
    }
    let mut xs = Vec::new();
    x_inner(n, &mut xs);
    xs
}
