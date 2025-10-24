/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 252. Meeting Rooms (Can Attend Meetings)
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
Given an array of meeting time intervals where intervals[i] = [start_i, end_i], 
determine if a person could attend all meetings.

In other words, check if there are any overlapping intervals. If any two meetings 
overlap, the person cannot attend both, so return false. If all meetings are 
non-overlapping, return true.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: intervals = [[0,30],[5,10],[15,20]]
Output: false
Explanation: [0,30] overlaps with both [5,10] and [15,20]

Example 2:
Input: intervals = [[7,10],[2,4]]
Output: true
Explanation: No meetings overlap

Example 3:
Input: intervals = [[1,5],[8,9]]
Output: true
Explanation: Meetings don't overlap (5 <= 8)

Example 4:
Input: intervals = [[1,5],[5,8]]
Output: true
Explanation: Meeting ends exactly when next starts (no overlap)

Example 5:
Input: intervals = []
Output: true
Explanation: No meetings to conflict

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 0 <= intervals.length <= 10^4
- intervals[i].length == 2
- 0 <= start_i < end_i <= 10^6

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Sorting Strategy**: Sort meetings by start time to enable efficient linear scan
   - After sorting, only need to check consecutive pairs for overlap
   - No need to compare every meeting with every other meeting

2. **Overlap Detection**: Two consecutive meetings (after sorting) overlap if:
   - previous.end > current.start
   - If previous ends BEFORE or WHEN current starts (<=), no overlap

3. **Early Exit**: Can return false immediately upon finding first overlap
   - No need to check remaining meetings once conflict found

