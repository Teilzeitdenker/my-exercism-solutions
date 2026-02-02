use std::cmp::Ordering;

/// `Palindrome` is a newtype which only exists when the contained value is a palindrome number in base ten.
///
/// A struct with a single field which is used to constrain behavior like this is called a "newtype", and its use is
/// often referred to as the "newtype pattern". This is a fairly common pattern in Rust.
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub struct Palindrome(u64);

impl Palindrome {
    /// Create a `Palindrome` only if `value` is in fact a palindrome when represented in base ten. Otherwise, `None`.
    pub fn new(value: u64) -> Option<Palindrome> {
        if Palindrome::is_palindrome(value) {Some(Palindrome(value))} else {None}
    }
    pub fn is_palindrome(value: u64) -> bool {
        let mut acc: u64 = 0;
        let mut number = value.clone();
        while number != 0 {
            acc = acc * 10 + (number % 10);
            number /= 10;
        }
        value == acc
    }
    /// Get the value of this palindrome.
    pub fn into_inner(&self) -> u64 {
        self.0
    }
    pub fn compare(&self, other: &Palindrome) -> Ordering {
        self.into_inner().cmp(&other.into_inner())
    }
}
// I'm sure one could also do this with the MinMaxResult from Itertools, but 
pub fn palindrome_products(min: u64, max: u64) -> Option<(Palindrome, Palindrome)> {
    let palindromes: Vec<Palindrome> = 
        (min..=max)
        .flat_map(|i| 
            (i..=max)
            .filter_map(move |j| Palindrome::new(i*j))).collect();
    let minimum = 
        &palindromes.iter()
        .min_by(|&a, &b| Palindrome::compare(a, b));
    let maximum = 
        &palindromes.iter()
        .max_by(|&a, &b| Palindrome::compare(a, b));
    match (minimum, maximum) {
        (Some(&p1), Some(&p2)) => Some((p1, p2)),
        _ => None
    }
}
