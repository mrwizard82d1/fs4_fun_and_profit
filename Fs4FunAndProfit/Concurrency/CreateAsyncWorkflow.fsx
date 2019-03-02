open System

// create an asynchronous workflow
let sleepWorkflow = async {
    printfn "Starting sleep workflow at %O." DateTime.Now.TimeOfDay
    
    // remember that the '!' suffix indicates an asynchronous call; that is,
    // `do!` indicates letting other async workflows run.
    do! Async.Sleep 2000
    
    printfn "Finished sleep workflow at %O." DateTime.Now.TimeOfDay
}

// Run the asynchronous workflow (waiting for it to finish)
#time
Async.RunSynchronously sleepWorkflow
#time
