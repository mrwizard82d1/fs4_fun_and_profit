open System.Threading

let testLoop = async {
   for i in [ 1..100 ] do
      // do something
      printf "%i before..." i
      
      // sleeping a bit allows this workflow to detect cancellation.
      do! Async.Sleep 10
      printfn "...and afterwards."
}

// Create a cancellation source
let cancellationSource = new CancellationTokenSource()

// start the async workflow passing in a cancellation token
Async.Start (testLoop, cancellationSource.Token)

// the main thread waits for a SHORT bit...
Thread.Sleep(200)

// ... and then cancels the loops.
cancellationSource.Cancel()
