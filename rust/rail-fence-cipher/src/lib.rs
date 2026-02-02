pub struct RailFence(u32);

impl RailFence {
    pub fn new(rails: u32) -> RailFence {
        RailFence(rails)
    }

    pub fn encode(&self, text: &str) -> String {
        let rails = self.0;
        let height_char_iterator = (1..rails)
            .chain((2..=rails).rev())
            .cycle()
            .take(text.len())
            .zip(text.chars());
        
        (1..=rails)
            .map(|n| {
                height_char_iterator
                    .clone()
                    .filter_map(|(s, c)| if s == n {Some(c)} else {None})
                    .collect::<String>()
            })
            .collect::<Vec<_>>()
            .join("")
    }

    pub fn decode(&self, cipher: &str) -> String {
        let rails = self.0;
        
        let place_height_iterator = (1..rails)
            .chain((2..=rails).rev())
            .cycle()
            .take(cipher.len())
            .enumerate();
        
        let mut sorted_for_zipping = place_height_iterator.collect::<Vec<_>>();
        sorted_for_zipping.sort_by_key(|(_, f)| *f);
        
        let mut pos_height_char = sorted_for_zipping.iter().zip(cipher.chars()).collect::<Vec<_>>();
        pos_height_char.sort_by_key(|((p, _), _)| *p);
        
        pos_height_char
            .iter()
            .map(|(_,c)| c)
            .collect()
    }
}
