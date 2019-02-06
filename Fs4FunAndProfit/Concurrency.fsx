// Lots of errors. Uncertain how to fix it.

// easy async logic with the `async` keyword
let! result = async { something }

// easy parallelism
Async.Parallel [ for i in 0..40 ->
    async {
        return fib(i)    
    } ]

// message queues
MailboxProcessor.Start(fun inbox ->
    async {
        let! msg = inbox.Receive()
        printfn "message is: %s" msg
    })