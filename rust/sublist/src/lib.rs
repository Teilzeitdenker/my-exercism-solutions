#[derive(Debug, PartialEq, Eq)]
pub enum Comparison {
    Equal,
    Sublist,
    Superlist,
    Unequal,
}

fn is_sublist<T: PartialEq>(first_list: &[T], second_list: &[T]) -> bool {
    if first_list.len() == 0 {return true;}
    second_list.windows(first_list.len()).any(|list| list == first_list)
}

pub fn sublist<T: PartialEq>(first_list: &[T], second_list: &[T]) -> Comparison {
    if first_list.len() == second_list.len() {
        if first_list == second_list {return Comparison::Equal;}
        else {return Comparison::Unequal;}
    }
    if first_list.len() < second_list.len() {
        if is_sublist(first_list, second_list) {return Comparison::Sublist;}
        else {return Comparison::Unequal;}
    }
    else {
        if is_sublist(second_list, first_list) {return Comparison::Superlist;}
        else {return Comparison::Unequal}
    }
}
