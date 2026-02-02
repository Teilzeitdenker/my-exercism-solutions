pub fn factors(n: u64) -> Vec<u64> {
    if n <= 1 {
        return Vec::new();
    }
    let mut n_copy = n;
    let mut primes = Vec::new();
    while (n_copy % 2) == 0 {
        primes.push(2);
        n_copy /= 2;
    }
    loop {
        let factor = get_next_factor(n_copy); 
        if factor == 0 {
            break;
        }
        primes.push(factor);
        n_copy /= factor;
    }
    primes
}

fn get_next_factor(n: u64) -> u64 {
    if n < 3 {
        return 0;
    }
    for factor in (3..=n).step_by(2) {
        if n % factor == 0 {
            return factor;
        }
    }
    0
}