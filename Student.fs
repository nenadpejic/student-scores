namespace StudentScores

type Student =
    {
        Name: string
        Id: string
        Avg: float
        Min: float
        Max: float
    }

module Student =
    let fromString (s : string) =
        let elements = s.Split('\t')
        let name = elements.[0]
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
            Name = name
            Id = id
            Avg = avg
            Min = min
            Max = max
        }

    let printStudentInfo (student : Student) =
        printfn "Name: %s\tId: %s\tAvg: %0.1f\tMin: %0.1f\tMax: %0.1f\t" student.Name student.Id student.Avg student.Min student.Max

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
            |> Array.iter printStudentInfo
