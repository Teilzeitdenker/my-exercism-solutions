use num_complex::Complex;

pub fn spiral_matrix(mut size: u32) -> Vec<Vec<u32>> {
    let mut matrix = vec![vec![0_u32; size as usize]; size as usize];
    let mut position = Complex::<i32>::new(0, -1);
    let mut direction = Complex::<i32>::new(0, 1);
    let mut num_spiral_parts_with_this_size: usize = 1;
    let mut counter: u32 = 1;
    while size > 0 {
        for _i in 0..size {
            position += direction;
            matrix[position.re as usize][position.im as usize] = counter;
            counter += 1;
        }
        direction *= - Complex::i();
        num_spiral_parts_with_this_size -= 1;
        if num_spiral_parts_with_this_size == 0 {
            num_spiral_parts_with_this_size = 2;
            size -= 1;
        }
    }
    matrix
}
