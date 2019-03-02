open System
open System.Net

// Fetches the resource identified by url
let fetchUrl url =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "Finshed downloading %s" url
    
// Some sites to fetch
let sites = [ "http://www.bing.com";
              "http://www.google.com";
              // "http://wwww.microsoft.com"; // takes too long (this morning - 20s)
              "http://wwww.amazon.com";
              "http://www.yahoo.com" ]

// Time fetching **all** the sites
#time
sites // for each sit
|> List.map fetchUrl // fetch the resource
#time
