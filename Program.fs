open StudentScores

[<EntryPoint>]
let main argv =
    if argv |> Array.length = 2 then
        let schoolCodesFilePath = argv.[0]
        let filePath = argv.[1]
        if not (System.IO.File.Exists schoolCodesFilePath) then
            printfn "File path: %s, does not exist" schoolCodesFilePath
            1
        elif not (System.IO.File.Exists filePath) then
            printfn "File path: %s, does not exist" filePath
            2
        else
            printfn "Processing file: %s" filePath
            try
                Student.handleStudents schoolCodesFilePath filePath
                0
            with
            | :? System.FormatException as e ->
                printfn "Error: %s" e.Message
                printfn "File was not in the expected format"
                3
            | :? System.IO.IOException as e ->
                printfn "Error: %s" e.Message
                printfn "File is in use by another program"
                4
            | _ as e ->
                printfn "Unexpected error: %s" e.Message
                5
    else
        printfn "Please enter file path"
        6
