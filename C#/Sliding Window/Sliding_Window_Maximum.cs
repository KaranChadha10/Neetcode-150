/*
PROBLEM: Sliding Window Maximum
You are given an array of integers nums, there is a sliding window of size k which is moving 
from the very left of the array to the very right. You can only see the k numbers in the window. 
Each time the sliding window moves right by one position.

Return the max sliding window.

Example 1:
Input: nums = [1,3,-1,-3,5,3,6,7], k = 3
Output: [3,3,5,5,6,7]
Explanation: 
Window position                Max
---------------               -----
[1  3  -1] -3  5  3  6  7       3
 1 [3  -1  -3] 5  3  6  7       3
 1  3 [-1  -3  5] 3  6  7       5
 1  3  -1 [-3  5  3] 6  7       5
 1  3  -1  -3 [5  3  6] 7       6
 1  3  -1  -3  5 [3  6  7]      7

Example 2:
Input: nums = [1], k = 1
Output: [1]

Constraints:
- 1 <= nums.length <= 10^5
- -10^4 <= nums[i] <= 10^4
- 1 <= k <= nums.length
*/

public class Solution
{
    public int[] MaxSlidingWindow(int[] nums, int k)
    {
        int n = nums.Length;
        
        // Precompute maximum values for blocks from left and right directions
        int[] leftMax = new int[n];   // leftMax[i] = max from block start to position i
        int[] rightMax = new int[n];  // rightMax[i] = max from position i to block end

        // Initialize first and last elements
        leftMax[0] = nums[0];
        rightMax[n - 1] = nums[n - 1];

        // Build leftMax and rightMax arrays simultaneously
        for (int i = 1; i < n; i++)
        {
            // Build leftMax array (left to right)
            if (i % k == 0) // Start of new block of size k
            {
                leftMax[i] = nums[i]; // Reset for new block
            }
            else
            {
                leftMax[i] = Math.Max(leftMax[i - 1], nums[i]); // Extend current block max
            }

            // Build rightMax array (right to left)
            int rightIndex = n - 1 - i; // Mirror index for right-to-left processing
            if ((n - rightIndex - 1) % k == 0) // Start of new block from right side
            {
                rightMax[rightIndex] = nums[rightIndex]; // Reset for new block
            }
            else
            {
                rightMax[rightIndex] = Math.Max(rightMax[rightIndex + 1], nums[rightIndex]); // Extend current block max
            }
        }

        // Generate sliding window maximums using precomputed arrays
        int[] output = new int[n - k + 1]; // Result array for all sliding windows
        for (int i = 0; i < n - k + 1; i++)
        {
            // For window [i, i+k-1], maximum is the max of:
            // - rightMax[i]: max from window start to end of its block
            // - leftMax[i+k-1]: max from start of block containing window end to window end
            output[i] = Math.Max(leftMax[i + k - 1], rightMax[i]);
        }
        
        return output;
    }
}

/*
TIME COMPLEXITY: O(n)
- Building leftMax array: O(n) - each element processed once
- Building rightMax array: O(n) - each element processed once (done simultaneously with leftMax)
- Generating output: O(n-k+1) ≈ O(n) - constant time lookup for each window
- Overall: O(n) where n is the length of input array

SPACE COMPLEXITY: O(n)
- leftMax array: O(n) space
- rightMax array: O(n) space  
- output array: O(n-k+1) space (required for result)
- Other variables: O(1)
- Overall: O(n) space

ALGORITHM EXPLANATION:
- Divide array into blocks of size k
- For each block, precompute maximum values in both directions:
  - leftMax[i]: maximum from block start to position i
  - rightMax[i]: maximum from position i to block end
- For any sliding window, the maximum is max(rightMax[start], leftMax[end])

KEY INSIGHTS:
- Any sliding window of size k either:
  1. Fits entirely within one block, OR
  2. Spans across two consecutive blocks
- rightMax[i] gives maximum from window start to end of its block
- leftMax[i+k-1] gives maximum from start of block to window end
- Taking max of both covers the entire window regardless of block boundaries

BLOCK DIVISION STRATEGY:
- Array is conceptually divided into blocks: [0,k-1], [k,2k-1], [2k,3k-1], ...
- leftMax builds maximum going left-to-right within each block
- rightMax builds maximum going right-to-left within each block
- This preprocessing allows O(1) lookup for any window maximum

EXAMPLE TRACE with nums = [1,3,-1,-3,5,3,6,7], k = 3:
Blocks: [1,3,-1] [-3,5,3] [6,7]

leftMax:  [1,3,3, -3,5,5, 6,7]  // Max from block start to current position
rightMax: [3,3,-1, 5,5,3, 7,7]  // Max from current position to block end

Windows:
- [1,3,-1]: max(rightMax[0], leftMax[2]) = max(3, 3) = 3
- [3,-1,-3]: max(rightMax[1], leftMax[3]) = max(3, -3) = 3  
- [-1,-3,5]: max(rightMax[2], leftMax[4]) = max(-1, 5) = 5
- [-3,5,3]: max(rightMax[3], leftMax[5]) = max(5, 5) = 5
- [5,3,6]: max(rightMax[4], leftMax[6]) = max(5, 6) = 6
- [3,6,7]: max(rightMax[5], leftMax[7]) = max(3, 7) = 7

COMPARISON WITH OTHER APPROACHES:
- Brute Force: O(n*k) - check each window separately
- Deque Approach: O(n) time, O(k) space - more complex implementation
- This Approach: O(n) time, O(n) space - simpler logic, easier to understand

EDGE CASES HANDLED:
- k = 1: Each element is its own maximum
- k = n: Single window covering entire array
- Single element array: Works correctly
*/