/*
PROBLEM: Find Minimum in Rotated Sorted Array
Suppose an array of length n sorted in ascending order is rotated between 1 and n times. 
For example, the array nums = [0,1,2,4,5,6,7] might become:
- [4,5,6,7,0,1,2] if it was rotated 4 times.
- [0,1,2,4,5,6,7] if it was rotated 7 times.

Notice that rotating an array [a[0], a[1], a[2], ..., a[n-1]] 1 time results in the array 
[a[n-1], a[0], a[1], a[2], ..., a[n-2]].

Given the sorted rotated array nums of unique elements, return the minimum element of this array.

You must write an algorithm that runs in O(log n) time.

Example 1:
Input: nums = [3,4,5,1,2]
Output: 1
Explanation: The original array was [1,2,3,4,5] rotated 3 times.

Example 2:
Input: nums = [4,5,6,7,0,1,2]
Output: 0
Explanation: The original array was [0,1,2,4,5,6,7] and it was rotated 4 times.

Example 3:
Input: nums = [11,13,15,17]
Output: 11
Explanation: The original array was [11,13,15,17] and it was rotated 4 times (no actual rotation).

Constraints:
- n == nums.length
- 1 <= n <= 5000
- -5000 <= nums[i] <= 5000
- All the integers of nums are unique.
- nums is sorted and rotated between 1 and n times.
*/

public class Solution
{
    public int FindMin(int[] nums)
    {
        int l = 0, r = nums.Length - 1; // Initialize left and right pointers
        
        while (l < r) // Continue until l equals r (found minimum)
        {
            int m = l + (r - l) / 2; // Calculate middle index (overflow safe)
            
            // Compare middle element with rightmost element
            if (nums[m] < nums[r])
            {
                // Middle is smaller than right → minimum is in left half (including mid)
                r = m; // Move right pointer to mid (mid could be the minimum)
            }
            else
            {
                // Middle is larger than right → rotation point is in right half
                l = m + 1; // Move left pointer past mid (mid cannot be minimum)
            }
        }
        return nums[l]; // When l==r, we've found the minimum element
    }
}

/*
TIME COMPLEXITY: O(log n)
- Binary search eliminates half of search space each iteration
- Number of iterations = log₂(n) where n is array length
- Each iteration performs constant time operations
- Overall: O(log n) - meets problem requirement

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers (l, r, m)
- No recursion or additional data structures
- Space usage independent of input size

ALGORITHM EXPLANATION:
- Use binary search to find the rotation point (minimum element)
- Compare middle element with rightmost element to determine which half contains minimum
- The minimum is always at the rotation point
- Rotation point is where sorted order breaks

KEY INSIGHTS:
- In rotated sorted array, minimum is at the rotation point
- One half is always properly sorted
- Compare mid with right to determine which half has the minimum
- Unlike standard binary search, we compare mid with boundary (right), not target

DECISION LOGIC:
If nums[m] < nums[r]:
- Array from mid to right is sorted
- Minimum must be in left half (including mid)
- Example: [4,5,1,2,3] mid=1 < right=3 → min in left half

If nums[m] > nums[r]:
- Rotation point is in right half
- Minimum is to the right of mid
- Example: [5,6,7,1,2] mid=7 > right=2 → min in right half

Why compare with nums[r] and not nums[l]?
- Comparing with right boundary gives clearer decision
- Tells us which side has rotation point (unsorted part)
- Comparing with left would require more complex logic

WHY r = m (not r = m - 1)?
- Mid could be the minimum
- Example: [2,1] → mid=0 points to 2, but minimum is at index 1
- But in [3,1,2] → mid=1 points to minimum directly
- Safe to include mid in search space

WHY l = m + 1 (not l = m)?
- When nums[m] > nums[r], mid cannot be minimum
- Mid is in the larger sorted segment
- Minimum must be to the right
- Safe to exclude mid

EXAMPLE TRACE with nums = [4,5,6,7,0,1,2]:

Initial: l=0, r=6

Iteration 1:
- m = 0 + (6-0)/2 = 3
- nums[m]=7, nums[r]=2
- 7 > 2 → rotation point in right half
- l = 3+1 = 4
- Search space: [0,1,2]

Iteration 2:
- l=4, r=6
- m = 4 + (6-4)/2 = 5
- nums[m]=1, nums[r]=2
- 1 < 2 → minimum in left half (including mid)
- r = 5
- Search space: [0,1]

Iteration 3:
- l=4, r=5
- m = 4 + (5-4)/2 = 4
- nums[m]=0, nums[r]=1
- 0 < 1 → minimum in left half (including mid)
- r = 4
- Search space: [0]

Iteration 4:
- l=4, r=4 → l == r, exit loop
- Return nums[4] = 0 ✓

VISUAL REPRESENTATION:
Array: [4,5,6,7,0,1,2]
        -------     --
         Sorted    Sorted
              ↓
        Rotation point (minimum)

LOOP INVARIANT:
- Minimum is always within [l, r]
- Each iteration reduces search space by half
- When l == r, we've pinpointed the minimum

EDGE CASES HANDLED:
- No rotation (already sorted): nums[m] < nums[r] throughout, finds first element
- Single element: l=r=0 immediately, returns only element
- Rotated by 1: works correctly
- Minimum at start: found when r moves to start
- Minimum at end: found when l moves to end

COMPARISON WITH "SEARCH IN ROTATED SORTED ARRAY":
- That problem: search for target value
- This problem: find minimum (rotation point)
- Both: use binary search on rotated array
- Different: comparison logic and decision making

WHY NOT COMPARE WITH nums[l]?
Consider [3,1,2]:
- If nums[m] > nums[l]: could be in either half
- If nums[m] < nums[l]: rotation point is in left half
- More complex logic needed
- Comparing with nums[r] is cleaner

ALTERNATIVE APPROACH:
- Could find rotation point, then return that element
- This approach directly finds minimum in one pass
- More elegant and efficient

WHY l < r (not l <= r)?
- We're finding a position, not searching for a value
- When l == r, we've found the answer
- l <= r would cause infinite loop when array has one element

PATTERN: FINDING ROTATION POINT
- Array rotated → creates two sorted segments
- Minimum is at rotation point (junction)
- Binary search can locate this efficiently
- Compare mid with boundary to determine which half
*/