use std::collections::HashMap;

pub enum Category {
    Ones,
    Twos,
    Threes,
    Fours,
    Fives,
    Sixes,
    FullHouse,
    FourOfAKind,
    LittleStraight,
    BigStraight,
    Choice,
    Yacht,
}

type Dice = [u8; 5];
pub fn score(dice: Dice, category: Category) -> u8 {
    use Category::*;
    let mut freqs: HashMap<u8, u8> = HashMap::new();
    for x in dice {
        *freqs.entry(x).or_default() += 1;
    }
    match category {
        Ones           => *freqs.get(&1).unwrap_or(&0) * 1,
        Twos           => *freqs.get(&2).unwrap_or(&0) * 2,
        Threes         => *freqs.get(&3).unwrap_or(&0) * 3,
        Fours          => *freqs.get(&4).unwrap_or(&0) * 4,
        Fives          => *freqs.get(&5).unwrap_or(&0) * 5,
        Sixes          => *freqs.get(&6).unwrap_or(&0) * 6,
        FullHouse      => if freqs.values().count() == 2 && freqs.values().any(|&n| n == 3) {dice.iter().sum()} else {0},
        FourOfAKind    => freqs.iter().map(|(&die, &freq)| if freq >= 4 {die * 4} else {0}).sum(),
        LittleStraight => if freqs.values().count() == 5 && !dice.contains(&6) {30} else {0},
        BigStraight    => if freqs.values().count() == 5 && !dice.contains(&1) {30} else {0},
        Choice         => dice.iter().sum(),
        Yacht          => if freqs.values().len() == 1 {50} else {0},
    }
}
