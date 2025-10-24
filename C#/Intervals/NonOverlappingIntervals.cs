/*
PROBLEM: 435. Non-overlapping Intervals (Erase Overlap Intervals)
Given an array of intervals where intervals[i] = [start_i, end_i], return the 
MINIMUM number of intervals you need to REMOVE to make the rest of the intervals 
non-overlapping.

Note: Intervals are overlapping if they share any point. For example, [1,2] and 
[2,3] have a common point 2, so they are considered overlapping.

Example 1:
Input: intervals = [[1,2],[2,3],[3,4],[1,3]]
Output: 1
Explanation: Remove [1,3] and the rest don't overlap.

Example 2:
Input: intervals = [[1,2],[1,2],[1,2]]
Output: 2
Explanation: Remove 2 intervals, keep 1.

Example 3:
Input: intervals = [[1,2],[2,3]]
Output: 0
Explanation: No intervals overlap.

Constraints:
- 1 <= intervals.length <= 10^5
- intervals[i].length == 2
- -5 * 10^4 <= start_i < end_i <= 5 * 10^4
*/

public class Solution
{
    public int EraseOverlapIntervals(int[][] intervals)
    {
        // PHASE 1: Sort intervals by ending time (greedy choice)
        // Why sort by end? Intervals ending earlier leave more "space" for future intervals
        // This is the key greedy insight: always prefer intervals that finish sooner
        Array.Sort(intervals, (a, b) => a[1].CompareTo(b[1]));
        
        int n = intervals.Length;
        
        // PHASE 2: Initialize DP array
        // dp[i] = maximum number of non-overlapping intervals we can keep
        //         considering intervals from 0 to i (where interval i is included/considered)
        int[] dp = new int[n];
        
        // Base case: First interval by itself forms 1 non-overlapping set
        dp[0] = 1;

        // PHASE 3: Fill DP table using binary search for optimization
        for (int i = 1; i < n; i++)
        {
            // Find the rightmost interval (at index idx-1) that DOESN'T overlap with current
            // We're looking for intervals where: intervals[j].end <= intervals[i].start
            int idx = Bs(i, intervals[i][0], intervals);
            
            if (idx == 0)
            {
                // CASE 1: All previous intervals overlap with current interval
                // Cannot include current with any previous ones
                // Best option: carry forward the previous maximum count
                dp[i] = dp[i - 1];
            }
            else
            {
                // CASE 2: Found non-overlapping intervals (at positions 0 to idx-1)
                // Two choices:
                //   Option A: Don't include current interval → dp[i-1]
                //   Option B: Include current interval → 1 + dp[idx-1]
                //             (current + best non-overlapping up to idx-1)
                // Take maximum of both choices
                dp[i] = Math.Max(dp[i - 1], 1 + dp[idx - 1]);
            }
        }
        
        // PHASE 4: Calculate final result
        // Total intervals - Maximum we can keep = Minimum to remove
        return n - dp[n - 1];
    }

    // Binary Search Helper: Find leftmost position where intervals[pos].end > target
    // Returns the count of intervals that DON'T overlap (end <= target)
    private int Bs(int r, int target, int[][] intervals)
    {
        int l = 0;  // Left pointer (inclusive)
        
        // Search for first interval where end > target (overlaps with target start)
        while (l < r)
        {
            int m = (l + r) >> 1;  // Middle index (bit shift for division by 2)
            
            if (intervals[m][1] <= target)
            {
                // intervals[m] DOESN'T overlap (its end <= current start)
                // Search right half for more non-overlapping intervals
                l = m + 1;
            }
            else
            {
                // intervals[m] OVERLAPS (its end > current start)
                // Search left half
                r = m;
            }
        }
        
        // Return value meaning:
        // - If l = 0: All intervals in [0, r-1] overlap with target
        // - If l > 0: Intervals [0, l-1] don't overlap, [l, ...] overlap
        return l;
    }
}


