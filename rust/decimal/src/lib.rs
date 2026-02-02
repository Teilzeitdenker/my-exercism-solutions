use num_bigint::BigInt;
use num_traits::Zero;
use std::cmp::Ordering;
use std::ops::{Add, Mul, Sub};

#[derive(PartialEq, Eq, Ord, Debug, Clone)]
pub struct Decimal {
    digits: BigInt,
    exponent: u64,
}

impl PartialOrd for Decimal {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        let (lhs, rhs) = align_digits(self.clone(), other.clone());
        Some(lhs.cmp(&rhs))
    }
}

impl Add for Decimal {
    type Output = Self;
    fn add(self, other: Self) -> Self {
        let (lhs, rhs) = align_digits(self.clone(), other.clone());
        Self::new(lhs + rhs, self.exponent.max(other.exponent))
    }
}

impl Sub for Decimal {
    type Output = Self;
    fn sub(self, other: Self) -> Self {
        self + Decimal::new(-other.digits, other.exponent)
    }
}

impl Mul for Decimal {
    type Output = Self;
    fn mul(self, other: Self) -> Self {
        Self::new(
            self.digits * other.digits,
            self.exponent + other.exponent,
        )
    }
}

impl Decimal {
    pub fn new(mut digits: BigInt, mut exponent: u64) -> Self {
        while exponent > Zero::zero() && &digits % 10 == Zero::zero() {
            digits /= 10;
            exponent -= 1;
        }
        Self { digits, exponent }
    }
    pub fn try_from(input: &str) -> Option<Decimal> {
        let digits = input.replace(".", "").parse::<BigInt>().ok()?;
        if let Some(index) = input.find(".") {
            Some(Decimal::new(
                digits,
                (input.len() - index - 1) as u64,
            ))
        } else {
            Some(Decimal::new(digits, Zero::zero()))
        }
    }          
}

fn align_digits(mut lhs: Decimal, mut rhs: Decimal) -> (BigInt, BigInt) {
    while lhs.exponent < rhs.exponent {
        lhs.digits *= 10;
        lhs.exponent += 1;
    }
    while rhs.exponent < lhs.exponent {
        rhs.digits *= 10;
        rhs.exponent += 1;
    }
    (lhs.digits, rhs.digits)
}