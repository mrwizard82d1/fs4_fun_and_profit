// ----- Don't use parentheses when calling a function

let add x y  = x + y

// wrong - this value is not a function and cannot be applied
//let result = add (1 2)

// correct
let result = add 1 2

// ----- Don't mix up tuples with multiple parameters
let addTwoParams x y = x + y

// wrong -- expression expected of type int but was 'a * 'b (tuple)
// addTwoParams (1, 2)

// correct
addTwoParams 1 2

// Add using a tuple
let addTuple (x, y) = x + y

// wrong - value not a function and cannot be applied
// a bit obscure
// addTuple 1 2

addTuple (1, 2)