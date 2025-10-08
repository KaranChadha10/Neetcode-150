/*
PROBLEM: Generate Parentheses
Given n pairs of parentheses, write a function to generate all combinations of well-formed parentheses.

Example 1:
Input: n = 3
Output: ["((()))","(()())","(())()","()(())","()()()"]

Example 2:
Input: n = 1
Output: ["()"]

Example 3:
Input: n = 2
Output: ["(())","()()"]

Constraints:
- 1 <= n <= 8
*/

public class Solution
{
    // Recursive backtracking function to build valid parentheses combinations
    public void Backtrack(int openN, int closedN, int n, List<string> res, string stack)
    {
        // Base case: if we've used n open and n closed parentheses, we have a valid combination
        if (openN == closedN && openN == n)
        {
            res.Add(stack); // Add complete valid combination to result
            return;
        }

        // We can add an open parenthesis if we haven't used all n yet
        if (openN < n)
        {
            Backtrack(openN + 1, closedN, n, res, stack + '('); // Recursively add '('
        }

        // We can add a close parenthesis only if it doesn't exceed open count
        // This ensures we never have more ')' than '(' at any point (maintains validity)
        if (closedN < openN)
        {
            Backtrack(openN, closedN + 1, n, res, stack + ')'); // Recursively add ')'
        }
    }

    // Main function to generate all valid parentheses combinations
    public List<string> GenerateParenthesis(int n)
    {
        List<string> res = new List<string>(); // Store all valid combinations
        string stack = ""; // Current combination being built
        Backtrack(0, 0, n, res, stack); // Start backtracking with 0 open, 0 closed
        return res; // Return all valid combinations
    }
}

/*
TIME COMPLEXITY: O(4^n / √n) or Catalan number C(n)
- Number of valid parentheses sequences = nth Catalan number ≈ 4^n / (n√n)
- For each valid sequence, we perform O(n) work to build the string
- Total: O(n × 4^n / √n) which simplifies to O(4^n / √n)
- Exact count: C(n) = (2n)! / ((n+1)! × n!)

SPACE COMPLEXITY: O(n)
- Recursion depth: O(n) - at most 2n recursive calls deep (n open + n closed)
- String concatenation creates intermediate strings: O(n) per path
- Result list stores all valid combinations but doesn't count as auxiliary space
- Overall: O(n) for recursion stack and string building

ALGORITHM EXPLANATION:
- Use backtracking to build valid parentheses strings character by character
- At each step, decide whether to add '(' or ')' based on validity rules
- Prune invalid branches early (don't explore paths that can't lead to valid solutions)
- Build only valid combinations by enforcing constraints during construction

KEY INSIGHTS:
- Valid parentheses rules:
  1. At any point, number of ')' cannot exceed number of '('
  2. Total '(' must equal n
  3. Total ')' must equal n
- Backtracking explores all possibilities while pruning invalid branches
- We build strings incrementally, adding one character at a time

BACKTRACKING CONSTRAINTS:
1. Add '(' only if openN < n (haven't used all open parentheses yet)
2. Add ')' only if closedN < openN (maintain validity: never more close than open)
3. Stop when openN == closedN == n (complete valid string formed)

DECISION TREE for n=2:
                    ""
                    |
                   "("                    (openN=1, closedN=0)
                 /    \
              "(("     "()"              (openN=2,1  closedN=0,1)
              /          \
           "(()"        "()()"          (valid)
            /
         "(())"                          (valid)

EXAMPLE TRACE with n = 2:
Start: openN=0, closedN=0, stack=""
├─ Add '(': openN=1, closedN=0, stack="("
│  ├─ Add '(': openN=2, closedN=0, stack="(("
│  │  └─ Add ')': openN=2, closedN=1, stack="(()"
│  │     └─ Add ')': openN=2, closedN=2, stack="(())" ✓ VALID
│  └─ Add ')': openN=1, closedN=1, stack="()"
│     └─ Add '(': openN=2, closedN=1, stack="()("
│        └─ Add ')': openN=2, closedN=2, stack="()()" ✓ VALID

Result: ["(())", "()()"]

WHY BACKTRACKING WORKS:
- Explores all possible valid sequences systematically
- Prunes invalid branches early (e.g., never tries ")(" at the start)
- Guarantees to find all solutions without generating invalid ones
- More efficient than generating all 2^(2n) possible strings and filtering

VALIDATION INVARIANT:
- At every step: closedN <= openN <= n
- This invariant ensures we only build valid parentheses strings
- When openN == closedN == n, we have a complete valid string

OPTIMIZATION TECHNIQUES:
- Early pruning: don't add ')' if it would make string invalid
- Don't generate invalid strings at all (vs generate then filter)
- String concatenation could be optimized with StringBuilder for very large n

CATALAN NUMBER CONNECTION:
- Number of valid parentheses sequences = Catalan number C(n)
- C(1) = 1: "()"
- C(2) = 2: "(())", "()()"
- C(3) = 5: "((()))", "(()())", "(())()", "()(())", "()()()"
- C(n) = C(0)C(n-1) + C(1)C(n-2) + ... + C(n-1)C(0)

EDGE CASES HANDLED:
- n = 1: Generates "()" correctly
- n = 0: Would generate "" (empty string) if allowed
- Small n (1-8): Works efficiently within constraint
*/