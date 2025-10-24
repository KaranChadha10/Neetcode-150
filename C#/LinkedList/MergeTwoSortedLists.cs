/*
PROBLEM: Merge Two Sorted Lists
You are given the heads of two sorted linked lists list1 and list2.

Merge the two lists into one sorted list. The list should be made by splicing together 
the nodes of the first two lists.

Return the head of the merged linked list.

Example 1:
Input: list1 = [1,2,4], list2 = [1,3,4]
Output: [1,1,2,3,4,4]

Example 2:
Input: list1 = [], list2 = []
Output: []

Example 3:
Input: list1 = [], list2 = [0]
Output: [0]

Constraints:
- The number of nodes in both lists is in the range [0, 50].
- -100 <= Node.val <= 100
- Both list1 and list2 are sorted in non-decreasing order.
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
    public ListNode MergeTwoLists(ListNode list1, ListNode list2)
    {
        // Create dummy node to simplify edge cases and serve as anchor
        ListNode dummy = new ListNode(0);
        ListNode node = dummy; // Pointer to build the merged list

        // Merge nodes while both lists have elements
        while (list1 != null && list2 != null)
        {
            // Compare values and attach smaller node to merged list
            if (list1.val < list2.val)
            {
                node.next = list1; // Attach list1's node
                list1 = list1.next; // Move list1 forward
            }
            else
            {
                node.next = list2; // Attach list2's node (handles equal case)
                list2 = list2.next; // Move list2 forward
            }
            node = node.next; // Move merged list pointer forward
        }

        // Attach remaining nodes from whichever list is not exhausted
        // Only one of these conditions will be true
        if (list1 != null)
        {
            node.next = list1; // Attach rest of list1
        }
        else
        {
            node.next = list2; // Attach rest of list2 (or null if both empty)
        }

        return dummy.next; // Return head of merged list (skip dummy)
    }
}

