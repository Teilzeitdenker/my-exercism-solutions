use std::iter::FromIterator;

struct Node<T> {
    data: T,
    next: Option<Box<Node<T>>>,
}

impl<T> Node<T> {
    fn new(data: T, next: Option<Box<Node<T>>>) -> Self {
        Self {data, next}
    }
}

pub struct SimpleLinkedList<T> {
    head: Option<Box<Node<T>>>,
}

impl<T> SimpleLinkedList<T> {
    pub fn new() -> Self { Self { head: None } }

    pub fn is_empty(&self) -> bool { self.head.is_none() }

    pub fn len(&self) -> usize {
        let mut res = 0;
        let mut actual_node = &self.head;
        while let Some(node) = actual_node {
            res += 1;
            actual_node = &node.next;
        }
        res
    }

    pub fn push(&mut self, element: T) {
        self.head = Some(Box::new(Node::new(element, self.head.take())))
    }

    pub fn pop(&mut self) -> Option<T> {
        if self.is_empty() {return None;}
        let old_head = self.head.take().unwrap();
        self.head = old_head.next;
        Some(old_head.data)
    }

    pub fn peek(&self) -> Option<&T> {
        // if self.is_empty() {return None;}
        // Some(&self.head.as_ref().unwrap().data)
        self.head.as_ref().map(|head| &head.data)
    }

    #[must_use]
    pub fn rev(mut self) -> SimpleLinkedList<T> {
        let mut reversed = SimpleLinkedList::new();
        while let Some(content) = self.pop() {
            reversed.push(content);
        }
        reversed
    }
}

impl<T> FromIterator<T> for SimpleLinkedList<T> {
    fn from_iter<I: IntoIterator<Item = T>>(iter: I) -> Self {
        let mut ls = SimpleLinkedList::new();
        iter.into_iter().for_each(|el| ls.push(el));
        ls
    }
}

impl<T> From<SimpleLinkedList<T>> for Vec<T> {
    fn from(mut linked_list: SimpleLinkedList<T>) -> Vec<T> {
        let mut res = Vec::new();
        while let Some(actual_el) = linked_list.pop() {
            res.push(actual_el);
        }
        res.reverse();
        res
    }
}
