/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 23. Merge k Sorted Lists
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
You are given an array of k linked-lists lists, each linked-list is sorted in 
ascending order.

Merge all the linked-lists into one sorted linked-list and return it.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: lists = [[1,4,5],[1,3,4],[2,6]]
Output: [1,1,2,3,4,4,5,6]
Explanation: The linked-lists are:
[
  1→4→5,
  1→3→4,
  2→6
]
merging them into one sorted list:
1→1→2→3→4→4→5→6

Example 2:
Input: lists = []
Output: []

Example 3:
Input: lists = [[]]
Output: []

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- k == lists.length
- 0 <= k <= 10^4
- 0 <= lists[i].length <= 500
- -10^4 <= lists[i][j] <= 10^4
- lists[i] is sorted in ascending order
- The sum of lists[i].length will not exceed 10^4

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Divide and Conquer Approach**: Merge lists in pairs repeatedly
   - Round 1: Merge pairs → k/2 lists
   - Round 2: Merge pairs → k/4 lists
   - Continue until 1 list remains
   - Similar to merge sort's merge phase

2. **Why Better Than Naive Approach**:
   - Naive: Merge lists one by one (list1+list2, then result+list3, etc.)
   - Time: O(kN) where N = total nodes
   - Divide & Conquer: O(N log k)
   - Significant improvement for large k

3. **Pairing Strategy**:
   - Take lists at indices i and i+1
   - If odd number of lists, last list has no pair (merge with null)
   - Reduces list count by ~half each round

4. **Why This Works**:
   - Merging two sorted lists creates a sorted list
   - Merging pairs in parallel maintains sorted property
   - Final result is one sorted list

5. **Alternative Approaches**:
   - Min Heap: O(N log k) time, better for streaming
   - Naive Sequential: O(kN) time, simple but slow
   - This approach: O(N log k), optimal and clean

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(N log k)
- N = total number of nodes across all lists
- k = number of lists
- Number of merge rounds: log k (divide by 2 each round)
- Each round processes all N nodes
- Total: O(N) * O(log k) = O(N log k)

Detailed breakdown:
- Round 1: k/2 merges, each processing ~2N/k nodes = N total
- Round 2: k/4 merges, each processing ~4N/k nodes = N total
- ...
- Final: 1 merge processing N nodes = N total
- Total rounds: log₂(k)
- Total work: O(N log k)

Space Complexity: O(log k)
- mergedLists array: O(k) at start, shrinks each round
- Average size across all rounds: O(k + k/2 + k/4 + ...) = O(k)
- But we reuse the array, so O(k) space for intermediate lists
- Recursion depth for merge: O(1) (iterative)
- If we don't count output list: O(k) for intermediate storage