/*
TIME COMPLEXITY: O(n + m)
- Visit each node exactly once from both lists
- n = length of list1, m = length of list2
- While loop runs min(n, m) times
- Attaching remaining nodes is O(1) operation
- Overall: O(n + m)

SPACE COMPLEXITY: O(1)
- Only using constant extra space (dummy, node pointers)
- No new nodes created, just rewiring existing nodes
- Not counting output list (required for result)
- Space usage independent of input size

ALGORITHM EXPLANATION:
- Two-pointer merge technique (like merge sort)
- Use dummy node to avoid special case for head
- Compare values, attach smaller node, advance that list's pointer
- After one list exhausted, attach remainder of other list
- Return dummy.next (actual merged list head)

KEY INSIGHTS:
- Both input lists are already sorted
- Don't need to create new nodes, just rewire existing ones
- Dummy node eliminates need to handle empty list edge cases
- Can attach remaining nodes directly (already sorted)

MERGE STRATEGY:
1. Compare front elements of both lists
2. Attach smaller one to result
3. Advance pointer of list we took from
4. Repeat until one list empty
5. Attach remaining list (already sorted)

DUMMY NODE PATTERN:
- Simplifies code by providing a starting point
- Eliminates special case for first node
- Always return dummy.next (never dummy itself)
- Common pattern in linked list problems

VISUAL TRACE with list1 = [1,2,4], list2 = [1,3,4]:

Initial:
list1: 1 → 2 → 4 → null
list2: 1 → 3 → 4 → null
dummy → null
node = dummy

Step 1: Compare 1 vs 1
1 <= 1, choose list2
dummy → 1(list2) → null
node moves to 1, list2 moves to 3

Step 2: Compare 1 vs 3
1 < 3, choose list1
dummy → 1(list2) → 1(list1) → null
node moves to 1(list1), list1 moves to 2

Step 3: Compare 2 vs 3
2 < 3, choose list1
dummy → 1 → 1 → 2 → null
node moves to 2, list1 moves to 4

Step 4: Compare 4 vs 3
4 > 3, choose list2
dummy → 1 → 1 → 2 → 3 → null
node moves to 3, list2 moves to 4

Step 5: Compare 4 vs 4
4 <= 4, choose list2
dummy → 1 → 1 → 2 → 3 → 4(list2) → null
node moves to 4(list2), list2 moves to null

Step 6: list2 is null, attach rest of list1
dummy → 1 → 1 → 2 → 3 → 4 → 4 → null

Return dummy.next = [1,1,2,3,4,4] ✓

POINTER MOVEMENT PATTERN:
```
while (list1 && list2) {
    if (list1.val < list2.val) {
        node.next = list1;  // 1. Attach
        list1 = list1.next; // 2. Advance source
    } else {
        node.next = list2;
        list2 = list2.next;
    }
    node = node.next;       // 3. Advance merged list
}
```

WHY HANDLE EQUAL VALUES WITH list2?
```csharp
if (list1.val < list2.val)  // Strict <, not <=
```
- When equal, we choose list2 (else branch)
- Either choice is correct for sorted output
- Consistent behavior, simpler logic
- Maintains stability (preserves original order when equal)

ATTACHING REMAINING NODES:
```csharp
if (list1 != null)
    node.next = list1;
else
    node.next = list2;
```
- One list will be empty (null)
- Other list is already sorted
- Can attach entire remainder in O(1)
- No need to iterate through remaining nodes

EDGE CASES HANDLED:
- Both lists empty:
  - Loop never executes
  - node.next = null (from else)
  - Returns null ✓

- List1 empty, list2 has nodes:
  - Loop never executes
  - Attaches entire list2
  - Returns list2 ✓

- List1 has nodes, list2 empty:
  - Loop never executes
  - Attaches entire list1
  - Returns list1 ✓

- Both lists single node:
  - One iteration, then attach remaining
  - Works correctly ✓

- Lists of different lengths:
  - Merges until shorter exhausted
  - Attaches remainder
  - Works correctly ✓

COMPARISON WITH MERGE SORT:
- This is the "merge" part of merge sort
- Merge sort: split then merge
- This problem: already split (two lists), just merge
- Same two-pointer technique
- O(n + m) time for merging

ITERATIVE VS RECURSIVE:
Iterative (this solution):
✓ O(1) space
✓ No stack overflow risk
✓ More explicit control flow

Recursive alternative:
```csharp
public ListNode MergeTwoLists(ListNode l1, ListNode l2) {
    if (l1 == null) return l2;
    if (l2 == null) return l1;
    
    if (l1.val < l2.val) {
        l1.next = MergeTwoLists(l1.next, l2);
        return l1;
    } else {
        l2.next = MergeTwoLists(l1, l2.next);
        return l2;
    }
}
```
✓ More elegant/concise
✗ O(n + m) space (recursion stack)
✗ Stack overflow risk for large lists

WHY DUMMY NODE IS CRUCIAL:
Without dummy:
```csharp
ListNode head = null;
ListNode node = null;
// Need special case for first node
if (list1.val < list2.val) {
    head = list1;
    node = list1;
    list1 = list1.next;
} else {
    head = list2;
    node = list2;
    list2 = list2.next;
}
// Then continue merging...
```
❌ Complex, error-prone

With dummy:
```csharp
ListNode dummy = new ListNode(0);
ListNode node = dummy;
// No special case needed!
```
✓ Clean, simple

REAL-WORLD APPLICATIONS:
- Merge sort implementation
- Database query result merging
- Combining sorted streams
- Priority queue operations
- File merging in external sorting

PATTERN RECOGNITION:
- "Merge two sorted" → two-pointer technique
- "Linked lists" → dummy node pattern
- "Sorted input" → can optimize with direct attachment
- Similar to: merge intervals, merge K lists

COMMON MISTAKES TO AVOID:
❌ Not using dummy node (complex first node logic)
❌ Forgetting to attach remaining nodes
❌ Creating new nodes (inefficient, not required)
❌ Not advancing pointers correctly
❌ Comparing node references instead of values

FOLLOW-UP EXTENSIONS:
- Merge K sorted lists (use min heap)
- Merge in descending order
- Remove duplicates while merging
- Merge with extra constraint (e.g., alternating)

OPTIMIZATION NOTES:
- Already optimal: O(n + m) time, O(1) space
- Can't do better than visiting each node once
- In-place merging (no extra nodes created)
- Could inline the final if-else but current version is clearer
*/