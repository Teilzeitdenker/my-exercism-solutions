use itertools::{izip, Itertools};
use lazy_static::lazy_static;
use std::collections::HashMap;

lazy_static! {
    static ref DIGITS_CHAR_MAP: HashMap<String, &'static str> = HashMap::from([
        (" _ \n".to_string() +
         "| |\n" +
         "|_|\n" +
         "   ", "0"), 
        ("   \n".to_string() +
         "  |\n" +
         "  |\n" +
         "   ", "1"),
        (" _ \n".to_string() +
         " _|\n" +
         "|_ \n" +
         "   ", "2"),
        (" _ \n".to_string() +
         " _|\n" +
         " _|\n" +
         "   ", "3"),
        ("   \n".to_string() +
         "|_|\n" +
         "  |\n" +
         "   ", "4"),
        (" _ \n".to_string() +
         "|_ \n" +
         " _|\n" +
         "   ", "5"),
        (" _ \n".to_string() +
         "|_ \n" +
         "|_|\n" +
         "   ", "6"),
        (" _ \n".to_string() +
         "  |\n" +
         "  |\n" +
         "   ", "7"),
        (" _ \n".to_string() +
         "|_|\n" +
         "|_|\n" +
         "   ", "8"),
        (" _ \n".to_string() +
         "|_|\n" +
         " _|\n" +
         "   ", "9")
    ]);
}

#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    InvalidRowCount(usize),
    InvalidColumnCount(usize),
}

pub fn convert(input: &str) -> Result<String, Error> {
    let row_count = input.split("\n").count();
    let column_count = input.split("\n").collect::<Vec<&str>>()[0].len();
    if row_count % 4 != 0 {
        Err(Error::InvalidRowCount(row_count))
    } else if column_count % 3 != 0 {
        Err(Error::InvalidColumnCount(column_count))
    } else {
        Ok(input
            .split("\n")
            .collect::<Vec<&str>>()
            .chunks(4)
            .map(|row| get_digits(row))
            .join(","))
    }
}

fn get_digits(row: &[&str]) -> String {
    izip!(row[0].as_bytes().chunks(3).map(|chunk| std::str::from_utf8(chunk).unwrap()), 
          row[1].as_bytes().chunks(3).map(|chunk| std::str::from_utf8(chunk).unwrap()), 
          row[2].as_bytes().chunks(3).map(|chunk| std::str::from_utf8(chunk).unwrap()), 
          row[3].as_bytes().chunks(3).map(|chunk| std::str::from_utf8(chunk).unwrap()))
    .map(|(a, b, c, d)| decode(a.to_string() + "\n" + b + "\n" + c + "\n" + d) )
    .join("")
}

fn decode(s: String) -> &'static str {
    if DIGITS_CHAR_MAP.keys().contains(&s) {
        DIGITS_CHAR_MAP[&s]
    } else {
        "?"
    }
}