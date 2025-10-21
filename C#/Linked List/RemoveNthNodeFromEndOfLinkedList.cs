/*
PROBLEM: Remove Nth Node From End of List
Given the head of a linked list, remove the nth node from the end of the list and return its head.

Example 1:
Input: head = [1,2,3,4,5], n = 2
Output: [1,2,3,5]
Explanation: Remove 4 (2nd node from end)

Example 2:
Input: head = [1], n = 1
Output: []
Explanation: Remove the only node

Example 3:
Input: head = [1,2], n = 1
Output: [1]
Explanation: Remove last node

Constraints:
- The number of nodes in the list is sz.
- 1 <= sz <= 30
- 0 <= Node.val <= 100
- 1 <= n <= sz

Follow up: Could you do this in one pass?
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
    public ListNode RemoveNthFromEnd(ListNode head, int n)
    {
        // Create dummy node to handle edge case of removing head
        ListNode dummy = new ListNode(0, head);
        ListNode left = dummy;  // Will point to node BEFORE the one to remove
        ListNode right = head;  // Will help find the position

        // PHASE 1: Move right pointer n steps ahead
        // This creates a gap of n nodes between left and right
        while (n > 0)
        {
            right = right.next;
            n--;
        }

        // PHASE 2: Move both pointers until right reaches end
        // When right is null, left is at node before the one to remove
        while (right != null)
        {
            left = left.next;
            right = right.next;
        }

        // Remove the nth node from end by skipping it
        left.next = left.next.next;
        
        // Return dummy.next (could be different if head was removed)
        return dummy.next;
    }
}

/*
TIME COMPLEXITY: O(n)
- First loop: moves right pointer n steps → O(n)
- Second loop: moves both pointers to end → O(length - n)
- Total: O(n + (length - n)) = O(length)
- Single pass through the list
- Meets the follow-up requirement!

SPACE COMPLEXITY: O(1)
- Only using constant extra space (dummy, left, right pointers)
- No additional data structures or recursion
- Space usage independent of list size

ALGORITHM EXPLANATION:
- Two-pointer technique with a gap of n nodes
- Use dummy node to simplify edge cases
- Move right n steps ahead first (create gap)
- Move both pointers together until right reaches end
- Left is now positioned before the node to remove
- Skip the target node by changing pointer

KEY INSIGHTS:
- "Nth from end" is hard because we don't know list length
- Two-pass solution: count length, then remove (O(2n))
- One-pass solution: maintain gap of n between pointers (O(n))
- Dummy node eliminates special case for removing head

TWO-POINTER GAP TECHNIQUE:
- Maintain exactly n nodes between left and right
- When right reaches end (null), left is n nodes before end
- Left points to node BEFORE the target (so we can remove it)

WHY DUMMY NODE?
Without dummy:
- Removing head requires special handling
- Need to check if n == length
- Return logic becomes complex

With dummy:
- Dummy always points to head
- No special case needed
- return dummy.next works for all cases

VISUAL TRACE with [1,2,3,4,5], n=2:

Initial setup:
dummy → 1 → 2 → 3 → 4 → 5 → null
left    right

PHASE 1 - Create gap of n=2:
Move right 2 steps ahead

After n=2 moves:
dummy → 1 → 2 → 3 → 4 → 5 → null
left         right
        └─2 gap─┘

PHASE 2 - Move both until right reaches null:
Move 1:
dummy → 1 → 2 → 3 → 4 → 5 → null
        left      right

Move 2:
dummy → 1 → 2 → 3 → 4 → 5 → null
             left       right

Move 3:
dummy → 1 → 2 → 3 → 4 → 5 → null
                  left      right

Move 4:
dummy → 1 → 2 → 3 → 4 → 5 → null
                       left      right=null

Left is now at node 3 (before target node 4)

PHASE 3 - Remove node:
left.next = left.next.next  (3.next = 5)

Result:
dummy → 1 → 2 → 3 → 5 → null
                  ↑_____↑ (skipped 4)

Return dummy.next = 1 ✓

VISUAL TRACE with [1,2], n=1 (remove last):

Initial:
dummy → 1 → 2 → null
left    right

PHASE 1 - Move right 1 step:
dummy → 1 → 2 → null
left         right

PHASE 2 - Move both:
dummy → 1 → 2 → null
        left      right=null

PHASE 3 - Remove:
left.next = left.next.next  (1.next = null)

Result:
dummy → 1 → null
Return [1] ✓

VISUAL TRACE with [1], n=1 (remove head):

Initial:
dummy → 1 → null
left    right

PHASE 1 - Move right 1 step:
dummy → 1 → null
left    right=null

PHASE 2 - Skip (right already null)

PHASE 3 - Remove:
left.next = left.next.next  (dummy.next = null)

Result:
dummy → null
Return dummy.next = null ✓

WHY LEFT STARTS AT DUMMY (NOT HEAD)?
- Left needs to point to node BEFORE target
- If target is head, left must point to dummy
- Starting at dummy works for all cases

GAP SIZE EXPLANATION:
After creating gap of n:
- If right is at position k, left is at position k-n
- When right reaches end (position length), left is at position length-n
- Position length-n is the node BEFORE the one we want to remove

TWO-PASS ALTERNATIVE (simpler but slower):
```csharp
// Pass 1: Count length
int length = 0;
ListNode curr = head;
while (curr != null) {
    length++;
    curr = curr.next;
}

// Pass 2: Remove node at position (length - n)
// ... implementation
```
✗ O(2n) time (two passes)
✓ This solution: O(n) time (one pass)

EDGE CASES HANDLED:
- Remove head (n = length):
  - Left stays at dummy
  - dummy.next = head.next ✓

- Remove tail (n = 1):
  - Right moves 1 step
  - Both move until right = null
  - Skip last node ✓

- Single node (remove only node):
  - Dummy handles this elegantly
  - Returns empty list ✓

- Two nodes, remove first:
  - Works correctly
  - Returns second node ✓

POINTER POSITIONING:
```
After gap creation and movement:
... → [left] → [TARGET] → ...
           ↑       ↑
      node before  node to remove

left.next = left.next.next
Skips TARGET node
```

WHY THIS WORKS IN ONE PASS:
- Don't need to know list length
- Gap of n ensures correct positioning
- When right reaches end, left is perfectly positioned

COMMON MISTAKES TO AVOID:
❌ Starting left at head (can't remove head correctly)
❌ Not using dummy node (complicates head removal)
❌ Off-by-one errors in gap creation
❌ Not returning dummy.next (might return wrong head)

PATTERN RECOGNITION:
- "Nth from end" → two pointers with gap
- "One pass" requirement → can't count length first
- "Remove node" → need pointer to node BEFORE target
- Dummy node pattern → simplifies head removal

ALTERNATIVE APPROACHES:
1. Count length, then remove: O(2n) time ✗
2. Recursion: O(n) time, O(n) space ✗
3. This approach: O(n) time, O(1) space ✓ Optimal!

REAL-WORLD APPLICATIONS:
- Implementing undo with limited history
- Circular buffer management
- Recent items tracking
- Cache eviction policies (LRU-like)

FOLLOW-UP VARIATIONS:
- Remove Kth node from beginning (easier)
- Remove middle node
- Remove nodes in specific pattern
- Validate if removal is possible

SKILLS DEMONSTRATED:
1. Two-pointer technique with gap
2. Dummy node pattern
3. One-pass algorithm design
4. Edge case handling
5. In-place list modification
*/