// this solution is quite short, but doesn't solve the tests with --features io

// use std::borrow::Borrow;

// /// A munger which XORs a key with some data
// #[derive(Clone)]
// pub struct Xorcism<'a> {
//     key: &'a [u8],
//     offset: usize,
// }

// impl<'a> Xorcism<'a> {
//     /// Create a new Xorcism munger from a key
//     ///
//     /// Should accept anything which has a cheap conversion to a byte slice.
//     pub fn new<Key: AsRef<[u8]> + ?Sized>(key: &'a Key) -> Xorcism<'a> {
//         Self { key: key.as_ref(), offset: 0 }
//     }

//     /// XOR each byte of the input buffer with a byte from the key.
//     ///
//     /// Note that this is stateful: repeated calls are likely to produce different results,
//     /// even with identical inputs.
//     pub fn munge_in_place(&mut self, data: &mut [u8]) {
//         for byte in data.into_iter() {
//             *byte ^= self.key[self.offset];
//             self.offset = (self.offset + 1) % self.key.len();
//         }
//     }

//     /// XOR each byte of the data with a byte from the key.
//     ///
//     /// Note that this is stateful: repeated calls are likely to produce different results,
//     /// even with identical inputs.
//     ///
//     /// Should accept anything which has a cheap conversion to a byte iterator.
//     /// Shouldn't matter whether the byte iterator's values are owned or borrowed.
//     pub fn munge<'b, Data: IntoIterator<Item = impl Borrow<u8>> + 'b>(&'b mut self, data: Data) -> impl Iterator<Item = u8> + 'b {
//         data.into_iter().map(|byte| {
//             let ret = byte.borrow() ^ self.key[self.offset];
//             self.offset = (self.offset + 1) % self.key.len();
//             ret
//         })
//     }
// }


// solution by menb111 that also solves the feature tests for io, since it implements Read and Write

use std::borrow::Borrow;
use std::io::Read;
use std::io::Write;

#[derive(Clone)]
pub struct Xorcism<'a> { key: &'a [u8], rot: usize }

pub trait MungeOutput<'a>: Iterator<Item = u8> + ExactSizeIterator {}
impl<'a, T> MungeOutput<'a> for T where T: Iterator<Item = u8> + ExactSizeIterator {}

impl<'a> Xorcism<'a> {
    pub fn new<Key: ?Sized + AsRef<[u8]>>(key: &'a Key) -> Xorcism<'a>
    { Self { key: key.as_ref(), rot: 0 } }

    pub fn munge_in_place(&mut self, data: &mut [u8]) {
        data.iter_mut()
            .enumerate()
            .for_each(|(i, v)| *v ^= self.key[(self.rot + i) % self.key.len()]);
        self.rot = (self.rot + data.len()) % self.key.len();
    }

    pub fn munge<'b, Data, L>(&'b mut self, data: Data) -> impl MungeOutput<'a> + 'b
    where
        L: Borrow<u8>,
        Data: IntoIterator<Item = L>,
        <Data as IntoIterator>::IntoIter: ExactSizeIterator + 'b,
    {
        let data_iter = data.into_iter();
        let data_iter_len = data_iter.len();
        let key_len = self.key.len();
        let rot = self.rot;
        let key = &self.key;
        let res = data_iter
            .enumerate()
            .map(move |(i, v)| (v.borrow()) ^ key[(rot + i) % key_len]);
        self.rot = (rot + data_iter_len) % key_len;
        res
    }

    pub fn reader(self, r: impl Read + 'a) -> impl Read + 'a 
    { XorcismReader::<'a> { xorcism: self, reader: r } }

    pub fn writer(self, w: impl Write + 'a) -> impl Write + 'a 
    { XorcismWriter::<'a> { xorcism: self, writer: w } }
}

struct XorcismReader<'a, R: Read> { xorcism: Xorcism<'a>, reader: R }
struct XorcismWriter<'a, W: Write> { xorcism: Xorcism<'a>, writer: W }

impl<'a, R: std::io::Read> Read for XorcismReader<'a, R> {
    fn read(&mut self, buf: &mut [u8]) -> std::io::Result<usize> {
        let len = self.reader.read(buf)?;
        self.xorcism.munge_in_place(&mut buf[..len]);
        Ok(len)
    }
}

impl<'a, W: std::io::Write> Write for XorcismWriter<'a, W> {
    fn write(&mut self, buf: &[u8]) -> std::io::Result<usize> {
        self.writer.write(&self.xorcism.munge(buf).collect::<Vec<_>>())
    }

    fn flush(&mut self) -> std::io::Result<()> {
        self.writer.flush()
    }
}
