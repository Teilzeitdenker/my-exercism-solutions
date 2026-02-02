use std::collections::{HashMap, HashSet};
use winnow::{PResult, Parser, 
    ascii::alpha1,
    combinator::{preceded, separated}
};
use itertools::Itertools;

struct Problem<'s> {
    summands: Vec<&'s str>,
    result: &'s str,
}

impl<'s> Problem<'s> {
    // use winnow crate to parse the problem
    fn parse_problem(input: &mut &'s str) -> PResult<Self> {
        let summands = separated(1.., alpha1, " + ").parse_next(input)?;
        let result = preceded(" == ", alpha1).parse_next(input)?;
        Ok(Self { summands, result })
    }
    fn try_from_string(input: &mut &'s str) -> Option<Self> {
        if let Ok(problem) = Self::parse_problem(input) { Some(problem) } else { None }
    }
    fn solve(&self) -> Option<HashMap<char, u8>> {
        let mut char_values: HashMap<char, i64> = HashMap::new();
        let mut non_zero_set: HashSet<char> = HashSet::new();
        self.summands.iter().for_each(|&s| { // process summands
            non_zero_set.insert(s.chars().next().unwrap());
            s.chars().rev().enumerate().for_each(|(idx, ch)| {
                let to_add = 10_i64.pow(idx as u32);
                *char_values.entry(ch).or_insert(0) += to_add;
            })
        });
        non_zero_set.insert(self.result.chars().next().unwrap()); // process result
        self.result.chars().rev().enumerate().for_each(|(idx, ch)| {
            let to_subtract = 10_i64.pow(idx as u32);
            *char_values.entry(ch).or_insert(0) -= to_subtract;
        });
        // split char_values into two vectors to fix order and for better access
        let chars: Vec<char> = char_values.keys().copied().collect();
        let values: Vec<i64> = chars.iter().map(|ch| char_values[ch]).collect();
        // go through all permutations of the right length
        for perm in (0..=9).permutations(chars.len()) {
            // check sum
            if perm.iter().zip(values.iter()).map(|(&p, v)| p * v).sum::<i64>() == 0 {
                // check that the char assigned to 0 (if any) is not the first char in any word
                if let Some((zero_idx, _)) = perm.iter().find_position(|&p| *p == 0) {
                    if !non_zero_set.contains(&chars[zero_idx]) {
                        return Some(chars.iter().copied().zip(perm.iter().map(|p| *p as u8)).collect())
                    }
                }
            }
        }
        None
    }
}

pub fn solve(input: &str) -> Option<HashMap<char, u8>> {
    let problem = Problem::try_from_string(&mut input.clone())?;
    problem.solve()
}