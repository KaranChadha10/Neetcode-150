/*
PROBLEM: Binary Search
Given an array of integers nums which is sorted in ascending order, and an integer target, 
write a function to search target in nums. If target exists, then return its index. 
Otherwise, return -1.

You must write an algorithm with O(log n) runtime complexity.

Example 1:
Input: nums = [-1,0,3,5,9,12], target = 9
Output: 4
Explanation: 9 exists in nums and its index is 4

Example 2:
Input: nums = [-1,0,3,5,9,12], target = 2
Output: -1
Explanation: 2 does not exist in nums so return -1

Constraints:
- 1 <= nums.length <= 10^4
- -10^4 < nums[i], target < 10^4
- All the integers in nums are unique.
- nums is sorted in ascending order.
*/

public class Solution
{
    public int Search(int[] nums, int target)
    {
        int l = 0, r = nums.Length; // Left pointer at start, right pointer at length (not length-1)

        while (l < r) // Continue until search space is exhausted (l < r, not l <= r)
        {
            int m = l + (r - l) / 2; // Calculate middle index (overflow safe)
            
            // Check if middle element is greater than or equal to target
            if (nums[m] >= target)
            {
                r = m; // Target is at or before mid, search left half (including mid)
            }
            else
            {
                l = m + 1; // Target is after mid, search right half (excluding mid)
            }
        }
        
        // After loop, l points to the leftmost position where target could be
        // Verify that l is within bounds and the element at l equals target
        return (l < nums.Length && nums[l] == target) ? l : -1;
    }
}

/*
TIME COMPLEXITY: O(log n)
- Binary search eliminates half of search space each iteration
- Number of iterations = log₂(n) where n is array length
- Each iteration performs constant time operations
- Overall: O(log n) - required by problem constraint

SPACE COMPLEXITY: O(1)
- Only using constant extra space for pointers (l, r, m)
- No recursion or additional data structures
- Space usage independent of input size

ALGORITHM EXPLANATION:
- Classic binary search with "left-biased" approach
- Search space is [l, r) - l is inclusive, r is exclusive
- Find the leftmost position where target could be inserted
- After loop terminates, verify if element at position l equals target

KEY INSIGHTS:
- This variant uses r = nums.Length (not nums.Length - 1)
- Search space is [l, r) where r is exclusive boundary
- Loop condition is l < r (not l <= r)
- Always moves towards leftmost position of target

INVARIANT MAINTAINED:
- All elements before l are < target
- All elements from r onwards are >= target
- Target (if exists) is in range [l, r)

LEFT-BIASED BINARY SEARCH:
- When nums[m] >= target, we keep mid in search space (r = m)
- This ensures we find the leftmost occurrence if duplicates exist
- Even with unique elements, it finds the correct position

LOOP TERMINATION:
- Loop exits when l == r
- At this point, l is the leftmost position where target should be
- Need to verify: is l valid and does nums[l] equal target?

EXAMPLE TRACE with nums = [-1,0,3,5,9,12], target = 9:

Initial: l=0, r=6

Iteration 1:
- m = 0 + (6-0)/2 = 3
- nums[m] = 5
- 5 >= 9? No
- l = 3 + 1 = 4
- Search space: [4, 6)

Iteration 2:
- l=4, r=6
- m = 4 + (6-4)/2 = 5
- nums[m] = 12
- 12 >= 9? Yes
- r = 5
- Search space: [4, 5)

Iteration 3:
- l=4, r=5
- m = 4 + (5-4)/2 = 4
- nums[m] = 9
- 9 >= 9? Yes
- r = 4
- Search space: [4, 4) → empty

Exit loop: l=4, r=4
Verify: l < 6? Yes, nums[4] == 9? Yes
Return 4 ✓

EXAMPLE TRACE with nums = [-1,0,3,5,9,12], target = 2:

Initial: l=0, r=6

Iteration 1:
- m = 0 + (6-0)/2 = 3
- nums[m] = 5
- 5 >= 2? Yes
- r = 3
- Search space: [0, 3)

Iteration 2:
- l=0, r=3
- m = 0 + (3-0)/2 = 1
- nums[m] = 0
- 0 >= 2? No
- l = 1 + 1 = 2
- Search space: [2, 3)

Iteration 3:
- l=2, r=3
- m = 2 + (3-2)/2 = 2
- nums[m] = 3
- 3 >= 2? Yes
- r = 2
- Search space: [2, 2) → empty

Exit loop: l=2, r=2
Verify: l < 6? Yes, nums[2] == 2? No (nums[2] = 3)
Return -1 ✓

COMPARISON WITH STANDARD BINARY SEARCH:

Standard approach (r = nums.Length - 1):
```csharp
while (l <= r) {
    int m = l + (r - l) / 2;
    if (nums[m] == target) return m;
    else if (nums[m] < target) l = m + 1;
    else r = m - 1;
}
return -1;
```

This approach (r = nums.Length):
```csharp
while (l < r) {
    int m = l + (r - l) / 2;
    if (nums[m] >= target) r = m;
    else l = m + 1;
}
return (l < nums.Length && nums[l] == target) ? l : -1;
```

WHY THIS VARIANT IS USEFUL:
- Finds leftmost position (useful for finding insertion points)
- Single comparison in loop (nums[m] >= target)
- Naturally handles "lower bound" searches
- Consistent with C++ std::lower_bound() behavior

EDGE CASES HANDLED:
- Single element array: works correctly
- Target smaller than all elements: l=0, nums[0] != target, returns -1
- Target larger than all elements: l=nums.Length, out of bounds check, returns -1
- Target at first position: found correctly
- Target at last position: found correctly
- Empty array: would require additional check (not in constraints)

BOUNDARY BEHAVIOR:
- Uses [l, r) half-open interval
- r is always exclusive (never accessed)
- l converges to insertion point for target
- Final check ensures we return index only if target exists

WHY l < r (not l <= r)?
- Search space is [l, r) where r is exclusive
- When l == r, search space is empty
- No need for <= comparison with exclusive right boundary

WHY r = m (not r = m - 1)?
- We keep mid in search space when nums[m] >= target
- Mid could be the target or the leftmost occurrence
- Safe because r is exclusive boundary

ALTERNATIVE VISUALIZATION:
Elements:     -1  0  3  5  9  12
Indices:       0  1  2  3  4  5
Search [0,6):  ←  [search space] →

After finding target at index 4:
Elements:     -1  0  3  5  9  12
Indices:       0  1  2  3  4  5
                         ↑
                       l=r=4
*/