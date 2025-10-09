/*
PROBLEM: Trapping Rain Water
Given n non-negative integers representing an elevation map where the width of each bar is 1, 
compute how much water it can trap after raining.

Example 1:
Input: height = [0,1,0,2,1,0,1,3,2,1,2,1]
Output: 6
Explanation: The above elevation map (black section) is represented by array [0,1,0,2,1,0,1,3,2,1,2,1]. 
In this case, 6 units of rain water (blue section) are being trapped.

Example 2:
Input: height = [4,2,0,3,2,5]
Output: 9

Constraints:
- n == height.length
- 1 <= n <= 2 * 10^4
- 0 <= height[i] <= 3 * 10^4
*/

public class Solution
{
    public int Trap(int[] height)
    {
        // Handle edge cases: null or empty array
        if (height == null || height.Length == 0)
        {
            return 0;
        }

        int l = 0, r = height.Length - 1; // Two pointers from both ends
        int leftMax = height[l], rightMax = height[r]; // Track max heights seen so far
        int res = 0; // Total trapped water
        
        while (l < r) // Process until pointers meet
        {
            if (leftMax < rightMax) // Left max is the limiting factor
            {
                l++; // Move left pointer inward
                leftMax = Math.Max(leftMax, height[l]); // Update left max if current is taller
                res += leftMax - height[l]; // Add trapped water at current position
            }
            else // Right max is the limiting factor (or they're equal)
            {
                r--; // Move right pointer inward
                rightMax = Math.Max(rightMax, height[r]); // Update right max if current is taller
                res += rightMax - height[r]; // Add trapped water at current position
            }
        }
        return res; // Return total trapped water
    }
}

/*
TIME COMPLEXITY: O(n)
- Single pass through the array with two pointers moving towards each other
- Each element is visited exactly once by either left or right pointer
- Each iteration performs constant time operations
- Overall: O(n) where n is the length of the input array

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers and variables
- No additional data structures like arrays needed
- Much more space-efficient than approaches using extra arrays

ALGORITHM EXPLANATION:
- Water trapped at any position depends on minimum of (left max, right max) minus current height
- Two-pointer approach: maintain left and right max heights as we move inward
- Key insight: when leftMax < rightMax, we know leftMax is the limiting factor for left side
- We can safely calculate trapped water for left position without knowing future right max values
- Same logic applies when rightMax <= leftMax

KEY INSIGHTS:
- At any position, trapped water = min(leftMax, rightMax) - currentHeight
- When leftMax < rightMax, leftMax is guaranteed to be the limiting factor
- We don't need to know the exact rightMax for positions on the left - current rightMax is sufficient
- This allows us to process in O(1) space instead of O(n) space (compared to preprocessing arrays)

INTUITIVE UNDERSTANDING:
- Imagine walking from both ends towards center
- Keep track of tallest "wall" seen from each direction
- When left wall is shorter, we know water on left side is limited by left wall height
- When right wall is shorter, we know water on right side is limited by right wall height

ALTERNATIVE APPROACHES COMPARISON:
1. Brute Force: O(n²) time, O(1) space - for each position, find left and right max
2. Dynamic Programming: O(n) time, O(n) space - precompute left and right max arrays
3. Two Pointers: O(n) time, O(1) space - this approach, most optimal

EXAMPLE TRACE with [0,1,0,2,1,0,1,3,2,1,2,1]:
- l=0, r=11: leftMax=0, rightMax=1, leftMax < rightMax, move l
- l=1, r=11: leftMax=1, rightMax=1, equal so move r  
- Continue this process, accumulating trapped water
- Total trapped water: 6 units
*/