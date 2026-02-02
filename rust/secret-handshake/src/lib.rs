pub fn actions(n: u8) -> Vec<&'static str> {
    let mut res = vec![];
    if 1 & n == 1 { res.push("wink"); }
    if 2 & n == 2 { res.push("double blink"); }
    if 4 & n == 4 { res.push("close your eyes"); }
    if 8 & n == 8 { res.push("jump"); }
    if 16 & n == 16 { res.reverse(); }
    res
}
