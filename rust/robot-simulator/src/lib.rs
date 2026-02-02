use std::ops::Add;

#[derive(PartialEq, Eq, Debug, Clone, Copy)]
pub enum Direction {
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

impl Direction {
    pub fn int_to_direction(n: u8) -> Self {
        use Direction::*;
        match n {
            0 => North,
            1 => East,
            2 => South,
            3 => West,
            _ => panic!("Invalid cast from u8 to direction")
        }
    }
}
#[derive(Clone, Copy)]
pub struct Position(i32, i32);
#[derive(Clone)]
pub struct Robot{
    position: Position,
    direction: Direction
}

impl Add<&Position> for Position {
    type Output = Self;
    fn add(self, rhs: &Self) -> Self {
        Self(self.0 + rhs.0, self.1 + rhs.1)
    }
}

impl Robot {
    pub fn new(x: i32, y: i32, d: Direction) -> Self {
        Robot {position: Position(x, y), direction: d}
    }

    #[must_use]
    pub fn turn_right(self) -> Self {
        let new_direction = Direction::int_to_direction((self.direction as u8 + 1) % 4);
        Robot { direction: new_direction , .. self }
    }

    #[must_use]
    pub fn turn_left(self) -> Self {
        let new_direction = Direction::int_to_direction((self.direction as u8 + 3) % 4);
        Robot { direction: new_direction , .. self }
    }

    #[must_use]
    pub fn advance(self) -> Self {
        let unit_indexed_by_direction: [Position; 4] = [Position(0, 1), Position(1, 0), Position(0, -1), Position(-1, 0)];
        Robot { position: self.position + &unit_indexed_by_direction[self.direction as usize] , .. self}
    }

    #[must_use]
    pub fn instructions(self, instructions: &str) -> Self {
        let mut robot = self.clone();
        for ch in instructions.chars() {
            match ch {
                'A' => robot = robot.advance(),
                'R' => robot = robot.turn_right(),
                'L' => robot = robot.turn_left(),
                _   => panic!("Invalid instruction!"),
            }
        }
        robot
    }

    pub fn position(&self) -> (i32, i32) {
        let Position(x, y) = self.position;
        (x, y)
    }

    pub fn direction(&self) -> &Direction {
        &self.direction
    }
}
