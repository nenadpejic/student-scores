open StudentScores

[<EntryPoint>]
let main argv =
    if argv |> Array.length = 1 then
        let filePath = argv.[0]
        if System.IO.File.Exists filePath then
            try
                printfn "Processing file: %s" filePath
                Student.handleStudents filePath
                0
            with
            | :? System.FormatException as e ->
                printfn "Error: %s" e.Message
                printfn "File was not in the expected format"
                1
            | :? System.IO.IOException as e ->
                printfn "Error: %s" e.Message
                printfn "File is in use by another program"
                2
            | _ as e ->
                printfn "Unexpected error: %s" e.Message
                3
        else
            printfn "File path: %s, does not exist" filePath
            4
    else
        printfn "Please enter file path"
        5
