/*
PROBLEM: Valid Sudoku
Determine if a 9 x 9 Sudoku board is valid. Only the filled cells need to be validated 
according to the following rules:

1. Each row must contain the digits 1-9 without repetition.
2. Each column must contain the digits 1-9 without repetition.
3. Each of the nine 3 x 3 sub-boxes of the grid must contain the digits 1-9 without repetition.

Note:
- A Sudoku board (partially filled) could be valid but is not necessarily solvable.
- Only the filled cells need to be validated according to the mentioned rules.

Example 1:
Input: board = 
[["5","3",".",".","7",".",".",".","."]
,["6",".",".","1","9","5",".",".","."]
,[".","9","8",".",".",".",".","6","."]
,["8",".",".",".","6",".",".",".","3"]
,["4",".",".","8",".","3",".",".","1"]
,["7",".",".",".","2",".",".",".","6"]
,[".","6",".",".",".",".","2","8","."]
,[".",".",".","4","1","9",".",".","5"]
,[".",".",".",".","8",".",".","7","9"]]
Output: true

Example 2:
Input: board = 
[["8","3",".",".","7",".",".",".","."]
,["6",".",".","1","9","5",".",".","."]
,[".","9","8",".",".",".",".","6","."]
,["8",".",".",".","6",".",".",".","3"]
,["4",".",".","8",".","3",".",".","1"]
,["7",".",".",".","2",".",".",".","6"]
,[".","6",".",".",".",".","2","8","."]
,[".",".",".","4","1","9",".",".","5"]
,[".",".",".",".","8",".",".","7","9"]]
Output: false
Explanation: Same as Example 1, except with the 5 in the top left corner being modified to 8. 
Since there are two 8's in the top left 3x3 sub-box, it is invalid.

Constraints:
- board.length == 9
- board[i].length == 9
- board[i][j] is a digit 1-9 or '.'.
*/

public class Solution {
    public bool IsValidSudoku(char[][] board) {
        // Track seen digits in each column using column index as key
        Dictionary<int, HashSet<char>> cols = new Dictionary<int, HashSet<char>>();
        
        // Track seen digits in each row using row index as key
        Dictionary<int, HashSet<char>> rows = new Dictionary<int, HashSet<char>>();
        
        // Track seen digits in each 3x3 box using "row/3,col/3" as key
        Dictionary<string, HashSet<char>> squares = new Dictionary<string, HashSet<char>>();

        // Iterate through every cell in the 9x9 board
        for (int r = 0; r < 9; r++) {
            for (int c = 0; c < 9; c++) {
                // Skip empty cells (represented by '.')
                if (board[r][c] == '.') continue;

                // Calculate which 3x3 box this cell belongs to (0,0 to 2,2)
                string squareKey = (r / 3) + "," + (c / 3);

                // Check if current digit already exists in same row, column, or 3x3 box
                if ((rows.ContainsKey(r) && rows[r].Contains(board[r][c])) ||
                    (cols.ContainsKey(c) && cols[c].Contains(board[r][c])) ||
                    (squares.ContainsKey(squareKey) && squares[squareKey].Contains(board[r][c]))) {
                    return false; // Duplicate found - invalid sudoku
                }

                // Initialize HashSet for current row if not exists
                if (!rows.ContainsKey(r)) rows[r] = new HashSet<char>();
                
                // Initialize HashSet for current column if not exists
                if (!cols.ContainsKey(c)) cols[c] = new HashSet<char>();
                
                // Initialize HashSet for current 3x3 box if not exists
                if (!squares.ContainsKey(squareKey)) squares[squareKey] = new HashSet<char>();

                // Add current digit to respective tracking sets
                rows[r].Add(board[r][c]);
                cols[c].Add(board[r][c]);
                squares[squareKey].Add(board[r][c]);
            }
        }
        return true; // No duplicates found - valid sudoku
    }
}

/*
TIME COMPLEXITY: O(1) or O(81) = O(1)
- We always traverse exactly 81 cells (9×9 board)
- Each HashSet operation (Contains, Add) is O(1) average case
- Since the board size is fixed, this is constant time

SPACE COMPLEXITY: O(1) or O(81) = O(1)
- At most 81 digits can be stored across all HashSets
- 9 rows × 9 possible digits = 81 maximum entries in rows dictionary
- 9 cols × 9 possible digits = 81 maximum entries in cols dictionary  
- 9 boxes × 9 possible digits = 81 maximum entries in squares dictionary
- Since the space is bounded by the fixed board size, this is constant space

ALGORITHM EXPLANATION:
- Use three hash maps to track digits seen in each row, column, and 3×3 box
- For 3×3 boxes: map coordinates (r,c) to box identifier (r/3, c/3)
- As we scan each cell, check if digit already exists in corresponding row/col/box
- If duplicate found, return false immediately
- If no duplicates after full scan, the sudoku is valid
*/