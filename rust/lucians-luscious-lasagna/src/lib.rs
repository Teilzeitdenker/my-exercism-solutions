// This stub file contains items that aren't used yet; feel free to remove this module attribute
// to enable stricter warnings.
#![allow(unused)]

pub fn expected_minutes_in_oven() -> i32 {
    40
}

pub fn remaining_minutes_in_oven(actual: i32) -> i32 {
    40 - actual
}

pub fn preparation_time_in_minutes(layers: i32) -> i32 {
    2*layers
}

pub fn elapsed_time_in_minutes(layers: i32, actual: i32) -> i32 {
    preparation_time_in_minutes(layers) + actual
}
