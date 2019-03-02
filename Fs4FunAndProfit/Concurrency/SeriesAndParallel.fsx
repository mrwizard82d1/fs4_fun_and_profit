// Workflows in series and in parallel

// create a workflow to sleep for a bit
let sleepWorkflowMs ms = async {
    printfn "%i ms workflow started" ms
    do! Async.Sleep ms
    printfn "%i ms workflow finished" ms
}

// combine workflows in series
let workflowInSeries = async {
    // remember that the bang suffix ('!') allows other workflows to run
    let! sleep1 = sleepWorkflowMs 1000
    printfn "First finished"
    
    let! sleep2 = sleepWorkflowMs 2000
    printfn "Second finished"
}

#time
Async.RunSynchronously workflowInSeries
#time

// combine workflows in parallel

// begin by creating the (not running) workflows
let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

// run them in parallel
#time
[ sleep1; sleep2 ]
|> Async.Parallel
|> Async.RunSynchronously
#time
