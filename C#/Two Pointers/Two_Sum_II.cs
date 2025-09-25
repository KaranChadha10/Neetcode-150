/*
PROBLEM: Two Sum II - Input Array Is Sorted
Given a 1-indexed array of integers numbers that is already sorted in non-decreasing order, 
find two numbers such that they add up to a specific target number. Let these two numbers 
be numbers[index1] and numbers[index2] where 1 <= index1 < index2 <= numbers.length.

Return the indices of the two numbers, index1 and index2, added by one as an integer array 
[index1, index2] of length 2.

The tests are generated such that there is exactly one solution. You may not use the same 
element twice.

Your solution must use only constant extra space.

Example 1:
Input: numbers = [2,7,11,15], target = 9
Output: [1,2]
Explanation: The sum of 2 and 7 is 9. Therefore, index1 = 1, index2 = 2. We return [1, 2].

Example 2:
Input: numbers = [2,3,4], target = 6
Output: [1,3]
Explanation: The sum of 2 and 4 is 6. Therefore, index1 = 1, index2 = 3. We return [1, 3].

Example 3:
Input: numbers = [-1,0], target = -1
Output: [1,2]
Explanation: The sum of -1 and 0 is -1. Therefore, index1 = 1, index2 = 2. We return [1, 2].

Constraints:
- 2 <= numbers.length <= 3 * 10^4
- -1000 <= numbers[i] <= 1000
- numbers is sorted in non-decreasing order.
- -1000 <= target <= 1000
- The tests are generated such that there is exactly one solution.
*/

public class Solution
{
    public int[] TwoSum(int[] numbers, int target)
    {
        int l = 0, r = numbers.Length - 1; // Two pointers: left at start, right at end
        
        while (l < r) // Continue until pointers meet
        {
            int curSum = numbers[l] + numbers[r]; // Calculate sum of current pair
            
            if (curSum > target) // Sum is too large
            {
                r--; // Move right pointer left to decrease sum
            }
            else if (curSum < target) // Sum is too small
            {
                l++; // Move left pointer right to increase sum
            }
            else // Found the target sum
            {
                return new int[] { l + 1, r + 1 }; // Return 1-indexed positions
            }
        }
        return new int[0]; // This should never be reached given problem constraints
    }
}

/*
TIME COMPLEXITY: O(n)
- Each pointer moves at most n positions
- We traverse the array at most once with two pointers moving towards each other
- Each iteration performs constant time operations
- Overall: O(n) where n is the length of the input array

SPACE COMPLEXITY: O(1)
- Only using constant extra space for the two pointers (l, r)
- No additional data structures needed
- The result array doesn't count as extra space since it's required for output

ALGORITHM EXPLANATION:
- Two-pointer technique leverages the sorted nature of the input array
- If current sum is too large, we need a smaller number → move right pointer left
- If current sum is too small, we need a larger number → move left pointer right
- This approach guarantees we find the solution in linear time

KEY INSIGHTS:
- The sorted array property allows us to use two pointers efficiently
- We can eliminate half the search space with each comparison
- No need for hash map (unlike regular Two Sum) since array is sorted
- Return indices as 1-indexed (add 1 to both l and r)
- Problem guarantees exactly one solution exists

COMPARISON WITH REGULAR TWO SUM:
- Regular Two Sum: O(n) time, O(n) space (using hash map)
- Two Sum II: O(n) time, O(1) space (using two pointers on sorted array)
- The sorted property makes this more space-efficient
*/