open System

// Type to represent a position on the grid
type Position = { Row: int; Col: int }

// Function to convert the input string into a 2D char array
let parseGrid (input: string) =
    input.Split('\n')
    |> Array.map (fun s -> s.Trim())
    |> Array.filter (not << String.IsNullOrEmpty)
    |> Array.map Seq.toArray

// Function to check if a position is within grid bounds
let isInBounds (grid: char[][]) pos =
    pos.Row >= 0 && pos.Row < grid.Length && 
    pos.Col >= 0 && pos.Col < grid.[0].Length

// Function to get character at a position
let getChar (grid: char[][]) pos =
    grid.[pos.Row].[pos.Col]

// Function to check if three positions form "MAS" (forward or backward)
let checkMAS (grid: char[][]) positions =
    let chars = positions |> List.map (getChar grid) |> Array.ofList |> String
    chars = "MAS" || chars = "SAM"

// Function to get all positions for a potential X pattern from a center point
let getXPositions centerPos =
    let topLeft = { Row = centerPos.Row - 1; Col = centerPos.Col - 1 }
    let topRight = { Row = centerPos.Row - 1; Col = centerPos.Col + 1 }
    let bottomLeft = { Row = centerPos.Row + 1; Col = centerPos.Col - 1 }
    let bottomRight = { Row = centerPos.Row + 1; Col = centerPos.Col + 1 }
    
    [
        // Possible MAS combinations for each diagonal
        ([topLeft; centerPos; bottomRight], [topRight; centerPos; bottomLeft])
    ]

// Function to check if an X-MAS pattern exists at a given center position
let isXMASPattern grid centerPos =
    getXPositions centerPos
    |> List.exists (fun (path1, path2) ->
        let allPositionsValid = 
            List.append path1 path2 
            |> List.forall (isInBounds grid)
            
        allPositionsValid && 
        checkMAS grid path1 && 
        checkMAS grid path2)

// Function to count all X-MAS patterns in the grid
let countXMASPatterns grid =
    let rows = Array.length grid
    let cols = Array.length grid.[0]
    
    seq {
        for row in 1 .. (rows - 2) do
            for col in 1 .. (cols - 2) do
                let centerPos = { Row = row; Col = col }
                if isXMASPattern grid centerPos then
                    yield 1
    } |> Seq.sum

[<EntryPoint>]
let main argv =
    // Read the input file
    let input = System.IO.File.ReadAllText("xmas_input.txt")
    
    // Parse the grid and count X-MAS patterns
    let grid = parseGrid input
    let count = countXMASPatterns grid
    
    printfn "Number of X-MAS patterns found: %d" count
    0