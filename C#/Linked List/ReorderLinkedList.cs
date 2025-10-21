/*
PROBLEM: Reorder List
You are given the head of a singly linked-list. The list can be represented as:
L0 → L1 → … → Ln-1 → Ln

Reorder the list to be on the following form:
L0 → Ln → L1 → Ln-1 → L2 → Ln-2 → …

You may not modify the values in the list's nodes. Only nodes themselves may be changed.

Example 1:
Input: head = [1,2,3,4]
Output: [1,4,2,3]

Example 2:
Input: head = [1,2,3,4,5]
Output: [1,5,2,4,3]

Constraints:
- The number of nodes in the list is in the range [1, 5 * 10^4].
- 1 <= Node.val <= 1000
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
    public void ReorderList(ListNode head)
    {
        // PHASE 1: Find the middle of the list using slow/fast pointers
        ListNode slow = head;
        ListNode fast = head.next; // Fast starts one ahead to find correct split point
        
        // Move slow 1 step, fast 2 steps until fast reaches end
        while (fast != null && fast.next != null)
        {
            slow = slow.next;       // Slow moves 1 step
            fast = fast.next.next;  // Fast moves 2 steps
        }
        // After loop: slow is at middle (or just before middle for odd length)

        // PHASE 2: Reverse the second half of the list
        ListNode second = slow.next; // Start of second half
        ListNode prev = slow.next = null; // Break list into two halves, prev tracks reversed portion
        
        // Standard iterative reversal of second half
        while (second != null)
        {
            ListNode temp = second.next;  // Save next node
            second.next = prev;           // Reverse pointer
            prev = second;                // Move prev forward
            second = temp;                // Move second forward
        }
        // After loop: prev points to head of reversed second half

        // PHASE 3: Merge first half and reversed second half alternately
        ListNode first = head;    // Start of first half
        second = prev;            // Start of reversed second half
        
        // Interleave nodes from both halves
        while (second != null)    // Continue until second half exhausted
        {
            ListNode tmp1 = first.next;   // Save next of first half
            ListNode tmp2 = second.next;  // Save next of second half
            
            first.next = second;          // Connect first to second
            second.next = tmp1;           // Connect second to rest of first
            
            first = tmp1;                 // Move to next in first half
            second = tmp2;                // Move to next in second half
        }
        // After loop: list is reordered in-place
    }
}

/*
TIME COMPLEXITY: O(n)
- Phase 1 (Find middle): O(n/2) ≈ O(n)
- Phase 2 (Reverse second half): O(n/2) ≈ O(n)
- Phase 3 (Merge): O(n/2) ≈ O(n)
- Overall: O(n) where n is number of nodes

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers
- No additional data structures or recursion
- In-place modification of the list
- Space usage independent of list size

ALGORITHM EXPLANATION:
Three-phase approach:
1. Find middle of list (slow/fast pointers)
2. Reverse second half
3. Merge first half and reversed second half alternately

KEY INSIGHTS:
- Can't use extra space for array/list
- Must reorder in-place by changing pointers
- Pattern: alternate between start and end elements
- Breaking into sub-problems makes it manageable

THREE-PHASE BREAKDOWN:

PHASE 1 - FIND MIDDLE:
- Use Floyd's slow/fast pointer technique
- Slow moves 1, fast moves 2
- When fast reaches end, slow is at middle
- Starting fast at head.next ensures correct split for both odd/even lengths

PHASE 2 - REVERSE SECOND HALF:
- Use standard three-pointer reversal
- Reverse only the second half (from middle to end)
- Also break connection between two halves (slow.next = null)

PHASE 3 - MERGE ALTERNATELY:
- Take one node from first, one from second (reversed)
- Repeat until second half exhausted
- First half may have one extra node (handled naturally)

VISUAL TRACE with [1,2,3,4,5]:

Initial list:
1 → 2 → 3 → 4 → 5 → null

PHASE 1 - Find middle:
slow starts at 1, fast starts at 2

Iteration 1:
slow → 2, fast → 4

Iteration 2:
slow → 3, fast → null

Result: slow at 3 (middle)

Split into two halves:
First:  1 → 2 → 3 → null
Second: 4 → 5 → null

PHASE 2 - Reverse second half:
4 → 5 → null
Becomes:
5 → 4 → null

PHASE 3 - Merge:
First:  1 → 2 → 3 → null
Second: 5 → 4 → null

Merge step by step:
1. Connect 1 → 5, 5 → 2
   Result: 1 → 5 → 2 → 3 → null
   Remaining second: 4

2. Connect 2 → 4, 4 → 3
   Result: 1 → 5 → 2 → 4 → 3 → null
   Remaining second: null

Final: 1 → 5 → 2 → 4 → 3 → null ✓

VISUAL TRACE with [1,2,3,4]:

Initial list:
1 → 2 → 3 → 4 → null

PHASE 1:
slow starts at 1, fast starts at 2

Iteration 1:
slow → 2, fast → 4

Iteration 2:
slow → 2, fast → null (fast.next = null)

Split:
First:  1 → 2 → null
Second: 3 → 4 → null

PHASE 2:
Reverse second half:
3 → 4 → null becomes 4 → 3 → null

PHASE 3:
Merge:
1. 1 → 4 → 2
2. 2 → 3 → null

Final: 1 → 4 → 2 → 3 → null ✓

WHY FAST STARTS AT head.next:
- Ensures proper split point for both odd and even length lists
- For odd length (5): slow stops at middle (3)
- For even length (4): slow stops at first half's last (2)
- This makes merging work correctly

MERGING LOGIC DETAIL:
```
while (second != null) {
    tmp1 = first.next   // Save: 2
    tmp2 = second.next  // Save: 4
    
    first.next = second  // 1 → 5
    second.next = tmp1   // 5 → 2
    
    first = tmp1         // Move to 2
    second = tmp2        // Move to 4
}
```

WHY CHECK second (NOT first)?
- First half may have one extra node
- Second half always finishes first or at same time
- When second exhausted, any remaining first node is in correct position

POINTER MANIPULATION ORDER:
1. Save next pointers (don't lose references)
2. Rewire current connections
3. Advance to saved positions
Order is critical - can't be rearranged!

EDGE CASES HANDLED:
- Single node [1]:
  - slow = 1, fast = null (fast.next initially null)
  - second = null, no reversal needed
  - Merge doesn't execute
  - Returns [1] ✓

- Two nodes [1,2]:
  - slow = 1, fast = 2
  - Second half: [2], reversed: [2]
  - Merge: 1 → 2
  - Returns [1,2] (already in order) ✓

- Three nodes [1,2,3]:
  - slow = 2, second = [3]
  - Reversed: [3]
  - Merge: 1 → 3 → 2
  - Returns [1,3,2] ✓

ALTERNATIVE APPROACHES:

1. Using Stack:
```csharp
// Push all to stack, pop and merge
// O(n) time, O(n) space ✗
```

2. Using Array:
```csharp
// Convert to array, reorder, rebuild list
// O(n) time, O(n) space ✗
```

3. This approach:
```csharp
// Three phases: find, reverse, merge
// O(n) time, O(1) space ✓ Optimal!
```

COMMON MISTAKES TO AVOID:
❌ Not breaking connection between two halves (creates cycle)
❌ Wrong starting position for fast pointer (incorrect split)
❌ Not saving next pointers before rewiring (lose references)
❌ Checking first instead of second in merge loop (may miss nodes)

PATTERN RECOGNITION:
- "Reorder linked list" → find middle + reverse + merge
- "In-place" requirement → O(1) space, pointer manipulation
- "Alternating pattern" → two-pointer merge technique

REAL-WORLD APPLICATIONS:
- Playlist shuffling (alternate old and new songs)
- Task scheduling (alternate high/low priority)
- Data interleaving for optimization
- Balancing load distribution

SKILLS DEMONSTRATED:
1. Floyd's cycle finding (slow/fast pointers)
2. Linked list reversal
3. Two-pointer merging
4. Complex pointer manipulation
5. In-place algorithms

FOLLOW-UP EXTENSIONS:
- Reorder in groups of K
- Reorder with different patterns
- Reorder doubly linked list
- Reorder maintaining certain order constraints
*/