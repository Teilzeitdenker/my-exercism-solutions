use winnow::{PResult, Parser};
use winnow::error::{ErrMode, ErrorKind, ParserError};
use winnow::ascii::digit1;
use winnow::combinator::{alt, opt, preceded, repeat_till};

#[derive(Debug)]
enum OperatorType { Add, Sub, Mul, Div }
#[derive(Debug)]
struct Operation { op: OperatorType, num: i32 }

fn parse_number(input: &mut &str) -> PResult<i32> {
    let _ = opt(' ').parse_next(input)?; // get rid of leading whitespace
    let sign = opt('-').parse_next(input)?; // parse optional minus sign
    digit1.parse_to().parse_next(input).map(|n: i32| if sign.is_none() { n } else { -n })
}

fn parse_start_and_first_num(input: &mut &str) -> PResult<i32> {
    preceded("What is", parse_number).parse_next(input)
}

fn parse_operation(input: &mut &str) -> PResult<Operation> {
    use OperatorType::*;
    let _ = opt(' ').parse_next(input)?;
    let op_type = alt(("plus","minus","multiplied by","divided by")).parse_next(input)?;
    let num = parse_number(input)?;
    match op_type {
        "plus"          => Ok(Operation { op: Add, num }),
        "minus"         => Ok(Operation { op: Sub, num }),
        "multiplied by" => Ok(Operation { op: Mul, num }),
        "divided by"    => Ok(Operation { op: Div, num }),
        _               => Err(ErrMode::from_error_kind(input, ErrorKind::Verify))
    }
}

fn parse_all_operations(input: &mut &str) -> PResult<(Vec<Operation>, char)> {
    repeat_till(0.., parse_operation, '?').parse_next(input)
}

fn full_parser(input: &mut &str) -> PResult<(i32, (Vec<Operation>, char))> {
    (parse_start_and_first_num, parse_all_operations).parse_next(input)
}

fn operate_on(a: i32, o: &Operation) -> i32 { // helper for the folding
    use OperatorType::*;
    match o.op {
        Add => a + o.num,
        Sub => a - o.num,
        Mul => a * o.num,
        Div => a / o.num,
    }
}

pub fn answer(command: &str) -> Option<i32> {
    let mut input = command.clone(); // parsing needs a mutable &str, so have to clone here
    if let Ok((start_number, (ops, _))) = full_parser.parse_next(&mut input) {
        Some(ops.iter().fold(start_number, |acc, op| operate_on(acc, op)))
    } else { None }
}