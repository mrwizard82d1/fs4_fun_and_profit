open System

let userTimerWithAsync =
    // Create a timer and associated async event
    let timer = new System.Timers.Timer(2000.0)
    let timerEvent = Async.AwaitEvent timer.Elapsed |> Async.Ignore
    
    // start work on the main thread
    printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
    timer.Start()
    
    // keep working
    printfn "Doing something useful while waiting for event."
    
    // block on the timer event now by waiting for the async to complete
    Async.RunSynchronously timerEvent
    
    // print a message when done
    printfn "Timer ticked at %O" DateTime.Now.TimeOfDay
    