use anyhow::Error;
use std::io::{BufReader, BufRead};
use std::fs::File;
use regex::RegexBuilder;

#[derive(Debug)]
pub struct Flags {
    line_numbers: bool,
    only_matching_filenames: bool,
    case_insensitive: bool,
    invert_match: bool,
    entire_lines: bool,
}

impl Flags {
    pub fn new(flags: &[&str]) -> Self {
        Flags { 
            line_numbers: flags.contains(&"-n"), 
            only_matching_filenames: flags.contains(&"-l"),
            case_insensitive: flags.contains(&"-i"), 
            invert_match: flags.contains(&"-v"), 
            entire_lines: flags.contains(&"-x") }
    }
}

pub fn grep(pattern: &str, flags: &Flags, files: &[&str]) -> Result<Vec<String>, Error> {
    let pattern_str = if flags.entire_lines {
        "^".to_string() + pattern + "$" 
    } else {
        pattern.to_string()
    };
    let multiple_files = files.len() > 1;
    let re = RegexBuilder::new(&pattern_str).case_insensitive(flags.case_insensitive).build()?;
    let mut results: Vec<String> = Vec::new();
    let print = |fname: &str, line_num: usize, val: &str| {
        format!("{}{}{}", if multiple_files {fname.to_string() + ":"} else {"".to_string()}, if flags.line_numbers {line_num.to_string() + ":"} else {"".to_string()}, val)
    };
    for &filename in files {
        let file = BufReader::new(File::open(&filename)?);
        for (line_number, line_result) in file.lines().enumerate() {
            let line = line_result?;
            if re.is_match(&line) ^ flags.invert_match {
                if flags.only_matching_filenames {results.push(filename.to_string()); break;}
                else {
                    results.push(print(filename, line_number + 1, &line));
                }
            }
        }
    }
    Ok(results)
}
