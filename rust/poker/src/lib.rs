use std::collections::HashSet; // could also use itertools and the unique()-function for my purposes
use std::collections::HashMap;

pub fn winning_hands<'a>(hands: &[&'a str]) -> Vec<&'a str> {
    let scored_deals: Vec<(usize, &str)> = hands.into_iter().map(|&h| (Deal::score(h), h)).collect();
    let max_score = scored_deals
        .iter()
        .map(|el| el.0)
        .max()
        .unwrap();
    scored_deals
        .iter()
        .filter(|(score, _)| *score == max_score)
        .map(|(_, deal)| *deal)
        .collect()
}

struct Deal {
    ranks: Vec<usize>,
    flush: bool,
    straight: bool,
}

impl<'a> Deal {
    const ACE_TO_5: [usize; 5] = [14, 5, 4, 3, 2]; // special case of a straight
    const FACTORS: [usize; 6] = [10_000_000_000, 100_000_000, 1_000_000, 10_000, 100, 1];

    pub fn score(deal: &'a str) -> usize {
        let (mut ranks, suits): (Vec<usize>, Vec<char>) = deal
            .split(' ')
            .map(|s| ("__234567891JQKA".char_indices().find(|(_, c )| s.chars().next().unwrap() == *c).unwrap().0, 
                    s.chars().last().unwrap()))
            .unzip();
        ranks.sort();
        ranks.reverse();
        let flush = suits.into_iter().collect::<HashSet<_>>().len() == 1; 
        let straight = (ranks.clone().into_iter().collect::<HashSet<_>>().len() == 5 && ranks[0] - ranks[4] == 4) 
                            || Deal::ACE_TO_5.into_iter().collect::<Vec<_>>() == ranks;
        let d = Deal { ranks, flush, straight };
        Deal::FACTORS.into_iter()
            .zip(d.get_level_and_crucial_ranks().into_iter())
            .map(|(fac, el)| fac * el)
            .sum()
    }

    fn get_level_and_crucial_ranks(&self) -> Vec<usize> {
        if self.ranks == Deal::ACE_TO_5.into_iter().collect::<Vec<_>>() {
            if self.flush { return vec![8, 5] } else { return vec![4, 5] }
        }
        let (ranks_by_freqs_desc, freqs): (Vec<usize>, Vec<usize>) = Deal::sorted_frequencies(&self.ranks).into_iter().unzip();
        match (self.flush, self.straight, freqs[0]) {
            (true, true, _)                  => vec![8, self.ranks[0]],                                                          // straight flush
            (_   , _   , 4)                  => {let mut res = vec![7]; res.extend(ranks_by_freqs_desc); res}, // 4 of a kind
            (_   , _   , 3) if freqs[1] == 2 => {let mut res = vec![6]; res.extend(ranks_by_freqs_desc); res}, // full house
            (true, _   , _)                  => {let mut res = vec![5]; res.extend(self.ranks.iter()); res},         // flush
            (_   , true, _)                  => vec![4, self.ranks[0]],                                                          // straight
            (_   , _   , 3)                  => {let mut res = vec![3]; res.extend(ranks_by_freqs_desc); res}, // 3 of a kind
            (_   , _   , 2) if freqs[1] == 2 => {let mut res = vec![2]; res.extend(ranks_by_freqs_desc); res}, // two pairs
            (_   , _   , 2)                  => {let mut res = vec![1]; res.extend(ranks_by_freqs_desc); res}, // one pair
            _                                => {let mut res = vec![0]; res.extend(self.ranks.iter()); res},         // high card 
        }
    }

    fn sorted_frequencies(x: &[usize]) -> Vec<(usize, usize)> {
        let mut map: HashMap<usize, usize> = HashMap::new();
        for item in x {
            map.entry(*item).and_modify(|counter| *counter += 1).or_insert(1);
        }
        let mut res: Vec<_> = map.into_iter().collect();
        res.sort_by_key(|(k, v)| (*v, *k)); // sort by freq-counters first and then by rank
        res.reverse(); // want the descending order of this
        res
    }
}