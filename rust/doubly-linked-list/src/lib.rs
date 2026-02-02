mod pre_implemented;
// for most of the used tools
// see https://rust-unofficial.github.io/too-many-lists/index.html
// didn't implement the cursor like there (which would be circular with a ghost element "between" front and back)

// to get covariance for raw pointers use the wrapper NonNull around  *const,
// see https://rust-unofficial.github.io/too-many-lists/sixth-variance.html

use std::ptr::NonNull;

type Link<T> = Option<NonNull<Node<T>>>;

struct Node<T> {
    next: Link<T>,
    prev: Link<T>,
    elem: T,
}

impl<T> Node<T> {
    // directly get the pointer to a newly allocated Node<T>
    fn new(elem: T) -> NonNull<Node<T>> {
        let new_node = Node { next: None, prev: None, elem };
        unsafe { NonNull::new_unchecked(Box::into_raw(Box::new(new_node))) }
    }
}

pub struct LinkedList<T> {
    front: Link<T>,
    back: Link<T>,
    len: usize,
}

impl<T> Drop for LinkedList<T> {
    fn drop(&mut self) {
        let mut cur = self.front;
        while let Some(node) = cur {
            let node = unsafe { Box::from_raw(node.as_ptr()) };
            cur = node.next;
            // Box gets implicitly freed here 
        }
    }
}

unsafe impl<T: Send> Send for LinkedList<T> {}
unsafe impl<T: Sync> Sync for LinkedList<T> {}

impl<T> LinkedList<T> {
    pub fn new() -> Self { Self { front: None, back: None, len: 0 } }
    pub fn is_empty(&self) -> bool { self.len == 0 }
    pub fn len(&self) -> usize { self.len }
    pub fn cursor_front(&mut self) -> Cursor<'_, T> { Cursor { cur: self.front, list: self } }
    pub fn cursor_back(&mut self) -> Cursor<'_, T> { Cursor { cur: self.back, list: self } }
    pub fn iter(&self) -> Iter<'_, T> { Iter { next: &self.front } }
}

pub struct Iter<'a, T> {
    next: &'a Link<T>,
}

impl<'a, T> Iterator for Iter<'a, T> {
    type Item = &'a T;
    fn next(&mut self) -> Option<Self::Item> {
        self.next.map(|next| unsafe {
            let node = &*next.as_ptr();
            self.next = &node.next;
            &node.elem
        })
    }
}

pub struct Cursor<'a, T> {
    cur: Link<T>, 
    list: &'a mut LinkedList<T>,
}

impl<T> Cursor<'_, T> {
    pub fn peek_mut(&mut self) -> Option<&mut T> {
        self.cur.map(|node| unsafe { &mut (*node.as_ptr()).elem })
    }
    pub fn next(&mut self) -> Option<&mut T> {
        self.cur.and_then(|node| {
            self.cur = unsafe { node.as_ref().next };
            self.peek_mut()
        })
    }
    pub fn prev(&mut self) -> Option<&mut T> {
        self.cur.and_then(|node| {
            self.cur = unsafe { node.as_ref().prev };
            self.peek_mut()
        })
    }
    pub fn take(&mut self) -> Option<T> {
        self.cur.map(|mut node| unsafe {
            // get the previous and next node of the cursor node
            let (prev, next) = (node.as_mut().prev, node.as_mut().next);
            // if there is no next node (end of the list), then set the cursor node to the previous
            self.cur = next.or(prev);
            self.list.len -= 1;
            match prev {
                // if there is a previous node, set its next node to the one after the cursor node
                Some(mut prev) => prev.as_mut().next = next,
                // otherwise we remove the front of the list and have to set it again
                None => self.list.front = next,
            }
            match next {
                // same logic as above reversed
                Some(mut next) => next.as_mut().prev = prev,
                None => self.list.back = prev,
            }
            // return the elem
            Box::from_raw(node.as_ptr()).elem
            // Box is dropped here implicitly
        })
    }
    // small helper function (not public)
    fn insert_new_node_in_empty_list(&mut self, node: NonNull<Node<T>>) {
        self.list.front = Some(node);
        self.list.back = Some(node);
        self.cur = Some(node);
    }
    pub fn insert_after(&mut self, element: T) {
        let mut new_node = Node::new(element);
        match self.cur {
            // if the cursor points to a node
            Some(mut cur) => unsafe {
                // set it as the previous node of the new_node
                new_node.as_mut().prev = Some(cur);
                match cur.as_mut().next {
                    // if the cursor node has a next node
                    Some(mut next) =>  {
                        // set it as the next node of the new_node
                        new_node.as_mut().next = cur.as_ref().next;
                        // and set the new_node as the previous node of it
                        next.as_mut().prev = Some(new_node);
                    },
                    // otherwise the new_node is the back of the list
                    None => self.list.back = Some(new_node),
                }
                // in any case, set the new_node as the next node of the cursor node
                cur.as_mut().next = Some(new_node);
            },
            // otherwise the list was empty
            None => self.insert_new_node_in_empty_list(new_node),
        }
        self.list.len += 1;
    }
    pub fn insert_before(&mut self, element: T) {
        // same logic as insert_after
        let mut new_node = Node::new(element);
        match self.cur {
            Some(mut cur) => unsafe {
                new_node.as_mut().next = Some(cur);
                match cur.as_mut().prev {
                    Some(mut prev) =>  {
                        new_node.as_mut().prev = cur.as_ref().prev;
                        prev.as_mut().next = Some(new_node);
                    },
                    None => self.list.front = Some(new_node),
                }
                cur.as_mut().prev = Some(new_node);
            },
            None => self.insert_new_node_in_empty_list(new_node),
        }
        self.list.len += 1;
    }
}