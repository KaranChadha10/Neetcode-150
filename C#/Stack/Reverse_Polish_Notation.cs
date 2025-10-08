/*
PROBLEM: Evaluate Reverse Polish Notation
You are given an array of strings tokens that represents an arithmetic expression in 
Reverse Polish Notation (RPN).

Evaluate the expression and return an integer that represents the value of the expression.

Note that:
- The valid operators are '+', '-', '*', and '/'.
- Each operand may be an integer or another expression.
- The division between two integers always truncates toward zero.
- There will not be any division by zero.
- The input represents a valid arithmetic expression in reverse polish notation.
- The answer and all the intermediate calculations can be represented in a 32-bit integer.

Example 1:
Input: tokens = ["2","1","+","3","*"]
Output: 9
Explanation: ((2 + 1) * 3) = 9

Example 2:
Input: tokens = ["4","13","5","/","+"]
Output: 6
Explanation: (4 + (13 / 5)) = 6

Example 3:
Input: tokens = ["10","6","9","3","+","-11","*","/","*","17","+","5","+"]
Output: 22
Explanation: ((10 * (6 / ((9 + 3) * -11))) + 17) + 5
= ((10 * (6 / (12 * -11))) + 17) + 5
= ((10 * (6 / -132)) + 17) + 5
= ((10 * 0) + 17) + 5
= (0 + 17) + 5
= 17 + 5
= 22

Constraints:
- 1 <= tokens.length <= 10^4
- tokens[i] is either an operator: "+", "-", "*", or "/", or an integer in the range [-200, 200].
*/

public class Solution
{
    public int EvalRPN(string[] tokens)
    {
        Stack<int> stack = new Stack<int>(); // Stack to store operands
        
        foreach (string c in tokens) // Process each token in order
        {
            if (c == "+") // Addition operator
            {
                // Pop two operands, add them, and push result back
                stack.Push(stack.Pop() + stack.Pop());
            }
            else if (c == "-") // Subtraction operator
            {
                int a = stack.Pop(); // Second operand (right side)
                int b = stack.Pop(); // First operand (left side)
                stack.Push(b - a); // Order matters: b - a (not a - b)
            }
            else if (c == "*") // Multiplication operator
            {
                // Pop two operands, multiply them, and push result back
                stack.Push(stack.Pop() * stack.Pop());
            }
            else if (c == "/") // Division operator
            {
                int a = stack.Pop(); // Second operand (divisor)
                int b = stack.Pop(); // First operand (dividend)
                stack.Push((int)(double)b / a); // Integer division truncating toward zero
            }
            else // Token is a number (operand)
            {
                stack.Push(int.Parse(c)); // Parse string to integer and push onto stack
            }
        }
        
        return stack.Pop(); // Final result is the only element left in stack
    }
}

/*
TIME COMPLEXITY: O(n)
- Single pass through the tokens array with n elements
- Each token is processed exactly once
- Stack operations (Push, Pop) are O(1)
- String parsing (int.Parse) is O(1) for valid integer strings
- Overall: O(n) where n is the number of tokens

SPACE COMPLEXITY: O(n)
- Stack can store at most n operands in worst case
- Example: all tokens are numbers before any operator
- In practice, stack size is limited by valid RPN structure
- Overall: O(n) in worst case

ALGORITHM EXPLANATION:
- Reverse Polish Notation (RPN): operators come after their operands
- Process tokens left to right:
  - Number: push onto stack
  - Operator: pop two operands, apply operation, push result
- Final stack contains single value (the result)

KEY INSIGHTS:
- RPN eliminates need for parentheses and operator precedence rules
- Stack naturally maintains operand order for evaluation
- Each operator consumes exactly two operands and produces one result
- Left-to-right processing with stack gives correct evaluation order

RPN EVALUATION RULES:
1. Operands are pushed onto stack
2. When operator encountered, pop required number of operands (2 for binary operators)
3. Apply operator to operands in correct order
4. Push result back onto stack
5. Continue until all tokens processed

OPERATOR ORDER IMPORTANCE:
- Addition (+) and Multiplication (*): Commutative, order doesn't matter
  - a + b = b + a, so stack.Pop() + stack.Pop() works
- Subtraction (-) and Division (/): Non-commutative, order matters
  - a - b ≠ b - a, must carefully track first (b) and second (a) operands
  - Pop order: second operand first (a), then first operand (b)
  - Operation: b - a or b / a

EXAMPLE TRACE with ["2","1","+","3","*"]:
- "2": push 2 → stack: [2]
- "1": push 1 → stack: [2, 1]
- "+": pop 1, pop 2, push 2+1=3 → stack: [3]
- "3": push 3 → stack: [3, 3]
- "*": pop 3, pop 3, push 3*3=9 → stack: [9]
- Result: 9

EXAMPLE TRACE with ["4","13","5","/","+"]:
- "4": push 4 → stack: [4]
- "13": push 13 → stack: [4, 13]
- "5": push 5 → stack: [4, 13, 5]
- "/": pop 5 (a), pop 13 (b), push 13/5=2 → stack: [4, 2]
- "+": pop 2, pop 4, push 4+2=6 → stack: [6]
- Result: 6

DIVISION TRUNCATION:
- Cast to double then back to int ensures truncation toward zero
- C# integer division naturally truncates, but this makes it explicit
- Handles both positive and negative results correctly

EDGE CASES HANDLED:
- Single number: returns that number
- Negative numbers: int.Parse handles negative strings
- Division by zero: problem states this won't occur
- Large expressions: stack handles arbitrary depth

WHY RPN IS USEFUL:
- No need for parentheses or operator precedence
- Simpler to evaluate programmatically (just use a stack)
- Commonly used in calculators and programming language implementations
- Unambiguous expression representation
*/