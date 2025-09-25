/*
PROBLEM: 3Sum
Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that 
i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.

Notice that the solution set must not contain duplicate triplets.

Example 1:
Input: nums = [-1,0,1,2,-1,-4]
Output: [[-1,-1,2],[-1,0,1]]
Explanation: 
nums[0] + nums[1] + nums[2] = (-1) + 0 + 1 = 0.
nums[1] + nums[2] + nums[4] = 0 + 1 + (-1) = 0.
The distinct triplets are [-1,0,1] and [-1,-1,2].
Notice that the order of the output and the order of the triplets does not matter.

Example 2:
Input: nums = [0,1,1]
Output: []
Explanation: The only possible triplet does not sum up to 0.

Example 3:
Input: nums = [0,0,0]
Output: [[0,0,0]]
Explanation: The only possible triplet sums up to 0.

Constraints:
- 3 <= nums.length <= 3000
- -10^5 <= nums[i] <= 10^5
*/

public class Solution
{
    public List<List<int>> ThreeSum(int[] nums)
    {
        Array.Sort(nums); // Sort array to enable two-pointer technique and handle duplicates
        List<List<int>> res = new List<List<int>>(); // Result list to store valid triplets

        for (int i = 0; i < nums.Length; i++) // Fix first element of triplet
        {
            // Optimization: if first element > 0, no valid triplets possible
            // (since array is sorted, all remaining elements are also > 0)
            if (nums[i] > 0) break;
            
            // Skip duplicate values for first element to avoid duplicate triplets
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            int l = i + 1, r = nums.Length - 1; // Two pointers for remaining two elements
            
            while (l < r) // Find pairs that sum with nums[i] to equal 0
            {
                int sum = nums[i] + nums[l] + nums[r]; // Calculate triplet sum
                
                if (sum > 0) // Sum too large, need smaller number
                {
                    r--; // Move right pointer left
                }
                else if (sum < 0) // Sum too small, need larger number
                {
                    l++; // Move left pointer right
                }
                else // Found valid triplet (sum == 0)
                {
                    // Add triplet to result
                    res.Add(new List<int> { nums[i], nums[l], nums[r] });
                    l++; // Move left pointer
                    r--; // Move right pointer
                    
                    // Skip duplicate values for left pointer to avoid duplicate triplets
                    while (l < r && nums[l] == nums[l - 1])
                    {
                        l++; // Continue moving left pointer past duplicates
                    }
                }
            }
        }
        return res; // Return all unique triplets that sum to 0
    }
}

/*
TIME COMPLEXITY: O(n²)
- Sorting the array: O(n log n)
- Outer loop runs n times
- Inner two-pointer search runs O(n) time for each outer iteration
- Overall: O(n log n) + O(n²) = O(n²) where n is the length of input array

SPACE COMPLEXITY: O(1) or O(log n)
- Only using constant extra space for pointers and variables
- The sorting may use O(log n) space depending on implementation
- Result list doesn't count as extra space since it's required for output
- Not counting the space used by the sorting algorithm: O(1)

ALGORITHM EXPLANATION:
- Sort array first to enable two-pointer technique and easy duplicate handling
- Fix first element, then use two pointers to find remaining two elements
- Skip duplicates at all levels to ensure unique triplets only
- Use sorted property: move pointers based on whether sum is too large/small

KEY INSIGHTS:
- Sorting enables efficient duplicate detection and two-pointer technique
- Early termination: if first element > 0, no valid triplets possible
- Three levels of duplicate handling:
  1. Skip duplicate first elements (i)
  2. Skip duplicate left elements after finding valid triplet
  3. Right pointer duplicates are handled automatically by the algorithm
- This reduces 3Sum to multiple 2Sum problems on sorted array

DUPLICATE HANDLING STRATEGY:
- For first element: skip if same as previous
- For second element: skip duplicates after finding a valid solution
- This ensures we don't generate duplicate triplets like [-1,0,1] multiple times
*/