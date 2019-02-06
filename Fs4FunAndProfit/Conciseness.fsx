// one-liners
[1..100] |> List.sum |> printfn "sum=%d"

// no curly braces, semicolons or parentheses
let square x = x * x
let sq = square 42
printfn "sq=%d" sq

// simple types in a single line
type Person = { Given:string; Family:string }

// complex types in a few lines (a recursive type definition)
type Employee =
    | Worker of Person
    | Manager of Employee list
    
// easy `IDisposable` logic with `use` keyword (warnings in script)
use reader = new System.IO.StreamReader("./README.md")
// Note that after running this line, the FSI process *locks* the file to be read.

// easy composition of functions
let add2times3 = (+) 2 >> (*) 3
let result = add2times3 5
printfn "Composition result=%d" result
