module Day7
open System


let evaluateExpression (numbers: int64 list) (operators: char list) : int64 =
    List.fold2 (fun acc num op -> 
        match op with
        | '+' -> acc + num
        | '*' -> acc * num
        | _ -> acc) (List.head numbers) (List.tail numbers) operators

let generateOperatorCombinations length =
    let rec combinations n acc =
        if n = 0 then acc
        else combinations (n - 1) ([for op in ['+'; '*'] do for comb in acc do yield op :: comb])
    combinations length [[]]

let findCalibrationTotal (lines: string list) : int64 =
    lines
    |> List.fold (fun totalCalibrationResult line ->
        let parts = line.Split(':')
        let target = Int64.Parse(parts.[0].Trim())
        let numbers = parts.[1].Trim().Split(' ') |> Array.map Int64.Parse |> Array.toList

        let operatorCombinations = generateOperatorCombinations (numbers.Length - 1)

        let valid = operatorCombinations |> List.exists (fun operators -> evaluateExpression numbers operators = target)

        if valid then totalCalibrationResult + target
        else 
            printfn "No valid operator combinations for target %d with numbers %A" target numbers
            totalCalibrationResult
    ) 0


// Calculate the total calibration result
let solve input = (input |> findCalibrationTotal, "")

//let result = (findCalibrationTotal |> data)
//printfn "Total Calibration Result: %d" input