4. **Edge Cases**: 
   - Empty list → true (no meetings to conflict)
   - Single meeting → true (can't overlap with itself)
   - Touching meetings [1,5] and [5,8] → NOT overlapping

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(n log n)
- Sorting intervals: O(n log n)
- Linear scan through intervals: O(n)
- Overall: O(n log n) dominated by sorting

Space Complexity: O(1) or O(log n)
- No extra data structures used
- Sorting space: O(log n) to O(n) depending on sort implementation
- In-place sorting: O(log n) for recursion stack
- Overall: O(1) auxiliary space (excluding sorting)

═══════════════════════════════════════════════════════════════════════════════
*/

/**
 * Definition of Interval:
 * public class Interval {
 *     public int start, end;
 *     public Interval(int start, int end) {
 *         this.start = start;
 *         this.end = end;
 *     }
 * }
 */

public class Solution
{
    public bool CanAttendMeetings(List<Interval> intervals)
    {
        // ════════════════════════════════════════════════════════════════════
        // EDGE CASE: Handle empty or single meeting
        // ════════════════════════════════════════════════════════════════════
        // Empty list: No meetings → no conflicts → return true
        // Single meeting: Can't conflict with itself → return true
        // These cases are handled naturally by the loop (won't execute)
        
        // ════════════════════════════════════════════════════════════════════
        // STEP 1: Sort intervals by start time
        // ════════════════════════════════════════════════════════════════════
        // Why sort by start time?
        // - Enables checking only consecutive pairs for overlaps
        // - After sorting, if meeting A starts before B, we only need to check
        //   if A ends after B starts to detect overlap
        // - Reduces problem from O(n²) comparisons to O(n)
        intervals.Sort((i1, i2) => i1.start.CompareTo(i2.start));

        // ════════════════════════════════════════════════════════════════════
        // STEP 2: Check consecutive meetings for overlaps
        // ════════════════════════════════════════════════════════════════════
        // After sorting by start time, if meetings don't overlap consecutively,
        // they won't overlap at all
        
        for (int i = 1; i < intervals.Count; i++)
        {
            Interval i1 = intervals[i - 1];  // Previous meeting
            Interval i2 = intervals[i];       // Current meeting

            // ────────────────────────────────────────────────────────────────
            // Overlap Detection Logic
            // ────────────────────────────────────────────────────────────────
            // Two meetings overlap if previous ends AFTER current starts
            // 
            // Timeline visualization:
            //   Previous: |-------|
            //   Current:      |-------|  (i1.end > i2.start → OVERLAP)
            //
            //   Previous: |-------|
            //   Current:           |-------|  (i1.end <= i2.start → NO overlap)
            //
            // Note: If previous ends exactly when current starts (i1.end == i2.start),
            // this is NOT considered an overlap (can attend both back-to-back)
            
            if (i1.end > i2.start)
            {
                // Found an overlap - person cannot attend both meetings
                return false;  // Early exit optimization
            }
        }
        
        // ════════════════════════════════════════════════════════════════════
        // No overlaps found - person can attend all meetings
        // ════════════════════════════════════════════════════════════════════
        return true;
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 1: [[0,30],[5,10],[15,20]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Sort by Start Time
───────────────────────────────────────────────────────
Before: [[0,30], [5,10], [15,20]]
After:  [[0,30], [5,10], [15,20]]  (already sorted)

Visual Timeline:
0    5    10   15   20   25   30
|----------------[0,30]------------|
     |--[5,10]--|
               |--[15,20]--|

STEP 2: Check Consecutive Pairs
───────────────────────────────────────────────────────
i=1: Check [0,30] and [5,10]
     Previous: [0,30], end=30
     Current:  [5,10], start=5
     
     Check: 30 > 5? YES → OVERLAP!
     
     Visual:
     [0,30]:   |----------------------------|
     [5,10]:        |------|
                    ↑ Previous ends after current starts
     
     Return: false ✓

RESULT: Cannot attend all meetings

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 2: [[7,10],[2,4]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Sort by Start Time
───────────────────────────────────────────────────────
Before: [[7,10], [2,4]]
After:  [[2,4], [7,10]]  (sorted)

Visual Timeline:
0    2    4    6    7    10
     |--[2,4]--|
                    |--[7,10]--|

STEP 2: Check Consecutive Pairs
───────────────────────────────────────────────────────
i=1: Check [2,4] and [7,10]
     Previous: [2,4], end=4
     Current:  [7,10], start=7
     
     Check: 4 > 7? NO → No overlap ✓
     
     Visual:
     [2,4]:    |------|
     [7,10]:                |------|
                            ↑ Previous ends before current starts

Loop ends, no overlaps found.

RESULT: true ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 3: [[1,5],[5,8]] (Touching Intervals)
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Sort by Start Time
───────────────────────────────────────────────────────
Already sorted: [[1,5], [5,8]]

Visual Timeline:
0    1    2    3    4    5    6    7    8
     |------[1,5]------|
                       |------[5,8]------|
                       ↑ Exactly touching at time 5

STEP 2: Check Consecutive Pairs
───────────────────────────────────────────────────────
i=1: Check [1,5] and [5,8]
     Previous: [1,5], end=5
     Current:  [5,8], start=5
     
     Check: 5 > 5? NO → No overlap ✓
     
     Note: Meetings that touch at a single point are NOT overlapping
     Person can attend meeting ending at 5 and another starting at 5
     (back-to-back meetings)

RESULT: true ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 4: [[1,3],[2,6],[4,5]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Sort by Start Time
───────────────────────────────────────────────────────
Before: [[1,3], [2,6], [4,5]]
After:  [[1,3], [2,6], [4,5]]  (already sorted)

Visual Timeline:
0    1    2    3    4    5    6
     |--[1,3]--|
          |-------[2,6]-------|
                  |--[4,5]--|

STEP 2: Check Consecutive Pairs
───────────────────────────────────────────────────────
i=1: Check [1,3] and [2,6]
     Previous: [1,3], end=3
     Current:  [2,6], start=2
     
     Check: 3 > 2? YES → OVERLAP!
     
     Visual:
     [1,3]:    |------|
     [2,6]:         |----------|
                    ↑ Overlap detected
     
     Return: false ✓

RESULT: Cannot attend all meetings

═══════════════════════════════════════════════════════════════════════════════
🔍 OVERLAP DETECTION EXPLAINED:
═══════════════════════════════════════════════════════════════════════════════

Two intervals [a,b] and [c,d] (where a <= c due to sorting):

OVERLAP (return false):
  b > c  (first meeting ends AFTER second starts)
  
  Timeline: |---[a,b]---|
                |---[c,d]---|
                ↑ Overlap region

NO OVERLAP (continue checking):
  b <= c  (first meeting ends BEFORE or WHEN second starts)
  
  Timeline: |---[a,b]---|
                         |---[c,d]---|
            OR
            |---[a,b]---|
                        |---[c,d]---|
                        ↑ Back-to-back (OK)

═══════════════════════════════════════════════════════════════════════════════
📊 WHY SORTING BY START TIME WORKS:
═══════════════════════════════════════════════════════════════════════════════

Without Sorting:
- Must compare every pair: O(n²)
- Example: [[10,20], [5,8], [1,3]]
  Need to check: (10,5), (10,1), (5,1) - all pairs

With Sorting:
- Only check consecutive pairs: O(n)
- Example sorted: [[1,3], [5,8], [10,20]]
  Only check: (1,5), (5,10) - adjacent pairs
- If [1,3] doesn't overlap with [5,8], and [5,8] doesn't overlap with [10,20],
  then [1,3] definitely doesn't overlap with [10,20]!

Intuition:
After sorting by start time:
- If meeting A starts before B, and A doesn't overlap B,
- Then A won't overlap with any meeting starting after B
- This is because A ends before B starts, so A must end before later meetings start

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Empty list: []
   - Loop doesn't execute
   - Return: true ✓

✅ Single meeting: [[1,5]]
   - Loop runs 0 times (i starts at 1, count is 1)
   - Return: true ✓

✅ Two non-overlapping: [[1,3],[4,6]]
   - Check: 3 > 4? NO
   - Return: true ✓

✅ Two overlapping: [[1,5],[3,7]]
   - Check: 5 > 3? YES
   - Return: false ✓

✅ Touching meetings: [[1,5],[5,8]]
   - Check: 5 > 5? NO
   - Return: true ✓

✅ One meeting inside another: [[1,10],[2,3]]
   - Check: 10 > 2? YES
   - Return: false ✓

✅ Multiple non-overlapping: [[1,2],[3,4],[5,6]]
   - Check: 2 > 3? NO
   - Check: 4 > 5? NO
   - Return: true ✓

✅ All overlapping: [[1,10],[2,5],[3,7]]
   - First check: 10 > 2? YES
   - Return: false ✓

✅ Identical meetings: [[5,10],[5,10]]
   - Check: 10 > 5? YES
   - Return: false ✓

✅ Already sorted: [[1,3],[5,7],[9,11]]
   - No change after sort
   - All checks pass
   - Return: true ✓

✅ Reverse sorted: [[9,11],[5,7],[1,3]]
   - After sort: [[1,3],[5,7],[9,11]]
   - All checks pass
   - Return: true ✓

═══════════════════════════════════════════════════════════════════════════════
⚡ OPTIMIZATION NOTES:
═══════════════════════════════════════════════════════════════════════════════

Current Approach:
✓ O(n log n) time - dominated by sorting
✓ O(1) space - in-place checking
✓ Early exit on first overlap
✓ Simple and clean logic

Alternative Approaches:

1. Brute Force (Not Recommended):
   - Check all pairs: O(n²)
   - No sorting needed
   - Inefficient for large inputs

2. Sweep Line Algorithm:
   - Same O(n log n) complexity
   - More complex implementation
   - No advantage for this problem

3. Interval Tree:
   - O(n log n) to build, O(log n) per query
   - Overkill for single pass problem
   - Useful for dynamic interval queries

Best Approach: Current (sort + linear scan)
- Optimal time complexity
- Minimal space usage
- Easy to understand and implement
- Industry standard solution

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Using >= instead of > for overlap check
   - [[1,5],[5,8]] would incorrectly return false
   - Touching meetings are NOT overlapping

❌ Comparing all pairs without sorting
   - O(n²) time complexity - inefficient
   - Sorting enables O(n) checking

❌ Sorting by end time instead of start time
   - Makes logic more complex
   - Doesn't provide same optimization benefits

❌ Not handling empty list
   - Should return true (no conflicts)
   - Current code handles naturally

❌ Modifying intervals in-place during comparison
   - Unnecessary and error-prone
   - Just read values, don't modify

❌ Using wrong comparison operator in sort
   - Must use CompareTo or subtract for correct ordering
   - i1.start - i2.start works but CompareTo is safer

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates classic patterns:

1. **Sort + Scan Pattern**:
   - Sort data to enable efficient linear scan
   - Common in interval problems

2. **Greedy Approach**:
   - Check consecutive pairs only
   - Early exit on first violation

3. **Interval Overlap**:
   - Two intervals [a,b] and [c,d] overlap if max(a,c) < min(b,d)
   - After sorting by start: b > c is sufficient check

4. **Problem Variants**:
   - Meeting Rooms II: Minimum rooms needed (use priority queue)
   - Merge Intervals: Combine overlapping intervals
   - Insert Interval: Add new interval and merge
   - Non-overlapping Intervals: Minimum removals needed

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Calendar Systems**:
   - Google Calendar: Check if event can be scheduled
   - Prevent double-booking

2. **Resource Scheduling**:
   - Conference room booking
   - Equipment rental validation
   - Vehicle reservation systems

3. **Project Management**:
   - Task scheduling validation
   - Resource allocation conflicts
   - Timeline feasibility checks

4. **Healthcare**:
   - Doctor appointment scheduling
   - Operating room allocation
   - Equipment usage planning

5. **Transportation**:
   - Flight scheduling
   - Train timetable validation
   - Delivery route planning

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS:
═══════════════════════════════════════════════════════════════════════════════

1. Meeting Rooms II (LeetCode 253):
   - Find minimum number of meeting rooms required
   - Uses priority queue/heap
   - O(n log n) time complexity

2. Merge Intervals (LeetCode 56):
   - Combine all overlapping intervals
   - Similar sorting approach
   - Returns merged intervals

3. Insert Interval (LeetCode 57):
   - Insert new interval and merge if needed
   - Maintains sorted order
   - Uses binary search optimization

4. Non-overlapping Intervals (LeetCode 435):
   - Minimum intervals to remove for non-overlapping
   - Uses greedy or DP approach
   - Interval scheduling maximization

5. Interval List Intersections (LeetCode 986):
   - Find intersection of two interval lists
   - Two-pointer approach
   - O(n + m) time complexity

═══════════════════════════════════════════════════════════════════════════════
*/