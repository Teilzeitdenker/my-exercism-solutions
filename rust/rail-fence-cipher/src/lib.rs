pub struct RailFence(u32);

impl RailFence {
    pub fn new(rails: u32) -> RailFence {
        RailFence(rails)
    }

    pub fn encode(&self, text: &str) -> String {
        let rails = self.0 as usize;
        let char_height_iterator = text.chars().zip(RailFence::up_and_down(rails));
        let mut vecs_on_rails = vec![Vec::new(); rails];
        for (c, h) in char_height_iterator {
            vecs_on_rails[h].push(c);
        }
        vecs_on_rails.iter().flatten().collect()
    }

    pub fn decode(&self, cipher: &str) -> String {
        let mut sorted_by_rails_for_zipping = RailFence::up_and_down(self.0 as usize)
            .take(cipher.len())
            .zip(1..)
            .collect::<Vec<_>>();
        sorted_by_rails_for_zipping.sort();
        
        let mut stringpos_char = sorted_by_rails_for_zipping.iter()
            .zip(cipher.chars())
            .map(|((_,p),c)| (p, c))
            .collect::<Vec<_>>();
        stringpos_char.sort();
        
        stringpos_char
            .iter()
            .map(|(_,c)| c)
            .collect()
    }

    fn up_and_down(rails: usize) -> impl Iterator<Item = usize> {
        (0..rails - 1)
            .chain((1..rails).rev())
            .cycle()
    }
}
