pub fn get_diamond(c: char) -> Vec<String> {
    let upper_part_of_diamond = ('A'..=c)
        .map(|ch| get_row(ch, (c as usize) - ('A' as usize)));
    let lower_part_of_diamond = upper_part_of_diamond.clone().rev().skip(1);
    upper_part_of_diamond.chain(lower_part_of_diamond).collect()
}

fn get_row(c: char, n: usize) -> String {
    let mut right_part = vec![' '; n];
    right_part.push(c);
    right_part.swap(n, (c as usize) - ('A' as usize));
    let mut row = right_part.clone().iter().skip(1).rev().map(|ch| ch.clone()).collect::<Vec<_>>();
    row.extend(right_part);
    row.iter().collect()
}
