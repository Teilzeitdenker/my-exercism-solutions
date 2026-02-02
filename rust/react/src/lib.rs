/// `InputCellId` is a unique identifier for an input cell.
#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub struct InputCellId(usize);
/// `ComputeCellId` is a unique identifier for a compute cell.
/// Values of type `InputCellId` and `ComputeCellId` should not be mutually assignable,
/// demonstrated by the following tests:
///
/// ```compile_fail
/// let mut r = react::Reactor::new();
/// let input: react::ComputeCellId = r.create_input(111);
/// ```
///
/// ```compile_fail
/// let mut r = react::Reactor::new();
/// let input = r.create_input(111);
/// let compute: react::InputCellId = r.create_compute(&[react::CellId::Input(input)], |_| 222).unwrap();
/// ```
#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub struct ComputeCellId(usize);
#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub struct CallbackId(usize);

#[derive(Clone, Copy, Debug, PartialEq, Eq)]
pub enum CellId {
    Input(InputCellId),
    Compute(ComputeCellId),
}

impl CellId {
    fn get_id(&self) -> usize {
        match self {
            CellId::Input(id) => (*id).0,
            CellId::Compute(id) => (*id).0,
        }
    }
}

#[derive(Debug, PartialEq, Eq)]
pub enum RemoveCallbackError {
    NonexistentCell,
    NonexistentCallback,
}

pub struct Reactor<'a, T> {
    input_cells: Vec<T>,
    compute_cells: Vec<(fn(&[T]) -> T, usize, Vec<CellId>)>,
    callbacks: Vec<(usize, T, Box<dyn 'a + Fn(T)>, usize)>  
}

// You are guaranteed that Reactor will only be tested against types that are Copy + PartialEq.
impl<'a, T: Copy + PartialEq> Reactor<'a, T> {
    pub fn new() -> Self {
        Self { input_cells: Vec::new(), compute_cells: Vec::new(), callbacks: Vec::new() }
    }

    // Creates an input cell with the specified initial value, returning its ID.
    pub fn create_input(&mut self, initial: T) -> InputCellId {
        self.input_cells.push(initial);
        InputCellId(self.input_cells.len() - 1)
    }

    // Creates a compute cell with the specified dependencies and compute function.
    // The compute function is expected to take in its arguments in the same order as specified in
    // `dependencies`.
    // You do not need to reject compute functions that expect more arguments than there are
    // dependencies (how would you check for this, anyway?).
    //
    // If any dependency doesn't exist, returns an Err with that nonexistent dependency.
    // (If multiple dependencies do not exist, exactly which one is returned is not defined and
    // will not be tested)
    //
    // Notice that there is no way to *remove* a cell.
    // This means that you may assume, without checking, that if the dependencies exist at creation
    // time they will continue to exist as long as the Reactor exists.
    pub fn create_compute<>(
        &mut self,
        dependencies: &[CellId],
        compute_func: fn(&[T]) -> T,
    ) -> Result<ComputeCellId, CellId> {
        let mut deps = Vec::with_capacity(dependencies.len());
        for cell_id in dependencies {
            match self.input_cells.get(cell_id.get_id()) {
                Some(x) => deps.push(*x),
                None => return Err(*cell_id),
            }
        }
        self.input_cells.push(compute_func(&deps));
        self.compute_cells.push((compute_func, self.input_cells.len() - 1, Vec::from(dependencies)));
        Ok(ComputeCellId(self.input_cells.len() - 1))
    }

    // Retrieves the current value of the cell, or None if the cell does not exist.
    //
    // You may wonder whether it is possible to implement `get(&self, id: CellId) -> Option<&Cell>`
    // and have a `value(&self)` method on `Cell`.
    //
    // It turns out this introduces a significant amount of extra complexity to this exercise.
    // We chose not to cover this here, since this exercise is probably enough work as-is.
    pub fn value(&self, id: CellId) -> Option<T> {
        self.input_cells.get(id.get_id()).copied()
    }

    // Sets the value of the specified input cell.
    //
    // Returns false if the cell does not exist.
    //
    // Similarly, you may wonder about `get_mut(&mut self, id: CellId) -> Option<&mut Cell>`, with
    // a `set_value(&mut self, new_value: T)` method on `Cell`.
    //
    // As before, that turned out to add too much extra complexity.
    pub fn set_value(&mut self, id: InputCellId, new_value: T) -> bool {
        match self.input_cells.get_mut(id.0) {
            None => return false,
            Some(value) => *value = new_value,
        }
        for compute in &self.compute_cells {
            let deps = compute.2.iter().map(|cell| self.input_cells[cell.get_id()]).collect::<Vec<_>>();
            self.input_cells[compute.1] = (compute.0)(&deps);
        }
        for cb in self.callbacks.iter_mut() {
            if self.input_cells[cb.0] != cb.1 {
                (cb.2)(self.input_cells[cb.0]);
            }
        }
        true
    }

    // Adds a callback to the specified compute cell.
    //
    // Returns the ID of the just-added callback, or None if the cell doesn't exist.
    //
    // Callbacks on input cells will not be tested.
    //
    // The semantics of callbacks (as will be tested):
    // For a single set_value call, each compute cell's callbacks should each be called:
    // * Zero times if the compute cell's value did not change as a result of the set_value call.
    // * Exactly once if the compute cell's value changed as a result of the set_value call.
    //   The value passed to the callback should be the final value of the compute cell after the
    //   set_value call.
    pub fn add_callback<F: Fn(T) + 'a>(
        &mut self,
        id: ComputeCellId,
        cb: F,
    ) -> Option<CallbackId> {
        if self.input_cells.get(id.0) == None { return None; }
        self.callbacks.push((id.0, self.input_cells[id.0], Box::new(cb), self.callbacks.len()));
        Some(CallbackId(self.callbacks.len() - 1))
    }

    // Removes the specified callback, using an ID returned from add_callback.
    //
    // Returns an Err if either the cell or callback does not exist.
    //
    // A removed callback should no longer be called.
    pub fn remove_callback(
        &mut self,
        cell: ComputeCellId,
        callback: CallbackId,
    ) -> Result<(), RemoveCallbackError> {
        if self.input_cells.get(cell.0) == None { 
            return Err(RemoveCallbackError::NonexistentCell);
        }
        if self.callbacks.iter().any(|x| x.3 == callback.0) == false {
            return Err(RemoveCallbackError::NonexistentCallback);
        }
        for idx in (0..self.callbacks.len()).rev() {
            if self.callbacks[idx].3 == callback.0 {
                let _res = self.callbacks.remove(idx);
            }
        }
        Ok(())
    }
}
