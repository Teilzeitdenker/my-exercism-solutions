use unicode_reverse::reverse_grapheme_clusters_in_place;
use unicode_segmentation::UnicodeSegmentation;

// with crate unicode_reverse (has only this one method)
pub fn reverse2(input: &str) -> String {
    let mut s = input.to_string();
    reverse_grapheme_clusters_in_place(&mut s);
    s
}
// slightly more involved with the unicode_segmentation crate
pub fn reverse(input: &str) -> String {
    input.graphemes(true).rev().collect()
}
  
