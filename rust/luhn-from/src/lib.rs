pub struct Luhn(String);

impl Luhn {
    pub fn is_valid(&self) -> bool {
        self.0.chars()
        .rev()
        .filter(|c| !c.is_whitespace())
        .try_fold((0, 0), |(sum, count), val|
        {
            val.to_digit(10)
                .map(|n| if count % 2 == 1 { Luhn::luhn_double(n) } else { n })
                .map(|num| (num + sum, count + 1))
        })
        .map_or(false, |(sum, count)| sum % 10 == 0 && count > 1)
    }

    fn luhn_double(n: u32) -> u32 {
        if 2 * n > 9 {
            return 2 * n - 9;
        }
        2 * n
    }
}

/// Here is the example of how the From trait could be implemented
/// for the &str type. Naturally, you can implement this trait
/// by hand for the every other type presented in the test suite,
/// but your solution will fail if a new type is presented.
/// Perhaps there exists a better solution for this problem?
impl<T> From<T> for Luhn
    where T: ToString {
    fn from(input: T) -> Self {
        Luhn(input.to_string())
    }
}
