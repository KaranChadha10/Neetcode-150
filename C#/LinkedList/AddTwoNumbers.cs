/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 2. Add Two Numbers
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
You are given two non-empty linked lists representing two non-negative integers. 
The digits are stored in reverse order, and each of their nodes contains a single 
digit. Add the two numbers and return the sum as a linked list.

You may assume the two numbers do not contain any leading zero, except the number 
0 itself.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: l1 = [2,4,3], l2 = [5,6,4]
Output: [7,0,8]
Explanation: 342 + 465 = 807
Visual:
  2 → 4 → 3  (represents 342)
+ 5 → 6 → 4  (represents 465)
─────────────
  7 → 0 → 8  (represents 807)

Example 2:
Input: l1 = [0], l2 = [0]
Output: [0]
Explanation: 0 + 0 = 0

Example 3:
Input: l1 = [9,9,9,9,9,9,9], l2 = [9,9,9,9]
Output: [8,9,9,9,0,0,0,1]
Explanation: 9999999 + 9999 = 10009998
Visual:
  9 → 9 → 9 → 9 → 9 → 9 → 9  (9,999,999)
+ 9 → 9 → 9 → 9              (9,999)
─────────────────────────────────────
  8 → 9 → 9 → 9 → 0 → 0 → 0 → 1  (10,009,998)

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- The number of nodes in each linked list is in the range [1, 100]
- 0 <= Node.val <= 9
- It is guaranteed that the list represents a number that does not have leading zeros

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Reverse Order Storage**: Digits stored in reverse order (least significant first)
   - Makes addition natural (add from least to most significant)
   - No need to reverse the lists

2. **Carry Propagation**: Just like hand addition
   - Sum of digits + carry from previous position
   - New carry = sum / 10
   - Current digit = sum % 10

3. **Different Length Lists**: Lists may have different lengths
   - Treat missing nodes as 0
   - Continue until both lists exhausted AND carry is 0

4. **Recursive Approach**: Natural for linked lists
   - Process current digit + recursively process rest
   - Base case: both lists null AND carry is 0
   - Build result list from bottom up (tail recursion)

5. **Edge Cases**:
   - One list longer than the other
   - Final carry (e.g., 99 + 1 = 100)
   - Lists of length 1

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(max(m, n))
- m = length of l1, n = length of l2
- Visit each node at most once
- Each recursive call processes one digit
- Overall: O(max(m, n))

Space Complexity: O(max(m, n))
- Recursion call stack: O(max(m, n)) depth
- Result list: O(max(m, n) + 1) nodes (if carry at end)
- Overall: O(max(m, n))

Note: Iterative approach would have O(1) auxiliary space (excluding result)

═══════════════════════════════════════════════════════════════════════════════
*/

/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int val=0, ListNode next=null) {
 *         this.val = val;
 *         this.next = next;
 *     }
 * }
 */

public class Solution
{
    // ════════════════════════════════════════════════════════════════════════
    // RECURSIVE HELPER: Add two linked lists with carry
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Recursively adds two linked lists digit by digit, handling carry
    /// </summary>
    /// <param name="l1">First linked list (or null if exhausted)</param>
    /// <param name="l2">Second linked list (or null if exhausted)</param>
    /// <param name="carry">Carry from previous digit addition</param>
    /// <returns>Head of the result linked list</returns>
    public ListNode Add(ListNode l1, ListNode l2, int carry)
    {
        // ────────────────────────────────────────────────────────────────────
        // BASE CASE: Both lists exhausted and no carry
        // ────────────────────────────────────────────────────────────────────
        // When both lists are null AND carry is 0, we're done
        // This is the termination condition for our recursion
        if (l1 == null && l2 == null && carry == 0)
        {
            return null;
        }

        // ────────────────────────────────────────────────────────────────────
        // Extract current digit values (treat null as 0)
        // ────────────────────────────────────────────────────────────────────
        int v1 = 0;
        int v2 = 0;
        
        if (l1 != null)
        {
            v1 = l1.val;
        }

        if (l2 != null)
        {
            v2 = l2.val;
        }

        // ────────────────────────────────────────────────────────────────────
        // Perform addition: current digits + carry
        // ────────────────────────────────────────────────────────────────────
        // Example: 7 + 8 + 1(carry) = 16
        int sum = v1 + v2 + carry;
        
        // Calculate new carry and current digit
        // Example: sum=16 → carry=1, digit=6
        int newCarry = sum / 10;      // Integer division: 16/10 = 1
        int nodeValue = sum % 10;     // Remainder: 16%10 = 6

        // ────────────────────────────────────────────────────────────────────
        // RECURSIVE CALL: Process next digits
        // ────────────────────────────────────────────────────────────────────
        // ⚠️ BUG FIX: The original code had a syntax error here!
        // Original (WRONG): Add((l1 != null ? l1.next : null) + ...)
        // This tries to use + operator on ListNode objects - won't compile!
        //
        // Correct: Pass three separate arguments
        ListNode nextNode = Add(
            l1 != null ? l1.next : null,  // Move to next digit in l1 (or null)
            l2 != null ? l2.next : null,  // Move to next digit in l2 (or null)
            newCarry                       // Pass the carry to next position
        );

        // ────────────────────────────────────────────────────────────────────
        // Create current node with computed digit value
        // ────────────────────────────────────────────────────────────────────
        // Build result list from bottom up (as recursion unwinds)
        return new ListNode(nodeValue) { next = nextNode };
    }
    
