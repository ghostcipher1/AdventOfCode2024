# XMAS Puzzle 2 Documentation

## Overview

The XMAS Puzzle 2 application is designed to identify and count "X-MAS" patterns within a two-dimensional grid. An X-MAS pattern is defined as an 'X' shape where both diagonals spell "MAS" or "SAM".

## Features

- **Grid Parsing:** Reads input from a text file and converts it into a two-dimensional character array.
- **Pattern Detection:** Scans the grid to identify all valid X-MAS patterns.
- **Counting Mechanism:** Counts and reports the total number of X-MAS patterns found in the grid.

## Getting Started

### Prerequisites

- **.NET SDK:** Ensure that the .NET SDK is installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).

### Installation

1. **Clone the Repository:**

    git clone https://github.com/your-repo/xmas-puzzle2.git


2. **Navigate to the Project Directory:**

    cd xmas-puzzle2


### Usage

1. **Prepare the Input File:**

    - Create a file named `xmas_input.txt` in the project directory.
    - Populate it with the grid data. Each line represents a row in the grid.

2. **Build and Run the Application:**
  
   dotnet build
   dotnet run


3. **View the Results:**

    - The application will output the number of X-MAS patterns found in the grid.

## Example

Given the following `xmas_input.txt`:


The application will identify and count the X-MAS patterns based on the defined criteria.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
