// ----- Can't mix ints and floats

// Wrong
1 + 2.0

// Correct
float 1 + 2.0

// Wrong
[ 1..10 ] |> List.average

// Correct (map each int to a flot)
[ 1..10 ] |> List.map float |> List.average

// Correct use `averageBy` with `float`
[ 1..10 ] |> List.averageBy float

// ----- Using the wrong numeric type

// Should be an int and not a float
printfn "hello %i" 1.0

// A possible fix
printfn "hello %i" (int 1.0)

// ----- Passing too many arguments to a function

let add x y = x + y

add 1 2 3

// A similar error occurs passing too many arguments to printf

printfn "hello" 42

printfn "hello %i" 42 43

printfn "hello %i %i" 42 43 44

// ----- Passing two few arguments to a function

// Passing too few arguments results in a partial function application. When you use it later, you will get
// an error because the result, a partial function, is not a simple type.

let reader = new System.IO.StringReader("hello")

// The result, `line`, is wrong, but the compiler doesn't complain.
let line = reader.ReadLine

// The compiler error occurs here. `line` is not a string, the expected type, but is a function of type,
// `unit -> string`.
printfn "The line is %s" line

// .NET functions that take no arguments are particularly prone to this issue. The solution is to supply
// no arguments, (), to the function.

let line = reader.ReadLine()

// No compiler error now.
printfn "The line is %s" line

// ----- Straightforward type mismatch

// The simplest case is that you have the wrong type, or you are using the wrong type in a format string.
printfn "hello %s" 1.0

// ------ Inconsistent return types in branches or matches

// A common mistake: not every branch returns that same type.
let f x =
    if x > 1 then "hello"
    else 42
    
let g x =
    match x with
    | 1 -> "hello"
    | _ -> 42

// The straightforward fix: make each branch return the same type.
let f x =
    if x > 1 then "hello"
    else "42"
    
let g x =
    match x with
    | 1 -> "hello"
    | _ -> "42"
    
// A common gotcha': if an `else` branch is missing, it is assumed to return `unit`.
let f x =
    if x > 1 then "hello"
    
type StringOrInt =
 | S of string
 | I of int  // new union type
let f x =
    if x > 1 then S "hello"
    else I 42
    
// ----- Watch out for type inference effects buried in a function.

// A function may cause an unexpected type inference that ripples throughout your code. For example, in the
// following code, the innocent `print` format string accidentally causes `doSomething` to expect a string.
let doSomething x =
    printfn "x is %s" x
    
// Now invoke doSomething with an int
doSomething 1

// Imagine the invocation occurring many, many lines later. :)

// The fix is to check function signatures (hover over the name of the function). And then drill down through
// the function until you find the "guilty party."
//
// Additionally, use the most generic types possible, and avoid type annotations if possible.

// ----- Have you used a comma instead of a semicolon?

// Define a two-parameter function.
let add x y = x + y

// Invoke it using a single argument, a tuple.
printfn "%i" add(1, 2)

// The solution: don't use a comma!
let add x y = x + y
printfn "%i" (add 1 2)

// Remember, .NET library functions take tuples as arguments. Consequently, invoking these functions looks
// just like a C# call.

// correct
System.String.Compare("a", "b")

// incorrect
System.String.Compare "a" "b"

// ----- Tuples must be the same type to be compared or pattern matched.
let t1 = (0, 1)  // type int * int
let t2 = (0, "hello")  // type int * string

// Type mismatch: expecting the right-hand side to be int * int
t1 = t2

// And the length must be the same.
let t1 = (0, 1)  // type int * int
let t2 = (0, 1, "hello")  // type int * int * stringA

// Type mismatch: tuples of two different lengths cannot be compared
t1 == t2

// The same issue can occur during pattern matching.
let x, y = 1, 2, 3

// And similarly for functions.
let f (x, y) = x + y
let z = (1, "hello")
let result = f z  // type string does not match the type int

// ----- Don't use ! as the "not" operator

let y = true
let z = !y  // wrong

// The fix is to use the "not" keyword instead
let y = true
let z = not y  // correct

