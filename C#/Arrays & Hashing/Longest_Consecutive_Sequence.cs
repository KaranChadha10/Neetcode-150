/*
PROBLEM: Longest Consecutive Sequence
Given an unsorted array of integers nums, return the length of the longest 
consecutive elements sequence.

You must write an algorithm that runs in O(n) time.

Example 1:
Input: nums = [100,4,200,1,3,2]
Output: 4
Explanation: The longest consecutive elements sequence is [1, 2, 3, 4]. 
Therefore its length is 4.

Example 2:
Input: nums = [0,3,7,2,5,8,4,6,0,1]
Output: 9
Explanation: The longest consecutive sequence is [0,1,2,3,4,5,6,7,8].
*/

public class Solution
{
    public int LongestConsecutive(int[] nums)
    {
        // Convert array to HashSet for O(1) lookup time and remove duplicates
        HashSet<int> numSet = new HashSet<int>(nums);
        int longest = 0; // Track the maximum consecutive sequence length found
        
        // Iterate through each unique number in the set
        foreach (int num in numSet)
        {
            // Check if current number is the START of a sequence
            // (i.e., num-1 doesn't exist, so num is the smallest in its sequence)
            if (!numSet.Contains(num - 1))
            {
                int length = 1; // Current sequence starts with length 1
                
                // Keep extending the sequence while consecutive numbers exist
                while (numSet.Contains(num + length))
                {
                    length++; // Increment length for each consecutive number found
                }
                
                // Update the longest sequence length if current one is longer
                longest = Math.Max(longest, length);
            }
        }
        return longest; // Return the length of longest consecutive sequence
    }
}

/*
TIME COMPLEXITY: O(n)
- Converting array to HashSet: O(n)
- The foreach loop runs n times in worst case
- The while loop might seem like it makes this O(n²), but each number is visited at most twice:
  - Once in the foreach loop
  - Once in the while loop (when it's part of a sequence starting from a smaller number)
- Overall: O(n) because each element is processed at most a constant number of times

SPACE COMPLEXITY: O(n)
- HashSet stores all unique numbers from the input array
- In worst case (all numbers unique), we store n elements
- No other significant space used

ALGORITHM EXPLANATION:
- Key insight: Only start counting from the beginning of each sequence
- We identify sequence starts by checking if (num - 1) exists
- If (num - 1) doesn't exist, then 'num' is the start of a potential sequence
- From each sequence start, we count consecutive numbers using the while loop
- This ensures each number is only counted once as part of exactly one sequence

EXAMPLE with [100, 4, 200, 1, 3, 2]:
- HashSet: {100, 4, 200, 1, 3, 2}
- num=100: 99 not in set → start sequence → 100 (length=1)
- num=4: 3 is in set → skip (not sequence start)
- num=200: 199 not in set → start sequence → 200 (length=1)  
- num=1: 0 not in set → start sequence → 1,2,3,4 (length=4)
- num=3: 2 is in set → skip (not sequence start)
- num=2: 1 is in set → skip (not sequence start)
- Result: longest = 4
*/