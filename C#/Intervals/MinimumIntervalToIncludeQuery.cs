/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 1851. Minimum Interval to Include Each Query
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
You are given a 2D integer array intervals, where intervals[i] = [left_i, right_i] 
describes the ith interval starting at left_i and ending at right_i (inclusive). 
The size of an interval is defined as the number of integers it contains, or more 
formally right_i - left_i + 1.

You are also given an integer array queries. The answer to the jth query is the 
size of the SMALLEST interval i such that left_i <= queries[j] <= right_i. If no 
such interval exists, the answer is -1.

Return an array containing the answers to the queries.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: intervals = [[1,4],[2,4],[3,6],[4,4]], queries = [2,3,4,5]
Output: [3,3,1,4]
Explanation:
- Query 2: [1,4] (size 4), [2,4] (size 3) ← smallest
  Output: 3
  
- Query 3: [1,4] (size 4), [2,4] (size 3) ← smallest, [3,6] (size 4)
  Output: 3
  
- Query 4: [1,4] (size 4), [2,4] (size 3), [3,6] (size 4), [4,4] (size 1) ← smallest
  Output: 1
  
- Query 5: [3,6] (size 4) ← only interval containing 5
  Output: 4

Example 2:
Input: intervals = [[2,3],[2,5],[1,8],[20,25]], queries = [2,19,5,22]
Output: [2,-1,4,6]
Explanation:
- Query 2: [2,3] (size 2) ← smallest, [2,5] (size 4), [1,8] (size 8)
- Query 19: No interval contains 19 → -1
- Query 5: [2,5] (size 4) ← smallest, [1,8] (size 8)
- Query 22: [20,25] (size 6) → only one

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 1 <= intervals.length <= 10^5
- 1 <= queries.length <= 10^5
- intervals[i].length == 2
- 1 <= left_i <= right_i <= 10^7
- 1 <= queries[j] <= 10^7

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Sorting Strategy**: Process queries in sorted order (left to right sweep)
   - Allows us to add intervals incrementally as we move right
   - Once an interval is added, we know it starts before or at current query

2. **Min Heap for Smallest Size**: Use priority queue ordered by interval size
   - Always gives us the smallest valid interval at the top
   - Efficiently find minimum among all valid intervals

3. **Sweep Line Algorithm**: Process events from left to right
   - Add intervals when query point reaches their start
   - Remove intervals when query point passes their end
   - Current valid intervals are those in the heap with end >= query

4. **Result Restoration**: Queries must be answered in original order
   - Store results in dictionary keyed by query value
   - Reconstruct final array using original query order

5. **Interval Size**: size = right - left + 1 (inclusive range)
   - [1,4] has size 4 (contains 1,2,3,4)
   - [4,4] has size 1 (contains only 4)

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(n log n + q log q + (n + q) log n)
- Sorting intervals: O(n log n) where n = intervals.length
- Sorting queries: O(q log q) where q = queries.length
- Processing queries:
  - Each interval added once: O(n log n) total heap insertions
  - Each interval removed once: O(n log n) total heap deletions
  - Processing q queries: O(q)
  - Heap peek operations: O(q)
- Building result: O(q)
- Overall: O(n log n + q log q + n log n) = O((n + q) log n)

Space Complexity: O(n + q)
- Min heap: O(n) for storing intervals
- Sorted queries array: O(q)
- Result dictionary: O(q) unique query values
- Result array: O(q)
- Overall: O(n + q)

═══════════════════════════════════════════════════════════════════════════════
*/

