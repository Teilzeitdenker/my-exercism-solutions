pub struct CircularBuffer<T: Clone + Default> {
    capacity: usize,
    items: Vec<T>,
    reader: usize,
    writer: usize,
}

#[derive(Debug, PartialEq, Eq)]
pub enum Error {
    EmptyBuffer,
    FullBuffer,
}

impl<T> CircularBuffer<T>
    where T: Clone + Default {
    
    pub fn new(capacity: usize) -> Self {
        let items = vec![T::default(); capacity];
        CircularBuffer { 
            capacity,
            items,
            reader: 0,
            writer: 0,
        }
    }

    pub fn write(&mut self, element: T) -> Result<(), Error> {
        use Error::*;
        if self.writer - self.reader == self.capacity {Err(FullBuffer)} else {
            let write_idx = self.writer % self.capacity;
            self.writer += 1;
            self.items[write_idx] = element;
            Ok(())
        }
    }

    pub fn read(&mut self) -> Result<T, Error> {
        use Error::*;
        if self.reader >= self.writer {Err(EmptyBuffer)} else {
            let read_idx = self.reader % self.capacity;
            self.reader += 1;
            Ok(self.items[read_idx].clone())
        }
    }

    pub fn clear(&mut self) {
        self.items = vec![T::default();self.capacity];
        self.reader = 0;
        self.writer = 0;
    }

    pub fn overwrite(&mut self, element: T) {
        if self.writer - self.reader < self.capacity {self.write(element).unwrap()} else {
            let write_idx = self.writer % self.capacity;
            self.writer += 1;
            self.reader += 1;
            self.items[write_idx] = element;
        }
    }
}
