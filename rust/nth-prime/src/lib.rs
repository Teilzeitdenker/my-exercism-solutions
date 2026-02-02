pub fn nth(n: usize) -> usize {
    let siz = 105_000;
    let sieve = get_sieve(siz);
    let primes = get_primes(siz, &sieve);
    primes[n]
}

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

fn is_prime(x: usize, sieve: &Vec<bool>) -> bool {
    if x <= 1 {return false;}
    if (x & 1) == 0 {return x == 2;}
    sieve[x >> 1]
}

fn get_primes(siz: usize, sieve: &Vec<bool>) -> Vec<usize> {
    let mut primes: Vec<usize> = vec![2];
    let mut i: usize = 3;
    while i < siz {
        if is_prime(i, sieve) {
            primes.push(i);
        }
        i += 2;
    }
    primes
}