#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    NotEnoughPinsLeft,
    GameComplete,
}

pub struct BowlingGame {
    score: u16,
    frames: u16,
    pins_left: u16, 
    roll_count: u16,
    bonus1: bool,
    bonus2: bool
}

impl BowlingGame {
    pub fn new() -> Self {
        BowlingGame { score: 0, frames: 0, pins_left: 10, roll_count: 0, bonus1: false, bonus2: false }
    }

    pub fn roll(&mut self, pins: u16) -> Result<(), Error> {
        if self.is_complete() { return Err(Error::GameComplete); }
        if pins > self.pins_left { return Err(Error::NotEnoughPinsLeft); }
        self.pins_left -= pins;
        // The crucial difference between the additional rolls in the last frame and the ones before is that they are ONLY counted as bonus
        self.score += pins * (
            (if self.is_last_frame() {0} else {1}) + 
            (if self.bonus1 {1} else {0}) + 
            (if self.bonus2 {1} else {0})
        );
        // new bonus situation for the next roll
        self.bonus1 = self.bonus2;
        self.bonus2 = false;

        self.roll_count += 1;        
        if self.roll_count < 2 && self.pins_left > 0 { return Ok(()); } // try again to get a spare
        
        if self.pins_left == 0 && !self.is_last_frame() { // don't get bonus for additional rolls in last frame
            if self.roll_count == 1 { self.bonus2 = true; } // strike
            else { self.bonus1 = true; } // spare
        }
        // if roll_count is 2 or no pins left, reset and increment the frames
        self.roll_count = 0;
        self.pins_left = 10;
        self.frames += 1;
        Ok(())
    }

    pub fn score(&self) -> Option<u16> {
        if !self.is_complete() { None }
        else { Some(self.score) } 
    }

    fn is_last_frame(&self) -> bool {
        self.frames >= 10
    }

    fn is_complete(&self) -> bool {
        self.is_last_frame() && !self.bonus1 && ! self.bonus2
    }
}
