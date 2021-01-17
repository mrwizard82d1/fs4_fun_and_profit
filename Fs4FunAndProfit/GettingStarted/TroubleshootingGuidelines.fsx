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

// ----- Too many or too few arguments

// ------ Using semicolons for list separators

// wrong - this is a one-element list containing a three-element tuple
// let list1 = [1, 2, 3]

// correct
let list1 = [1; 2; 3]

// wrong - unexpected symbol ',' in  type definition
// type Customer = { Name: string, Address: string }

// correct
type Customer = { Name: string; Address: string }

// ----- Do not use ! for not or != for "not equal"
let y = true

// wrong - this expression was expected to have type `a ref ...
// let z = !y

// correct
let z = not y

// For "not equal", use `<>` similar to SQL or VB
let w = 1 <> 2

// ----- Don't use `=` for assignment (to mutable values)

// wrong -- the second expression assigns `false` to x because it can never equal the increment of itself
let mutable x = 1
x = x + 1

// correct
x <- x + 1  // `<-` is the **reassignment** operator

// ------ Watch out for hidden tab characters

// wrong -- tabs are **not allowed** in F# code
// Note that one cannot actually insert a tab in Rider
//let add_tab x y =
//  x + y

// ----- Don't mistake simple values for function values

let reader = new System.IO.StringReader("hello")

// wrong - but not at compilation
// type inference concludes that `nextLineFn` is a `string` (the result of invoking the `ReadLine` member
// let nextLineFn = reader.ReadLine()

// correct -- the trailing parentheses inform the type inference engine that `nextLineFn` is a function
// of type `unit -> string`
let nextLineFn() = reader.ReadLine()

// as an alternative, one can use the `fun` keyword
// let nextLineFn = fun() -> reader.ReadLine()

let r = System.Random()

// wrong - type inference concludes that `randomFn` is an `int` - the result of executing the
// `Next()` function
// let randomFn = r.Next()

// correct - a function `unit -> int`
let randomFn() = r.Next()

// an alternative - using a lambda
// let randomFn() = fun() -> r.Next()

// a suggested replacement from Rider - perhaps newer syntax; however, the function is now of type
// `int -> int` which is the type of `r.Next`. This appears to create an alias for the original function.
let randomFn1 = r.Next

// ----- Tips for troubleshooting "not enough information" errors

// The F# compiler is currently a one-pass, left-to-right compiler. Consequently, type information from "later
// in the program is unavailable to the compiler if it hasn't been parsed yet."

// To prevent these kinds of errors,
// - Define things before they are used (including putting files in the correct order)
// - Put things with known types earlier than things with unknown types. In particular, you might be able to
//   reorder pipes and similar channel functions so that typed objects come first.
// - Annotate as needed. A common trick is to annotate everything and then **remove** annotations until you
//   have the minimum needed.
//
// Although you may need annotations, try to avoid annotations if possible. Annotations can make the code
// more brittle. It is much easier to change types if there are no explicit dependencies on them.