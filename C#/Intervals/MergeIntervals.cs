/*
PROBLEM: Merge Intervals
Given an array of intervals where intervals[i] = [start_i, end_i], merge all overlapping 
intervals, and return an array of the non-overlapping intervals that cover all the intervals 
in the input.

Example 1:
Input: intervals = [[1,3],[2,6],[8,10],[15,18]]
Output: [[1,6],[8,10],[15,18]]
Explanation: Since intervals [1,3] and [2,6] overlap, merge them into [1,6].

Example 2:
Input: intervals = [[1,4],[4,5]]
Output: [[1,5]]
Explanation: Intervals [1,4] and [4,5] are considered overlapping.

Constraints:
- 1 <= intervals.length <= 10^4
- intervals[i].length == 2
- 0 <= start_i <= end_i <= 10^4
*/

public class Solution
{
    public int[][] Merge(int[][] intervals)
    {
        // PHASE 1: Find maximum start value to determine array size
        int max = 0;
        for (int i = 0; i < intervals.Length; i++)
        {
            max = Math.Max(intervals[i][0], max); // Track largest start value
        }

        // PHASE 2: Create mapping array where mp[i] = max_end+1 for intervals starting at i
        int[] mp = new int[max + 1]; // Array to map start positions to their max ends
        
        for (int i = 0; i < intervals.Length; i++)
        {
            int start = intervals[i][0]; // Start of current interval
            int end = intervals[i][1];   // End of current interval
            
            // Store end+1 for this start position (use max if multiple intervals start here)
            mp[start] = Math.Max(end + 1, mp[start]);
        }

        // PHASE 3: Scan through mapping to construct merged intervals
        var res = new List<int[]>(); // Result list of merged intervals
        int have = -1;          // Tracks furthest end point we can reach
        int intervalStart = -1; // Tracks start of current merging interval

        for (int i = 0; i < mp.Length; i++)
        {
            if (mp[i] != 0) // Found an interval starting at position i
            {
                // If no interval in progress, mark this as start
                if (intervalStart == -1) intervalStart = i;
                
                // Update furthest reachable point (end of interval is mp[i] - 1)
                have = Math.Max(mp[i] - 1, have);
            }
            
            // If we've reached the end of current merged interval
            if (have == i)
            {
                res.Add(new int[] { intervalStart, have }); // Add merged interval
                have = -1;          // Reset for next interval
                intervalStart = -1; // Reset start tracker
            }
        }

        // Handle case where last interval extends beyond mp array
        if (intervalStart != -1)
        {
            res.Add(new int[] { intervalStart, have }); // Add remaining interval
        }
        
        return res.ToArray(); // Convert list to array and return
    }
}

