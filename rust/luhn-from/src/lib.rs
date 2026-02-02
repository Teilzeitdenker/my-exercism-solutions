pub struct Luhn(String);
impl Luhn {
    pub fn is_valid(&self) -> bool {
        self.0.chars().rev().filter(|c| !c.is_whitespace())
        .try_fold((0, 0), |(sum, count), val| {
            val.to_digit(10)
                .map(|n| if count % 2 == 1 { if 2 * n > 9 { 2 * n - 9 } else {2 * n} } else { n })
                .map(|num| (num + sum, count + 1)) })
        .map_or(false, |(sum, count)| sum % 10 == 0 && count > 1)
    }
}
impl<T> From<T> for Luhn where T: ToString {
    fn from(input: T) -> Self { Luhn(input.to_string()) }
}
