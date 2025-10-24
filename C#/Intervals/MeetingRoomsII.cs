/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 253. Meeting Rooms II (Minimum Meeting Rooms)
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
Given an array of meeting time intervals where intervals[i] = [start_i, end_i], 
return the MINIMUM number of conference rooms required to schedule all meetings.

Unlike Meeting Rooms I which asks if a person can attend all meetings, this problem
asks how many rooms are needed if we want to accommodate ALL meetings simultaneously.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: intervals = [[0,30],[5,10],[15,20]]
Output: 2
Explanation:
- At time 0: Meeting 1 starts (room 1)
- At time 5: Meeting 2 starts (room 2) - Meeting 1 still ongoing
- At time 10: Meeting 2 ends (room 2 freed)
- At time 15: Meeting 3 starts (room 2) - Reuse room 2
- At time 20: Meeting 3 ends
- At time 30: Meeting 1 ends
Maximum rooms needed at any point: 2

Example 2:
Input: intervals = [[7,10],[2,4]]
Output: 1
Explanation: These meetings don't overlap, so only 1 room needed

Example 3:
Input: intervals = [[1,5],[2,3],[4,6]]
Output: 2
Explanation:
- At time 1: Meeting 1 starts (room 1)
- At time 2: Meeting 2 starts (room 2) - Meeting 1 ongoing
- At time 3: Meeting 2 ends (room 2 freed)
- At time 4: Meeting 3 starts (room 2) - Meeting 1 still ongoing
- At time 5: Meeting 1 ends
- At time 6: Meeting 3 ends
Maximum rooms needed: 2 (at time 2-3 and 4-5)

Example 4:
Input: intervals = [[1,10],[2,5],[3,7]]
Output: 3
Explanation: All three meetings overlap at some point (time 3-5)

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 1 <= intervals.length <= 10^4
- intervals[i].length == 2
- 0 <= start_i < end_i <= 10^6

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Event-Based Thinking**: Think of meetings as events
   - Meeting start = +1 room needed
   - Meeting end = -1 room needed (room freed)
   - Track rooms in use at each event

2. **Separate Start and End Times**: Don't need to track which meeting is which
   - Only care about WHEN meetings start and end
   - Sorting both arrays independently works!

3. **Two-Pointer Simulation**: Process events chronologically
   - If next event is a START: increment room count
   - If next event is an END: decrement room count
   - Maximum count seen = minimum rooms needed

4. **Why This Works**: 
   - We're simulating a timeline where we allocate/free rooms
   - Peak concurrent meetings = minimum rooms required
   - Meetings ending at time T free rooms for meetings starting at time T

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(n log n)
- Extracting start/end times: O(n)
- Sorting start array: O(n log n)
- Sorting end array: O(n log n)
- Two-pointer scan: O(n)
- Overall: O(n log n) dominated by sorting

