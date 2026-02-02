use num_traits::Euclid;

#[derive(Debug, PartialEq)]
pub struct Clock {
    minutes: i32
}

impl Clock {
    pub fn new(hours: i32, minutes: i32) -> Self {
        let day: i32 = 1440;
        let raw: i32 = hours * 60 + minutes;
        Clock { minutes: Euclid::rem_euclid(&raw, &day) }
    }

    pub fn add_minutes(&self, minutes: i32) -> Self {
        Clock::new(0, self.minutes + minutes)
    }
    pub fn to_string(&self) -> String {
        format!("{:02}:{:02}", self.minutes / 60, self.minutes % 60)
    }
}

