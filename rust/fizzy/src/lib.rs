use std::ops::Rem;

pub struct Matcher<'a, T>(Box<dyn Fn(T) -> bool + 'a>, String);

impl<'a, T> Matcher<'a, T> {
    pub fn new<F, S>(pred: F, subs: S) -> Self 
        where 
            F: Fn(T) -> bool + 'a, 
            S: ToString
    { 
        Self(
            Box::new(pred), 
            subs.to_string()
        ) 
    }
}

pub struct Fizzy<'a, T>(Vec<Matcher<'a, T>>);

impl<'a, T> Fizzy<'a, T> 
    where 
        T: Copy + ToString + 'a
{
    pub fn new() -> Self { 
        Self(vec![]) 
    }

    #[must_use]
    pub fn add_matcher(mut self, matcher: Matcher<'a, T>) -> Self {
        self.0.push(matcher); 
        self
    }

    pub fn apply<I>(self, iter: I) -> impl 'a + Iterator<Item = String>
        where 
            I: Iterator<Item = T> + 'a
    {
        iter.map(move |i| {
            let line = self.0.iter()
                .filter(|m| (m.0)(i))
                .map(|m| m.1.clone())
                .collect::<String>();
            if line.is_empty() { 
                i.to_string() 
            } else { 
                line 
            }
        })
    }
}

pub fn fizz_buzz<'a, T>() -> Fizzy<'a, T>
    where 
        T: Copy + ToString + From<u8> + Rem<Output = T> + PartialEq + 'a
{
    Fizzy::new()
        .add_matcher(Matcher::new(|i| i % 3.into() == 0.into(), "fizz"))
        .add_matcher(Matcher::new(|i| i % 5.into() == 0.into(), "buzz"))
}