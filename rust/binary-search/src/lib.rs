pub fn find(array: &[i32], key: i32) -> Option<usize> {
    if array.len() == 0 { return None; }
    let middle: usize = array.len() / 2;
    if array[middle] == key { return Some(middle); }
    if array[middle] > key { find(&array[..middle], key) } 
    else { 
        if let Some(ind) = find(&array[(middle + 1)..], key) { 
            Some(ind + middle + 1) 
        } else { 
            None 
        }
    }
}
