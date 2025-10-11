/*
PROBLEM: Search in Rotated Sorted Array
There is an integer array nums sorted in ascending order (with distinct values).

Prior to being passed to your function, nums is possibly rotated at an unknown pivot index k 
(1 <= k < nums.length) such that the resulting array is [nums[k], nums[k+1], ..., nums[n-1], 
nums[0], nums[1], ..., nums[k-1]] (0-indexed). For example, [0,1,2,4,5,6,7] might be rotated 
at pivot index 3 and become [4,5,6,7,0,1,2].

Given the array nums after the possible rotation and an integer target, return the index of 
target if it is in nums, or -1 if it is not in nums.

You must write an algorithm with O(log n) runtime complexity.

Example 1:
Input: nums = [4,5,6,7,0,1,2], target = 0
Output: 4

Example 2:
Input: nums = [4,5,6,7,0,1,2], target = 3
Output: -1

Example 3:
Input: nums = [1], target = 0
Output: -1

Constraints:
- 1 <= nums.length <= 5000
- -10^4 <= nums[i] <= 10^4
- All values of nums are unique.
- nums is an ascending array that is possibly rotated.
- -10^4 <= target <= 10^4
*/

public class Solution
{
    public int Search(int[] nums, int target)
    {
        int l = 0, r = nums.Length - 1; // Initialize left and right pointers
        
        while (l <= r) // Continue until search space is exhausted
        {
            int mid = (l + r) / 2; // Calculate middle index
            
            // Check if middle element is the target
            if (target == nums[mid])
            {
                return mid; // Found target, return its index
            }

            // Determine which half is sorted (left or right)
            if (nums[l] <= nums[mid]) // Left half is sorted
            {
                // Check if target is in the sorted left half
                if (target > nums[mid] || target < nums[l])
                {
                    l = mid + 1; // Target not in left half, search right half
                }
                else
                {
                    r = mid - 1; // Target in left half, search left half
                }
            }
            else // Right half is sorted (rotation point is in left half)
            {
                // Check if target is in the sorted right half
                if (target < nums[mid] || target > nums[r])
                {
                    r = mid - 1; // Target not in right half, search left half
                }
                else
                {
                    l = mid + 1; // Target in right half, search right half
                }
            }
        }
        return -1; // Target not found in array
    }
}

/*
TIME COMPLEXITY: O(log n)
- Binary search eliminates half of search space in each iteration
- Number of iterations = log₂(n) where n is array length
- Each iteration performs constant time operations
- Overall: O(log n) - required by problem constraint

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers (l, r, mid)
- No recursion or additional data structures
- Space usage independent of input size

ALGORITHM EXPLANATION:
- Modified binary search for rotated sorted array
- At any point, at least one half (left or right) is properly sorted
- Identify which half is sorted, then check if target is in that sorted half
- If target in sorted half → search that half
- If target not in sorted half → search other half

KEY INSIGHTS:
- Original array was sorted, then rotated at some pivot
- After rotation, array has two sorted subarrays
- One half from mid will always be properly sorted
- Use sorted half to determine which direction to search

IDENTIFYING SORTED HALF:
- If nums[l] <= nums[mid] → left half is sorted
- Otherwise → right half is sorted (rotation point in left half)

DECISION LOGIC FOR LEFT HALF SORTED:
- If target > nums[mid] → target can't be in left half
- If target < nums[l] → target can't be in left half
- Otherwise → target might be in left half, search it

DECISION LOGIC FOR RIGHT HALF SORTED:
- If target < nums[mid] → target can't be in right half
- If target > nums[r] → target can't be in right half
- Otherwise → target might be in right half, search it

EXAMPLE TRACE with nums = [4,5,6,7,0,1,2], target = 0:

Initial: l=0, r=6, nums=[4,5,6,7,0,1,2]

Iteration 1:
- mid = (0+6)/2 = 3
- nums[mid] = 7 ≠ 0
- nums[l]=4 <= nums[mid]=7 → left half sorted [4,5,6,7]
- target=0 > nums[mid]=7? No
- target=0 < nums[l]=4? Yes
- Target not in sorted left half → search right half
- l = 3+1 = 4
- New range: l=4, r=6, search [0,1,2]

Iteration 2:
- mid = (4+6)/2 = 5
- nums[mid] = 1 ≠ 0
- nums[l]=0 <= nums[mid]=1 → left half sorted [0,1]
- target=0 > nums[mid]=1? No
- target=0 < nums[l]=0? No
- Target might be in sorted left half → search left half
- r = 5-1 = 4
- New range: l=4, r=4, search [0]

Iteration 3:
- mid = (4+4)/2 = 4
- nums[mid] = 0 == target ✓
- Return 4

VISUAL REPRESENTATION:
Array: [4,5,6,7,0,1,2]
        ↑     ↑     ↑
        l    mid    r

Left sorted: [4,5,6,7] | Right sorted: [0,1,2]
             ↑                          ↑
        Rotation point here

WHY THIS WORKS:
- Rotation creates two sorted segments
- Mid point lies in one of these segments
- Using mid, we can identify which segment is sorted
- Check if target is in sorted segment using boundary values
- Eliminate half the search space each iteration

EDGE CASES HANDLED:
- Single element array: l=r=0, checks mid and returns
- Target not in array: exhausts search space, returns -1
- No rotation (already sorted): works as standard binary search
- Target at rotation point: correctly identifies and returns
- Target at boundaries (first/last element): handled correctly

COMPARISON WITH STANDARD BINARY SEARCH:
- Standard: works on fully sorted array
- This: works on rotated sorted array
- Both: O(log n) time complexity
- Difference: extra logic to identify sorted half

ALTERNATIVE APPROACHES:
1. Find pivot, then binary search in correct half: O(log n) + O(log n) = O(log n)
2. Linear search: O(n) - simpler but doesn't meet time requirement
3. This approach: O(log n) - optimal, single pass
*/