    // ════════════════════════════════════════════════════════════════════════
    // MAIN METHOD: Entry point for adding two numbers
    // ════════════════════════════════════════════════════════════════════════
    public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
    {
        // Start recursion with initial carry of 0
        return Add(l1, l2, 0);
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example: l1=[2,4,3], l2=[5,6,4]
═══════════════════════════════════════════════════════════════════════════════

Represents: 342 + 465 = 807

Visual Structure:
  l1: 2 → 4 → 3 → null
  l2: 5 → 6 → 4 → null

Recursion Call Stack (building up):
───────────────────────────────────────────────────────

Call 1: Add(Node(2), Node(5), carry=0)
  ├─ v1=2, v2=5, carry=0
  ├─ sum = 2+5+0 = 7
  ├─ newCarry = 7/10 = 0
  ├─ nodeValue = 7%10 = 7
  ├─ Recursive call: Add(Node(4), Node(6), carry=0)
  │
  │  Call 2: Add(Node(4), Node(6), carry=0)
  │    ├─ v1=4, v2=6, carry=0
  │    ├─ sum = 4+6+0 = 10
  │    ├─ newCarry = 10/10 = 1
  │    ├─ nodeValue = 10%10 = 0
  │    ├─ Recursive call: Add(Node(3), Node(4), carry=1)
  │    │
  │    │  Call 3: Add(Node(3), Node(4), carry=1)
  │    │    ├─ v1=3, v2=4, carry=1
  │    │    ├─ sum = 3+4+1 = 8
  │    │    ├─ newCarry = 8/10 = 0
  │    │    ├─ nodeValue = 8%10 = 8
  │    │    ├─ Recursive call: Add(null, null, carry=0)
  │    │    │
  │    │    │  Call 4: Add(null, null, carry=0)
  │    │    │    └─ Base case! Return null
  │    │    │
  │    │    └─ Create Node(8) → null
  │    │
  │    └─ Create Node(0) → Node(8)
  │
  └─ Create Node(7) → Node(0) → Node(8)

Result: 7 → 0 → 8 ✓ (represents 807)

Step-by-Step Execution:
───────────────────────────────────────────────────────

Position 0 (ones place):
  l1: 2, l2: 5, carry: 0
  sum: 2+5+0 = 7
  digit: 7, carry: 0

Position 1 (tens place):
  l1: 4, l2: 6, carry: 0
  sum: 4+6+0 = 10
  digit: 0, carry: 1  ← Carry generated!

Position 2 (hundreds place):
  l1: 3, l2: 4, carry: 1
  sum: 3+4+1 = 8
  digit: 8, carry: 0

Position 3:
  l1: null, l2: null, carry: 0
  Base case reached → null

Final result: 7 → 0 → 8

Verification:
  342
+ 465
─────
  807 ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 DETAILED WALKTHROUGH - Example: l1=[9,9,9,9,9,9,9], l2=[9,9,9,9]
═══════════════════════════════════════════════════════════════════════════════

Represents: 9,999,999 + 9,999 = 10,009,998

Position 0: 9+9+0 = 18 → digit=8, carry=1
Position 1: 9+9+1 = 19 → digit=9, carry=1
Position 2: 9+9+1 = 19 → digit=9, carry=1
Position 3: 9+9+1 = 19 → digit=9, carry=1
Position 4: 9+0+1 = 10 → digit=0, carry=1 (l2 exhausted, treat as 0)
Position 5: 9+0+1 = 10 → digit=0, carry=1
Position 6: 9+0+1 = 10 → digit=0, carry=1
Position 7: 0+0+1 =  1 → digit=1, carry=0 (both exhausted, but carry remains!)
Position 8: null+null+0 → return null (base case)

Result: 8 → 9 → 9 → 9 → 0 → 0 → 0 → 1 ✓

Key Observation: Final carry creates an extra node!
This is why we check: if (l1 == null && l2 == null && carry == 0)
We continue as long as carry exists, even if both lists are exhausted.

═══════════════════════════════════════════════════════════════════════════════
🎯 EDGE CASE WALKTHROUGH - Example: l1=[9,9], l2=[1]
═══════════════════════════════════════════════════════════════════════════════

Represents: 99 + 1 = 100

Position 0: 9+1+0 = 10 → digit=0, carry=1
Position 1: 9+0+1 = 10 → digit=0, carry=1 (l2 exhausted)
Position 2: 0+0+1 =  1 → digit=1, carry=0 (l1 exhausted)
Position 3: null+null+0 → return null

Result: 0 → 0 → 1 ✓

Visual:
  9 → 9       (99)
+ 1           (1)
─────────
  0 → 0 → 1   (100)

═══════════════════════════════════════════════════════════════════════════════
🐛 BUG FIX EXPLANATION:
═══════════════════════════════════════════════════════════════════════════════

Original Code (INCORRECT):
───────────────────────────────────────────────────────
```csharp
ListNode nextNode = Add((l1 != null ? l1.next : null) +
                        (l2 != null ? l2.next : null) +
                        newCarry);
```

Problems:
1. ❌ Trying to use + operator on ListNode objects
2. ❌ C# doesn't support adding ListNode + ListNode
3. ❌ Can't add ListNode + int (newCarry)
4. ❌ Won't compile - type mismatch error

Corrected Code:
───────────────────────────────────────────────────────
```csharp
ListNode nextNode = Add(
    l1 != null ? l1.next : null,
    l2 != null ? l2.next : null,
    newCarry
);
```

Why This Works:
1. ✓ Three separate arguments to Add method
2. ✓ First arg: next node of l1 (or null)
3. ✓ Second arg: next node of l2 (or null)
4. ✓ Third arg: carry value (int)
5. ✓ Matches method signature: Add(ListNode, ListNode, int)

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Both lists same length, no final carry:
   l1=[2,4,3], l2=[5,6,4]
   Result: [7,0,8] ✓

✅ Both lists same length, with final carry:
   l1=[5], l2=[5]
   Result: [0,1] ✓

✅ Different length lists:
   l1=[9,9,9,9,9,9,9], l2=[9,9,9,9]
   Result: [8,9,9,9,0,0,0,1] ✓

✅ One list much longer:
   l1=[1,2,3,4,5], l2=[6]
   Result: [7,2,3,4,5] ✓

✅ Single digit each:
   l1=[0], l2=[0]
   Result: [0] ✓

✅ All 9s (maximum carry propagation):
   l1=[9,9,9], l2=[9,9,9]
   Result: [8,9,9,1] ✓

✅ Zero with non-zero:
   l1=[0], l2=[1,2,3]
   Result: [1,2,3] ✓

✅ Large numbers:
   l1=[9,9,9,9,9,9,9,9,9], l2=[1]
   Result: [0,0,0,0,0,0,0,0,0,1] ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Forgetting to handle null nodes as 0
   - Must check if l1/l2 is null before accessing val
   - Treat null as 0 in addition

❌ Not propagating final carry
   - After both lists exhausted, carry might still exist
   - Must continue until carry is 0
   - Example: 99 + 1 = 100 (final digit is the carry)

❌ Using + operator on ListNode objects (original bug)
   - Can't add ListNode objects with +
   - Must pass them as separate arguments

❌ Wrong base case
   - Base case must check: both null AND carry is 0
   - Just checking both null is insufficient

❌ Not handling different length lists
   - Can't assume lists have same length
   - Must handle when one is shorter

❌ Modifying input lists
   - Should not change l1 or l2
   - Create new nodes for result

❌ Forgetting to advance to next node
   - Must use l1.next and l2.next in recursive call
   - Not just l1 and l2 (would infinite loop)

❌ Integer overflow concerns
   - In other languages, sum might overflow
   - C# int is safe: max sum = 9+9+1 = 19 (fits in int)

❌ Returning wrong base case value
   - Base case should return null, not new ListNode(0)
   - Null properly terminates the list

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Digit-by-Digit Processing**:
   - Process numbers digit by digit
   - Handle carry between positions
   - Build result progressively

2. **Recursive Linked List Processing**:
   - Process current node + recurse on rest
   - Base case when list exhausted
   - Build result during unwinding

3. **Carry Propagation**:
   - Like hand addition from right to left
   - Carry to next higher position
   - Continue until carry is 0

4. **Handling Variable Length**:
   - Treat missing nodes as 0
   - Continue until both exhausted
   - One list can be longer

5. **Result Construction**:
   - Build list node by node
   - Each recursion level creates one node
   - Natural for linked list output

═══════════════════════════════════════════════════════════════════════════════
🔄 ITERATIVE ALTERNATIVE:
═══════════════════════════════════════════════════════════════════════════════

The iterative approach is more space-efficient:
```csharp
public ListNode AddTwoNumbers(ListNode l1, ListNode l2) 
{
    ListNode dummy = new ListNode(0);
    ListNode current = dummy;
    int carry = 0;
    
    while (l1 != null || l2 != null || carry != 0) 
    {
        int v1 = (l1 != null) ? l1.val : 0;
        int v2 = (l2 != null) ? l2.val : 0;
        
        int sum = v1 + v2 + carry;
        carry = sum / 10;
        
        current.next = new ListNode(sum % 10);
        current = current.next;
        
        if (l1 != null) l1 = l1.next;
        if (l2 != null) l2 = l2.next;
    }
    
    return dummy.next;
}
```

Comparison:

Recursive:
✓ More elegant and concise
✓ Natural for linked lists
✗ O(n) call stack space
✗ Potential stack overflow for very long lists

Iterative:
✓ O(1) auxiliary space
✓ No stack overflow risk
✓ More efficient in practice
✗ Slightly more code
✗ Needs dummy node pattern

For interviews: Both acceptable, mention trade-offs!

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Big Integer Arithmetic**:
   - Numbers larger than native integer types
   - Java BigInteger, Python unlimited integers
   - Cryptography, scientific computing

2. **Financial Calculations**:
   - Arbitrary precision decimal arithmetic
   - Banking systems
   - Avoid floating point errors

3. **Calculator Applications**:
   - User input as digit sequences
   - Display results digit by digit
   - Handle very large numbers

4. **Compiler/Interpreter**:
   - Parse and evaluate large numbers
   - Constant folding optimizations
   - Literal value processing

5. **Digital Circuit Design**:
   - Ripple carry adder
   - Hardware addition circuits
   - Carry propagation logic

6. **Educational Software**:
   - Teaching addition with carry
   - Visual representation
   - Step-by-step calculation

7. **Data Compression**:
   - Arithmetic coding
   - Precision arithmetic for probabilities
   - Lossless compression algorithms

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Add Two Numbers II (LeetCode 445)**:
   - Digits stored in normal order (most significant first)
   - Requires reversing lists first (or using stack)
   - More complex than this problem

2. **Multiply Two Numbers (Interview Question)**:
   - Similar linked list representation
   - Requires multiple additions
   - More complex carry handling

3. **Subtract Two Numbers (Interview Question)**:
   - Handle borrowing instead of carry
   - Need to determine which is larger
   - Handle negative results

4. **Add Binary (LeetCode 67)**:
   - Similar concept but with strings
   - Binary instead of decimal
   - Same carry propagation

5. **Add Strings (LeetCode 415)**:
   - String representation of numbers
   - Array/string manipulation
   - Similar digit-by-digit processing

6. **Plus One (LeetCode 66)**:
   - Add 1 to number represented as array
   - Simpler - only one operand
   - Similar carry propagation

7. **Factorial Large Numbers**:
   - Compute factorial of large numbers
   - Use linked list for result
   - Requires multiplication

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Solution (Recursive):

Strengths:
✓ Clean and elegant code
✓ Easy to understand
✓ Natural for linked lists
✓ Minimal code duplication

Weaknesses:
✗ O(n) call stack space
✗ Function call overhead
✗ Risk of stack overflow for very long lists

Optimizations:

1. **Tail Recursion** (not possible here):
   - Can't optimize to tail recursion
   - Need to create node after recursive call returns
   - Compiler can't eliminate stack frames

2. **Iterative Version**:
   - O(1) auxiliary space
   - Eliminates recursion overhead
   - More efficient for very long lists
   - See iterative alternative above

3. **Early Termination**:
   - Already optimal
   - Returns as soon as both lists done and carry is 0
   - No unnecessary processing

4. **Micro-optimizations**:
   - Could combine null checks
   - Could avoid ternary operators
   - Minimal performance impact

Best Approach:
- Recursive for clarity in interviews
- Iterative for production code
- Both have O(n) time complexity
- Trade-off is space: O(n) vs O(1)

When to use each:
- Recursive: Short lists, clarity priority
- Iterative: Very long lists, memory constrained

═══════════════════════════════════════════════════════════════════════════════
*/