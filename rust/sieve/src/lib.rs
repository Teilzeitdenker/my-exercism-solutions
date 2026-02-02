fn get_sieve(siz: usize) -> Vec<bool> {
    let half: usize = (siz >> 1) + 1;
    let mut sieve: Vec<bool> = Vec::with_capacity(half);
    sieve.resize(half, true);
    let sqr = ((half as f64)/2.0).sqrt() as usize;
    for i in 1..=sqr {
        if sieve[i] {
            let mut current: usize = ((2*i+1)*(2*i+1)-1)/2;
            while current < half {
                sieve[current] = false;
                current += 2*i + 1;
            }
        }
    }
    sieve
}
// only used for numbers bigger than 3 below
fn is_prime(x: usize, sieve: &Vec<bool>) -> bool {
    // if x <= 1 {return false;}
    // if (x & 1) == 0 {return x == 2;}
    sieve[x >> 1]
}

pub fn primes_up_to(upper_bound: usize) -> Vec<usize> {
    if upper_bound < 2 {
        return vec![];
    }
    let sieve = get_sieve(upper_bound);
    let mut primes: Vec<usize> = vec![2];
    let mut i: usize = 3;
    while i <= upper_bound {
        if is_prime(i, &sieve) {
            primes.push(i);
        }
        i += 2;
    }
    primes
}