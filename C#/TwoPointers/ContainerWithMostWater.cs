/*
PROBLEM: Container With Most Water
You are given an integer array height of length n. There are n vertical lines drawn such that 
the two endpoints of the ith line are (i, 0) and (i, height[i]).

Find two lines that together with the x-axis form a container, such that the container 
contains the most water.

Return the maximum amount of water a container can store.

Notice that you may not slant the container.

Example 1:
Input: height = [1,8,6,2,5,4,8,3,7]
Output: 49
Explanation: The above vertical lines are represented by array [1,8,6,2,5,4,8,3,7]. 
In this case, the max area of water (blue section) the container can contain is 49.

Example 2:
Input: height = [1,1]
Output: 1

Constraints:
- n == height.length
- 2 <= n <= 10^5
- 0 <= height[i] <= 10^4
*/

public class Solution
{
    public int MaxArea(int[] heights)
    {
        int res = 0; // Track maximum area found so far
        int l = 0, r = heights.Length - 1; // Two pointers: start from widest possible container
        
        while (l < r) // Continue until pointers meet
        {
            // Calculate current container area: width × height
            // Height is limited by the shorter line (water would overflow otherwise)
            int area = Math.Min(heights[l], heights[r]) * (r - l);
            
            res = Math.Max(area, res); // Update maximum area if current is larger
            
            // Move the pointer with smaller height inward
            // This gives us the best chance to find a larger area
            if (heights[l] < heights[r])
            {
                l++; // Move left pointer right (try taller left line)
            }
            else
            {
                r--; // Move right pointer left (try taller right line)
            }
        }
        return res; // Return maximum area found
    }
}

/*
TIME COMPLEXITY: O(n)
- We traverse the array with two pointers that move towards each other
- Each element is visited at most once by either left or right pointer
- Each iteration performs constant time operations
- Overall: O(n) where n is the length of the input array

SPACE COMPLEXITY: O(1)
- Only using constant extra space for variables (res, l, r, area)
- No additional data structures needed
- Space usage doesn't depend on input size

ALGORITHM EXPLANATION:
- Two-pointer technique starting from the widest possible container
- At each step, we have a choice: move left pointer right or right pointer left
- Key insight: Always move the pointer with the smaller height
- Why? Moving the taller pointer can only decrease area (width decreases, height stays same or decreases)
- Moving the shorter pointer might find a taller line, potentially increasing area

KEY INSIGHTS:
- Start with maximum width to give best chance for large area
- Water level is determined by the shorter of the two lines
- Greedy approach: always try to improve the limiting factor (shorter height)
- We don't need to check all possible pairs - this greedy strategy guarantees optimal solution

WHY GREEDY WORKS:
- When heights[l] < heights[r], any container using l with a line between l and r 
  will have area ≤ current area (same height, smaller width)
- So we can safely eliminate all such containers and move l inward
- Same logic applies when heights[r] < heights[l]

EXAMPLE TRACE with [1,8,6,2,5,4,8,3,7]:
- l=0, r=8: area = min(1,7) × 8 = 8, move l (height 1 is smaller)
- l=1, r=8: area = min(8,7) × 7 = 49, move r (height 7 is smaller) 
- Continue until l meets r
- Maximum area found: 49
*/