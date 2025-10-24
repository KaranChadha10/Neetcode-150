/*
PROBLEM: Insert Interval
You are given an array of non-overlapping intervals where intervals[i] = [start_i, end_i] 
represent the start and the end of the ith interval and intervals is sorted in ascending 
order by start_i. You are also given an interval newInterval = [start, end] that represents 
the start and end of another interval.

Insert newInterval into intervals such that intervals is still sorted in ascending order by 
start_i and intervals still does not have any overlapping intervals (merge overlapping 
intervals if necessary).

Return intervals after the insertion.

Note that you don't need to modify intervals in-place. You can make a new array and return it.

Example 1:
Input: intervals = [[1,3],[6,9]], newInterval = [2,5]
Output: [[1,5],[6,9]]

Example 2:
Input: intervals = [[1,2],[3,5],[6,7],[8,10],[12,16]], newInterval = [4,8]
Output: [[1,2],[3,10],[12,16]]
Explanation: Because the new interval [4,8] overlaps with [3,5],[6,7],[8,10].

Constraints:
- 0 <= intervals.length <= 10^4
- intervals[i].length == 2
- 0 <= start_i <= end_i <= 10^5
- intervals is sorted by start_i in ascending order.
- newInterval.length == 2
- 0 <= start <= end <= 10^5
*/

public class Solution
{
    public int[][] Insert(int[][] intervals, int[] newInterval)
    {
        // Handle empty intervals array
        if (intervals.Length == 0)
        {
            return new int[][] { newInterval };
        }

        int n = intervals.Length;
        int target = newInterval[0]; // Start of new interval
        int left = 0, right = n - 1;

        // PHASE 1: Binary search to find insertion position
        // Find leftmost position where newInterval should be inserted
        while (left <= right)
        {
            int mid = (left + right) / 2;
            
            // If mid interval starts before newInterval, search right
            if (intervals[mid][0] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1; // Search left to find leftmost valid position
            }
        }
        // After loop: left is the insertion position

        // PHASE 2: Build array with newInterval inserted
        List<int[]> result = new List<int[]>();
        
        // Add all intervals before insertion position
        for (int i = 0; i < left; i++)
        {
            result.Add(intervals[i]);
        }

        // Add the new interval at insertion position
        result.Add(newInterval);
        
        // Add all remaining intervals after insertion position
        for (int i = left; i < n; i++)
        {
            result.Add(intervals[i]);
        }

        // PHASE 3: Merge overlapping intervals
        List<int[]> merged = new List<int[]>();
        
        foreach (int[] interval in result)
        {
            // If merged is empty OR current interval doesn't overlap with last merged interval
            if (merged.Count == 0 || merged[merged.Count - 1][1] < interval[0])
            {
                merged.Add(interval); // Add as new separate interval
            }
            else
            {
                // Intervals overlap, merge by extending the end of last interval
                merged[merged.Count - 1][1] = Math.Max(merged[merged.Count - 1][1], interval[1]);
            }
        }

        return merged.ToArray(); // Convert list to array and return
    }
}