// ----- Operator precedence (especially in functions and pipes)

// Function application has the highest precedence compared to other operators, so you get an error
// in the following.
String.length "hello" + "world"  // Type string does not match the type int

(String.length "hello") + "world"  // What's really happening

// The fix is to use parentheses to override precedence.
String.length ("hello" + "world")

// Conversely, the pipe operator is low precedence compared to other operators.
let result = 42 + [1..10] |> List.sum  // The type 'a List does not match the type int

let result = (42 + [1..10]) |> List.sum  // What is really happening

// Again, the fix is to use parentheses to override precedence
let result = 42 + ([1..10] |> List.sum)

// ----- let! error in computation expressions (monads)

// Here's a simple computation expression

type Wrapper<'a> = Wrapped of 'a  // A wrapper of a generic type

// A class implementing the interface of a computation expression
type wrongWrapBuilder() =
    member this.Bind (wrapper: Wrapper<'a>) (func:'a -> Wrapper<'b>) =
        match wrapper with
        | Wrapped(innerThing) -> func innerThing
        
    member this.Return innerThing =
        Wrapped(innerThing)
        
// Construct an "factory" for a computation expression
let wrongWrap = wrongWrapBuilder()

// However, if you now try to use the expression, you get an error.
wrongWrap {
    let! x = Wrapped(1)
    let! y = Wrapped(2)
    let z = x + y
    return z
}

// (I don't yet understand computation expressions so I simply rely on the explanation in the book.)

// The reason for the error is that the type of the member, `Bind`, is "wrapper * func".
// THe fix is to change the bind function to accept a tuple as its single parameter.

type rightWrapBuilder() =
    member this.Bind (wrapper: Wrapper<'a>, func:'a -> Wrapper<'b>) =
        match wrapper with
        | Wrapped(innerThing) -> func innerThing
        
    member this.Return innerThing =
        Wrapped(innerThing)

let rightWrap = rightWrapBuilder()

// However, if you now try to use the expression, you get an error.
rightWrap {
    let! x = Wrapped(1)
    let! y = Wrapped(2)
    let z = x + y
    return z
}

// ------ This value is not a function and cannot be applied

// Often occurs when passing too many arguments to a function
let add1 = x + 1
let x = add1 2 3

// But can also occur when doing operator overloading

let (!!) x y = x + y
(!!) 1 2

// Fails because !! cannot be used as an infix operator
1 !! 2

// ------ This runtime coercion or type test involves an indeterminate type

let detectType v =
    match v with
    | :? int -> printfn "This is an int"
    | _ -> printfn "something else"
    
// The answer is to "box" the value which forces it into a reference type which can then be type checked.
let detectTypeBoxed v =
    match box v with
    | :? int -> printfn "This is an int"
    | _ -> printfn "something else"
    
 // Some tests
detectTypeBoxed 1
detectTypeBoxed 3.14

// ----- Unexpected identifier in binding

// Typically caused by breaking the "offside" rule for aligning expressions
//let f =
//  let x = 1 // offside line is at column 3
//    x + 1   // oops! don't start at column 4
    
// ----- Incomplete structured construct

// Often occurs if you are missing parentheses from a class constructor.

type Something() =
    let field = ()

// Different behavior from book. **Both** expressions are valid. x1 is a function taking `unit` and returning
// `Something`. x2 is an instance of `Something`

let x1 = Something
let x2 = Something()

// Can also occur if you forget to put parentheses about an operator

let (|+) a = -a

|+ 1 // Error: unexpected infix operator
(|+) 1 // OK with parentheses

// Additionally, one cannot define a namespace in F# interactive
//namespace Customer
//    // declare a type
//    type Person = { First: string; Last:string }

// ----- This expression should have type 'unit'

// Occurs with expressions that are **not** the last expression in a block. Appears to no longer be an issue.
// However, the interactive window indicates that the following expression is **not** a function but a simple
// value. 
let something =
    2 + 2
    "hello"
    
// What happens if `something` **is** a function?
let something () =
    2 + 2
    "hello"
// The interactive window allows the expression, but it also **prints** a warning.

// The solution to the warning is to ignore the intermediate expression.
let something () =
    2 + 2 |> ignore
    "hello"
// I do not understand the squiggles underneath the `let` of the previous expression. :( Everything appears
// fine in the interactive window.

// The same problem occurs if you think your writing C# and use a semicolon to separate multiple statements
// on one line.

// wrong
let result = 2 + 2; "hello"

// fixed
let result = 2 + 2 |> ignore; "hello"

// Another variant of this error occurs when assigning to a property (a mutable value)

// `=` versus `<-`
let add() =
    let mutable x = 1
    x = x + 1  // warning
    printfn "%d" x

// fixed
let add() =
    let mutable x = 1
    x <- x + 1
    printfn "%d" x
    
// ----- Value restriction

// By default, the F# inference mechanism tries to generalize to generic types whenever possible.

let id x = x
let compose f g x = g (f x)
let opt = None

// However, in some cases, the F# compiler believes the code to be ambiguous.
let idMap = List.map id
let blankConcat = String.concat

// Almost always this error occurs when trying to define a partially applied function. Almost always, the
// easiest fix is to explicitly add the missing parameter.
let idMap list = List.map id list

// Note that the compiler does not have a problem with `blankConcat`.

// ----- This construct is deprecated.
let x = 10
let rnd1 = System.Random x          // Correct
let rnd2 = new System.Random(x)     // Correct
let rnd3 = new System.Random x      // Error

// ----- The field, constructor, or member X is not defined.

// This error commonly occurs in four situations:
// - The value actually is not defined! (Be sure to check for typos or a case mismatch.)
// - Interfaces
// - Recursion
// - Extension methods

// Interfaces

// Remember that all interfaces are **explicit** (so typically "casts" are necessary).

// A class that implements an interface.
type MyResource() =
    interface System.IDisposable with
        member this.Dispose() = printfn "disposed"
        
// This does not work (because of the lack of implicit interfaces). `Dispose` not defined for type.
let x = new MyResource()
x.Dispose()


// The fix: cast the object to the interface.
(x :> System.IDisposable).Dispose()

// With recursion

let fib i =
    match i with
    | 1 -> 1
    | 2 -> 1
    | n -> fib(n - 1) + fib(n - 2)
    
// The preceding definition fails to compile because `fib` is invoked **before** `fib` is completely defined.
// (Remember, a one-pass, left-to-right compiler.)

// The fix is to use the `rec` keyword
let rec fib i =
    match i with
    | 1 -> 1
    | 2 -> 1
    | n -> fib(n - 1) + fib(n - 2)

// Note that these comments only apply to functions defined with `let`. Member functions do not need `rec`
// because the scope rules are slightly different.
type FibHelper() =
    member this.fib i =
        match i with
        | 1 -> 1
        | 2 -> 1
        | n -> fib(n - 1) + fib(n - 2)
        
// Extension methods

// One cannot use an extension method unless the module is in scope.

module IntExtensions =
    type System.Int32 with
        member this.IsEven = this % 2 = 0
        
// Using this extension results in a compiler error.
let i = 2
let result = i.IsEven

// The fix is to open the `IntExtensions` module.
open IntExtensions
let i = 2
let result = i.IsEven

// ----- A unique overload for could not be determined

// This error con result from calling a .NET library function that has multiple overloads.

let streamReader filename = new System.IO.StreamReader(filename)  // Error

// We have a number of ways to resolve this issue. First, use an explicit type annotation.
let streamReader filename = new System.IO.StreamReader(filename: string)  // OK

// You can sometimes use a named parameter to avoid the type annotation.
let streamReader filename = new System.IO.StreamReader(path=filename)  // OK

// Or you can create intermediate objects that help the type inference (engine) without needing type
// annotations.
let streamReader filename =
    let fileInfo = System.IO.FileInfo(filename)
    new System.IO.StreamReader(fileInfo.FullName)  // OK
    
// ----- Uppercase variable identifiers should not generally be used in patterns

// Beware of a difference between pure F# union types (consisting of a tag only) and .NET Enum types.
type ColorUnion = Red | Yellow
let redUnion = Red

match redUnion with
    | Red -> printfn "red"  // no problem
    | _ -> printfn "something else"
    
// With .NET Enums, you must fully qualify them.
type ColorEnum = Green=0 | Blue=1  // an enum
let blueEnum = ColorEnum.Blue

match blueEnum with
| Blue -> printfn "blue"  // warning
| _ -> printfn "something else"

match blueEnum with
| ColorEnum.Blue -> printfn "blue"
| _ -> printfn "something else"

// ----- Lookup on object of indeterminate type

// Occurs when "dotting into" an object whose type is unknown.
let stringLength x = x.Length  // error

// There are a number of ways to fix this.

// Crudest way: an explicit type annotation.
let stringLength (x:string) = x.Length  // OK

// In some cases, judicious rearrangement of code con help. The example below looks like it should work.
List.map (fun x -> x.Length) [ "hello"; "world" ]  // error

// This is a result of a single-pass compiler.

// One can explicitly annotate.
List.map (fun (x:string) -> x.Length) [ "hello"; "world" ]  // OK

// Another, more elegant way, is to rearrange things so that known types come first, and the compiler can
// digest them before it moves to the next clause.
[ "hello"; "world" ] |> List.map (fun x -> x.Length)

// ----- Block following this `let` is unfinished

// Caused by outdenting an expression in a block; thus, breaking the "offside rule."
let f =
    let x = 1  // offside line is at column 3
x + 1          // offside! You are ahead of the ball!

// The fix: align the code properly.

// -- FS0010: Incomplete structured constant
type Something() =
    let field = ()
    
// error -- but not at compile time. Expression creates an alias for the Something constructor
let s1 = Something

// correct - include trailing `()`
let s2 = new Something()

// Can also occur if you forget to put parentheses around an operator.

// define a new operator
let (|+) a = -a

// error FS0010: unexpected infix operator
|+ 1

// correct usage - with surrounding parentheses
(|+) 1

// Can also occur if you attempt to send a namespace definition to F# interactive console. The interactive
// console does not allow namespaces.
//namespace Customer
//    type Person = { First:string; Last:string }
    
// FS0013: The static coercion from type X to Y involves an indeterminate type

// ???

// FS0020: This expression should have type `unit`
// Commonly occurs in two situations:
// - Expressions are **not** last in the block
// - Using the wrong assignment operator

// FS0020: Not last expression in the block

// Only the last expression in a block can return a value. Any others that return a value must explicitly be
// ignored.
//let something =
//    2 + 2
//    "hello"

// The easy fix is to use `ignore`. But you may want to ask yourself, "Why are you throwing away this
// intermediate value."
let something =
    2 + 2 |> ignore
    "hello"
    
// This error can also occur if you think you are writing C# and use a semicolon to separate "statements"
// let result = 2 + 2; "hello"

// let result = 2 + 2 |> ignore; "hello"

// FS0020 with assignment

// Another variant of this error occurs when assigning to a property. With this error, you have most likely
// confused the assignment operator `<-` for mutable values with the equality comparison operator, `=`.

// wrong
//let add() =
//    let mutable x = 1
//    x = x + 1 // warning
//    printfn "%d" x

// The fix is to use the proper assignment operator.
let add() =
    let mutable x = 1
    x <- x + 1 
    printfn "%d" x

// FS0030: Value restriction

// Relates to F# automatic generalization to generic types whenever possible.
let id x = x
let compose f g x = g (f x)
let opt = None

// The type inference engine of F# cleverly determines the generic types.

// However, in some cases, the F# compiler feels that the code is ambiguous, and, even though it looks like
// it is guessing the type correctly, it needs you to be more specific.
// FS0020
let idMap = List.map id
let blankConcat = String.concat ""

// Almost always this will be caused by trying to define a partially applied function, and, almost always,
// the easiest fix is to explicitly add the missing parameter.
let idMap list = List.map id list
let blankConcat list = String.concat "" list