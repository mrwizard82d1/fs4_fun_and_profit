// ================================
// Description:
//   Downloads the given URL and stores it as a file with a time stamp.
//
//   Example command line:
//      dotnet fsi ShellScriptExample.fsx http://google.com google


// "Open" brings a .NET namespace into visiblity
open System.Net
open System

// Download the contents of a web page
let downloadUriToFile url targetFile =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    let timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm")
    let path = sprintf "%s.%s.html" targetFile timeStamp
    use writer = new IO.StreamWriter(path)
    writer.Write(reader.ReadToEnd())
    printfn "Finished downloading %s to %s" url path
    
// Running from FSI, the script name is first argument; other arguments follow.
match fsi.CommandLineArgs with
    | [| scriptName; url; targetFile |] ->
        printfn "Running script %s" scriptName
        downloadUriToFile url targetFile
    | _ ->
        printfn "USAGE: <url> <targetFile>"
        