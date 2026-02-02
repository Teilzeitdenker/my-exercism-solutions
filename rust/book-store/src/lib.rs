use itertools::Itertools;

pub fn lowest_price(books: &[u32]) -> u32 {
    let book_counts = vec![1, 2, 3, 4, 5]
        .iter()
        .map(|b| books
            .iter()
            .filter(|&c| c.eq(b))
            .count())
        .sorted()
        .collect::<Vec<_>>();
    let fives = book_counts[0];
    let fours = book_counts[1] - book_counts[0];
    let threes = book_counts[2] - book_counts[1];
    let twos = book_counts[3] - book_counts[2];
    let ones = book_counts[4] - book_counts[3];
    let pairs_of_3_and_5 = fives.min(threes);
    (ones * 800 + 
        twos * 1_520 + 
        (threes - pairs_of_3_and_5) * 2_160 + 
        (fours + 2 * pairs_of_3_and_5) * 2_560 + 
        (fives - pairs_of_3_and_5) * 3_000) as u32
}
