use std::collections::{HashMap, HashSet};
use winnow::{PResult, Parser, 
    ascii::alpha1,
    combinator::{preceded, separated}
};
use itertools::Itertools;

struct Problem {
    non_zero_set: HashSet<char>,
    char_values: HashMap<char, i64>
}

impl Problem {
    // use winnow crate to parse the problem
    fn parse_problem(input: &mut &str) -> PResult<Self> {
        let summands: Vec<&str> = separated(1.., alpha1, " + ").parse_next(input)?;
        let result = preceded(" == ", alpha1).parse_next(input)?;
        let mut non_zero_set: HashSet<char> = summands.iter().map(|&word| word.chars().next().unwrap()).collect();
        non_zero_set.insert(result.chars().next().unwrap());
        let mut char_values = HashMap::new();
        summands.iter().for_each(|&word| Self::process_word(&mut char_values, word, false));
        Self::process_word(&mut char_values, result, true);
        Ok(Self { non_zero_set, char_values })
    }
    fn try_from_string(input: &mut &str) -> Option<Self> {
        if let Ok(problem) = Self::parse_problem(input) { Some(problem) } else { None }
    }
    fn process_word(char_values: &mut HashMap<char, i64>, word: &str, negative: bool) {
        let sign = if negative { -1 } else { 1 };
        word.chars().rev().enumerate().for_each(|(idx, ch)| {
            let to_add = 10_i64.pow(idx as u32);
            *char_values.entry(ch).or_insert(0) += to_add * sign;
        });
    }
    fn solve(&self) -> Option<HashMap<char, u8>> {
        for perm in (0..=9).permutations(self.char_values.len()) { // check all permutations
            if perm.iter().zip(self.char_values.values()).map(|(&p, &v)| p * v).sum::<i64>() == 0 {
                if let Some((zero_idx, _)) = perm.iter().find_position(|&p| *p == 0) {
                    if !self.non_zero_set.contains(&self.char_values.keys().skip(zero_idx).next().unwrap()) {
                        return Some(self.char_values.keys().copied().zip(perm.iter().map(|p| *p as u8)).collect())
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