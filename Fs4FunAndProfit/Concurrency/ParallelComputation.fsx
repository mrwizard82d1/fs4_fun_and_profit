// A child task doing useful "work"
let childTask () =
    // chew up some CPU
    for i in [ 1..1000 ] do
        for i in [ 1..1000 ] do
            do "Hello".Contains("H") |> ignore
            // don't care about the answer
            
// test the child on its own
// adjust the upper bounds as necessary
// to make the child consume about 200 ms (0.2 s)
#time
childTask()
#time

// Combine a bunch of these child tasks and run serially (using composition!)
let syncParentTask =
    childTask
    |> List.replicate 20
    |> List.reduce (>>)
    
#time
syncParentTask()
#time

// Make the unit of computation parallelizable by wrapping it an an `async`
let asyncChildTask = async { return childTask() }

// Combine a bunch of these tasks into a single task
// running child tasks in parallel
let asyncParentTask =
    asyncChildTask
    |> List.replicate 20
    |> Async.Parallel
    
// And time it to test.
#time
asyncParentTask
|> Async.RunSynchronously
#time
