module AOC2024.Day7P2

open System
open System.IO
open System.Collections.Generic

// Function to evaluate an expression given numbers and operators
let evaluateExpression (numbers: int64 list) (operators: string list) : int64 =
    let mutable result = numbers.Head
    for i in 0 .. (operators.Length - 1) do
        match operators.[i] with
        | "+" -> result <- result + numbers.[i + 1]
        | "*" -> result <- result * numbers.[i + 1]
        | "||" -> result <- int64 (string result + string numbers.[i + 1])
        | _ -> failwith "Unknown operator"
    result

// Function to check if a test value can be formed
let canFormTestValue (testValue: int64) (numbers: int64 list) : bool =
    let n = numbers.Length
    if n = 1 then
        numbers.[0] = testValue
    else
        let operators = ["+"; "*"; "||"]
        let rec generateCombinations depth acc =
            if depth = 0 then acc
            else generateCombinations (depth - 1) (acc |> List.collect (fun x -> operators |> List.map (fun op -> x @ [op])))

        let operatorCombinations = generateCombinations (n - 1) [[]]
        operatorCombinations
        |> List.exists (fun ops -> evaluateExpression numbers ops = testValue)

// Function to calculate the total calibration result
let totalCalibrationResult (equations: string list) : int64 =
    equations
    |> List.fold (fun total line ->
        let parts = line.Split(": ", StringSplitOptions.RemoveEmptyEntries)
        let testValue = int64 parts.[0]
        let numbers = parts.[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map int64 |> Array.toList

        if canFormTestValue testValue numbers then
            total + testValue
        else
            total
    ) 0

// Read the puzzle input from Day7.txt
let solve input = (input |> totalCalibrationResult, "")

// Calculate the total calibration result
//let result = totalCalibrationResult equations
//printfn "%d" result


