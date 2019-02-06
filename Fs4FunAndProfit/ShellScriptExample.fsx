// Description:
//
// Downloads the specified URL and stores it as a file with a timestamp.
//
// Example command line:
//
// fsi ShellScriptExample.fsx http://google.com google_homepage
//
// On my system, fsi.exe is found at:
// c:/Program Files (x86)/Microsoft Visual Studio/2017/Community/Common7/IDE/CommonExtensions/Microsoft/FSharp/

// `open` brings a .NET namespace into visibility
open System.Net
open System

// Download the contents of a web page
let downloadUriToFile url targetFile =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    
    let timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm")
    let path = sprintf "%s.%s.html" targetFile timestamp
    
    use writer = new IO.StreamWriter(path)
    writer.Write(reader.ReadToEnd())
    
    printfn "Finished downloading %s to %s" url path
    
// Running from FSI, the script name is first and the other arguments are after
match fsi.CommandLineArgs with
    | [| scriptName; url; targetfile |] ->
        printfn "Running script: %s" scriptName
        downloadUriToFile url targetfile
    | _ ->
        printfn "USAGE: [url] | [targetfile]"
