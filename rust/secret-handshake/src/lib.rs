const COMMANDS: &'static [&'static str] = &["wink", "double blink", "close your eyes", "jump", "reverse it"];

pub fn actions(n: u8) -> Vec<&'static str> {
    let mut res = vec![];
    COMMANDS
        .iter()
        .enumerate()
        .filter(|(i, _)| (1 << i & n) > 0)
        .for_each(|(i, &s)| if i != 4 {res.push(s);} else {res.reverse()});
    // if 1 & n != 0 { res.push("wink"); }
    // if 2 & n != 0 { res.push("double blink"); }
    // if 4 & n != 0 { res.push("close your eyes"); }
    // if 8 & n != 0 { res.push("jump"); }
    // if 16 & n != 0 { res.reverse(); }
    res
}