/*
TIME COMPLEXITY: O(n)
- Binary search: O(log n)
- Building result list: O(n) - copying all intervals
- Merging overlaps: O(n) - single pass through result
- Overall: O(log n) + O(n) + O(n) = O(n)

SPACE COMPLEXITY: O(n)
- Result list: O(n) to store all intervals plus newInterval
- Merged list: O(n) worst case (no merging happens)
- Overall: O(n) for output (required)

ALGORITHM EXPLANATION:
Three-phase approach:
1. Binary search to find correct insertion position
2. Insert newInterval at that position
3. Merge any overlapping intervals in single pass

KEY INSIGHTS:
- Original intervals are sorted and non-overlapping
- Binary search optimizes finding insertion point
- After insertion, only need one merge pass
- Overlaps can only occur around newInterval

BINARY SEARCH FOR INSERTION:
- Find leftmost position where interval starts >= newInterval start
- This is the correct sorted position for newInterval
- Uses standard binary search pattern for finding insertion point

MERGE LOGIC:
- Two intervals overlap if: interval1.end >= interval2.start
- Check: merged[last][1] < interval[0] means NO overlap
- If overlap: extend end to maximum of both intervals
- If no overlap: add as new separate interval

VISUAL TRACE with intervals = [[1,3],[6,9]], newInterval = [2,5]:

PHASE 1 - Binary Search:
Target = 2 (newInterval start)
Array: [[1,3], [6,9]]
        
Binary search finds: left = 1 (insert after [1,3])

PHASE 2 - Insert:
Before: [[1,3], [6,9]]
After:  [[1,3], [2,5], [6,9]]

PHASE 3 - Merge:
Process [1,3]: merged = [[1,3]]
Process [2,5]: 
  - Check: 3 >= 2? Yes → Overlap!
  - Merge: [1,3] extends to [1,5]
  - merged = [[1,5]]
Process [6,9]:
  - Check: 5 < 6? Yes → No overlap
  - Add: merged = [[1,5], [6,9]]

Result: [[1,5], [6,9]] ✓

VISUAL TRACE with intervals = [[1,2],[3,5],[6,7],[8,10],[12,16]], newInterval = [4,8]:

PHASE 1 - Binary Search:
Target = 4
Binary search finds: left = 2 (insert before [6,7])

PHASE 2 - Insert:
[[1,2], [3,5], [4,8], [6,7], [8,10], [12,16]]

PHASE 3 - Merge:
[1,2]: merged = [[1,2]]
[3,5]: No overlap, merged = [[1,2], [3,5]]
[4,8]: 
  - 5 >= 4? Yes → Overlap with [3,5]
  - Merge: [3,5] → [3,8]
  - merged = [[1,2], [3,8]]
[6,7]:
  - 8 >= 6? Yes → Overlap with [3,8]
  - Merge: [3,8] → [3,8] (8 > 7)
  - merged = [[1,2], [3,8]]
[8,10]:
  - 8 >= 8? Yes → Overlap with [3,8]
  - Merge: [3,8] → [3,10]
  - merged = [[1,2], [3,10]]
[12,16]:
  - 10 < 12? Yes → No overlap
  - merged = [[1,2], [3,10], [12,16]]

Result: [[1,2], [3,10], [12,16]] ✓

OVERLAP DETECTION:
Two intervals [a,b] and [c,d] overlap if:
- b >= c (first ends at or after second starts)

No overlap if:
- b < c (first ends before second starts)

MERGE OPERATION:
When merging [a,b] and [c,d]:
- New start: a (already first)
- New end: max(b, d) (take rightmost end)

BINARY SEARCH DETAIL:
```
Finding insertion position for newInterval start:
- If mid starts before target → look right (left = mid + 1)
- If mid starts at or after target → look left (right = mid - 1)
- Result: left points to first interval starting >= target
```

WHY THREE PHASES:
1. Binary search: O(log n) - efficient position finding
2. Insert: O(n) - must copy all intervals
3. Merge: O(n) - single pass to fix overlaps

Could combine 2 and 3, but this is clearer.

EDGE CASES HANDLED:
- Empty intervals:
  - Returns [newInterval] ✓

- newInterval before all:
  - Binary search: left = 0
  - Inserts at start
  - May merge with first interval ✓

- newInterval after all:
  - Binary search: left = n
  - Inserts at end
  - May merge with last interval ✓

- newInterval overlaps multiple:
  - Merge phase combines all overlapping
  - Works correctly ✓

- newInterval doesn't overlap any:
  - Simply inserted in correct position
  - No merging occurs ✓

- newInterval contained within existing:
  - Merges with containing interval
  - Removed during merge ✓

ALTERNATIVE APPROACH (Single Pass):
```csharp
// Add all intervals before newInterval
// Merge newInterval with overlapping ones
// Add all intervals after newInterval
```
✓ O(n) time, O(n) space
✗ More complex logic

This approach:
✓ O(n) time, O(n) space  
✓ Clearer three-phase structure
✓ Easier to understand and debug

MERGE PHASE INVARIANT:
- merged always contains non-overlapping intervals
- Last interval in merged is rightmost processed
- Each new interval either:
  - Extends last interval (overlap), or
  - Starts new interval (no overlap)

COMMON MISTAKES TO AVOID:
❌ Forgetting to handle empty intervals
❌ Not using binary search (O(n) insertion search)
❌ Complex merge logic with multiple passes
❌ Not handling newInterval that spans multiple intervals
❌ Incorrect overlap detection

PATTERN RECOGNITION:
- "Insert into sorted array" → binary search
- "Merge overlapping intervals" → single pass merge
- "Sorted intervals" → leverage ordering for efficiency

REAL-WORLD APPLICATIONS:
- Calendar event scheduling
- Resource allocation (time slots)
- Range merging in databases
- Meeting room booking systems
- Timeline management

OPTIMIZATION NOTES:
- Binary search optimizes from O(n) to O(log n) for finding position
- Could skip binary search and use linear scan (still O(n) overall)
- Current approach is more efficient for large arrays
- Three phases make code more maintainable
*/