#[derive(Debug)]
pub struct Clock {
    minutes: i32,
    pub display: (i32, i32)
}

impl Clock {
    const DAY: i32 = 1440;
    pub fn new(hours: i32, minutes: i32) -> Self {
        let raw: i32 = hours * 60 + minutes;
        let good: i32 = (raw % Clock::DAY + Clock::DAY) % Clock::DAY;
        Clock { minutes: good, display: (good / 60, good % 60) }
    }

    pub fn add_minutes(&self, minutes: i32) -> Self {
        Clock::new(self.display.0, self.display.1 + minutes)
    }
    pub fn to_string(&self) -> String {
        format!("{:02}:{:02}", self.display.0, self.display.1)
    }
}

impl PartialEq for Clock {
    fn eq(&self, other: &Self) -> bool {
        self.display == other.display
    }
}
