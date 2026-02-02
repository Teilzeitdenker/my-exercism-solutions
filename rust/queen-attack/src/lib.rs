#[derive(Debug)]
pub struct ChessPosition {
    rank: i32, 
    file: i32 
}

#[derive(Debug)]
pub struct Queen {
    position: ChessPosition
}

impl ChessPosition {
    pub fn new(rank: i32, file: i32) -> Option<Self> {
        match (ChessPosition::in_range(rank), ChessPosition::in_range(file)) {
            (true, true) => Some(ChessPosition { rank, file }),
            _ => None,
        }
    }
    fn in_range(n: i32) -> bool {
        n >= 0 && n <= 7 
    }
}

impl Queen {
    pub fn new(position: ChessPosition) -> Self {
        Queen { position }
    }

    pub fn can_attack(&self, other: &Queen) -> bool {
        self.position.rank == other.position.rank 
        || self.position.file == other.position.file
        || (self.position.rank - other.position.rank).abs() == (self.position.file - other.position.file).abs()
    }
}
