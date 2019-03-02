let testLoop = async {
   for i in [ 1..100 ] do
      // do something
      printf "%i before..." i
      
      // sleep a bit
      do! Async.Sleep 10
      printfn "...and afterwards."
}

// Wait for the async workflow to finish
Async.RunSynchronously testLoop
