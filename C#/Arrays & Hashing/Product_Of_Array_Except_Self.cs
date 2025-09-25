/*
PROBLEM: Product of Array Except Self
Given an integer array nums, return an array answer such that answer[i] is equal to 
the product of all the elements of nums except nums[i].

The product of any prefix or suffix of nums is guaranteed to fit in a 32-bit integer.

You must write an algorithm that runs in O(n) time and without using the division operation.

Example 1:
Input: nums = [1,2,3,4]
Output: [24,12,8,6]
Explanation:
- answer[0] = 2*3*4 = 24
- answer[1] = 1*3*4 = 12  
- answer[2] = 1*2*4 = 8
- answer[3] = 1*2*3 = 6

Example 2:
Input: nums = [-1,1,0,-3,3]
Output: [0,0,9,0,0]
Explanation:
- answer[0] = 1*0*(-3)*3 = 0
- answer[1] = (-1)*0*(-3)*3 = 0
- answer[2] = (-1)*1*(-3)*3 = 9
- answer[3] = (-1)*1*0*3 = 0
- answer[4] = (-1)*1*0*(-3) = 0

Constraints:
- 2 <= nums.length <= 10^5
- -30 <= nums[i] <= 30
- The product of any prefix or suffix of nums is guaranteed to fit in a 32-bit integer.

Follow up: Can you solve the problem in O(1) extra space complexity? 
(The output array does not count as extra space for space complexity analysis.)
*/

public class Solution
{
    public int[] ProductExceptSelf(int[] nums)
    {
        int n = nums.Length; // Get the length of input array
        int[] res = new int[n]; // Create result array of same size
        Array.Fill(res, 1); // Initialize all elements to 1
        
        // First pass: Calculate prefix products (products of elements to the left)
        for (int i = 1; i < n; i++)
        {
            // res[i] stores product of all elements before index i
            res[i] = res[i - 1] * nums[i - 1];
        }
        
        int postfix = 1; // Variable to keep track of suffix product (elements to the right)
        
        // Second pass: Multiply with suffix products (products of elements to the right)
        for (int i = n - 1; i >= 0; i--)
        {
            // Multiply current prefix product with suffix product
            res[i] *= postfix;
            
            // Update postfix to include current element for next iteration
            postfix *= nums[i];
        }
        
        return res; // Return the result array
    }
}

/*
TIME COMPLEXITY: O(n)
- First loop runs n-1 times to calculate prefix products
- Second loop runs n times to multiply with suffix products
- Overall: O(n) where n is the length of the input array

SPACE COMPLEXITY: O(1)
- Only using constant extra space (postfix variable)
- The result array doesn't count as extra space since it's required for output
- No additional data structures used

ALGORITHM EXPLANATION:
- For each index i, the result should be: (product of all elements before i) × (product of all elements after i)
- First pass: Store prefix products in result array
- Second pass: Multiply each element with suffix products calculated on-the-fly
- This avoids division and handles zero elements correctly
*/