/*
PROBLEM: Valid Parentheses
Given a string s containing just the characters '(', ')', '{', '}', '[' and ']', 
determine if the input string is valid.

An input string is valid if:
1. Open brackets must be closed by the same type of brackets.
2. Open brackets must be closed in the correct order.
3. Every close bracket has a corresponding open bracket of the same type.

Example 1:
Input: s = "()"
Output: true

Example 2:
Input: s = "()[]{}"
Output: true

Example 3:
Input: s = "(]"
Output: false

Example 4:
Input: s = "([)]"
Output: false

Example 5:
Input: s = "{[]}"
Output: true

Constraints:
- 1 <= s.length <= 10^4
- s consists of parentheses only '()[]{}'.
*/

public class Solution
{
    public bool IsValid(string s)
    {
        Stack<char> stack = new Stack<char>(); // Stack to keep track of opening brackets
        
        // Map each closing bracket to its corresponding opening bracket
        Dictionary<char, char> closeToOpen = new Dictionary<char, char>
        {
            {')', '('}, // Closing parenthesis maps to opening parenthesis
            {']', '['}, // Closing square bracket maps to opening square bracket
            {'}', '{'} // Closing curly brace maps to opening curly brace
        };

        foreach (char c in s) // Process each character in the string
        {
            if (closeToOpen.ContainsKey(c)) // Current character is a closing bracket
            {
                // Check if stack has matching opening bracket on top
                if (stack.Count > 0 && stack.Peek() == closeToOpen[c])
                {
                    stack.Pop(); // Remove the matched opening bracket
                }
                else
                {
                    return false; // No matching opening bracket or wrong type
                }
            }
            else // Current character is an opening bracket
            {
                stack.Push(c); // Push opening bracket onto stack
            }
        }
        
        // Valid if all opening brackets have been matched (stack is empty)
        return stack.Count == 0;
    }
}

/*
TIME COMPLEXITY: O(n)
- Single pass through the string with n characters
- Each character is processed exactly once
- Stack operations (Push, Pop, Peek) are O(1)
- Dictionary lookup (ContainsKey) is O(1) on average
- Overall: O(n) where n is the length of the input string

SPACE COMPLEXITY: O(n)
- Stack can store at most n/2 opening brackets in worst case
- Example: "(((((" would push all characters onto stack
- Dictionary uses constant space (only 3 key-value pairs)
- Overall: O(n) in worst case

ALGORITHM EXPLANATION:
- Use stack to keep track of unmatched opening brackets
- When encountering opening bracket: push onto stack
- When encountering closing bracket: check if it matches top of stack
- Valid parentheses string has all brackets properly matched and nested

KEY INSIGHTS:
- Stack naturally handles the "last opened, first closed" property of valid parentheses
- LIFO (Last In, First Out) behavior matches the nesting requirement
- Each closing bracket must match the most recent unmatched opening bracket
- Final stack must be empty (all opening brackets were matched)

VALIDATION RULES:
1. Correct type matching: ')' matches '(', ']' matches '[', '}' matches '{'
2. Correct order: inner brackets must close before outer brackets
3. Complete matching: every opening bracket has a corresponding closing bracket

EXAMPLE TRACE with s = "{[()]}":
- '{': opening bracket → push to stack: ['{']
- '[': opening bracket → push to stack: ['{', '[']  
- '(': opening bracket → push to stack: ['{', '[', '(']
- ')': closing bracket, matches '(' on top → pop: ['{', '[']
- ']': closing bracket, matches '[' on top → pop: ['{']
- '}': closing bracket, matches '{' on top → pop: []
- Stack is empty → return true

EXAMPLE TRACE with s = "([)]":
- '(': opening bracket → push to stack: ['(']
- '[': opening bracket → push to stack: ['(', '[']
- ')': closing bracket, should match '[' but matches '(' → return false

EDGE CASES HANDLED:
- Empty string: stack.Count == 0 returns true (valid)
- Only opening brackets: stack not empty at end → false
- Only closing brackets: stack.Count == 0 check fails → false  
- Mismatched types: closeToOpen[c] comparison fails → false
- Wrong nesting order: stack top doesn't match → false

WHY DICTIONARY APPROACH:
- Maps closing brackets to opening brackets for easy lookup
- Cleaner than multiple if-else statements for bracket type checking
- O(1) lookup time for determining correct opening bracket
- Makes code more readable and maintainable
*/