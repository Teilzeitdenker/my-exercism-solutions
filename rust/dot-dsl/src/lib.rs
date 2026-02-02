pub mod graph {
    use std::collections::HashMap;
    #[derive(Debug, Default)]
    pub struct Graph {
        pub attrs: HashMap<String, String>,
        pub nodes: Vec<graph_items::node::Node>,
        pub edges: Vec<graph_items::edge::Edge>,
    }
    impl Graph {
        pub fn new() -> Self { 
            Self::default() 
        }
        pub fn with_attrs(mut self, attrs: &[(&str, &str)]) -> Self {
            for (k, v) in attrs {
                self.attrs.insert(k.to_string(), v.to_string());
            }
            self
        }
        pub fn with_nodes(mut self, nodes: &[graph_items::node::Node]) -> Self  {
            self.nodes = nodes.to_vec(); self 
        }
        pub fn with_edges(mut self, edges: &[graph_items::edge::Edge]) -> Self {
            self.edges = edges.to_vec(); self
        }
        pub fn node(&self, name: &str) -> Option<graph_items::node::Node> {
            self.nodes.iter().find(|&n| n.name == name).cloned()
        }
        pub fn attr(&self, key: &str) -> Option<&str> {
            self.attrs.get(key).map(String::as_str)
        }
    }
    pub mod graph_items {
        pub mod node {
            use std::collections::HashMap;
            #[derive(Debug, PartialEq, Eq, Clone)]
            pub struct Node {
                pub name: String,
                pub attrs: HashMap<String, String>,
            }
            impl Node {
                pub fn new(name: &str) -> Self {
                    Self { name: name.to_string(), attrs: HashMap::new() }
                }
                pub fn with_attrs(mut self, attrs: &[(&str, &str)]) -> Self {
                    for (k, v) in attrs {
                        self.attrs.insert(k.to_string(), v.to_string());
                    }
                    self
                }
                pub fn attr(&self, key: &str) -> Option<&str> {
                    self.attrs.get(key).map(String::as_str)
                }
            }
        }
        pub mod edge {
            use std::collections::HashMap;
            #[derive(Debug, PartialEq, Eq, Clone)]
            pub struct Edge {
                pub node1: String,
                pub node2: String,
                pub attrs: HashMap<String, String>,
            }
            impl Edge {
                pub fn new(node1: &str, node2: &str) -> Self {
                    Self { node1: node1.to_string(), node2: node2.to_string(), attrs: HashMap::new() }
                }
                pub fn with_attrs(mut self, attrs: &[(&str, &str)]) -> Self {
                    for (k, v) in attrs {
                        self.attrs.insert(k.to_string(), v.to_string());
                    }
                    self
                }
                pub fn attr(&self, key: &str) -> Option<&str> {
                    self.attrs.get(key).map(String::as_str)
                }
            }
        }
    }
}