/*
PROBLEM: Largest Rectangle in Histogram
Given an array of integers heights representing the histogram's bar height where the width of 
each bar is 1, return the area of the largest rectangle in the histogram.

Example 1:
Input: heights = [2,1,5,6,2,3]
Output: 10
Explanation: The above is a histogram where width of each bar is 1.
The largest rectangle is shown in the red area (heights 5 and 6), which has an area = 10 units.

Example 2:
Input: heights = [2,4]
Output: 4

Constraints:
- 1 <= heights.length <= 10^5
- 0 <= heights[i] <= 10^4
*/

public class Solution
{
    public int LargestRectangleArea(int[] heights)
    {
        int maxArea = 0; // Track maximum rectangle area found
        Stack<int[]> stack = new Stack<int[]>(); // Stack stores [start_index, height] pairs

        // Process each bar in the histogram
        for (int i = 0; i < heights.Length; i++)
        {
            int start = i; // Potential start index for current height
            
            // Pop taller bars that can't extend further right
            while (stack.Count > 0 && stack.Peek()[1] > heights[i])
            {
                int[] top = stack.Pop(); // Get bar that's taller than current
                int index = top[0];      // Start index of popped bar
                int height = top[1];     // Height of popped bar
                
                // Calculate area: height × width (from start_index to current position)
                maxArea = Math.Max(maxArea, height * (i - index));
                
                // Current height can extend back to where popped bar started
                start = index;
            }
            
            // Push current bar with its effective start index
            stack.Push(new int[] { start, heights[i] });
        }

        // Process remaining bars in stack (they extend to end of histogram)
        foreach (int[] pair in stack)
        {
            int index = pair[0];   // Start index of bar
            int height = pair[1];  // Height of bar
            
            // Calculate area: height × width (from start_index to end of array)
            maxArea = Math.Max(maxArea, height * (heights.Length - index));
        }
        
        return maxArea; // Return maximum area found
    }
}

/*
TIME COMPLEXITY: O(n)
- Each bar is pushed onto stack exactly once: O(n)
- Each bar is popped from stack at most once: O(n)
- Final loop processes remaining stack elements: O(n) worst case
- Total: O(n) where n is the number of bars

SPACE COMPLEXITY: O(n)
- Stack can store at most n bars in worst case (strictly increasing heights)
- Example worst case: [1,2,3,4,5] - all bars remain until end
- Overall: O(n)

ALGORITHM EXPLANATION:
- Use monotonic increasing stack to efficiently find rectangle boundaries
- For each bar, determine how far left it can extend (using stack)
- When encountering shorter bar, calculate areas for all taller bars that must end
- Stack maintains bars in increasing height order with their start positions

KEY INSIGHTS:
- For a rectangle of height h, we want to know the maximum width it can span
- A bar of height h can extend left until it meets a shorter bar
- A bar can extend right until it meets a shorter bar
- Stack helps us efficiently track where each height started

MONOTONIC INCREASING STACK:
- Stack maintains bars in non-decreasing height order
- When new bar is shorter, pop taller bars (they can't extend further right)
- Each popped bar has found its right boundary (current position)
- Left boundary was stored when bar was pushed (start index)

START INDEX LOGIC:
- When we pop bars, current height can "inherit" their start position
- Example: heights [2,1,5,6,2] at index 4 (height 2)
  - Pop height 6 starting at index 3
  - Pop height 5 starting at index 2
  - Current height 2 can extend back to index 2 (where 5 started)
- This captures the idea that shorter bar can span where taller bars were

EXAMPLE TRACE with heights = [2,1,5,6,2,3]:

i=0: height=2, start=0
- Stack empty, push [0,2]
- Stack: [[0,2]]

i=1: height=1, start=1
- Pop [0,2]: area = 2*(1-0) = 2, start=0
- Push [0,1] (height 1 extends back to index 0)
- Stack: [[0,1]]
- maxArea = 2

i=2: height=5, start=2
- 5 > 1, push [2,5]
- Stack: [[0,1], [2,5]]

i=3: height=6, start=3
- 6 > 5, push [3,6]
- Stack: [[0,1], [2,5], [3,6]]

i=4: height=2, start=4
- Pop [3,6]: area = 6*(4-3) = 6, start=3
- Pop [2,5]: area = 5*(4-2) = 10 ✓, start=2
- 2 > 1, push [2,2] (height 2 extends back to index 2)
- Stack: [[0,1], [2,2]]
- maxArea = 10

i=5: height=3, start=5
- 3 > 2, push [5,3]
- Stack: [[0,1], [2,2], [5,3]]

Final processing (remaining stack):
- [5,3]: area = 3*(6-5) = 3
- [2,2]: area = 2*(6-2) = 8
- [0,1]: area = 1*(6-0) = 6

Result: maxArea = 10

WHY MONOTONIC STACK WORKS:
- Taller bars block shorter bars from extending right
- When we see shorter bar, all taller bars must end here (right boundary found)
- Stack helps us remember where each bar started (left boundary)
- Width = right_boundary - left_boundary

RECTANGLE AREA CALCULATION:
- For each bar: area = height × width
- Width = current_position - start_index (when bar is popped)
- We calculate area when bar is popped (boundaries are known)

EXTENDING BACKWARDS:
- Key optimization: when we pop bars, current bar inherits their start position
- This means: "If those taller bars could start there, so can I (I'm shorter)"
- Allows us to consider wider rectangles for shorter heights

EDGE CASES HANDLED:
- Single bar: Processed in final loop
- All increasing heights: All processed in final loop
- All decreasing heights: Each processed immediately when encountered
- Duplicate heights: Handled correctly with start index tracking

WHY THIS IS OPTIMAL:
- Brute force: O(n²) - for each bar, scan left and right to find boundaries
- This solution: O(n) - each bar processed once using stack
- Stack eliminates redundant boundary searches

VISUAL UNDERSTANDING:
Heights: [2,1,5,6,2,3]
         █     █
         █     █ █
         █ █ █ █ █
         █ █ █ █ █ █

Largest rectangle (area 10):
             ███
             ███
         █ █ ███ █
         █ █ ███ █ █
*/