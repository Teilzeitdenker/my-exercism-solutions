pub fn collatz(n: u64) -> Option<u64> {
    let mut nc: u64 = n;
    match nc {
        0 => None,
        _ => {
            let mut steps: u64 = 0;
            while nc != 1 {
                steps += 1;
                match next_step(nc) {
                    Some(ncc) => {nc = ncc;},
                    None => return None
                }
            }
            Some(steps)
        }
    }
}

fn next_step(n: u64) -> Option<u64> {
    match n % 2 {
        0 => Some(n / 2),
        _ => match n.checked_mul(3_u64) {
            Some(x) => x.checked_add(1),
            _ => None
        }
    }
}
