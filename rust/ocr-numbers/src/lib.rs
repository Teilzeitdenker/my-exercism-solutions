use itertools::{izip, Itertools};
use lazy_static::lazy_static;
use std::collections::HashMap;

#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    InvalidRowCount(usize),
    InvalidColumnCount(usize),
}

pub fn convert(input: &str) -> Result<String, Error> {
    let input_vec = input.split("\n").collect::<Vec<&str>>();
    if input_vec.len() % 4 != 0 {
        Err(Error::InvalidRowCount(input_vec.len()))
    } else if input_vec[0].len() % 3 != 0 {
        Err(Error::InvalidColumnCount(input_vec[0].len()))
    } else {
        Ok(input_vec
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
    DIGITS_CHAR_MAP.get(&s).unwrap_or(&"?")
}

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
