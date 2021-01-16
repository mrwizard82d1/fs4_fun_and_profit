// ----------------------------------
// Description:
//   Simply examples and explanations of F# syntax..

// Single line comments use a double slash.
(* Multi-line comments use the pair (* ... *)
- end of multiline comment- *)

// ----- "Variables" (but not really) -----
// THe `let` keyword defines an immutable value
let myInt = 5
let myFloat = 3.14
let myString = "hello"  // note that we needed not types for declarations

// ----- Lists ------
let twoToFive = [2; 3; 4; 5]  // square brackets create a list with semicolon delimiters
let oneToFive = 1 :: twoToFive  // :: create a list with a new 1st element (cons)
                                // The result is [1; 2; 3; 4; 5]
let zeroToFive = [0; 1] @ twoToFive  // @ concatenates two lists

// IMPORTANT: commas are **never** used as delimiters (of lists) only semicolons

// ----- Functions ------
// The `let` keyword also defines a named function
let square x = x * x  // Note that no parentheses are used to define the function
square 3  // Now to run the function. Again, no parentheses.

let add x y = x + y  // **don't** use (x, y) - it means something completely different
add 2 3  // now run the function

// To define a multiline function, use indentation (like Python). We need neither semicolons nor braces.
let evens list =
    let isEven x = x % 2 = 0  // define `isEven` as an inner (nested) function
    List.filter isEven list   // `List.filter` is a library functions that expects two arguments: a boolean
                              // function (a predicate) and a list to filter
                              
evens oneToFive

// You can use parentheses to clarify precedence. In this example, execute `map` first with two arguments
// then execute `sum` on the resulting list.
// Without the parentheses, `List.map` would be passed as an argument to `List.sum`.
let sumOfSquaresTo100 =
    List.sum ( List.map square [1..100] )  // `square` was defined earlier
sumOfSquaresTo100

// You can pipe the output of one operation to the next using the pipe operator `|>`.
let sumOfSquaresTo100Piped =
    [1..100] |> List.map square |> List.sum
sumOfSquaresTo100Piped

// You can define lambda's (anonymous functions) using the `fun` keyword.
let sumOfSquaresTo100WithFun =
    [1..100] |> List.map (fun x -> x * x) |> List.sum
sumOfSquaresTo100WithFun

// In F#, returns are implicit; no "return" is needed. A function always returns the value of the last
// expression used.

// ----- Pattern Matching ------
// A `match ... with ...` expression is a supercharged case/switch/nested if statement.
let simplePatternMatch =
    let x = "a"
    // let x = "b"
    // let x = "c"
    match x with
        | "a" -> printfn "x is a"
        | "b" -> printfn "x is b"
        | _ -> printfn "x is something else"  // An underscore (`_`) matches anything.
simplePatternMatch

// Some(...) and None are roughly analogous to `Nullable` wrappers.
let validValue = Some(99)
let invalidValue = None

// In this example, `match ... with` matches the `Some` and the `None`. At the same time, the match unpacks
// the value in the `Some`.
let optionPatternMatch input =
    match input with
        | Some i -> printfn "`input` is an int=%d" i
        | None -> printfn "`input` is 'missing'"
        
optionPatternMatch validValue
optionPatternMatch invalidValue

// ----- Complex data types -----

// Tuple types are pairs, triples, and so on. Tuples use commas.
let twoTuple = 1, 2
let threeTuple = "a", 2, true

// Record types have named fields. Semicolons are separators.
type Person = { First: string; Last: string }
let person = { First="John"; Last="Doe" }

// Union types have choices. Vertical bars are separators.
type Temp =
    | DegreesC of float
    | DegreesF of float
let tempF = DegreesF 90.6
let tempC = DegreesC 22.8

// Types can be combined recursively in complex ways.
// For example, here is a union type that contains a list of the same type.
type Employee =
    | Worker of Person
    | Manager of Employee list
let jdoe = { First="John"; Last="Doe" }
let worker = Worker jdoe

// ----- Printing -----

// The `print` / `printfn` are similar to the `Console.WriteLine` functions in C#.
printfn "Printing an int %i, a float %f, a bool %b" 1 2.0 true
printfn "A string %s, and something generic %A" "hello" [ 1; 2; 3 ]

// All complex types have pretty printing built in.
printfn "twoTuple=%A\nPerson=%A,\nTemp (F)=%A,\nTemp (C)=%A,\nEmployee=%A" twoTuple person tempF tempC worker
