/// What should the type of _function be?
pub fn map<S, F, T>(input: Vec<S>, mut func: F) -> Vec<T>
    where F: FnMut(S) -> T {
    let mut res = Vec::new();
        for item in input {
            res.push(func(item));
        }
        res
}
