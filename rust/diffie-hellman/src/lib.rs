use rand::{thread_rng, Rng};

pub fn mod_exp(base: u64, exponent: u64, modulus: u64) -> u64 {
    let mut result = 1 as u128;
    let mut base = (base % modulus) as u128;
    let mut exponent = exponent as u128;
    let modulus = modulus as u128;
    loop {
        if exponent <= 0 {
            break;
        }

        if exponent % 2 == 1 {
            result = (result * base) % modulus;
        }

        exponent = exponent >> 1;
        base = (base * base) % modulus;
    }
    result as u64
}


pub fn private_key(p: u64) -> u64 {
    let mut rng = thread_rng();
    rng.gen_range(2, p)
}

pub fn public_key(p: u64, g: u64, a: u64) -> u64 {
    // A/B = g**a mod p
    mod_exp(g, a, p)
}

pub fn secret(p: u64, b_pub: u64, a: u64) -> u64 {
    // s = B**a mod p
    mod_exp(b_pub, a, p)
}
