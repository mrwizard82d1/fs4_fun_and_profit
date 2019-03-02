open System

let fileWriteWithAsync =
    // create a stream to which to write
    use stream = new IO.FileStream("test.txt", IO.FileMode.Create)
    
    // start processing on the "main" thread
    printfn "Starting asynchronous write."
    let asyncResult = stream.BeginWrite(Array.empty, 0, 0, null, null)
    
    // create an async wrapper around an IAsyncResult (waiting for write to complete)
    let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore
    
    // keep working on the main thread while the write occurs
    printfn "Doing something useful while waiting for write to complete."
    
    // block on the timer now by waiting for the asynchronous write to complete
    Async.RunSynchronously async
    
    // done
    printfn "Asynchronous write completed."
    