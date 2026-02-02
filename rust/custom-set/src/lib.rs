use std::{collections::HashSet, hash::Hash};

#[derive(Debug, PartialEq, Eq)]
pub struct CustomSet<T: PartialEq + Eq + Hash> {
    st: HashSet<T>,
}

impl<T> CustomSet<T>
where 
    T: PartialEq + Eq + Hash + Clone 
{
    pub fn new(input: &[T]) -> Self {
        Self { st: input.into_iter().cloned().collect() }
    }

    pub fn contains(&self, el: &T) -> bool {
        self.st.contains(el)
    }

    pub fn add(&mut self, el: T) {
        self.st.insert(el);
    }

    pub fn is_subset(&self, other: &Self) -> bool {
        self.st.is_subset(&other.st)
    }

    pub fn is_empty(&self) -> bool {
        self.st.is_empty()
    }

    pub fn is_disjoint(&self, other: &Self) -> bool {
        self.st.is_disjoint(&other.st)
    }

    #[must_use]
    pub fn intersection(&self, other: &Self) -> Self {
        Self { st: self.st.intersection(&other.st).cloned().collect() }
    }

    #[must_use]
    pub fn difference(&self, other: &Self) -> Self {
        Self { st: self.st.difference(&other.st).cloned().collect() }
    }

    #[must_use]
    pub fn union(&self, other: &Self) -> Self {
        Self { st: self.st.union(&other.st).cloned().collect() }
    }
}
