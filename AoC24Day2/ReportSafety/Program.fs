open System
open System.IO

// Function to check if a sequence is strictly increasing with adjacent differences within bounds
let isStrictlyIncreasing levels =
    levels
    |> Seq.pairwise
    |> Seq.forall (fun (a, b) -> b > a && b - a <= 3)

// Function to check if a sequence is strictly decreasing with adjacent differences within bounds
let isStrictlyDecreasing levels =
    levels
    |> Seq.pairwise
    |> Seq.forall (fun (a, b) -> b < a && a - b <= 3)

// Function to determine if a report is safe
let isSafeReport levels =
    match levels with
    | [] -> false // Empty rows are not considered safe
    | _ -> isStrictlyIncreasing levels || isStrictlyDecreasing levels

//Part 2 Enhancement
// Function to check if removing a single level makes the report safe
let isSafeWithDampener levels =
    match levels with
    | [] -> false
    | _ when isSafeReport levels -> true // Already safe
    | _ ->
        levels
        |> List.mapi (fun idx _ -> List.removeAt idx levels) // Remove each level one by one
        |> List.exists isSafeReport // Check if any modified report is safe

// Enhanced main function to process the file and count safe reports (with Problem Dampener)
let countSafeReportsWithDampener filePath =
    File.ReadAllLines(filePath)
    |> Array.map (fun line -> line.Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map int |> List.ofArray) // Handle varying column counts
    |> Array.filter isSafeWithDampener // Apply the enhanced safety rule
    |> Array.length // Count the number of safe reports

// Entry point of the program
[<EntryPoint>]
let main argv =
    let filePath = "input.txt" // File path is relative to the project root
    if not (File.Exists filePath) then
        printfn "Error: input.txt not found in the project root."
        1
    else
        try
            let safeReportsCount = countSafeReportsWithDampener filePath
            printfn "Number of safe reports: %d" safeReportsCount
            0
        with
        | ex ->
            printfn "An error occurred: %s" ex.Message
            1
