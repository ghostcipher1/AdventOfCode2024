module AOC2024.Day7
open System


let evaluateExpression (numbers: int64 list) (operators: char list) : int64 =
    let mutable result = List.head numbers
    for i in 0 .. operators.Length - 1 do
        match operators.[i] with
        | '+' -> result <- result + numbers.[i + 1]
        | '*' -> result <- result * numbers.[i + 1]
        | '|' -> result <- Int64.Parse(string result + string numbers.[i + 1])
        | _ -> ()
    result

let generateOperatorCombinations length =
    let rec combinations n acc =
        if n = 0 then acc
        else combinations (n - 1) ([for op in ['+'; '*'; '^'] do for comb in acc do yield op :: comb])
    combinations length [[]]

let findCalibrationTotal (lines: string list) : int64 =
    let mutable totalCalibrationResult = 0L

    for line in lines do
        let parts = line.Split(':')
        let target = Int64.Parse(parts.[0].Trim())
        let numbers = parts.[1].Trim().Split(' ') |> Array.map Int64.Parse |> Array.toList

        let operatorCombinations = generateOperatorCombinations (numbers.Length - 1)

        let mutable valid = false
        for operators in operatorCombinations do
        if evaluateExpression numbers operators = target then
                valid <- true
                totalCalibrationResult <- totalCalibrationResult + target
                printf "Valid operator combinations for %d " target        
            else
            if not valid then
                printfn "No valid operator combinations for target %d with numbers %A" target numbers

    totalCalibrationResult

// Calculate the total calibration result
let solve input = (input |> findCalibrationTotal, "")
