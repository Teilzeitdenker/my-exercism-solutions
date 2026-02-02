use std::str::FromStr;
use std::io::{Error, ErrorKind};
use std::vec;
use std::fmt;

#[derive(Clone, Copy, PartialEq)]
enum MatchResult {
    Win = 3,
    Draw = 1,
    Loss = 0,
}
impl MatchResult {
    fn inverse(self) -> Self {
        use MatchResult::*;
        match self {
            Win  => Loss,
            Loss => Win,
            Draw => Draw,
        }
    }
}
// impl fmt::Display for MatchResult {
//     fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
//         match self {
//             MatchResult::Win => write!(f, "Team1 won the match!"),
//             MatchResult::Draw => write!(f, "Match ended in a tie!"),
//             MatchResult::Loss => write!(f, "Team2 won the match!"),
//         }
//     }
// }
impl FromStr for MatchResult {
    type Err = Error;
    fn from_str(s: &str) -> Result<MatchResult, Error> {
        if s == "win" {
            Ok(MatchResult::Win)
        } else if s == "draw" {
            Ok(MatchResult::Draw)
        } else if s == "loss" {
            Ok(MatchResult::Loss)
        } else {
            Err(Error::new(ErrorKind::InvalidInput, "error when parsing the match result"))
        }
    }
}
#[derive(Clone)]
struct Match {
    team1: String,
    team2: String,
    match_result: MatchResult,
}
impl Match {
    fn new(match_result: &str) -> Option<Self> {
        let mut split_array = match_result.split(";");
        let team1 = split_array.next()?.to_string();
        let team2 = split_array.next()?.to_string();
        let match_result_slice = split_array.next()?;
        let match_result = MatchResult::from_str(match_result_slice).ok()?;
        Some(Match { team1, team2, match_result })    
    }
}
// impl fmt::Display for Match {
//     fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
//         write!(f, "Match with id: {}, team1: {}, team2: {}, match_result: {}", self.id, self.team1, self.team2, self.match_result)
//     }
// }
struct Team {
    name: String,
    matches: Vec<Match>,
}
impl Team {
    fn new(name: &str, first_match: Match) -> Team {
        Team { name: name.to_string(), matches: vec![first_match] }
    }
    fn add_match(&mut self, next_match: Match) {
        self.matches.push(next_match);
    }
    fn get_mp_w_d_l_p(&self) -> [usize; 5] {
        let point_array: Vec<usize> = self.matches.iter().map(|m| {
            if m.team1 == self.name {m.match_result as usize} 
            else {m.match_result.inverse() as usize}
        }).collect();
        [
            point_array.len(),
            point_array.iter().filter(|&&n| n == 3).count(),
            point_array.iter().filter(|&&n| n == 1).count(),
            point_array.iter().filter(|&&n| n == 0).count(),
            point_array.iter().sum()
        ]
    }
}
impl fmt::Display for Team {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        let nums = self.get_mp_w_d_l_p();
        write!(f, "{:<30} | {:>2} | {:>2} | {:>2} | {:>2} | {:>2}", 
            self.name, nums[0], nums[1], nums[2], nums[3], nums[4])
    }
}
struct Tournament {
    teams: Vec<Team>,
}
impl Tournament {
    fn new() -> Self {
        Tournament { teams: vec![] }
    }
    fn add(&mut self, team: Team) {
        self.teams.push(team);
    }
    fn sort(&mut self) {
        // sort descending by points and then ascending by name
        self.teams.sort_by_key(|t| (-(t.get_mp_w_d_l_p()[4] as i32), t.name.clone()));
    }
}

pub fn tally(match_results: &str) -> String {
    let mut result_string = format!("{:<30} | {:>2} | {:>2} | {:>2} | {:>2} | {:>2}", "Team", "MP", "W", "D", "L", "P");
    if match_results == "" {
        return result_string;
    }
    let mut tournament = Tournament::new();
    for match_result in match_results.split('\n') {
        let new_match = Match::new(match_result).unwrap();
        for team_name in [&new_match.team1, &new_match.team2] {
            match tournament.teams.iter_mut().find(|t| t.name == *team_name) {
                Some(team) => {
                    team.add_match(new_match.clone()); },
                None              => {
                    let new_team = Team::new(&team_name, new_match.clone());
                    tournament.add(new_team);
                }
            }
        }
    }
    tournament.sort();
    for team in tournament.teams {
        result_string += &format!("\n{}", team);
    }
    result_string
}
