// impure code when needed
let mutable counter = 0
printfn "(mutable) counter before=%d" counter
counter <- 3
printfn "(mutable) counter after=%d" counter

// create C# compatible classes and interfaces
type IEnumerator<'a> =
    abstract member Current : 'a
    abstract MoveNext : unit -> bool
    
// extension methods
type System.Int32 with
    member this.IsEven = this % 2 = 0
let i = 7
printfn "%d.IsEven = %A" i i.IsEven
let j = 8
printfn "%d.IsEven = %A" j j.IsEven

// UI code
open System.Windows.Forms
let form = new Form(Width=400, Height=300, Visible=true, Text="Hello Windows Form World")
form.TopMost <- true
form.Click.Add (fun args ->
    printfn "clicked")
form.Show()

