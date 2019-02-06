// strict type checking
printfn "print string %s" 123 // compile error

// all values immutable by default
type Person = { Given:string; Family:string }
let person1 = { Given="Shannon"; Family="Nixon" }
person1.Given <- "new name"

// never have to check for nulls
let makeNewString str =
    // str can always be appended to safely (unlike C#)
    let newString = str + " new!"
    newString
printfn "makeNewString '%s' returns '%s'" "calvisti" (makeNewString "calvisiti")

// embed business logic into types
// emptyShoppingCart.remove // compile error
// I'm uncertain how to demonstrate this. :(

// units of measure
open FSharp.Data.UnitSystems.SI.UnitSymbols
[<Measure>]
type ft
let distance = 10.0<m> + 10.0<ft> // error!
