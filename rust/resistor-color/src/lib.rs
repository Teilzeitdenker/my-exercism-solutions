use int_enum::IntEnum;
use enum_iterator::IntoEnumIterator;

#[repr(usize)]
#[derive(Clone, Copy, Debug, PartialEq, IntEnum, IntoEnumIterator)]
pub enum ResistorColor {
    Black = 0,
    Blue = 6,
    Brown = 1,
    Green = 5,
    Grey = 8,
    Orange = 3,
    Red = 2,
    Violet = 7,
    White = 9,
    Yellow = 4,
}

pub fn color_to_value(_color: ResistorColor) -> usize {
    _color.int_value()
}

pub fn value_to_color_string(value: usize) -> String {
    // extract the color with from_int() - function from the IntEnum trait (yields a Result<Ok(Enum), IntEnumErr>)
    // then use the debug trait to get a string with {:?}
    // check the upper bound and give an error message back when value is too large
    if value <= 9 {
        format!("{:?}", ResistorColor::from_int(value).unwrap())
    } else {
        String::from("value out of range")
    }
}

pub fn colors() -> Vec<ResistorColor> {
    let mut vec = Vec::new();
    for i in 0..10 {
        vec.push(ResistorColor::from_int(i).unwrap())
    }
    vec
}
