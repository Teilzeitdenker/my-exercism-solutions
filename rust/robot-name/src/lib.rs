use lazy_static::lazy_static;
use std::sync::Mutex;
use rand::{Rng, seq::IteratorRandom};
use std::collections::HashSet;

lazy_static! {
    static ref GIVENNAMES: Mutex<HashSet<String>> = Mutex::new(HashSet::new());
}
pub struct Robot(String);

impl Robot {

    fn get_unique_random_name() -> String {
        let mut new_name = String::new();
        let mut guard = GIVENNAMES.lock().unwrap();
        while guard.contains(&new_name) {
            let mut rng = rand::thread_rng();
            let number = rng.gen_range(100..=999).to_string();
            let letter1 = ('A'..='Z').choose(&mut rng).unwrap().to_string();
            let letter2 = ('A'..='Z').choose(&mut rng).unwrap().to_string();
            new_name = letter1 + &letter2 + &number;
        }
        guard.insert(new_name.clone());
        new_name
    }

    pub fn new() -> Self {
        Robot(Robot::get_unique_random_name())
    }

    pub fn name(&self) -> &str {
        &self.0
    }

    pub fn reset_name(&mut self) {
        self.0 = Robot::get_unique_random_name()
    }
}
