// Question: Given an array of strings strs, group the anagrams together. 
// You can return the answer in any order.
// An Anagram is a word or phrase formed by rearranging the letters of 
// a different word or phrase, typically using all the original letters exactly once.

public class Solution
{
    public List<List<string>> GroupAnagrams(string[] strs)
    {
        // Dictionary to group words by their "character count key"
        var res = new Dictionary<string, List<string>>();

        // Process each word
        foreach (var s in strs)
        {
            // Array to count frequency of each letter (a-z)
            int[] count = new int[26];
            foreach (char c in s)
            {
                count[c - 'a']++;
            }

            // Convert count array into a unique key (e.g., "1,0,0,...")
            string key = string.Join(",", count);

            // If this key is new, initialize a new list
            if (!res.ContainsKey(key))
            {
                res[key] = new List<string>();
            }

            // Add the current word into the group
            res[key].Add(s);
        }

        // Return all grouped anagrams as a list of lists
        return res.Values.ToList<List<string>>();
    }
}

/*
Time Complexity: O(n * k)
 - n = number of words, k = average length of a word.
 - For each word, we count characters (O(k)) and build a key.

Space Complexity: O(n * k)
 - Dictionary stores all words, and keys take O(26) → O(1) per word,
   but storing the words themselves makes it O(n * k) overall.
*/
