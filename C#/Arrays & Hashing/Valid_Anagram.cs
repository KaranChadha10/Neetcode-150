// Question: Given two strings s and t, return true if t is an anagram of s, 
// and false otherwise. An anagram is a word formed by rearranging the letters 
// of another word, using all the original letters exactly once.

public class Solution
{
    public bool IsAnagram(string s, string t)
    {
        // If lengths are different, they cannot be anagrams
        if (s.Length != t.Length)
        {
            return false;
        }

        // Array to count frequency of characters (26 for 'a' to 'z')
        int[] count = new int[26];

        // Increase count for characters in s, decrease for t
        for (int i = 0; i < s.Length; i++)
        {
            count[s[i] - 'a']++;  // increment for s
            count[t[i] - 'a']--;  // decrement for t
        }

        // If all counts are zero, they are anagrams
        foreach (int val in count)
        {
            if (val != 0)
            {
                return false;
            }
        }

        return true;
    }
}

/*
Time Complexity: O(n) → we go through both strings once (same length n).
Space Complexity: O(1) → uses a fixed-size array of 26 letters, independent of input size.
*/