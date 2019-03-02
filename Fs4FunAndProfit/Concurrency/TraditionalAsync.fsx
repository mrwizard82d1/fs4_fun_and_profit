open System

let userTimerWithCallback =
    // create an event upon which to wait
    let event = new System.Threading.AutoResetEvent(false)
    
    // create a timer and an event handler that signals the event
    let timer = new System.Timers.Timer(2000.0)
    timer.Elapsed.Add(fun _ -> event.Set() |> ignore)
    
    // start everything up
    printfn "Waiting for timer at %O." DateTime.Now.TimeOfDay
    timer.Start()
    
    // keep working
    printfn "Doing something 'useful' while waiting for event."
    
    //  block on the time via the AutoResetEvent
    event.WaitOne() |> ignore
    
    // done
    printfn "Time ticked at %O." DateTime.Now.TimeOfDay
    