// Question: Given an integer array nums, 
// return true if any value appears at least twice in the array,
// and return false if every element is distinct.

public class Solution
{
    public bool hasDuplicate(int[] nums)
    {
        // Create a HashSet to store numbers we've seen so far
        HashSet<int> seen = new HashSet<int>();

        // Loop through each number in the array
        foreach (int num in nums)
        {
            // If number is already in the set, it's a duplicate → return true
            if (seen.Contains(num))
            {
                return true;
            }

            // Otherwise, add the number to the set
            seen.Add(num);
        }

        // If loop finishes with no duplicates found, return false
        return false;
    }
}

/*
Time Complexity: O(n) 
 - We loop through all n elements once, and HashSet operations (Contains/Add) are O(1) on average.

Space Complexity: O(n)
 - In the worst case (all elements are unique), we store all n elements in the HashSet.
*/
