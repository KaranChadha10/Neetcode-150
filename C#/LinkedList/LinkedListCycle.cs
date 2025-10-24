/*
PROBLEM: Linked List Cycle
Given head, the head of a linked list, determine if the linked list has a cycle in it.

There is a cycle in a linked list if there is some node in the list that can be reached 
again by continuously following the next pointer. Internally, pos is used to denote the 
index of the node that tail's next pointer is connected to. Note that pos is not passed 
as a parameter.

Return true if there is a cycle in the linked list. Otherwise, return false.

Example 1:
Input: head = [3,2,0,-4], pos = 1
Output: true
Explanation: There is a cycle in the linked list, where the tail connects to the 1st node (0-indexed).

Example 2:
Input: head = [1,2], pos = 0
Output: true
Explanation: There is a cycle in the linked list, where the tail connects to the 0th node.

Example 3:
Input: head = [1], pos = -1
Output: false
Explanation: There is no cycle in the linked list.

Constraints:
- The number of the nodes in the list is in the range [0, 10^4].
- -10^5 <= Node.val <= 10^5
- pos is -1 or a valid index in the linked-list.

Follow up: Can you solve it using O(1) (i.e. constant) memory?
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

public class Solution {
    public bool HasCycle(ListNode head) {
        ListNode slow = head, fast = head; // Initialize both pointers at head
        
        // Continue while fast can move two steps ahead
        while (fast != null && fast.next != null) {
            fast = fast.next.next; // Move fast pointer 2 steps
            slow = slow.next;      // Move slow pointer 1 step
            
            // If slow and fast meet, cycle detected
            if (slow.Equals(fast)) return true;
        }
        
        return false; // Fast reached end, no cycle exists
    }
}

/*
TIME COMPLEXITY: O(n)
- Without cycle: fast reaches end in n/2 steps → O(n)
- With cycle: slow and fast will meet within n steps
  - Fast catches up to slow at rate of 1 node per iteration
  - Maximum distance before meeting ≤ cycle length ≤ n
- Overall: O(n) where n is number of nodes

SPACE COMPLEXITY: O(1)
- Only using two pointers (slow and fast)
- No additional data structures like hash set
- Constant space regardless of list size
- Meets the follow-up requirement!

ALGORITHM EXPLANATION:
- Floyd's Cycle Detection Algorithm (Tortoise and Hare)
- Use two pointers moving at different speeds
- Slow moves 1 step, fast moves 2 steps per iteration
- If cycle exists, fast will eventually catch up to slow
- If no cycle, fast reaches end (null)

KEY INSIGHTS:
- In a cycle, faster pointer will eventually lap slower pointer
- Like runners on a circular track - faster runner catches slower
- No need to track visited nodes (unlike HashSet approach)
- Mathematical guarantee that they will meet if cycle exists

WHY THEY WILL MEET:
If there's a cycle:
1. Eventually both pointers enter the cycle
2. Once in cycle, distance between them decreases by 1 each iteration
3. Gap closes: d, d-1, d-2, ..., 2, 1, 0 (meet!)
4. They must meet - can't "jump over" each other

MATHEMATICAL PROOF:
- Let distance between slow and fast = d
- Each iteration: fast moves +2, slow moves +1
- Relative speed: fast gains 1 position per iteration
- Eventually: d → d-1 → d-2 → ... → 1 → 0 (collision!)

VISUAL TRACE - NO CYCLE [1→2→3→null]:
Initial:
slow, fast → 1 → 2 → 3 → null

Iteration 1:
slow → 2, fast → 3

Iteration 2:
slow → 3, fast → null (fast.next = null)

Exit loop: fast = null
Return false ✓

VISUAL TRACE - WITH CYCLE [1→2→3→4→2 (cycle)]:
Initial:
slow, fast → 1 → 2 → 3 → 4 ↴
                 ↑_________↓

Iteration 1:
slow → 2, fast → 3

Iteration 2:
slow → 3, fast → 2 (went through cycle)

Iteration 3:
slow → 4, fast → 4
slow == fast → return true ✓

STEP-BY-STEP BREAKDOWN:
```
while (fast != null && fast.next != null) {
    // Move pointers first, then check
    // This ensures we don't false-positive on first iteration
    fast = fast.next.next;  // 2 steps
    slow = slow.next;       // 1 step
    
    if (slow.Equals(fast))  // Check after moving
        return true;
}
```

WHY CHECK fast AND fast.next?
- fast.next != null: ensures fast.next.next is valid
- fast != null: base case, prevents null reference
- Need both checks to safely do fast.next.next

WHY MOVE BEFORE COMPARING?
- Both start at head (same position)
- If we compare first, would return true for non-cycle list!
- Move first, then compare on subsequent iterations

LOOP TERMINATION CONDITIONS:
1. fast == null: reached end, no cycle
2. fast.next == null: reached end, no cycle
3. slow == fast: found cycle, return true

EDGE CASES HANDLED:
- Empty list (head = null):
  - fast = null, loop doesn't execute
  - Returns false ✓

- Single node, no cycle [1→null]:
  - Iteration 1: fast.next = null
  - Loop exits
  - Returns false ✓

- Single node, self-loop [1→1]:
  - Iteration 1: slow→1, fast→1
  - slow == fast → return true ✓

- Two nodes, no cycle [1→2→null]:
  - Iteration 1: slow→2, fast→null
  - Loop exits
  - Returns false ✓

- Two nodes, cycle [1→2→1]:
  - Iteration 1: slow→2, fast→2
  - slow == fast → return true ✓

ALTERNATIVE APPROACH - HASH SET:
```csharp
public bool HasCycle(ListNode head) {
    HashSet<ListNode> visited = new HashSet<ListNode>();
    while (head != null) {
        if (visited.Contains(head)) return true;
        visited.Add(head);
        head = head.next;
    }
    return false;
}
```
✓ Easier to understand
✗ O(n) space (violates follow-up)
✗ Slower in practice (hashing overhead)

COMPARISON OF APPROACHES:
| Approach | Time | Space | Difficulty |
|----------|------|-------|------------|
| Hash Set | O(n) | O(n) | Easy |
| Two Pointers (this) | O(n) | O(1) | Medium |
| Modify List | O(n) | O(1) | Hard (destructive) |

WHY TWO POINTERS IS OPTIMAL:
✓ O(1) space - meets follow-up requirement
✓ O(n) time - same as hash set
✓ Non-destructive - doesn't modify list
✓ Simple once understood - only 2 pointers

SPEED COMPARISON:
- Why not move fast by 3, 4, or more steps?
- Moving by 2 is optimal:
  - Guarantees meeting if cycle exists
  - Minimal iterations to detect cycle
  - Moving faster might skip over slow pointer!

REAL-WORLD APPLICATIONS:
- Detecting infinite loops in data structures
- Finding circular references in object graphs
- Memory leak detection
- Validating graph structures

PATTERN RECOGNITION:
- "Detect cycle in linked list" → Floyd's algorithm
- "O(1) space requirement" → two pointers technique
- "Fast and slow pointers" → cycle detection pattern

COMMON MISTAKES TO AVOID:
❌ Comparing before moving (false positive at start)
❌ Only checking fast != null (need fast.next too)
❌ Using == instead of .Equals() (reference comparison)
❌ Moving slow by 2 and fast by 3 (they might miss each other)

FOLLOW-UP EXTENSIONS:
- Find the node where cycle begins (Floyd's cycle finding)
- Calculate length of the cycle
- Remove the cycle (modify list)
- Detect cycle in undirected graph

FLOYD'S ALGORITHM VARIANTS:
- Cycle detection (this problem)
- Cycle start detection (requires phase 2)
- Duplicate detection in array
- Finding middle of linked list (similar technique)
*/