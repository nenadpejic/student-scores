namespace StudentScores

type Student =
    {
        LastName: string
        FirstName: string
        Id: string
        Avg: float
        Min: float
        Max: float
    }

module Student =
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

    let fromString (s : string) =
        let elements = s.Split('\t')
        let name = elements.[0] |> getNamePart
        let id = elements.[1]
        let scores =
            elements
            |> Array.skip 2
            // |> Array.choose Float.tryFromString
            |> Array.map (Float.fromStringOr 50.0)
        let avg = scores |> Array.average
        let min = scores |> Array.min
        let max = scores |> Array.max
        {
            LastName = name.LastName
            FirstName = name.FirstName
            Id = id
            Avg = avg
            Min = min
            Max = max
        }

    let printStudentInfo (student : Student) =
        printfn "Name: %s, %s\tId: %s\tAvg: %0.1f\tMin: %0.1f\tMax: %0.1f\t" student.LastName student.FirstName student.Id student.Avg student.Min student.Max

    let printStudentCount a =
        let studentCount = a |> Array.length
        printfn "Student count is: %i" studentCount

    let handleStudents s =
        let studentRows =
            s
            |> System.IO.File.ReadAllLines
            |> Array.skip 1
        printStudentCount studentRows
        studentRows
            |> Array.map fromString
            |> Array.sortBy (fun student -> student.LastName)
            |> Array.iter printStudentInfo
