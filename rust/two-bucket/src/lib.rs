use itertools::Itertools;
use std::{collections::{VecDeque, HashSet}, hash::Hash};

#[derive(PartialEq, Eq, Debug)]
pub enum Bucket { One, Two }

#[derive(PartialEq, Eq, Debug)]
pub struct BucketStats { pub moves: u8, pub goal_bucket: Bucket, pub other_bucket: u8 }

impl BucketStats {
    fn search(goal: u8, state_queue: &mut VecDeque<State>, forbidden_fillings: &mut HashSet<Fillings>) -> Option<Self> {
        if let Some(state) = state_queue.pop_front() {
            let Fillings(a, b) = state.fillings;
            if a == goal || b == goal {
                Some(Self { 
                    moves: state.moves, 
                    goal_bucket:  if a == goal { Bucket::One } else { Bucket::Two }, 
                    other_bucket: if a == goal { b } else { a },
                })
            } else {
                let next_states = state
                    .apply_all_moves()
                    .filter(|st| !forbidden_fillings.contains(&st.fillings))
                    .collect::<Vec<_>>();
                for s in next_states {
                    forbidden_fillings.insert(s.fillings.clone());
                    state_queue.push_back(s);
                }
                Self::search(goal, state_queue, forbidden_fillings)
            }
        } else {
            None
        }
    }
}

#[derive(PartialEq, Eq, Debug, Clone, Hash)]
struct Fillings(u8, u8);

#[derive(PartialEq, Eq, Debug, Clone, Hash)]
struct State { fillings: Fillings, cap1: u8, cap2: u8, moves: u8 }

impl State {
    fn apply_all_moves(&self) -> impl Iterator<Item = State> + '_ {
        Move::VALUES
            .into_iter()
            .map(|m| Move::apply_move(&m, self))
            .unique()
    }
}

#[derive(PartialEq, Eq, Debug)]
enum Move { FillOne, FillTwo, EmptyOne, EmptyTwo, PourLeft, PourRight }

impl Move {
    const VALUES: [Self; 6] = 
    [Self::FillOne, Self::FillTwo, Self::EmptyOne, Self::EmptyTwo, Self::PourLeft, Self::PourRight];
    
    fn apply_move(&self, state: &State) -> State {    
        let Fillings(a, b) = state.fillings;
        let pour_lft = b.min(state.cap1 - a);
        let pour_rgt = a.min(state.cap2 - b);
        let fillings =  match self {
            Self::FillOne   => Fillings(state.cap1, b),
            Self::FillTwo   => Fillings(a, state.cap2),
            Self::EmptyOne  => Fillings(0, b),
            Self::EmptyTwo  => Fillings(a, 0),
            Self::PourLeft  => Fillings(a + pour_lft, b - pour_lft),
            Self::PourRight => Fillings(a - pour_rgt, b + pour_rgt),
        };
        let (cap1, cap2) = (state.cap1, state.cap2);
        let moves = state.moves + 1;
        State {fillings, cap1, cap2, moves}
    }
}

pub fn solve(capacity_1: u8, capacity_2: u8, goal: u8, start_bucket: &Bucket) -> Option<BucketStats> {
    let mut state_queue = VecDeque::new();
    let mut forbidden_fillings = HashSet::new();
    let bucket_one_filled = State { 
        fillings: Fillings(capacity_1, 0), 
        cap1: capacity_1, 
        cap2: capacity_2, 
        moves: 1 
    };
    let bucket_two_filled = State { 
        fillings: Fillings(0, capacity_2), 
        cap1: capacity_1, 
        cap2: capacity_2, 
        moves: 1 
    };
    if *start_bucket == Bucket::One {
        state_queue.push_back(bucket_one_filled);
        forbidden_fillings.insert(bucket_two_filled.fillings);
    } else {
        state_queue.push_back(bucket_two_filled);
        forbidden_fillings.insert(bucket_one_filled.fillings);
    }
    BucketStats::search(goal, &mut state_queue, &mut forbidden_fillings)
}
