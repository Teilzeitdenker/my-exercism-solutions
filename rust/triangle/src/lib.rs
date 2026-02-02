use num::Num;
use std::cmp::{PartialOrd, PartialEq};
pub struct Triangle<T: Num + PartialEq + PartialOrd + Copy> {
    sides: [T; 3]
}

impl<T: Num + PartialEq + PartialOrd + Copy> Triangle<T> {
    pub fn build(sides: [T; 3]) -> Option<Triangle<T>> {
        let [a, b, c] = &sides;
        match Triangle::triangle_inequalities_check(a, b, c) {
            false => None,
            _     => Some(Triangle { sides }),
        }
    }

    fn triangle_inequalities_check(&a: &T,  &b: &T, &c: &T) -> bool {
        (a > T::zero()) && (b > T::zero()) && (c > T::zero()) && (a + b > c) && (a + c > b) && (b + c > a)
    }

    pub fn is_equilateral(&self) -> bool {
        let [a, b, c] = &self.sides;
        a == b && b == c
    }

    pub fn is_scalene(&self) -> bool {
        !Triangle::is_equilateral(&self) && !Triangle::is_isosceles(&self)
    }

    pub fn is_isosceles(&self) -> bool {
        let [a, b, c] = &self.sides;
        a == b || b == c || c == a
    }
}
