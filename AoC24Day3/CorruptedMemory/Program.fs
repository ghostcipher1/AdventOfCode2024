open System
open System.IO
open System.Text.RegularExpressions

/// Parses a given string input representing a sequence of operations and computes
/// the sum of products of multiplication operations ('mul(a,b)') that are enabled.
/// A multiplication operation is considered enabled if a 'do()' instruction has been
/// encountered most recently compared to a 'don't()' instruction, which disables it.
/// The input can contain noise and invalid instructions that are ignored. Only
/// recognized instructions ('do()', 'don't()', and 'mul(a,b)') are processed.
///
/// The function first uses regular expressions to extract valid instructional segments
/// from the input string. Then, it processes each instruction appropriately:
/// - 'do()' enables future multiplication operations.
/// - 'don't()' disables future multiplication operations.
/// - 'mul(a,b)' multiplies the integers 'a' and 'b' and adds the result to the total sum
///   if multiplications are currently enabled; otherwise, it ignores the operation.
///
/// Parameters:
///  - input: A string containing potential valid and invalid instruction substrings.
///
/// Returns:
///  An integer representing the sum of all products of enabled multiplication operations
///  contained in the input.
module MemoryProcessor =

    /// Parses a given input string to identify and process instructions for enabling or disabling
    /// multiplication operations. The function calculates the sum of products from enabled multiplication
    /// instructions while skipping those marked as disabled. Valid instructions are identified using specific
    /// patterns: `do()`, `don't()`, and `mul(a,b)`, where `a` and `b` are integers. When `do()` is encountered,
    /// multiplication operations become enabled; `don't()` disables them. Only enabled `mul(a,b)` instructions
    /// contribute their product to the total sum. Non-instructional text is ignored.
    let parseAndSumEnabledMultiplications (input: string) =
        // Regular expressions for different instructions
        let instructionRegex = Regex(@"(do\(\)|don't\(\)|mul\(\d+,\d+\))")
        let mulRegex = Regex(@"mul\((\d+),(\d+)\)")
        let doRegex = Regex(@"do\(\)")
        let dontRegex = Regex(@"don't\(\)")

        // State to track whether multiplication is enabled
        let mutable mulEnabled = true
        let mutable totalSum = 0

        // Function to process individual instructions
        let processInstruction (instruction: string) =
            printfn $"Processing: %s{instruction} (mulEnabled: %b{mulEnabled})"
            if doRegex.IsMatch(instruction) then
                mulEnabled <- true
                printfn $"do() encountered, mulEnabled: %b{mulEnabled}"
            elif dontRegex.IsMatch(instruction) then
                mulEnabled <- false
                printfn $"don't() encountered, mulEnabled: %b{mulEnabled}"
            elif mulRegex.IsMatch(instruction) then
                if mulEnabled then
                    let regexMatch = mulRegex.Match(instruction)
                    let a = int (regexMatch.Groups.[1].Value)
                    let b = int (regexMatch.Groups.[2].Value)
                    let product = a * b
                    printfn $"mul(%d{a},%d{b}) = %d{product} added to total"
                    totalSum <- totalSum + product
                else
                    printfn $"mul instruction ignored due to mulEnabled: %b{mulEnabled}"

        // Extract valid instructions using regex
        let matches = instructionRegex.Matches(input)
        for m in matches do
            processInstruction m.Value

        totalSum

/// Entry point for the F# application.
/// Processes a given corrupted memory string to compute the sum of enabled multiplication instructions.
/// Utilizes regular expressions to identify and handle specific instructions: enabling or disabling the
/// multiplication process and computing the product of two numbers if multiplication is currently enabled.
[<EntryPoint>]
let main argv =
    // Example corrupted memory input. Should result in 48
    //let corruptedMemory = 
    //    "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"

    // Compute the sum of enabled multiplications
    //let result = MemoryProcessor.parseAndSumEnabledMultiplications corruptedMemory

    // Print the result
    //printfn $"Sum of enabled multiplications: %d{result}"

    // Exit program
    //0

    // Read the corrupted memory input from the file
    let corruptedMemory = 
        try
            File.ReadAllText("corrupted_memory.txt")
        with
        | :? FileNotFoundException ->
            printfn "Error: The file 'corrupted_memory.txt' was not found."
            Environment.Exit(1)
            ""
        | ex ->
            printfn $"An error occurred: %s{ex.Message}"
            Environment.Exit(1)
            ""

    // Compute the sum of enabled multiplications
    let result = MemoryProcessor.parseAndSumEnabledMultiplications corruptedMemory

    // Print the result
    printfn $"Sum of enabled multiplications: %d{result}"

    // Exit program
    0