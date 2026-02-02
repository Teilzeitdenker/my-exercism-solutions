// This stub file contains items that aren't used yet; feel free to remove this module attribute
// to enable stricter warnings.
#![allow(unused)]

pub fn production_rate_per_hour(speed: u8) -> f64 {
    let mut successrate: f64 = 0.0;
    if (speed >= 1 && speed <= 4) {
        successrate = 1.0;
    }
    else if (speed >= 5 && speed <= 8) {
        successrate = 0.9;
    }
    else if (speed >= 9 && speed <= 10) {
        successrate = 0.77;
    }
    221.0*(speed as f64)*successrate
}

pub fn working_items_per_minute(speed: u8) -> u32 {
    (production_rate_per_hour(speed) / 60.0) as u32
}
