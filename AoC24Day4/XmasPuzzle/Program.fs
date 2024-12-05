open System
open System.IO

// Function to find all occurrences of a word in a grid
let findWordOccurrences grid word =
    let rows = Array.length grid
    let cols = Array.length grid.[0]
    let directions = 
        [ (0, 1)   // Right
          (1, 0)   // Down
          (0, -1)  // Left
          (-1, 0)  // Up
          (1, 1)   // Diagonal Down-Right
          (-1, -1) // Diagonal Up-Left
          (1, -1)  // Diagonal Down-Left
          (-1, 1)  // Diagonal Up-Right 
        ]
    
    // Check if a given position is within grid bounds
    let inBounds x y = 
        x >= 0 && y >= 0 && x < rows && y < cols

    // Check if the word matches starting at (x, y) in a given direction (dx, dy)
    let rec matchesFrom x y dx dy i =
        if i = String.length word then 
            true
        elif not (inBounds x y) || grid.[x].[y] <> word.[i] then 
            false
        else 
            matchesFrom (x + dx) (y + dy) dx dy (i + 1)

    // Count occurrences of the word starting at (x, y) in all directions
    let countAt x y =
        directions 
        |> List.sumBy (fun (dx, dy) ->
            if matchesFrom x y dx dy 0 then 1 else 0)
    
    // Sum occurrences across all grid positions
    [ for x in 0 .. rows - 1 do
        for y in 0 .. cols - 1 do
            yield countAt x y ]
    |> List.sum

    // Function to read grid from a file
let readGridFromFile filePath =
    try
        File.ReadAllLines(filePath)
        |> Array.map (fun line -> line.ToCharArray())
    with
    | :? IOException as ex ->
        printfn "Error reading file: %s" ex.Message
        Array.empty

// Entry point of the program
[<EntryPoint>]
let main argv =
    // Define the input file path
    let filePath = "xmas_input.txt"

    // Read the grid from the file
    let charGrid = readGridFromFile filePath

    // Check if the grid is valid
    if charGrid.Length = 0 then
        printfn "Failed to load the grid from the file."
        1 // Return error code
    else
        // Define the word to search for
        let wordToFind = "XMAS"

        // Find all occurrences of the word
        let occurrences = findWordOccurrences charGrid wordToFind

        // Print the result
        printfn "The word '%s' appears %d times." wordToFind occurrences

        // Exit the program
        0 // Return success code