Space Complexity: O(n)
- Start array: O(n)
- End array: O(n)
- Overall: O(n) for storing times

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
    public int MinMeetingRooms(List<Interval> intervals) 
    {
        // ════════════════════════════════════════════════════════════════════
        // STEP 1: Extract start and end times into separate arrays
        // ════════════════════════════════════════════════════════════════════
        // Key insight: We don't need to track which meeting each time belongs to
        // We only need to know WHEN meetings start and WHEN they end
        // This decoupling allows us to sort independently
        
        int n = intervals.Count;
        int[] start = new int[n];  // All start times
        int[] end = new int[n];    // All end times

        for(int i = 0; i < n; i++)
        {
            start[i] = intervals[i].start;
            end[i] = intervals[i].end;
        }

        // ════════════════════════════════════════════════════════════════════
        // STEP 2: Sort both arrays independently
        // ════════════════════════════════════════════════════════════════════
        // Why sort separately?
        // - We want to process events chronologically
        // - Start events and end events can be processed in any pairing
        // - What matters is the ORDER of events, not which meeting they belong to
        //
        // Example: [[0,30],[5,10]]
        // start = [0,5] (sorted)
        // end = [10,30] (sorted)
        // We process: start(0), start(5), end(10), end(30)
        
        Array.Sort(start);
        Array.Sort(end);

        // ════════════════════════════════════════════════════════════════════
        // STEP 3: Two-pointer simulation to find peak concurrent meetings
        // ════════════════════════════════════════════════════════════════════
        // Variables:
        // - res: Maximum rooms needed (our answer)
        // - count: Current number of active meetings (rooms in use)
        // - s: Pointer for start times
        // - e: Pointer for end times
        
        int res = 0;     // Maximum rooms needed so far
        int count = 0;   // Current active meetings
        int s = 0;       // Start time pointer
        int e = 0;       // End time pointer

        // ────────────────────────────────────────────────────────────────────
        // Process all start events (meetings beginning)
        // ────────────────────────────────────────────────────────────────────
        // We process start events one by one
        // At each start event, we check if any meetings have ended
        
        while(s < n)
        {
            // ────────────────────────────────────────────────────────────
            // Compare next start time with next end time
            // ────────────────────────────────────────────────────────────
            
            if(start[s] < end[e])
            {
                // ════════════════════════════════════════════════════════
                // CASE 1: Next event is a MEETING START
                // ════════════════════════════════════════════════════════
                // A meeting is starting before the next meeting ends
                // This means we need an additional room
                //
                // Example timeline:
                // Current time: start[s]
                // Next meeting ends at: end[e]
                // Since start[s] < end[e], a meeting is starting NOW
                
                s++;        // Move to next start event
                count++;    // Increment active meeting count (allocate room)
            } 
            else 
            {
                // ════════════════════════════════════════════════════════
                // CASE 2: Next event is a MEETING END
                // ════════════════════════════════════════════════════════
                // A meeting is ending at or before the next meeting starts
                // This means a room is being freed
                //
                // Example timeline:
                // Next meeting starts at: start[s]
                // Meeting ending at: end[e]
                // Since start[s] >= end[e], a meeting is ending NOW
                //
                // Note: If start[s] == end[e], we process the END first
                // This allows the freed room to be reused immediately
                
                e++;        // Move to next end event
                count--;    // Decrement active meeting count (free room)
            }
            
            // ────────────────────────────────────────────────────────────────
            // Track maximum concurrent meetings
            // ────────────────────────────────────────────────────────────────
            // After each event (start or end), update the maximum
            // The peak count is the minimum number of rooms we need
            res = Math.Max(res, count);
        }
        
        // ════════════════════════════════════════════════════════════════════
        // Return the maximum concurrent meetings seen
        // ════════════════════════════════════════════════════════════════════
        return res;
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 1: [[0,30],[5,10],[15,20]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Extract Times
───────────────────────────────────────────────────────
start = [0, 5, 15]
end   = [30, 10, 20]

STEP 2: Sort Both Arrays
───────────────────────────────────────────────────────
start = [0, 5, 15]  (already sorted)
end   = [10, 20, 30]  (sorted)

Visual Timeline:
0    5    10   15   20        30
|----------------[0,30]--------|
     |--[5,10]--|
               |--[15,20]--|

STEP 3: Two-Pointer Simulation
───────────────────────────────────────────────────────
Initial: res=0, count=0, s=0, e=0

Event 1: Compare start[0]=0 vs end[0]=10
  0 < 10? YES → Meeting starts
  s=1, count=1
  res = max(0, 1) = 1
  Status: 1 meeting active [0,30]

Event 2: Compare start[1]=5 vs end[0]=10
  5 < 10? YES → Meeting starts
  s=2, count=2
  res = max(1, 2) = 2 ★ Peak!
  Status: 2 meetings active [0,30], [5,10]

Event 3: Compare start[2]=15 vs end[0]=10
  15 < 10? NO → Meeting ends
  e=1, count=1
  res = max(2, 1) = 2
  Status: 1 meeting active [0,30], [5,10] ended

Event 4: Compare start[2]=15 vs end[1]=20
  15 < 20? YES → Meeting starts
  s=3, count=2
  res = max(2, 2) = 2
  Status: 2 meetings active [0,30], [15,20]

Loop ends (s=3, n=3)

RESULT: 2 rooms needed ✓

Timeline with room assignments:
Time:    0    5    10   15   20        30
Room 1:  |----------------[0,30]--------|
Room 2:       |--[5,10]--|  |--[15,20]--|

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 2: [[7,10],[2,4]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Extract Times
───────────────────────────────────────────────────────
start = [7, 2]
end   = [10, 4]

STEP 2: Sort Both Arrays
───────────────────────────────────────────────────────
start = [2, 7]
end   = [4, 10]

Visual Timeline:
0    2    4    6    7    10
     |--[2,4]--|
                    |--[7,10]--|

STEP 3: Two-Pointer Simulation
───────────────────────────────────────────────────────
Initial: res=0, count=0, s=0, e=0

Event 1: Compare start[0]=2 vs end[0]=4
  2 < 4? YES → Meeting starts
  s=1, count=1
  res = max(0, 1) = 1 ★
  Status: 1 meeting active [2,4]

Event 2: Compare start[1]=7 vs end[0]=4
  7 < 4? NO → Meeting ends
  e=1, count=0
  res = max(1, 0) = 1
  Status: No meetings active, [2,4] ended

Event 3: Compare start[1]=7 vs end[1]=10
  7 < 10? YES → Meeting starts
  s=2, count=1
  res = max(1, 1) = 1
  Status: 1 meeting active [7,10]

Loop ends (s=2, n=2)

RESULT: 1 room needed ✓

Timeline:
Time:    0    2    4    6    7    10
Room 1:       |--[2,4]--|    |--[7,10]--|

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example 3: [[1,10],[2,5],[3,7]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1: Extract Times
───────────────────────────────────────────────────────
start = [1, 2, 3]
end   = [10, 5, 7]

STEP 2: Sort Both Arrays
───────────────────────────────────────────────────────
start = [1, 2, 3]  (already sorted)
end   = [5, 7, 10]  (sorted)

Visual Timeline:
0  1  2  3  4  5  6  7  8  9  10
   |--------[1,10]---------|
      |--[2,5]--|
         |-[3,7]--|

STEP 3: Two-Pointer Simulation
───────────────────────────────────────────────────────
Initial: res=0, count=0, s=0, e=0

Event 1: start[0]=1 vs end[0]=5
  1 < 5? YES → count=1
  res = 1

Event 2: start[1]=2 vs end[0]=5
  2 < 5? YES → count=2
  res = 2

Event 3: start[2]=3 vs end[0]=5
  3 < 5? YES → count=3
  res = 3 ★ Peak!
  Status: All 3 meetings active at time 3-5

Event 4: start[3]=? (none) vs end[0]=5
  s >= n, loop ends

RESULT: 3 rooms needed ✓

Timeline with room assignments:
Time:    1  2  3  4  5  6  7  8  9  10
Room 1:  |--------[1,10]---------|
Room 2:     |--[2,5]--|
Room 3:        |-[3,7]--|
             ↑ All 3 overlap here

═══════════════════════════════════════════════════════════════════════════════
🎯 SPECIAL CASE: Touching Meetings [[1,5],[5,8]]
═══════════════════════════════════════════════════════════════════════════════

STEP 1 & 2: Sort
───────────────────────────────────────────────────────
start = [1, 5]
end   = [5, 8]

STEP 3: Simulation
───────────────────────────────────────────────────────
Event 1: start[0]=1 vs end[0]=5
  1 < 5? YES → count=1
  res = 1

Event 2: start[1]=5 vs end[0]=5
  5 < 5? NO → Meeting ends first!
  e=1, count=0
  res = 1

Event 3: start[1]=5 vs end[1]=8
  5 < 8? YES → count=1
  res = 1

RESULT: 1 room needed ✓

Key Point: When start time equals end time (touching meetings),
we process the END first (count--), freeing the room immediately.
This allows the new meeting to reuse the same room!

Timeline:
Time:    1    2    3    4    5    6    7    8
Room 1:  |------[1,5]------|------[5,8]------|
                           ↑ Room freed and reused

═══════════════════════════════════════════════════════════════════════════════
💡 WHY SEPARATE SORTING WORKS:
═══════════════════════════════════════════════════════════════════════════════

Intuition:
- We're simulating a timeline of events
- Each meeting start is an event: "allocate a room"
- Each meeting end is an event: "free a room"
- We don't care WHICH specific meeting starts/ends
- We only care WHEN events happen

Example: [[1,3],[2,4]]
Original: Meeting A = [1,3], Meeting B = [2,4]

After separating:
start = [1, 2]
end   = [3, 4]

We process events:
- Time 1: Some meeting starts (allocate room) → count=1
- Time 2: Some meeting starts (allocate room) → count=2
- Time 3: Some meeting ends (free room) → count=1
- Time 4: Some meeting ends (free room) → count=0

The "some meeting" could be A or B - doesn't matter!
What matters is: at time 2-3, we have 2 concurrent meetings.

Mathematical Proof:
- At any time T, the number of active meetings is:
  (# of meetings started before T) - (# of meetings ended before T)
- Sorting start and end arrays independently preserves these counts
- Maximum difference = minimum rooms needed

═══════════════════════════════════════════════════════════════════════════════
🔄 COMPARISON WITH ALTERNATIVE APPROACHES:
═══════════════════════════════════════════════════════════════════════════════

APPROACH 1: Min Heap (Priority Queue)
───────────────────────────────────────────────────────
```csharp
// Sort by start time
// Use min heap to track end times of ongoing meetings
// For each meeting:
//   - Remove all meetings that ended before current starts
//   - Add current meeting's end time to heap
//   - Max heap size = rooms needed
```
✓ Time: O(n log n)
✓ Space: O(n)
✓ More intuitive for some people
✗ Slightly more complex implementation
✗ Heap operations have higher constant factors

APPROACH 2: Event List (Our Approach - Optimized)
───────────────────────────────────────────────────────
```csharp
// Separate start and end times
// Sort both arrays
// Two-pointer scan
```
✓ Time: O(n log n)
✓ Space: O(n)
✓ Simpler implementation
✓ Better cache locality (arrays vs heap)
✓ Fewer operations overall

APPROACH 3: Chronological Events (Alternative)
───────────────────────────────────────────────────────
```csharp
// Create list of (time, type) events
// Type: +1 for start, -1 for end
// Sort by time (end events before start if same time)
// Scan and track running count
```
✓ Time: O(n log n)
✓ Space: O(n)
✓ Very intuitive
✗ More memory for event objects
✗ Requires custom sort comparator

Best Choice: Approach 2 (Current Solution)
- Optimal complexity
- Clean implementation
- Efficient in practice

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Single meeting: [[1,5]]
   start=[1], end=[5]
   Event: 1 < 5 → count=1
   Result: 1 ✓

✅ All non-overlapping: [[1,2],[3,4],[5,6]]
   start=[1,3,5], end=[2,4,6]
   Max count at any time: 1
   Result: 1 ✓

✅ All overlapping: [[1,10],[2,9],[3,8]]
   start=[1,2,3], end=[8,9,10]
   All start before any end
   Max count: 3
   Result: 3 ✓

✅ Touching meetings: [[1,5],[5,10],[10,15]]
   start=[1,5,10], end=[5,10,15]
   When comparing 5 vs 5: end processed first
   Each time count goes to 1, then 0, then 1
   Result: 1 ✓

✅ Nested meetings: [[1,20],[2,5],[3,4]]
   start=[1,2,3], end=[4,5,20]
   Max count: 3 (all overlap at time 3-4)
   Result: 3 ✓

✅ Identical meetings: [[5,10],[5,10]]
   start=[5,5], end=[10,10]
   Both start at 5: count=2
   Result: 2 ✓

✅ Large interval with many small: [[1,100],[10,11],[20,21],[30,31]]
   Max concurrent: 2 (large + any small)
   Result: 2 ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Sorting intervals by start time and trying to track end times
   - More complex than separating times
   - Requires priority queue or complex bookkeeping

❌ Using start[s] <= end[e] instead of start[s] < end[e]
   - Incorrect handling of touching meetings
   - Would allocate extra room for back-to-back meetings

❌ Not processing all start events (while s < n)
   - Missing meetings in the count
   - Incorrect room calculation

❌ Updating res inside only one branch (if or else)
   - Should update after both cases
   - Ensures we track peak correctly

❌ Thinking we need to track which meeting is in which room
   - Overcomplicating the problem
   - Only need to track COUNT of concurrent meetings

❌ Using O(n²) brute force approach
   - Checking each meeting against all others
   - Inefficient and unnecessary

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Event-Based Processing**:
   - Convert intervals to events (start/end)
   - Process chronologically
   - Track state changes

2. **Two-Pointer Technique**:
   - Two sorted arrays
   - Compare elements from both
   - Advance pointers based on comparison

3. **Sweep Line Algorithm**:
   - Imagine a line sweeping across time
   - Track what's active at each point
   - Maximum active = answer

4. **Decoupling Data**:
   - Separate related data (start/end times)
   - Sort independently
   - Process together

5. **Peak Finding**:
   - Track running count
   - Record maximum
   - Maximum indicates bottleneck

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Conference Room Management**:
   - Building scheduling systems
   - Resource allocation
   - Capacity planning

2. **Server Resource Management**:
   - Connection pooling (database, HTTP)
   - Thread pool sizing
   - Worker capacity planning

3. **Airport Gate Assignment**:
   - Minimum gates needed for flight schedule
   - Resource optimization
   - Cost reduction

4. **Operating Room Scheduling**:
   - Hospital surgery scheduling
   - Equipment allocation
   - Staff planning

5. **CPU Scheduling**:
   - Process scheduling
   - Core allocation
   - Resource management

6. **Parking Lot Management**:
   - Minimum spots needed
   - Peak usage prediction
   - Capacity planning

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. Meeting Rooms (LeetCode 252):
   - Check if person can attend all meetings
   - Simpler: just check for any overlap
   - O(n log n) time, O(1) space

2. Merge Intervals (LeetCode 56):
   - Combine overlapping intervals
   - Sort by start, merge consecutively
   - O(n log n) time

3. Insert Interval (LeetCode 57):
   - Insert and merge new interval
   - Binary search + merge
   - O(n) time

4. Non-overlapping Intervals (LeetCode 435):
   - Minimum removals for non-overlapping
   - Greedy or DP approach
   - O(n log n) time

5. Car Pooling (LeetCode 1094):
   - Check if car capacity sufficient
   - Similar event processing
   - Can use same technique

6. Employee Free Time (LeetCode 759):
   - Find common free intervals
   - Merge all busy times, find gaps
   - Sweep line approach

7. My Calendar I/II/III (LeetCode 729/731/732):
   - Book meetings with different rules
   - Interval tree or sweep line
   - Various complexity levels

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Why This Solution is Optimal:

1. **Time Complexity**:
   - O(n log n) is optimal for comparison-based sorting
   - Cannot do better without additional constraints
   - Linear scan is optimal for counting

2. **Space Complexity**:
   - O(n) is necessary to store all times
   - Cannot reduce without losing information
   - Arrays are more efficient than objects

3. **Practical Efficiency**:
   - Good cache locality (sequential array access)
   - Minimal object creation
   - Simple arithmetic operations
   - Branch prediction friendly

4. **Code Simplicity**:
   - Easy to understand and maintain
   - Few lines of code
   - No complex data structures
   - Robust and bug-resistant

Alternative optimizations for specific scenarios:

- If intervals are already sorted by start: Skip start array sorting
- If many duplicate times: Use counting approach
- If intervals are streaming: Use segment tree or interval tree
- If querying multiple times: Precompute and cache result

═══════════════════════════════════════════════════════════════════════════════
*/