/*
TIME COMPLEXITY: O(n + m)
- First loop (find max): O(n) where n = number of intervals
- Second loop (populate mp): O(n)
- Third loop (construct result): O(m) where m = max start value
- Overall: O(n + m)
- For typical cases where m is bounded: O(n + m)

SPACE COMPLEXITY: O(m)
- mp array: O(m) where m = max start value
- Result list: O(n) worst case (no merging)
- Overall: O(m + n)
- Note: If max start value is very large, this can be inefficient

ALGORITHM EXPLANATION:
- Map-based approach instead of sorting
- Use array to track intervals by start position
- Scan linearly to merge overlapping intervals
- Works well when interval values are in reasonable range

KEY INSIGHTS:
- Stores end+1 for each start position (allows tracking reachability)
- "have" variable tracks how far we can reach from current merged interval
- When position equals "have", we've finished current merged interval
- Multiple intervals with same start handled by taking maximum end

MAPPING STRATEGY:
- mp[i] = end+1 of interval(s) starting at position i
- Why end+1? Makes it easier to check if we've reached interval end
- mp[i] = 0 means no interval starts at position i
- If multiple intervals start at same position, store max(end+1)

MERGING LOGIC:
- Start new merge when we see non-zero mp[i] and intervalStart == -1
- Extend current merge by updating "have" to maximum end we can reach
- Complete merge when current position equals "have"
- This naturally handles overlapping and touching intervals

VISUAL TRACE with [[1,3],[2,6],[8,10],[15,18]]:

PHASE 1 - Find max start:
Intervals: [1,3], [2,6], [8,10], [15,18]
Starts: 1, 2, 8, 15
max = 15

PHASE 2 - Build mapping:
mp[1] = 3 + 1 = 4
mp[2] = 6 + 1 = 7
mp[8] = 10 + 1 = 11
mp[15] = 18 + 1 = 19

mp array: [0,4,7,0,0,0,0,0,11,0,0,0,0,0,0,19]
          0 1 2 3 4 5 6 7 8  9 10 11 12 13 14 15

PHASE 3 - Scan and merge:
i=0: mp[0]=0, skip
i=1: mp[1]=4, intervalStart=1, have=3
i=2: mp[2]=7, have=max(7-1,3)=6
i=3: have=6, continue
...
i=6: have=6, i=6 → Complete! Add [1,6]
i=7: have=-1, reset
i=8: mp[8]=11, intervalStart=8, have=10
i=9: have=10, continue
i=10: have=10, i=10 → Complete! Add [8,10]
...
i=15: mp[15]=19, intervalStart=15, have=18

After loop: intervalStart=15, have=18 → Add [15,18]

Result: [[1,6],[8,10],[15,18]] ✓

WHY end+1 INSTEAD OF end?
Using end+1 simplifies the logic:
- Check "have == i" instead of "have == i-1"
- Makes boundary conditions cleaner
- Avoids off-by-one errors

HANDLING SAME START POSITIONS:
If [[1,3],[1,5]] both start at 1:
mp[1] = max(4, 6) = 6
Takes maximum end, naturally merging them

REACHABILITY CONCEPT:
"have" tracks maximum position reachable from current interval group
- If new interval starts at or before "have", it overlaps
- Update "have" to extend merged interval
- When position > "have", start new interval

ALTERNATIVE APPROACH (Sorting):
```csharp
// Sort by start time: O(n log n)
Array.Sort(intervals, (a,b) => a[0].CompareTo(b[0]));
// Then merge: O(n)
```
✓ O(n log n) time
✓ O(1) extra space (not counting output)
✓ Works for any value range

This approach:
✓ O(n + m) time where m = max start
✗ O(m) space - inefficient if m is large
✓ No sorting needed

WHEN TO USE THIS APPROACH:
✓ When interval values are in small range (m << n log n)
✓ When sorting is expensive
✗ When max value is very large (wastes space)
✗ When values are sparse

COMPARISON:
| Approach | Time | Space | Best For |
|----------|------|-------|----------|
| Sorting | O(n log n) | O(1) | General case |
| This (mapping) | O(n + m) | O(m) | Small value range |

EDGE CASES HANDLED:
- Single interval: Works correctly ✓
- All overlapping: Merges into one ✓
- No overlapping: Returns all separately ✓
- Touching intervals [1,2],[2,3]: Merges correctly ✓
- Same start position: Takes max end ✓
- Last interval extends beyond: Handled with final check ✓

OPTIMIZATION NOTES:
- Could use HashMap instead of array for sparse values
- Would trade O(m) space for O(n) space
- But adds hashing overhead

COMMON MISTAKES TO AVOID:
❌ Not handling intervals beyond mp array
❌ Using end instead of end+1 (complicates logic)
❌ Not taking max when multiple intervals at same start
❌ Not resetting have and intervalStart after adding interval

PATTERN RECOGNITION:
- "Merge intervals" → sorting or range mapping
- "Small value range" → array-based approach
- "Overlapping intervals" → track reachable endpoints

REAL-WORLD APPLICATIONS:
- Calendar event merging
- Time slot consolidation
- Resource allocation optimization
- Range query processing



This is an **interesting alternative approach** to the classic Merge Intervals problem! Here's what makes it unique:

## 🔑 Key Insight - Map-Based Instead of Sorting:

Instead of sorting intervals, **map start positions to their ends**:
```
Traditional: Sort by start (O(n log n)) → Merge (O(n))
This: Map intervals (O(n)) → Scan map (O(m)) where m = max start

*/