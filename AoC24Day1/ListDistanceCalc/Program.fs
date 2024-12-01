open System
open System.IO

/// Function to calculate the total distance between two lists
let calculateTotalDistance (leftList: int list) (rightList: int list) =
    // Sort both lists
    let sortedLeft = List.sort leftList
    let sortedRight = List.sort rightList

    // Pair up the elements and calculate the distances
    let distances = 
        List.zip sortedLeft sortedRight
        |> List.map (fun (left, right) -> abs (left - right))
    
    // Sum the distances
    List.sum distances

/// Function to calculate the similarity score
let calculateSimilarityScore (leftList: int list) (rightList: int list) =
    // Count the frequency of each number in the right list
    let rightFrequency = 
        rightList 
        |> List.groupBy id 
        |> List.map (fun (num, occurrences) -> num, List.length occurrences)
        |> Map.ofList

    // For each number in the left list, calculate its contribution to the similarity score
    leftList
    |> List.sumBy (fun num ->
        match Map.tryFind num rightFrequency with
        | Some count -> num * count // Multiply the number by its count in the right list
        | None -> 0                 // If the number is not in the right list, contribution is 0
    )

/// Function to read the file and extract left and right lists
let parseFileToLists (filePath: string) =
    // Check if the file exists
    if not (File.Exists(filePath)) then
        failwithf "Error: File '%s' not found." filePath

    // Read all lines and split into left and right lists
    let leftList = ResizeArray<int>()
    let rightList = ResizeArray<int>()

    File.ReadLines(filePath)
    |> Seq.iter (fun line ->
        match line.Split([| ' '; '\t' |], StringSplitOptions.RemoveEmptyEntries) with
        | [| left; right |] ->
            leftList.Add(int left)
            rightList.Add(int right)
        | _ -> failwithf "Error: Invalid file format in line: '%s'" line
    )

    leftList |> Seq.toList, rightList |> Seq.toList

[<EntryPoint>]
let main argv =
    try
        // Specify the file path
        let filePath = "large_list_no_format.txt"

        // Parse the file into left and right lists
        let leftList, rightList = parseFileToLists filePath

        // Calculate the total distance between the two lists
        let totalDistance = calculateTotalDistance leftList rightList

        // Calculate the similarity score between the two lists
        let similarityScore = calculateSimilarityScore leftList rightList

        // Output the results
        printfn "The total distance between the two lists is: %d" totalDistance
        printfn "The similarity score between the two lists is: %d" similarityScore
    with
    | ex -> printfn "An error occurred: %s" ex.Message

    0 // Return an integer exit code