/* 
TIME COMPLEXITY: O(n log n)
- Sorting by end time: O(n log n)
- DP loop: O(n) iterations
- Binary search per iteration: O(log n)
- Total: O(n log n) + O(n * log n) = O(n log n)

SPACE COMPLEXITY: O(n)
- dp array: O(n)
- Sorting space: O(log n) to O(n) depending on implementation
- Overall: O(n)

ALGORITHM EXPLANATION:
Dynamic Programming with Binary Search optimization:
1. Sort intervals by ending time (greedy strategy)
2. Use DP to find maximum non-overlapping intervals we can keep
3. Use binary search to efficiently find compatible previous intervals
4. Answer = Total intervals - Maximum kept

KEY INSIGHTS:
- Reframe problem: "minimum to remove" = "total - maximum to keep"
- Greedy choice: Sort by end time (intervals ending earlier leave more room)
- DP state: dp[i] = max non-overlapping intervals we can select up to i
- Binary search: Quickly find last non-overlapping interval for each position
- Two intervals [a,b] and [c,d] overlap if b > c (touching counts as overlap)

GREEDY INTUITION - Why Sort by End Time?
Consider: [[1,5], [1,2], [2,3]]
- If we pick [1,5] first: Can't pick any others
- If we pick [1,2] first: Can still pick [2,3] (NO! They overlap at 2)
- If we pick [1,2] first: Actually [2,3] overlaps because touching = overlap

Better example: [[1,4], [1,2], [3,5]]
- Pick [1,4]: Can't pick [1,2] or [3,5]
- Pick [1,2]: Can pick [3,5]! (2 intervals total)
Conclusion: Intervals ending sooner allow more future selections

OVERLAP DEFINITION:
Two intervals [a,b] and [c,d] overlap if they share ANY point:
- [1,2] and [2,3] overlap (share point 2)
- [1,3] and [2,4] overlap (share points 2 and 3)
- [1,2] and [3,4] don't overlap

Technical: [a,b] and [c,d] overlap if: max(a,c) <= min(b,d)
Simpler: After sorting by end, [a,b] overlaps with [c,d] if b > c

BINARY SEARCH EXPLANATION:
For each interval i with start time S:
- Find rightmost interval j where intervals[j].end <= S
- This j is the last interval that CAN be included with interval i
- All intervals from 0 to j are compatible with i
- All intervals from j+1 to i-1 overlap with i

Example: intervals sorted by end = [[1,2], [1,3], [2,4], [3,5]]
For interval [3,5]:
  - Binary search for intervals where end <= 3
  - Find: [[1,2], [1,3]] both have end <= 3 ✓
  - [[2,4]] has end=4 > 3 ✗ (overlaps)

VISUAL TRACE with intervals = [[1,2],[2,3],[3,4],[1,3]]:

PHASE 1 - Sort by End Time:
Before: [[1,2], [2,3], [3,4], [1,3]]
After:  [[1,2], [2,3], [1,3], [3,4]]
         end=2  end=3  end=3  end=4

Timeline:
0   1   2   3   4   5
    |---[1,2]
        |---[2,3]
    |-----[1,3]---|
            |---[3,4]

PHASE 2 - Initialize DP:
dp[0] = 1  (can keep [1,2])

PHASE 3 - Fill DP Table:

i=1: interval=[2,3], start=2
     Binary Search: Bs(1, 2, intervals)
       - intervals[0]=[1,2], end=2 <= 2? YES
       - Return idx=1
     dp[1] = max(dp[0], 1 + dp[0]) = max(1, 2) = 2
     Keep: [[1,2], [2,3]] - Wait, these overlap!
     
     ERROR DETECTED IN TRACE! Let me recalculate...
     
     Actually: intervals[0].end = 2, current.start = 2
     Do they overlap? [1,2] and [2,3] share point 2 → YES, they overlap!
     
     But binary search returns idx=1 (count of non-overlapping)
     This means intervals[0] is considered non-overlapping because end <= start
     
     IMPORTANT: In this problem, intervals touching at a point are OVERLAPPING
     So binary search should use intervals[m][1] < target (not <=)
     
     Let me check the code again... The code uses: intervals[m][1] <= target
     This treats touching intervals as non-overlapping!
     
     There's a discrepancy here. Let me re-analyze...

CORRECTION: Based on the code logic:
The binary search uses intervals[m][1] <= target, which means:
- If interval ends exactly at target start, it's considered non-overlapping
- This would incorrectly handle [1,2] and [2,3] as non-overlapping

However, the problem states: "Intervals are overlapping if they share any point"
So [1,2] and [2,3] DO overlap!

The code appears to have a bug for touching intervals. Let me trace with corrected logic:

CORRECTED APPROACH:
Binary search should use: intervals[m][1] < target (strict inequality)
This ensures touching intervals are treated as overlapping.

Let me retrace with correct logic:

PHASE 3 - Fill DP Table (Corrected):

i=1: interval=[2,3], start=2
     Bs(1, 2): Find intervals where end < 2
       - intervals[0]=[1,2], end=2 < 2? NO
       - Return idx=0 (no compatible intervals)
     dp[1] = dp[0] = 1
     Best: Keep either [1,2] OR [2,3], not both

i=2: interval=[1,3], start=1
     Bs(2, 1): Find intervals where end < 1
       - No intervals have end < 1
       - Return idx=0
     dp[2] = dp[1] = 1
     Best: Keep one of the first two

i=3: interval=[3,4], start=3
     Bs(3, 3): Find intervals where end < 3
       - intervals[0]=[1,2], end=2 < 3? YES
       - intervals[1]=[2,3], end=3 < 3? NO
       - Return idx=1
     dp[3] = max(dp[2], 1 + dp[0]) = max(1, 2) = 2
     Best: Keep [1,2] and [3,4]

PHASE 4 - Calculate Result:
Total = 4, Max keep = 2
Remove = 4 - 2 = 2

But the expected answer is 1! So there's still an issue...

Let me reconsider the problem...

ACTUAL PROBLEM ANALYSIS:
Looking at Example 1: [[1,2],[2,3],[3,4],[1,3]]
Expected output: 1 (remove [1,3])

If we remove [1,3], we have: [[1,2],[2,3],[3,4]]
Do these overlap? According to problem: "share any point" means overlap
- [1,2] and [2,3] share point 2 → Should overlap
- [2,3] and [3,4] share point 3 → Should overlap

But the expected answer says these don't overlap after removing [1,3]!

RESOLUTION: The problem statement is ambiguous, but based on expected outputs:
Intervals are non-overlapping if one ends exactly where another starts!
So [1,2] and [2,3] are considered NON-OVERLAPPING!

This means: Two intervals [a,b] and [c,d] overlap ONLY if they share interior points
Mathematically: b > c AND a < d (strict inequality for overlap check)

Let me retrace with this understanding:

VISUAL TRACE with intervals = [[1,2],[2,3],[3,4],[1,3]] (FINAL CORRECT VERSION):

PHASE 1 - Sort by End Time:
Sorted: [[1,2], [2,3], [1,3], [3,4]]
         end=2  end=3  end=3  end=4

PHASE 2 - Initialize:
dp[0] = 1

PHASE 3 - Fill DP:

i=1: interval=[2,3], start=2
     Bs(1, 2): Find intervals where end <= 2
       - intervals[0]=[1,2], end=2 <= 2? YES
       - Return idx=1
     dp[1] = max(dp[0], 1 + dp[0]) = max(1, 2) = 2
     Keep: [[1,2], [2,3]] ✓

i=2: interval=[1,3], start=1
     Bs(2, 1): Find intervals where end <= 1
       - No intervals have end <= 1
       - Return idx=0
     dp[2] = dp[1] = 2
     Keep: [[1,2], [2,3]] (can't add [1,3])

i=3: interval=[3,4], start=3
     Bs(3, 3): Find intervals where end <= 3
       - intervals[0]=[1,2], end=2 <= 3? YES
       - intervals[1]=[2,3], end=3 <= 3? YES
       - intervals[2]=[1,3], end=3 <= 3? YES
       - Return idx=3
     dp[3] = max(dp[2], 1 + dp[2]) = max(2, 3) = 3
     Keep: [[1,2], [2,3], [3,4]] ✓

PHASE 4 - Result:
Total = 4, Max keep = 3
Remove = 4 - 3 = 1 ✓

This matches! The code is correct.

KEY CLARIFICATION:
In this problem, intervals [a,b] and [c,d] are non-overlapping if b <= c
They overlap only if they share INTERIOR points (b > c)

VISUAL TRACE with intervals = [[1,2],[1,2],[1,2]]:

PHASE 1 - Sort:
All have end=2: [[1,2], [1,2], [1,2]]

PHASE 2 - Initialize:
dp[0] = 1

PHASE 3 - Fill DP:

i=1: interval=[1,2], start=1
     Bs(1, 1): Find intervals where end <= 1
       - No intervals have end <= 1
       - Return idx=0
     dp[1] = dp[0] = 1

i=2: interval=[1,2], start=1
     Bs(2, 1): Find intervals where end <= 1
       - No intervals have end <= 1
       - Return idx=0
     dp[2] = dp[1] = 1

PHASE 4 - Result:
Total = 3, Max keep = 1
Remove = 3 - 1 = 2 ✓

VISUAL TRACE with intervals = [[1,2],[2,3]]:

PHASE 1 - Sort:
[[1,2], [2,3]]

PHASE 2 - Initialize:
dp[0] = 1

PHASE 3 - Fill DP:

i=1: interval=[2,3], start=2
     Bs(1, 2): Find intervals where end <= 2
       - intervals[0]=[1,2], end=2 <= 2? YES
       - Return idx=1
     dp[1] = max(dp[0], 1 + dp[0]) = max(1, 2) = 2

PHASE 4 - Result:
Total = 2, Max keep = 2
Remove = 2 - 2 = 0 ✓

OVERLAP vs NON-OVERLAP:
NON-OVERLAPPING: [1,2] and [2,3] (touching is OK)
OVERLAPPING: [1,3] and [2,4] (share interior points)

Rule: Intervals [a,b] and [c,d] (where b <= d) don't overlap if b <= c
*/