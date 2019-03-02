open System

// create an asynchronous workflow
let sleepWorkflow = async {
    printfn "Starting sleep workflow at %O." DateTime.Now.TimeOfDay
    
    // remember that the '!' suffix indicates an asynchronous call; that is,
    // `do!` indicates letting other async workflows run.
    do! Async.Sleep 2000
    
    printfn "Finished sleep workflow at %O." DateTime.Now.TimeOfDay
}

let nestedWorkflow = async {
    printfn "Starting parent workflow"
    let! childWorkflow = Async.StartChild sleepWorkflow
    
    // give the child workflow a chance and then keep working
    do! Async.Sleep 100
    printfn "Doing something useful while awaiting child"
    
    // block on the child
    let! result = childWorkflow
    
    // Report when done
    printfn "Parent finished"
}

#time
Async.RunSynchronously nestedWorkflow
#time
