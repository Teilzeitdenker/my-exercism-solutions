use std::collections::BTreeMap;
// This annotation prevents Clippy from warning us that `School` has a
// `fn new()` with no arguments, but doesn't implement the `Default` trait.
//
// Normally, it's good practice to just do what Clippy tells you, but in this
// case, we want to keep things relatively simple. The `Default` trait is not the point
// of this exercise.
#[allow(clippy::new_without_default)]
pub struct School<'a> {
    roster: BTreeMap<u32, Vec<&'a str>>,
}

impl<'a> School<'a> {
    pub fn new() -> School<'a> {
        let roster = BTreeMap::new();
        Self { roster }
    }

    pub fn add(&mut self, grade: u32, student: &'a str) {
        self.roster.entry(grade).and_modify(|students| {students.push(student); students.sort();}).or_insert(vec![student]);
        // let empty_vec: Vec<&'a str> = Vec::new();
        // if !self.roster.contains_key(&grade) {
        //     self.roster.insert(grade, empty_vec);
        // }
        // let student_list = self.roster.get_mut(&grade).unwrap();
        // student_list.push(student);
        // student_list.sort();
    }

    // using a BTreeMap instead of a HashMap makes sorting of grades superfluous
    pub fn grades(&self) -> Vec<u32> {
        self.roster.keys().cloned().collect()
    }

    // If `grade` returned a reference, `School` would be forced to keep a `Vec<String>`
    // internally to lend out. By returning an owned vector of owned `String`s instead,
    // the internal structure can be completely arbitrary. The tradeoff is that some data
    // must be copied each time `grade` is called.
    pub fn grade(&self, grade: u32) -> Vec<String> {
        // sorting has been done in add
        self.roster.get(&grade).unwrap_or(&Vec::<&'a str>::new())
            .iter().map(|&s| String::from(s)).collect()
        
        // match self.roster.get(&grade) {
        //     None => vec![],
        //     Some(v) => {
        //         let mut sorted: Vec<String> = v.iter().map(|&s| s.to_string()).collect();
        //         sorted.sort();
        //         sorted }
        // }

    }
}
