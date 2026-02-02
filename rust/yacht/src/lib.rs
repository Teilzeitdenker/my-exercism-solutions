use std::collections::HashMap;

#[derive(Clone, Copy)]
pub enum Category {
    Ones           = 1,
    Twos           = 2,
    Threes         = 3,
    Fours          = 4,
    Fives          = 5,
    Sixes          = 6,
    FullHouse      = 7,
    FourOfAKind    = 8,
    LittleStraight = 9,
    BigStraight    = 10,
    Choice         = 11,
    Yacht          = 12,
}

type Dice = [u8; 5];
pub fn score(dice: Dice, category: Category) -> u8 {
    use Category::*;
    let cat: u8 = category as u8;
    let mut freqs: HashMap<u8, u8> = HashMap::new();
    for x in dice {
        *freqs.entry(x).or_default() += 1;
    }
    match category {
        Ones | Twos | Threes | Fours | Fives | Sixes  => *freqs.get(&cat).unwrap_or(&0) * cat,
        FullHouse      => if freqs.values().count() == 2 && freqs.values().any(|&n| n == 3) {dice.iter().sum()} else {0},
        FourOfAKind    => freqs.iter().map(|(&die, &freq)| if freq >= 4 {die * 4} else {0}).sum(),
        LittleStraight => if freqs.values().count() == 5 && !dice.contains(&6) {30} else {0},
        BigStraight    => if freqs.values().count() == 5 && !dice.contains(&1) {30} else {0},
        Choice         => dice.iter().sum(),
        Yacht          => if freqs.values().len() == 1 {50} else {0},
    }
}
