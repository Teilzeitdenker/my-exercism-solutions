use itertools::Itertools; // using tuple_windows() in find_euler_path()

#[derive(Debug, Clone)]
struct DominoGraph {
    deg: [u8; 6], // degrees of nodes 
    edges: Vec<(u8, u8)>, 
}

impl DominoGraph {
    fn new(input: &[(u8, u8)]) -> DominoGraph { 
        let mut edges = vec![];
        let mut deg: [u8; 6] = [0; 6];
        input.iter().for_each(|(u, v)| {
            edges.push((*u, *v));
            let iu = *u as usize - 1; // diminish index value iu in deg-array by hand
            let iv = *v as usize - 1;
            deg[iu] += 1; 
            deg[iv] += 1;
        });
        DominoGraph { deg, edges }
    }
    // only if all degrees are even a closed Eulerian path is possible
    fn all_degrees_even(&self) -> bool {
        self.deg.iter().all(|d| d % 2 == 0)
    }
    
    fn find_euler_path(&self) -> Option<Vec<(u8, u8)>> {
        // special case
        if self.edges.is_empty() { return Some(vec![]); }
        // check precondition
        if !self.all_degrees_even() { return None; }
        // Hierholzer algorithm, res will hold the nodes in visiting order
        let mut visited: Vec<bool> = vec![false; self.edges.len()];
        let mut res: Vec<u8> = vec![];
        let mut stck: Vec<u8> = vec![self.edges[0].0]; // start with front node in first edge
        let mut v: u8;
        while !stck.is_empty() {
            v = *stck.last().unwrap();
            if let Some((idx, &ed)) = // search for a nonvisited matching edge in edges
                self.edges
                .iter()
                .enumerate()
                .find(|&(idx, ed)| !visited[idx] && (v == ed.0 || v == ed.1)) {
                    visited[idx] = true;
                    // push the unmatched number of the edge to the stack
                    if ed.0 == v { stck.push(ed.1); } else {stck.push(ed.0); } 
            } else {
                // fill the result vector and pop v from the stack
                res.push(v);
                stck.pop();
            }
        }
        // use the tuple_windows() function to get the edges back
        let mut path = vec![];
        for (&a, &b) in res.iter().tuple_windows() {
            path.push((a, b));
        }
        // check if the Euler path contains all input dominoes (i.e. the graph is connected)
        if path.len() == self.edges.len() { Some(path) } else { None }
    }
}

pub fn chain(input: &[(u8, u8)]) -> Option<Vec<(u8, u8)>> {
    DominoGraph::new(input).find_euler_path()
}
