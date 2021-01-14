namespace StudentScores

module Float =
    let fromString s =
        if s = "N/A" then
            None
        else
            Some(float s)
