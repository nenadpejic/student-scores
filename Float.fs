namespace StudentScores

module Float =
    let tryFromString s =
        if s = "N/A" then
            None
        else
            Some(float s)

    let fromStringOr param1 s =
        s
        |> tryFromString
        |> Option.defaultValue param1
