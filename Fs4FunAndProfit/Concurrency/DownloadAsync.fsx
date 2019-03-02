open System
open System.Net

// fetch a resource asynchronously
let fetchUrlAsync url = async {
    let req = WebRequest.Create(Uri(url))
    // remember that bang ('!') allows other workflows to run
    use! resp = req.AsyncGetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    // differs from the book!
    let! html = Async.AwaitTask(reader.ReadToEndAsync())
    printfn "Finished downloading %s" url
}

// Some sites to fetch
let sites = [ "http://www.bing.com";
              "http://www.google.com";
              // "http://wwww.microsoft.com"; // takes too long (this morning - 20s)
              "http://wwww.amazon.com";
              "http://www.yahoo.com" ]

// Time fetching **all** the sites
#time
sites // for each sit
|> List.map fetchUrlAsync // does no work! creates work to be done
|> Async.Parallel // now do work in parallel
|> Async.RunSynchronously // and wait for all to finish
#time