public class Solution
{
    public int[] MinInterval(int[][] intervals, int[] queries)
    {
        // ════════════════════════════════════════════════════════════════════
        // STEP 1: Sort intervals by start time
        // ════════════════════════════════════════════════════════════════════
        // Why sort by start?
        // - Process intervals from left to right
        // - As we sweep through queries, we can add intervals incrementally
        // - Once query point >= interval start, that interval is a candidate
        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

        // ════════════════════════════════════════════════════════════════════
        // STEP 2: Initialize data structures
        // ════════════════════════════════════════════════════════════════════
        
        // CRITICAL: PriorityQueue<TElement, TPriority> requires TWO type parameters!
        // TElement: (int Size, int End) - the tuple we're storing
        // TPriority: int - the priority value (interval size, lower = higher priority)
        var minHeap = new PriorityQueue<(int Size, int End), int>();
        
        // Dictionary to store answer for each query value
        // Key: query value, Value: smallest interval size
        // Needed because we process queries in sorted order but must return in original order
        var res = new Dictionary<int, int>();
        
        // Pointer to track which intervals we've already added to heap
        int i = 0;
        
        // ════════════════════════════════════════════════════════════════════
        // STEP 3: Sort queries to enable sweep line algorithm
        // ════════════════════════════════════════════════════════════════════
        // Processing queries in sorted order allows us to:
        // - Add intervals progressively as we move right
        // - Remove intervals that no longer contain the query point
        // - Avoid re-processing intervals
        int[] sortedQueries = queries.OrderBy(q => q).ToArray();
        
        // ════════════════════════════════════════════════════════════════════
        // STEP 4: Process each query in sorted order (sweep line)
        // ════════════════════════════════════════════════════════════════════
        foreach (int q in sortedQueries)
        {
            // ────────────────────────────────────────────────────────────────
            // PHASE A: Add all intervals that START before or at query point
            // ────────────────────────────────────────────────────────────────
            // These are potential candidates for containing the query point
            // We add them to heap ordered by size (smallest first)
            
            while (i < intervals.Length && intervals[i][0] <= q)
            {
                int l = intervals[i][0];  // Start of interval
                int r = intervals[i][1];  // End of interval
                
                // Calculate interval size (inclusive range)
                // Example: [1,4] has size 4 (contains 1,2,3,4)
                int size = r - l + 1;
                
                // Add to min heap with size as priority
                // Lower size = higher priority (will be at top)
                // Store both size (for answer) and end (for validity check)
                minHeap.Enqueue((size, r), size);
                
                i++;  // Move to next interval
            }

            // ────────────────────────────────────────────────────────────────
            // PHASE B: Remove intervals that END before query point
            // ────────────────────────────────────────────────────────────────
            // These intervals no longer contain the query point
            // They are invalid and should not be considered
            //
            // Example: Query = 5, Interval = [1,3]
            // Interval ends at 3, which is < 5, so it doesn't contain 5
            
            while (minHeap.Count > 0 && minHeap.Peek().End < q)
            {
                minHeap.Dequeue();  // Remove invalid interval
            }
            
            // ────────────────────────────────────────────────────────────────
            // PHASE C: Get answer for current query
            // ────────────────────────────────────────────────────────────────
            // After removing invalid intervals, heap contains only valid ones:
            // - Intervals where start <= q (added in Phase A)
            // - Intervals where end >= q (not removed in Phase B)
            // Therefore: start <= q <= end (interval contains q)
            //
            // The top of min heap is the smallest valid interval
            
            res[q] = minHeap.Count == 0 ? -1 : minHeap.Peek().Size;
        }

        // ════════════════════════════════════════════════════════════════════
        // STEP 5: Build result array in ORIGINAL query order
        // ════════════════════════════════════════════════════════════════════
        // We processed queries in sorted order, but must return results
        // in the same order as the input queries array
        
        int[] result = new int[queries.Length];
        for (int j = 0; j < queries.Length; j++)
        {
            // Lookup answer for this query value from dictionary
            result[j] = res[queries[j]];
        }
        
        return result;
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example: intervals=[[1,4],[2,4],[3,6],[4,4]], 
                                     queries=[2,3,4,5]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Sort Intervals by Start
───────────────────────────────────────────────────────
Sorted: [[1,4], [2,4], [3,6], [4,4]]  (already sorted)

Visual Timeline:
0   1   2   3   4   5   6   7
    |----[1,4]----|
        |--[2,4]--|
            |--[3,6]--|
                |[4,4]|

STEP 2: Initialize
───────────────────────────────────────────────────────
minHeap = empty (PriorityQueue<(int,int), int>)
res = {}
i = 0

STEP 3: Sort Queries
───────────────────────────────────────────────────────
Original: [2, 3, 4, 5]
Sorted:   [2, 3, 4, 5]  (already sorted)

STEP 4: Process Queries
───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Query: q = 2                                                       ║
╚════════════════════════════════════════════════════════════════════╝

Phase A: Add intervals where start <= 2
  - intervals[0] = [1,4], start=1 <= 2 ✓
    size = 4-1+1 = 4
    minHeap.Enqueue((4, 4), priority=4)
    i = 1
    
  - intervals[1] = [2,4], start=2 <= 2 ✓
    size = 4-2+1 = 3
    minHeap.Enqueue((3, 4), priority=3)
    i = 2
    
  - intervals[2] = [3,6], start=3 <= 2? NO, stop
  
  Current heap (ordered by priority/size): [(3,4), (4,4)]
  Top: (3, 4) - size 3, ends at 4

Phase B: Remove intervals where end < 2
  - Peek: (3, 4), end=4 < 2? NO, keep all
  
  Current heap: [(3,4), (4,4)]

Phase C: Get answer
  - Heap not empty
  - Smallest interval: size = 3
  - res[2] = 3 ✓

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Query: q = 3                                                       ║
╚════════════════════════════════════════════════════════════════════╝

Phase A: Add intervals where start <= 3
  - intervals[2] = [3,6], start=3 <= 3 ✓
    size = 6-3+1 = 4
    minHeap.Enqueue((4, 6), priority=4)
    i = 3
    
  - intervals[3] = [4,4], start=4 <= 3? NO, stop
  
  Current heap: [(3,4), (4,4), (4,6)]

Phase B: Remove intervals where end < 3
  - Peek: (3, 4), end=4 < 3? NO, keep all
  
  Current heap: [(3,4), (4,4), (4,6)]

Phase C: Get answer
  - Smallest interval: size = 3
  - res[3] = 3 ✓

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Query: q = 4                                                       ║
╚════════════════════════════════════════════════════════════════════╝

Phase A: Add intervals where start <= 4
  - intervals[3] = [4,4], start=4 <= 4 ✓
    size = 4-4+1 = 1
    minHeap.Enqueue((1, 4), priority=1)
    i = 4 (no more intervals)
  
  Current heap: [(1,4), (3,4), (4,4), (4,6)]

Phase B: Remove intervals where end < 4
  - Peek: (1, 4), end=4 < 4? NO, keep all
  
  Current heap: [(1,4), (3,4), (4,4), (4,6)]

Phase C: Get answer
  - Smallest interval: size = 1
  - res[4] = 1 ✓

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Query: q = 5                                                       ║
╚════════════════════════════════════════════════════════════════════╝

Phase A: Add intervals where start <= 5
  - i = 4, no more intervals to add
  
  Current heap: [(1,4), (3,4), (4,4), (4,6)]

Phase B: Remove intervals where end < 5
  - Peek: (1, 4), end=4 < 5? YES, remove
    Dequeue: (1, 4)
  
  - Peek: (3, 4), end=4 < 5? YES, remove
    Dequeue: (3, 4)
  
  - Peek: (4, 4), end=4 < 5? YES, remove
    Dequeue: (4, 4)
  
  - Peek: (4, 6), end=6 < 5? NO, stop
  
  Current heap: [(4,6)]

Phase C: Get answer
  - Heap not empty
  - Only valid interval: [3,6] with size = 4
  - res[5] = 4 ✓

───────────────────────────────────────────────────────

STEP 5: Build Result in Original Order
───────────────────────────────────────────────────────
Original queries: [2, 3, 4, 5]
Dictionary res: {2→3, 3→3, 4→1, 5→4}

result[0] = res[2] = 3
result[1] = res[3] = 3
result[2] = res[4] = 1
result[3] = res[5] = 4

FINAL RESULT: [3, 3, 1, 4] ✓

═══════════════════════════════════════════════════════════════════════════════
🔧 CRITICAL FIX EXPLANATION:
═══════════════════════════════════════════════════════════════════════════════

❌ INCORRECT Declaration:
```csharp
var minHeap = new PriorityQueue<(int Size, int End)>();
```
Problem: Missing TPriority type parameter

✅ CORRECT Declaration:
```csharp
var minHeap = new PriorityQueue<(int Size, int End), int>();
```
- TElement: (int Size, int End) - what we store
- TPriority: int - the priority value (interval size)

C# PriorityQueue Signature:
```csharp
public class PriorityQueue<TElement, TPriority>
```

Both type parameters are REQUIRED!

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ No interval contains query:
   intervals = [[1,2],[5,6]], queries = [3]
   Result: [-1] ✓

✅ Single interval, single query:
   intervals = [[1,5]], queries = [3]
   Result: [5] ✓

✅ Query at interval boundary:
   intervals = [[1,5]], queries = [1,5]
   Result: [5,5] ✓

✅ Nested intervals:
   intervals = [[1,10],[2,5]], queries = [3]
   Sizes: 10 and 6, return 6 ✓

✅ Duplicate queries:
   intervals = [[1,5]], queries = [3,3,3]
   Result: [5,5,5] ✓

✅ Point intervals:
   intervals = [[5,5]], queries = [5]
   Size 1, result: [1] ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Wrong PriorityQueue declaration (missing TPriority)
❌ Not sorting queries before processing
❌ Forgetting to restore original query order
❌ Using max heap instead of min heap
❌ Wrong interval size: (end - start) instead of (end - start + 1)
❌ Not removing expired intervals from heap
❌ Comparing with < instead of <= when adding intervals

═══════════════════════════════════════════════════════════════════════════════
*/