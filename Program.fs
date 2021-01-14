open StudentScores

[<EntryPoint>]
let main argv =
    if argv |> Array.length = 1 then
        let filePath = argv.[0]
        if System.IO.File.Exists filePath then
            printfn "Processing file: %s" filePath
            Student.handleStudents filePath
            0
        else
            printfn "File path: %s, does not exist" filePath
            2
    else
        printfn "Please enter file path"
        1
