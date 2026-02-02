use std::collections::HashMap;
// This annotation prevents Clippy from warning us that `School` has a
// `fn new()` with no arguments, but doesn't implement the `Default` trait.
//
// Normally, it's good practice to just do what Clippy tells you, but in this
// case, we want to keep things relatively simple. The `Default` trait is not the point
// of this exercise.
#[allow(clippy::new_without_default)]
pub struct School<'a> {
    roster: HashMap<u32, Vec<&'a str>>,
}

impl<'a> School<'a> {
    pub fn new() -> School<'a> {
        let roster = HashMap::new();
        Self { roster }
    }

    pub fn add(&mut self, grade: u32, student: &'a str) {
        let empty_vec: Vec<&'a str> = Vec::new();
        if !self.roster.contains_key(&grade) {
            self.roster.insert(grade, empty_vec);
        }
        let student_list = self.roster.get_mut(&grade).unwrap();
        student_list.push(student);
        student_list.sort();
    }

    
    pub fn grades(&self) -> Vec<u32> {
        let mut sorted: Vec<u32> = self.roster.keys().cloned().collect();
        sorted.sort();
        sorted
    }

    // If `grade` returned a reference, `School` would be forced to keep a `Vec<String>`
    // internally to lend out. By returning an owned vector of owned `String`s instead,
    // the internal structure can be completely arbitrary. The tradeoff is that some data
    // must be copied each time `grade` is called.
    pub fn grade(&self, grade: u32) -> Vec<String> {
        match self.roster.get(&grade) {
            None => vec![],
            Some(v) => {v.iter().map(|&s| s.to_string()).collect()}
        }
    }
}
