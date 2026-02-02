#[derive(Debug)]
pub enum CalculatorInput {
    Add,
    Subtract,
    Multiply,
    Divide,
    Value(i32),
}

fn get_last_two_el(st: &mut Vec<i32>) -> Option<(i32, i32)> {
    let el1: i32;
    let el2: i32;
    match (*st).pop() {
        None => return None,
        Some(v) => el1 = v,
    }
    match (*st).pop() {
        None => return None,
        Some(o) => el2 = o,
    }
    Some((el2, el1))
}

pub fn evaluate(inputs: &[CalculatorInput]) -> Option<i32> {
    let mut stack: Vec<i32>= Vec::new();
    let iter = inputs.iter();
    for el in iter {
        match el {
            CalculatorInput::Add => {
                match get_last_two_el(&mut stack) {
                    None => return None,
                    Some((v1,v2)) => stack.push(v1 + v2),
                }
            },
            CalculatorInput::Subtract => {
                match get_last_two_el(&mut stack) {
                    None => return None,
                    Some((v1,v2)) => stack.push(v1 - v2),
                }
            },
            CalculatorInput::Multiply => {
                match get_last_two_el(&mut stack) {
                    None => return None,
                    Some((v1,v2)) => stack.push(v1 * v2),
                }
            },
            CalculatorInput::Divide => {
                match get_last_two_el(&mut stack) {
                    None => return None,
                    Some((v1,v2)) => stack.push(v1 / v2),
                }
            },
            CalculatorInput::Value(v) => { stack.push(*v)},
        }
    }
    if stack.len() >= 2 || stack.is_empty() {
        return None;
    }
    Some(stack[0])
}
