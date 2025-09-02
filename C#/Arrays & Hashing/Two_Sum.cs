// Question: Given an array of integers nums and an integer target, 
// return the indices of the two numbers such that they add up to target.
// You may assume that each input would have exactly one solution, 
// and you may not use the same element twice.
// You can return the answer in any order.

public class Solution {
    public int[] TwoSum(int[] nums, int target) {
        // Dictionary to store number → index mapping
        Dictionary<int, int> prevMap = new Dictionary<int, int>();

        // Loop through all numbers in the array
        for(int i = 0; i < nums.Length; i++){
            // Find the difference needed to reach target
            var diff = target - nums[i];

            // If the difference is already in the dictionary,
            // we found the two numbers → return their indices
            if(prevMap.ContainsKey(diff)){
                return new int[] { prevMap[diff], i };
            }

            // Otherwise, store the current number with its index
            prevMap[nums[i]] = i;
        }

        // This line will not be reached as per the problem guarantee
        return null;
    }
}

/*
Time Complexity: O(n)
 - We iterate over all n elements once, and dictionary lookups (ContainsKey / assignment) are O(1) on average.

Space Complexity: O(n)
 - In the worst case, the dictionary stores all n elements before finding the answer.
*/
