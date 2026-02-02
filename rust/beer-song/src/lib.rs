use std::fmt::Write;

pub fn verse(n: u32) -> String {
    let mut result = String::new();
    if n > 2 {
        writeln!(result, "{} bottles of beer on the wall, {} bottles of beer.", n, n).unwrap();
        writeln!(result, "Take one down and pass it around, {} bottles of beer on the wall.", n-1).unwrap();
    }
    else if n == 2 {
        writeln!(result, "{} bottles of beer on the wall, {} bottles of beer.", n, n).unwrap();
        writeln!(result, "Take one down and pass it around, {} bottle of beer on the wall.", n-1).unwrap();
    }
    else if n == 1 {
        writeln!(result, "{} bottle of beer on the wall, {} bottle of beer.", n, n).unwrap();
        writeln!(result, "Take it down and pass it around, no more bottles of beer on the wall.").unwrap();
    } else {
        writeln!(result, "No more bottles of beer on the wall, no more bottles of beer.").unwrap();
        writeln!(result, "Go to the store and buy some more, 99 bottles of beer on the wall.").unwrap();
    }
    result
}

pub fn sing(start: u32, end: u32) -> String {
    let mut result = String::new();
    let mut count = start;
    while count >= end {
        result += &verse(count);
        if count != end {
            result += "\n";
        }
        if count == 0 {
            return result;
        }
        count -= 1;
    }
    result
}
