namespace StudentScores

type Student =
    {
        LastName: string
        FirstName: string
        Id: string
        SchoolName: string
        Avg: float
        Min: float
        Max: float
    }

module Student =

    open System.Collections.Generic

    let getNamePart (s : string) =
        let elements = s.Split(',')
        match elements with
            | [|lastName; firstName|] ->
                {|
                    LastName = lastName.Trim()
                    FirstName = firstName.Trim()
                |}
            | [|lastName|] ->
                {|
                    LastName = lastName.Trim()
                    FirstName = "(None)"
                |}
            | _ -> raise (System.FormatException (sprintf "Invalid name format: %s" s))

    let fromString (schoolCodes : Map<_,_>) (s : string) =
        let elements = s.Split('\t')
        let name = elements.[0] |> getNamePart
        let id = elements.[1]
        let schoolCode = elements.[2]
        let schoolName =
            schoolCodes
            |> Map.tryFind schoolCode
            |> Option.defaultValue "(Unknown)"
        let scores =
            elements
            |> Array.skip 3
            // |> Array.choose Float.tryFromString
            // |> Array.map (Float.fromStringOr 50.0)
            |> Array.map TestResult.fromString
            |> Array.choose TestResult.tryFromTestResult
        let avg = scores |> Array.average
        let min = scores |> Array.min
        let max = scores |> Array.max
        {
            LastName = name.LastName
            FirstName = name.FirstName
            Id = id
            SchoolName = schoolName
            Avg = avg
            Min = min
            Max = max
        }

    let printGroupInfo (lastName : string) (students : seq<Student>) =
        printfn "%s" (lastName.ToUpperInvariant())
        students
        |> Seq.sortBy (fun student -> student.FirstName, student.Id)
        |> Seq.iter (fun student ->
            printfn "\t%15s\t%s\t%s\tAvg: %0.1f\tMin: %0.1f\tMax: %0.1f\t"
                student.FirstName student.Id student.SchoolName student.Avg student.Min student.Max
        )

    let handleStudents schoolCodesFilePath filePath =
        let studentRows =
            filePath
            |> System.IO.File.ReadLines
            |> Seq.skip 1
            |> Seq.cache
        let studentCount = studentRows |> Seq.length
        printfn "Student count is: %i" studentCount
        let schoolCodes = SchoolCodes.load schoolCodesFilePath
        studentRows
        |> Seq.map (fromString schoolCodes)
        |> Seq.groupBy (fun student -> student.LastName)
        |> Seq.sortBy fst
        |> Seq.iter (fun (lastName, students) -> printGroupInfo lastName students)
