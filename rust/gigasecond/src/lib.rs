use time::{Duration};
use time::macros::{date, time};
use time::PrimitiveDateTime as DateTime;

// Returns a DateTime one billion seconds after start.
pub fn after(start: DateTime) -> DateTime {
    let gigsec = Duration::new(1_000_000_000, 0);
    match start.checked_add(gigsec) {
        Some(d) => d,
        None => DateTime::new(date!(0001-01-01), time!(0:00))
    }
}
