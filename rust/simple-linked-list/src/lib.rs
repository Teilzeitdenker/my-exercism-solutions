use std::iter::FromIterator;

#[derive(Debug, Clone)]
struct Node<T> where T: Clone + Copy {
    data: T,
    next: Option<Box<Node<T>>>,
}

pub struct SimpleLinkedList<T> where T: Clone + Copy {
    head: Option<Box<Node<T>>>,
}

impl<T> SimpleLinkedList<T> where T: Clone + Copy {
    pub fn new() -> Self {
        SimpleLinkedList { head: None }
    }

    pub fn is_empty(&self) -> bool {
        self.head.is_none()
    }

    pub fn len(&self) -> usize {
        if self.is_empty() {return 0;}
        let mut res = 1;
        let mut actual_node = self.head.as_ref().unwrap().clone();
        while  actual_node.next.is_some() {
            res += 1;
            actual_node = actual_node.next.unwrap();
        }
        res
    }

    pub fn push(&mut self, element: T) where T: Clone {
        self.head = Some(Box::new(Node { data: element, next: self.head.clone() }))
    }

    pub fn pop(&mut self) -> Option<T> {
        if self.is_empty() {return None;}
        let data = self.head.as_ref().unwrap().data;
        self.head = self.head.clone().unwrap().next;
        Some(data)
    }

    pub fn peek(&self) -> Option<&T> {
        if self.is_empty() {return None;}
        Some(&self.head.as_ref().unwrap().data)
    }

    #[must_use]
    pub fn rev(self) -> SimpleLinkedList<T> {
        let mut as_vec = Vec::from(self);
        as_vec.reverse();
        SimpleLinkedList::from_iter(as_vec)
    }
}

impl<T> FromIterator<T> for SimpleLinkedList<T> where T: Clone + Copy {
    fn from_iter<I: IntoIterator<Item = T>>(iter: I) -> Self {
        let mut ls = SimpleLinkedList::new();
        iter.into_iter().for_each(|el| ls.push(el));
        ls
    }
}

impl<T> From<SimpleLinkedList<T>> for Vec<T> where T: Clone + Copy {
    fn from(mut linked_list: SimpleLinkedList<T>) -> Vec<T> {
        let mut res = Vec::new();
        while let Some(actual_el) = linked_list.pop() {
            res.push(actual_el);
        }
        res.reverse();
        res
    }
}
