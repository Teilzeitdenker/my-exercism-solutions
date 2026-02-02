const PLANTS: [(char, &str); 4] = [('G', "grass"), ('C', "clover"), ('R', "radishes"), ('V', "violets")];

pub fn plants(diagram: &str, student: &str) -> Vec<&'static str> {
    let idx: usize = student.chars().next().unwrap() as usize - 'A' as usize;
    diagram.split('\n').flat_map(|row| row.chars().skip(2*idx).take(2).map(char_to_plant)).collect()
}

fn char_to_plant(c: char) -> &'static str {
    (PLANTS.iter().find(|p| p.0 == c).unwrap()).1
}
