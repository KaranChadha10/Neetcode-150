/*
PROBLEM: Reverse Linked List
Given the head of a singly linked list, reverse the list, and return the reversed list.

Example 1:
Input: head = [1,2,3,4,5]
Output: [5,4,3,2,1]

Example 2:
Input: head = [1,2]
Output: [2,1]

Example 3:
Input: head = []
Output: []

Constraints:
- The number of nodes in the list is the range [0, 5000].
- -5000 <= Node.val <= 5000

Follow up: A linked list can be reversed either iteratively or recursively. Could you implement both?
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
    public ListNode ReverseList(ListNode head) {
        ListNode prev = null; // Previous node (will become new head at the end)
        ListNode curr = head; // Current node being processed

        while (curr != null) // Traverse until end of list
        {
            ListNode temp = curr.next; // Save next node before changing pointer
            curr.next = prev; // Reverse the pointer (point to previous node)
            prev = curr; // Move prev forward (curr becomes new prev)
            curr = temp; // Move curr forward (saved next becomes new curr)
        }
        return prev; // prev is now the new head of reversed list
    }
}

/*
TIME COMPLEXITY: O(n)
- Single pass through the linked list
- Visit each node exactly once
- Each iteration performs constant time operations
- Overall: O(n) where n is the number of nodes

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers (prev, curr, temp)
- No recursion stack or additional data structures
- In-place reversal by changing pointers
- Space usage independent of list size

ALGORITHM EXPLANATION:
- Iterative approach using three pointers
- Reverse direction of each node's pointer one by one
- Maintain reference to previous node to avoid losing connection
- Use temp variable to save next node before changing pointers

KEY INSIGHTS:
- Singly linked list only has 'next' pointer
- To reverse, we need to change each node's next pointer to point backward
- Problem: changing next loses reference to rest of list
- Solution: save next in temp variable before changing pointer

THREE-POINTER TECHNIQUE:
1. prev: tracks node that comes before in reversed list
2. curr: current node being reversed
3. temp: saves next node to continue traversal

POINTER REVERSAL STEPS (per iteration):
1. Save curr.next in temp (preserve rest of list)
2. Point curr.next to prev (reverse the link)
3. Move prev to curr (advance prev for next iteration)
4. Move curr to temp (advance curr to next node)

VISUAL TRACE with [1,2,3,4,5]:

Initial state:
prev = null
curr = 1 → 2 → 3 → 4 → 5 → null

Iteration 1 (curr=1):
temp = 2 → 3 → 4 → 5 → null  (save next)
1 → null                        (reverse pointer)
prev = 1, curr = 2

Result: null ← 1    2 → 3 → 4 → 5 → null
             prev  curr

Iteration 2 (curr=2):
temp = 3 → 4 → 5 → null
2 → 1 → null
prev = 2, curr = 3

Result: null ← 1 ← 2    3 → 4 → 5 → null
                  prev  curr

Iteration 3 (curr=3):
temp = 4 → 5 → null
3 → 2 → 1 → null
prev = 3, curr = 4

Result: null ← 1 ← 2 ← 3    4 → 5 → null
                      prev  curr

Iteration 4 (curr=4):
temp = 5 → null
4 → 3 → 2 → 1 → null
prev = 4, curr = 5

Result: null ← 1 ← 2 ← 3 ← 4    5 → null
                            prev  curr

Iteration 5 (curr=5):
temp = null
5 → 4 → 3 → 2 → 1 → null
prev = 5, curr = null

Result: null ← 1 ← 2 ← 3 ← 4 ← 5    null
                            prev  curr

Loop exits: curr == null
Return prev (which points to 5, the new head)

Final reversed list: 5 → 4 → 3 → 2 → 1 → null ✓

STEP-BY-STEP BREAKDOWN:
Before reversal:    1 → 2 → 3 → 4 → 5 → null
After reversal:     5 → 4 → 3 → 2 → 1 → null

Each arrow (→) is a 'next' pointer that gets reversed

POINTER MOVEMENT PATTERN:
```
while (curr != null):
    temp = curr.next    // 1. Save connection to rest
    curr.next = prev    // 2. Reverse current pointer
    prev = curr         // 3. Advance prev
    curr = temp         // 4. Advance curr
```

WHY WE NEED TEMP:
Without temp:
curr.next = prev  // Lost reference to rest of list!
curr = curr.next  // curr.next is now prev, not the original next!

With temp:
temp = curr.next  // Save original next
curr.next = prev  // Safe to reverse now
curr = temp       // Move to saved next ✓

LOOP INVARIANT:
- All nodes from head to prev have been reversed
- curr points to next node to be reversed
- When curr becomes null, prev is the new head

EDGE CASES HANDLED:
- Empty list (head = null): 
  - curr = null, loop never executes
  - Returns prev = null ✓

- Single node (head = [1]):
  - Iteration 1: temp=null, 1→null, prev=1, curr=null
  - Returns prev (node 1) ✓

- Two nodes (head = [1,2]):
  - Iteration 1: 1→null, prev=1, curr=2
  - Iteration 2: 2→1→null, prev=2, curr=null
  - Returns prev (node 2) ✓

WHY RETURN prev (not curr)?
- After loop, curr is null
- prev points to last node processed (new head)
- Example: after reversing [1,2,3], prev=3, curr=null
- Return prev to get new head (3) ✓

COMPARISON: ITERATIVE VS RECURSIVE

Iterative (this solution):
✓ O(1) space
✓ No stack overflow risk
✓ More intuitive for beginners
✓ Better for very long lists

Recursive:
✓ More elegant/concise
✓ Natural for divide-and-conquer thinking
✗ O(n) space (call stack)
✗ Risk of stack overflow for large lists

ALTERNATIVE APPROACHES:
1. Recursive reversal: O(n) time, O(n) space
2. Stack-based: O(n) time, O(n) space
3. This approach: O(n) time, O(1) space ← Optimal!

COMMON MISTAKES TO AVOID:
❌ Forgetting to save curr.next before changing pointer
❌ Returning curr instead of prev at the end
❌ Not handling empty list or single node
❌ Incorrect pointer advancement order

PATTERN RECOGNITION:
- "Reverse linked list" → iterative three-pointer technique
- "In-place modification" → use O(1) extra space
- "Pointer manipulation" → careful ordering of operations

RECURSIVE SOLUTION (for reference):
```csharp
public ListNode ReverseList(ListNode head) {
    if (head == null || head.next == null) 
        return head;
    
    ListNode newHead = ReverseList(head.next);
    head.next.next = head;
    head.next = null;
    return newHead;
}
```

REAL-WORLD APPLICATIONS:
- Undo operations (reverse action history)
- Browser back button (reverse navigation)
- Text editors (reverse text direction)
- Graph algorithms (reverse edge directions)

FOLLOW-UP EXTENSIONS:
- Reverse first K nodes only
- Reverse in groups of K
- Reverse between positions m and n
- Check if list is palindrome (reverse and compare)
*/