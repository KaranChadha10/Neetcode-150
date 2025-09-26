/*
PROBLEM: Longest Repeating Character Replacement
You are given a string s and an integer k. You can choose any character of the string and 
change it to any other uppercase English letter. You can perform this operation at most k times.

Return the length of the longest substring containing the same letter you can get after 
performing the above operations.

Example 1:
Input: s = "ABAB", k = 2
Output: 4
Explanation: Replace the two 'A's with two 'B's or vice versa.

Example 2:
Input: s = "AABABBA", k = 1
Output: 4
Explanation: Replace the one 'A' in the middle with 'B' and form "AABBBBA".
The substring "BBBB" has the longest repeating letters, which is 4.

Constraints:
- 1 <= s.length <= 10^5
- s consists of only uppercase English letters.
- 0 <= k <= s.length
*/

public class Solution
{
    public int CharacterReplacement(string s, int k)
    {
        int res = 0; // Track maximum valid substring length found
        
        // Try every possible starting position
        for (int i = 0; i < s.Length; i++)
        {
            Dictionary<char, int> count = new Dictionary<char, int>(); // Count characters in current window
            int maxf = 0; // Track frequency of most common character in current window
            
            // Expand window from current starting position
            for (int j = i; j < s.Length; j++)
            {
                // Add current character to frequency count
                if (count.ContainsKey(s[j]))
                {
                    count[s[j]]++; // Increment existing character count
                }
                else
                {
                    count[s[j]] = 1; // Initialize new character count
                }
                
                // Update maximum frequency in current window
                maxf = Math.Max(maxf, count[s[j]]);
                
                // Check if current window is valid
                // Valid window: (window_size - most_frequent_char_count) <= k
                // This means we need at most k replacements to make all characters the same
                if ((j - i + 1) - maxf <= k)
                {
                    res = Math.Max(res, j - i + 1); // Update result if current window is larger
                }
                // Note: If window becomes invalid, we continue to next j 
                // (this could be optimized with sliding window)
            }
        }
        return res; // Return length of longest valid substring
    }
}

/*
TIME COMPLEXITY: O(n²)
- Outer loop runs n times (for each starting position)
- Inner loop runs at most n times for each starting position
- Dictionary operations (ContainsKey, indexing) are O(1) on average
- Overall: O(n²) where n is the length of the input string

SPACE COMPLEXITY: O(1) or O(26) = O(1)
- Dictionary stores at most 26 uppercase English letters
- Since the character set is fixed and small, space is effectively constant
- Space doesn't grow with input size n

ALGORITHM EXPLANATION:
- For each starting position, expand window and track character frequencies
- Key insight: A window is valid if (window_size - max_frequency) <= k
- This means we need at most k character replacements to make all characters identical
- Track the maximum valid window size found across all starting positions

KEY INSIGHTS:
- Valid window condition: replacements_needed = window_size - most_frequent_char <= k
- We want to maximize window_size while keeping replacements_needed <= k
- Most frequent character in window should remain unchanged (optimal strategy)
- All other characters in window would be replaced to match the most frequent one

VALIDATION LOGIC:
- Window size = j - i + 1 (inclusive range)
- Characters to replace = window_size - maxf (all except most frequent)
- Valid if characters_to_replace <= k

EXAMPLE TRACE with s = "AABABBA", k = 1:
Starting at i=0:
- j=0: "A", count={A:1}, maxf=1, window=1, need=0 replacements ✓, res=1
- j=1: "AA", count={A:2}, maxf=2, window=2, need=0 replacements ✓, res=2  
- j=2: "AAB", count={A:2,B:1}, maxf=2, window=3, need=1 replacement ✓, res=3
- j=3: "AABA", count={A:3,B:1}, maxf=3, window=4, need=1 replacement ✓, res=4
- j=4: "AABAB", count={A:3,B:2}, maxf=3, window=5, need=2 replacements ✗
Continue with other starting positions...
Final result: 4

OPTIMIZATION NOTE:
- This solution checks all possible substrings starting from each position
- A more efficient O(n) sliding window approach exists using two pointers
- Current approach is simpler to understand but less optimal for large inputs
*/