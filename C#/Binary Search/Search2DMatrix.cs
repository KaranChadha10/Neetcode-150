/*
PROBLEM: Search a 2D Matrix
You are given an m x n integer matrix with the following two properties:
- Each row is sorted in non-decreasing order.
- The first integer of each row is greater than the last integer of the previous row.

Given an integer target, return true if target is in matrix or false otherwise.

You must write a solution in O(log(m * n)) time complexity.

Example 1:
Input: matrix = [[1,3,5,7],[10,11,16,20],[23,30,34,60]], target = 3
Output: true

Example 2:
Input: matrix = [[1,3,5,7],[10,11,16,20],[23,30,34,60]], target = 13
Output: false

Constraints:
- m == matrix.length
- n == matrix[i].length
- 1 <= m, n <= 100
- -10^4 <= matrix[i][j], target <= 10^4
*/

public class Solution
{
    public bool SearchMatrix(int[][] matrix, int target)
    {
        int ROWS = matrix.Length, COLS = matrix[0].Length; // Get matrix dimensions

        // Treat 2D matrix as 1D sorted array for binary search
        int l = 0, r = ROWS * COLS - 1; // Left pointer at start, right at end
        
        while (l <= r) // Standard binary search loop
        {
            int m = l + (r - l) / 2; // Calculate middle index (avoids overflow)
            
            // Convert 1D index to 2D coordinates
            int row = m / COLS; // Row index in matrix
            int col = m % COLS; // Column index in matrix

            // Compare target with middle element
            if (target > matrix[row][col])
            {
                l = m + 1; // Target is larger, search right half
            }
            else if (target < matrix[row][col])
            {
                r = m - 1; // Target is smaller, search left half
            }
            else
            {
                return true; // Found target in matrix
            }
        }
        return false; // Target not found in matrix
    }
}

/*
TIME COMPLEXITY: O(log(m * n))
- Binary search on m * n total elements
- Each iteration eliminates half of search space
- Number of iterations = log₂(m * n)
- Overall: O(log(m * n)) - meets problem requirement

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers and variables
- No recursion or additional data structures
- Space usage independent of matrix size

ALGORITHM EXPLANATION:
- Treat 2D matrix as virtual 1D sorted array
- Use binary search on this "flattened" array
- Convert between 1D index and 2D coordinates on-the-fly
- Never actually flatten the matrix (memory efficient)

KEY INSIGHTS:
- Matrix properties guarantee it can be treated as one sorted array
- Each row sorted + first element of row > last of previous row
- This creates a fully sorted sequence when read row by row
- Binary search works perfectly on this sorted sequence

1D TO 2D COORDINATE CONVERSION:
- Given 1D index 'm' in virtual array
- Row = m / COLS (integer division)
- Col = m % COLS (modulo/remainder)

Example with 3x4 matrix (3 rows, 4 columns):
1D index: 0  1  2  3  4  5  6  7  8  9  10  11
2D coord: (0,0)(0,1)(0,2)(0,3)(1,0)(1,1)(1,2)(1,3)(2,0)(2,1)(2,2)(2,3)

VIRTUAL FLATTENING CONCEPT:
Matrix:          Virtual 1D Array:
[1,  3,  5,  7]  [1, 3, 5, 7, 10, 11, 16, 20, 23, 30, 34, 60]
[10, 11, 16, 20]  ↑                                         ↑
[23, 30, 34, 60]  l                                         r

EXAMPLE TRACE with matrix = [[1,3,5,7],[10,11,16,20],[23,30,34,60]], target = 3:

Initial: l=0, r=11 (total 12 elements)

Iteration 1:
- m = 0 + (11-0)/2 = 5
- row = 5/4 = 1, col = 5%4 = 1
- matrix[1][1] = 11
- target=3 < 11 → search left half
- r = 5-1 = 4

Iteration 2:
- l=0, r=4
- m = 0 + (4-0)/2 = 2
- row = 2/4 = 0, col = 2%4 = 2
- matrix[0][2] = 5
- target=3 < 5 → search left half
- r = 2-1 = 1

Iteration 3:
- l=0, r=1
- m = 0 + (1-0)/2 = 0
- row = 0/4 = 0, col = 0%4 = 0
- matrix[0][0] = 1
- target=3 > 1 → search right half
- l = 0+1 = 1

Iteration 4:
- l=1, r=1
- m = 1 + (1-1)/2 = 1
- row = 1/4 = 0, col = 1%4 = 1
- matrix[0][1] = 3
- target=3 == 3 → found!
- Return true ✓

INDEX CALCULATION EXAMPLE:
For matrix[1][2] (10th element overall):
- 1D index = row * COLS + col = 1 * 4 + 2 = 6
- Verify: 6 / 4 = 1 (row), 6 % 4 = 2 (col) ✓

OVERFLOW PREVENTION:
- Use m = l + (r - l) / 2 instead of m = (l + r) / 2
- Prevents integer overflow when l + r > INT_MAX
- Both formulas mathematically equivalent for valid indices

WHY NOT SEARCH ROW THEN COLUMN:
- Two binary searches: O(log m) + O(log n)
- This approach: One binary search: O(log(m*n))
- log(m*n) = log m + log n (mathematically equivalent)
- Single search is cleaner and more elegant

ALTERNATIVE APPROACHES:
1. Linear search: O(m * n) - too slow
2. Binary search row, then column: O(log m + log n) - works but two steps
3. This approach: O(log(m*n)) - optimal, single elegant search

EDGE CASES HANDLED:
- Single element matrix: l=r=0, works correctly
- Target smaller than all elements: r becomes -1, returns false
- Target larger than all elements: l exceeds r, returns false
- Target at boundaries (first/last element): found correctly

MATRIX PROPERTY IMPORTANCE:
- Sorted rows + first of row > last of previous
- Creates guarantee of complete sorted order
- Without this property, different algorithm needed
- This property enables virtual 1D treatment

WHY THIS IS ELEGANT:
- Never actually flattens matrix (memory efficient)
- Single pass binary search
- Simple coordinate conversion
- Optimal O(log(m*n)) time
- Clean and readable code

PATTERN RECOGNITION:
- Matrix with sorted rows and sorted columns (row-major order)
- Can be treated as virtual 1D sorted array
- Apply standard binary search with coordinate conversion
*/