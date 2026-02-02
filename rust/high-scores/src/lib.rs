#[derive(Debug)]
pub struct HighScores {
    scores: Vec<u32>
}

impl HighScores {
    pub fn new(scores: &[u32]) -> Self {
        let mut v = Vec::new();
        v.extend_from_slice(scores);
        HighScores { scores: v }
    }

    pub fn scores(&self) -> &[u32] {
        &self.scores
    }

    pub fn latest(&self) -> Option<u32> {
        match self.scores.last() {
            None => None,
            Some(&x) => Some(x)
        }
    }

    pub fn personal_best(&self) -> Option<u32> {
        match self.scores.iter().max() {
            None => None,
            Some(&x) => Some(x)
        }
    }

    pub fn personal_top_three(&self) -> Vec<u32> {
        let mut score_copy = self.scores.clone();
        score_copy.sort_by(|a, b| b.cmp(a));
        score_copy.into_iter().take(3).collect()
    }
}
