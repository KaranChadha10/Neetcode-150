/*
PROBLEM: Min Stack
Design a stack that supports push, pop, top, and retrieving the minimum element in constant time.

Implement the MinStack class:
- MinStack() initializes the stack object.
- void push(int val) pushes the element val onto the stack.
- void pop() removes the element on the top of the stack.
- int top() gets the top element of the stack.
- int getMin() retrieves the minimum element in the stack.

You must implement a solution with O(1) time complexity for each function.

Example 1:
Input:
["MinStack","push","push","push","getMin","pop","top","getMin"]
[[],[-2],[0],[-3],[],[],[],[]]

Output:
[null,null,null,null,-3,null,0,-2]

Explanation:
MinStack minStack = new MinStack();
minStack.push(-2);
minStack.push(0);
minStack.push(-3);
minStack.getMin(); // return -3
minStack.pop();
minStack.top();    // return 0
minStack.getMin(); // return -2

Constraints:
- -2^31 <= val <= 2^31 - 1
- Methods pop, top and getMin operations will always be called on non-empty stacks.
- At most 3 * 10^4 calls will be made to push, pop, top, and getMin.
*/

public class MinStack
{
    private Stack<int> stack;    // Main stack to store all values
    private Stack<int> minStack; // Auxiliary stack to track minimum values

    // Constructor: Initialize both stacks
    public MinStack()
    {
        stack = new Stack<int>();    // Stack for normal operations
        minStack = new Stack<int>(); // Stack to maintain minimum at each state
    }

    // Push value onto stack and update minimum tracking
    public void Push(int val)
    {
        stack.Push(val); // Push value to main stack
        
        // Calculate minimum: either current value or previous minimum, whichever is smaller
        val = Math.Min(val, minStack.Count == 0 ? val : minStack.Peek());
        
        minStack.Push(val); // Push current minimum to minStack
    }

    // Remove top element from both stacks
    public void Pop()
    {
        stack.Pop();    // Remove from main stack
        minStack.Pop(); // Remove corresponding minimum from minStack
    }

    // Return top element without removing it
    public int Top()
    {
        return stack.Peek(); // Return top of main stack
    }

    // Return current minimum element in O(1) time
    public int GetMin()
    {
        return minStack.Peek(); // Top of minStack is always current minimum
    }
}

/*
TIME COMPLEXITY:
- Push(int val): O(1) - constant time stack operations
- Pop(): O(1) - constant time stack operations
- Top(): O(1) - constant time peek operation
- GetMin(): O(1) - constant time peek operation
All operations achieve O(1) time complexity as required

SPACE COMPLEXITY: O(n)
- Main stack: stores n elements
- Min stack: stores n elements (one minimum for each stack state)
- Total: O(2n) = O(n) where n is the number of elements
- Trade-off: Extra O(n) space to achieve O(1) getMin operation

ALGORITHM EXPLANATION:
- Maintain two parallel stacks:
  1. Main stack: stores all pushed values normally
  2. Min stack: at each position, stores the minimum value from bottom to that position
- When pushing: store value in main stack, store current minimum in min stack
- When popping: remove from both stacks to maintain synchronization
- GetMin: simply peek at minStack top (always has current minimum)

KEY INSIGHTS:
- MinStack top always contains minimum of all elements currently in stack
- Both stacks grow and shrink together, staying synchronized
- Each element in minStack represents "minimum so far" at that point
- When we pop, we restore the previous minimum automatically

TWO-STACK SYNCHRONIZATION:
- Both stacks always have same number of elements
- Push: add to both stacks
- Pop: remove from both stacks
- This maintains correct minimum at all times

EXAMPLE TRACE:
Operation          stack        minStack     GetMin()
---------          -----        --------     --------
Push(-2)          [-2]         [-2]         -2
Push(0)           [-2, 0]      [-2, -2]     -2
Push(-3)          [-2, 0, -3]  [-2, -2, -3] -3
GetMin()                                     -3
Pop()             [-2, 0]      [-2, -2]     -2
Top()                                        0
GetMin()                                     -2

DETAILED PUSH LOGIC:
Push(-2): stack=[-2], minStack=[-2] (first element, so min is -2)
Push(0):  stack=[-2,0], minStack=[-2,-2] (min(-2, 0) = -2)
Push(-3): stack=[-2,0,-3], minStack=[-2,-2,-3] (min(-2, -3) = -3)

WHY THIS WORKS:
- At any point, minStack.Peek() tells us minimum of all elements below and including current position
- When we pop, we remove current element AND its associated minimum
- Previous minimum automatically becomes accessible (it's the new top of minStack)

ALTERNATIVE APPROACHES:
1. Single stack with pairs: Store (value, min) tuples - same space, more complex
2. Scan stack on getMin(): O(n) time - violates O(1) requirement
3. This approach: Two stacks - optimal, clean implementation

EDGE CASES HANDLED:
- Empty stack: minStack.Count check prevents errors
- Single element: works correctly (element is its own minimum)
- Duplicate minimums: handled correctly (each gets stored)
- All operations on non-empty stack: guaranteed by problem constraints
*/