Note: Some implementations use O(1) by modifying in-place, but this is cleaner.

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
    // MAIN METHOD: Merge K Sorted Lists using Divide and Conquer
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Merges k sorted linked lists into one sorted list
    /// Uses divide-and-conquer: repeatedly merges lists in pairs
    /// Time: O(N log k), Space: O(log k) where N = total nodes, k = list count
    /// </summary>
    /// <param name="lists">Array of sorted linked lists</param>
    /// <returns>Head of merged sorted list</returns>
    public ListNode MergeKLists(ListNode[] lists)
    {
        // ────────────────────────────────────────────────────────────────────
        // EDGE CASE: Empty input or no lists
        // ────────────────────────────────────────────────────────────────────
        if (lists == null || lists.Length == 0)
        {
            return null;
        }

        // ────────────────────────────────────────────────────────────────────
        // DIVIDE AND CONQUER: Repeatedly merge pairs until one list remains
        // ────────────────────────────────────────────────────────────────────
        // Strategy:
        // Round 1: [L0, L1, L2, L3, L4, L5] → [L0+L1, L2+L3, L4+L5]
        // Round 2: [L0+L1, L2+L3, L4+L5]    → [(L0+L1)+(L2+L3), L4+L5]
        // Round 3: [(L0+L1)+(L2+L3), L4+L5] → [Final merged list]
        
        while (lists.Length > 1)
        {
            // Create list to store merged results from this round
            List<ListNode> mergedLists = new List<ListNode>();
            
            // ────────────────────────────────────────────────────────────────
            // Merge pairs of lists
            // ────────────────────────────────────────────────────────────────
            // Step through array by 2, merging lists[i] with lists[i+1]
            for (int i = 0; i < lists.Length; i += 2)
            {
                // First list of the pair
                ListNode l1 = lists[i];
                
                // Second list of the pair (might not exist if odd number of lists)
                // If i+1 is out of bounds, treat as null (merge l1 with nothing)
                ListNode l2 = (i + 1) < lists.Length ? lists[i + 1] : null;
                
                // Merge the two lists and add result to mergedLists
                // If l2 is null, MergeList will just return l1
                mergedLists.Add(MergeList(l1, l2));
            }
            
            // ────────────────────────────────────────────────────────────────
            // Replace original array with merged results
            // ────────────────────────────────────────────────────────────────
            // Array size reduced by approximately half
            // Continue until only one list remains
            lists = mergedLists.ToArray();
        }
        
        // ────────────────────────────────────────────────────────────────────
        // Return the final merged list
        // ────────────────────────────────────────────────────────────────────
        // At this point, lists.Length == 1
        return lists[0];
    }

    // ════════════════════════════════════════════════════════════════════════
    // HELPER METHOD: Merge two sorted linked lists
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Merges two sorted linked lists into one sorted list
    /// Classic two-pointer approach with dummy node
    /// Time: O(n + m), Space: O(1) where n, m are list lengths
    /// </summary>
    /// <param name="l1">First sorted list (or null)</param>
    /// <param name="l2">Second sorted list (or null)</param>
    /// <returns>Head of merged sorted list</returns>
    private ListNode MergeList(ListNode l1, ListNode l2)
    {
        // ────────────────────────────────────────────────────────────────────
        // Create dummy node to simplify edge cases
        // ────────────────────────────────────────────────────────────────────
        // Dummy node eliminates special case for first node
        // tail tracks the last node in merged list
        ListNode dummy = new ListNode();
        ListNode tail = dummy;

        // ────────────────────────────────────────────────────────────────────
        // Merge lists while both have nodes
        // ────────────────────────────────────────────────────────────────────
        // Compare front nodes, take smaller one, advance that list
        while (l1 != null && l2 != null)
        {
            if (l1.val < l2.val)
            {
                // l1's value is smaller, add it to result
                tail.next = l1;
                l1 = l1.next;
            }
            else
            {
                // l2's value is smaller or equal, add it to result
                tail.next = l2;
                l2 = l2.next;
            }
            
            // Move tail pointer forward
            tail = tail.next;
        }

        // ────────────────────────────────────────────────────────────────────
        // Append remaining nodes from non-empty list
        // ────────────────────────────────────────────────────────────────────
        // At most one of these will execute
        // No need to traverse remaining nodes, just link them
        if (l1 != null)
        {
            tail.next = l1;
        }
        if (l2 != null)
        {
            tail.next = l2;
        }

        // Return merged list (skip dummy node)
        return dummy.next;
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example: lists = [[1,4,5],[1,3,4],[2,6]]
═══════════════════════════════════════════════════════════════════════════════

Initial State:
───────────────────────────────────────────────────────
lists = [
  L0: 1→4→5→null,
  L1: 1→3→4→null,
  L2: 2→6→null
]

Visual:
L0: 1 → 4 → 5
L1: 1 → 3 → 4
L2: 2 → 6

╔════════════════════════════════════════════════════════════════════════════╗
║ ROUND 1: Merge pairs                                                      ║
╚════════════════════════════════════════════════════════════════════════════╝

Iteration i=0: Merge L0 and L1
───────────────────────────────────────────────────────
L0: 1→4→5
L1: 1→3→4

Merge Process:
  Compare 1 vs 1 → take L0's 1 → Result: 1
  Compare 4 vs 1 → take L1's 1 → Result: 1→1
  Compare 4 vs 3 → take L1's 3 → Result: 1→1→3
  Compare 4 vs 4 → take L1's 4 → Result: 1→1→3→4
  L1 empty, append L0: → Result: 1→1→3→4→4→5

Result M0: 1→1→3→4→4→5

Iteration i=2: Merge L2 and null
───────────────────────────────────────────────────────
L2: 2→6
null

MergeList(2→6, null) simply returns 2→6 (no merge needed)

Result M1: 2→6

After Round 1:
───────────────────────────────────────────────────────
lists = [
  M0: 1→1→3→4→4→5,
  M1: 2→6
]

Visual:
M0: 1 → 1 → 3 → 4 → 4 → 5
M1: 2 → 6

╔════════════════════════════════════════════════════════════════════════════╗
║ ROUND 2: Merge pairs (final round)                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

Iteration i=0: Merge M0 and M1
───────────────────────────────────────────────────────
M0: 1→1→3→4→4→5
M1: 2→6

Merge Process:
  Compare 1 vs 2 → take M0's 1 → Result: 1
  Compare 1 vs 2 → take M0's 1 → Result: 1→1
  Compare 3 vs 2 → take M1's 2 → Result: 1→1→2
  Compare 3 vs 6 → take M0's 3 → Result: 1→1→2→3
  Compare 4 vs 6 → take M0's 4 → Result: 1→1→2→3→4
  Compare 4 vs 6 → take M0's 4 → Result: 1→1→2→3→4→4
  Compare 5 vs 6 → take M0's 5 → Result: 1→1→2→3→4→4→5
  M0 empty, append M1: → Result: 1→1→2→3→4→4→5→6

Final Result: 1→1→2→3→4→4→5→6

After Round 2:
───────────────────────────────────────────────────────
lists = [1→1→2→3→4→4→5→6]

lists.Length == 1, exit while loop

FINAL OUTPUT: 1→1→2→3→4→4→5→6 ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 DETAILED WALKTHROUGH - 4 Lists Example
═══════════════════════════════════════════════════════════════════════════════

Input: lists = [[1,5,9],[2,6],[3,7,11],[4,8,10]]

Initial:
───────────────────────────────────────────────────────
L0: 1→5→9
L1: 2→6
L2: 3→7→11
L3: 4→8→10

ROUND 1: Merge Pairs
───────────────────────────────────────────────────────

Pair 1: Merge L0(1→5→9) with L1(2→6)
  → Result: 1→2→5→6→9

Pair 2: Merge L2(3→7→11) with L3(4→8→10)
  → Result: 3→4→7→8→10→11

After Round 1:
lists = [
  1→2→5→6→9,
  3→4→7→8→10→11
]

ROUND 2: Merge Pairs (Final)
───────────────────────────────────────────────────────

Pair 1: Merge (1→2→5→6→9) with (3→4→7→8→10→11)
  → Result: 1→2→3→4→5→6→7→8→9→10→11

After Round 2:
lists = [1→2→3→4→5→6→7→8→9→10→11]

FINAL: 1→2→3→4→5→6→7→8→9→10→11 ✓

Rounds needed: log₂(4) = 2 rounds ✓

═══════════════════════════════════════════════════════════════════════════════
💡 COMPLEXITY ANALYSIS - DETAILED
═══════════════════════════════════════════════════════════════════════════════

Given:
- k = number of lists
- N = total number of nodes across all lists
- Average list length = N/k

Time Complexity Breakdown:
───────────────────────────────────────────────────────

Round 1: k/2 merges
  - Each merge combines ~2N/k nodes
  - Work per merge: O(2N/k)
  - Total work: (k/2) * O(2N/k) = O(N)

Round 2: k/4 merges
  - Each merge combines ~4N/k nodes
  - Work per merge: O(4N/k)
  - Total work: (k/4) * O(4N/k) = O(N)

...

Final Round: 1 merge
  - Combines all N nodes
  - Work: O(N)

Total Rounds: log₂(k)
Total Work: O(N) per round × log₂(k) rounds = O(N log k)

Space Complexity:
───────────────────────────────────────────────────────

Intermediate Storage:
- Round 1: k/2 list pointers
- Round 2: k/4 list pointers
- ...
- Total: k/2 + k/4 + k/8 + ... ≈ k

Actual implementation uses O(k) for mergedLists array

If we count only auxiliary space: O(k)
If we ignore output list: O(k)

═══════════════════════════════════════════════════════════════════════════════
🔄 ALGORITHM COMPARISON
═══════════════════════════════════════════════════════════════════════════════

APPROACH 1: Naive Sequential Merging
───────────────────────────────────────────────────────
```csharp
ListNode result = lists[0];
for (int i = 1; i < lists.Length; i++) {
    result = MergeTwoLists(result, lists[i]);
}
```

Time Complexity: O(kN)
- Merge 1: O(N/k + N/k) = O(2N/k)
- Merge 2: O(2N/k + N/k) = O(3N/k)
- Merge 3: O(3N/k + N/k) = O(4N/k)
- ...
- Merge k: O((k-1)N/k + N/k) = O(N)
- Total: O(2N/k + 3N/k + ... + N) = O(kN)

Space: O(1)

✗ Slow for large k
✓ Simple to implement

APPROACH 2: Min Heap (Priority Queue)
───────────────────────────────────────────────────────
```csharp
PriorityQueue<ListNode> heap = new();
// Add first node of each list
while (heap not empty) {
    node = heap.Dequeue();
    add to result;
    if (node.next) heap.Enqueue(node.next);
}
```

Time Complexity: O(N log k)
- N nodes total, each pushed/popped once
- Each heap operation: O(log k)
- Total: O(N log k)

Space: O(k) for heap

✓ Good for streaming (can start output immediately)
✗ More complex implementation
✓ Same time complexity as divide-and-conquer

APPROACH 3: Divide and Conquer (Current)
───────────────────────────────────────────────────────

Time: O(N log k)
Space: O(k)

✓ Optimal time complexity
✓ Clean and elegant
✓ Easy to understand
✓ No external dependencies (no PriorityQueue needed)

Best Choice: Divide and Conquer or Min Heap
- Both O(N log k)
- D&C simpler in C# (no built-in PriorityQueue in older versions)
- Heap better for streaming scenarios

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Empty array:
   lists = []
   Result: null ✓

✅ Array with one empty list:
   lists = [[]]
   Result: null ✓

✅ Array with multiple empty lists:
   lists = [[], [], []]
   After Round 1: [null, null]
   After Round 2: [null]
   Result: null ✓

✅ Single list:
   lists = [[1,2,3]]
   Result: [1,2,3] (no merging needed) ✓

✅ Two lists:
   lists = [[1,3], [2,4]]
   Result: [1,2,3,4] ✓

✅ Odd number of lists:
   lists = [[1], [2], [3]]
   Round 1: merge(1,2)→[1,2], merge(3,null)→[3]
   Round 2: merge([1,2],[3])→[1,2,3]
   Result: [1,2,3] ✓

✅ Lists of very different lengths:
   lists = [[1], [2,3,4,5,6,7,8,9,10]]
   Handles correctly ✓

✅ Duplicate values across lists:
   lists = [[1,1,1], [1,1,1]]
   Result: [1,1,1,1,1,1] ✓

✅ Negative values:
   lists = [[-2,-1], [-3,0]]
   Result: [-3,-2,-1,0] ✓

✅ All same values:
   lists = [[5,5,5], [5,5], [5]]
   Result: [5,5,5,5,5,5] ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Not handling odd number of lists
   - When i+1 >= lists.Length, must pass null to merge
   - Current code correctly handles this: (i + 1) < lists.Length ? ...

❌ Not handling empty lists in array
   - MergeList correctly handles null inputs
   - Returns the non-null list, or null if both null

❌ Forgetting to advance tail pointer
   - Must do tail = tail.next after each append
   - Otherwise, stuck in infinite loop

❌ Creating new nodes in merge
   - Should reuse existing nodes, not create copies
   - Just rearrange pointers

❌ Not using dummy node
   - Makes code much more complex
   - Need special case for first node

❌ Off-by-one in loop
   - i += 2 is correct (pairs)
   - i++ would process each list multiple times

❌ Modifying while iterating
   - Can't modify lists array during for loop
   - Correctly uses separate mergedLists

❌ Not checking for null/empty input
   - Must handle lists == null or lists.Length == 0
   - Current code handles both

❌ Inefficient array operations
   - ToArray() is O(n) but called O(log k) times
   - Acceptable since dominated by merge operations

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Divide and Conquer**:
   - Break problem into smaller subproblems
   - Solve recursively (or iteratively)
   - Combine solutions
   - Similar to merge sort

2. **Tournament Method**:
   - Pair-wise competition
   - Winners advance to next round
   - Continue until one winner
   - Like sports tournaments

3. **Reduction by Half**:
   - Problem size reduced by half each iteration
   - Leads to O(log n) rounds
   - Common in optimal algorithms

4. **Dummy Node Pattern**:
   - Simplifies linked list operations
   - Eliminates edge cases
   - Standard technique

5. **Two-Pointer Merging**:
   - Classic merge of two sorted sequences
   - Foundation of merge sort
   - O(n + m) time

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Database Query Results**:
   - Merge sorted results from multiple shards
   - Distributed database queries
   - Each shard returns sorted data

2. **Log File Analysis**:
   - Merge logs from multiple servers
   - Each server's log is time-sorted
   - Produce unified timeline

3. **External Sorting**:
   - Sorting data too large for memory
   - Split into sorted chunks
   - Merge chunks efficiently

4. **Search Engine**:
   - Merge results from multiple indices
   - Each index returns sorted by relevance
   - Combine into final ranking

5. **Time Series Data**:
   - Merge multiple time series
   - Each series chronologically ordered
   - Produce unified timeline

6. **Version Control**:
   - Merge multiple branches
   - Each branch has sorted commits
   - Create merged history

7. **Stream Processing**:
   - Merge multiple data streams
   - Each stream ordered
   - Real-time unified stream

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Merge Two Sorted Lists (LeetCode 21)**:
   - Foundation of this problem
   - Helper function here
   - Simpler, single merge

2. **Merge Sorted Array (LeetCode 88)**:
   - Merge two sorted arrays
   - Similar concept, different structure
   - Can merge in-place

3. **Find K Pairs with Smallest Sums (LeetCode 373)**:
   - Similar to merging k lists
   - Use min heap approach
   - Find k smallest pairs

4. **Kth Smallest Element in Sorted Matrix (LeetCode 378)**:
   - Matrix where each row sorted
   - Similar to k sorted lists
   - Use heap or binary search

5. **Smallest Range Covering Elements from K Lists (LeetCode 632)**:
   - Find smallest range containing element from each list
   - Uses similar merging concept
   - More complex optimization

6. **Sort Items by Groups Respecting Dependencies (LeetCode 1203)**:
   - Topological sort + merging
   - Complex graph problem
   - Similar divide-and-conquer

7. **Merge k Sorted Arrays (Interview Question)**:
   - Arrays instead of linked lists
   - Same algorithms apply
   - Slight implementation differences

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Solution Analysis:

Time: O(N log k) - Optimal ✓
- Cannot do better asymptotically
- Must process all N nodes
- log k comparisons per node minimum

Space: O(k) - Near Optimal ✓
- Could potentially do O(1) with in-place modifications
- But current approach is cleaner

Why This is Optimal:

1. **Lower Bound**:
   - Must examine all N nodes: Ω(N)
   - Must do k-way comparison: Ω(log k) per node
   - Therefore: Ω(N log k)

2. **Upper Bound**:
   - Current algorithm: O(N log k)
   - Matches lower bound
   - Therefore optimal!

Micro-optimizations (Not Recommended):

1. **Avoid ToArray()**:
   - Could use array directly
   - Minimal benefit
   - Hurts readability

2. **In-place Merging**:
   - Could modify input array directly
   - Saves space but mutates input
   - Not worth complexity

3. **Custom List Structure**:
   - Could use LinkedList<ListNode>
   - Avoid ToArray() overhead
   - Marginal benefit

Best Practices:
✓ Current implementation is production-ready
✓ Clean, readable, correct
✓ Optimal time complexity
✓ Reasonable space usage

When to Use Each Approach:

Divide and Conquer (Current):
- ✓ All lists available at start
- ✓ Want simple, clean code
- ✓ No streaming required

Min Heap:
- ✓ Lists arriving dynamically
- ✓ Want to start output early
- ✓ Streaming scenario

Sequential Merge:
- ✓ Only acceptable for small k
- ✓ When simplicity paramount
- ✗ Generally avoid for large k

═══════════════════════════════════════════════════════════════════════════════
*/