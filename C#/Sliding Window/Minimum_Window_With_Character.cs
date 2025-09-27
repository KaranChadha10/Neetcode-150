/*
PROBLEM: Minimum Window Substring
Given two strings s and t of lengths m and n respectively, return the minimum window 
substring of s such that every character in t (including duplicates) is included in the window. 
If there is no such window, return the empty string "".

The testcases will be generated such that the answer is unique.

Example 1:
Input: s = "ADOBECODEBANC", t = "ABC"
Output: "BANC"
Explanation: The minimum window substring "BANC" includes 'A', 'B', and 'C' from string t.

Example 2:
Input: s = "a", t = "a"
Output: "a"
Explanation: The entire string s is the minimum window.

Example 3:
Input: s = "a", t = "aa"
Output: ""
Explanation: Both 'a's from t must be included in the window.
Since the largest window of s only has one 'a', return empty string.

Constraints:
- m == s.length
- n == t.length
- 1 <= m, n <= 10^5
- s and t consist of uppercase and lowercase English letters.

Follow up: Could you find an algorithm that runs in O(m + n) time?
*/

public class Solution
{
    public string MinWindow(string s, string t)
    {
        if (t == "") return ""; // Edge case: empty target string

        // Count frequency of each character in target string t
        Dictionary<char, int> countT = new Dictionary<char, int>();
        Dictionary<char, int> window = new Dictionary<char, int>(); // Track characters in current window

        // Build frequency map for target string t
        foreach (char c in t)
        {
            if (countT.ContainsKey(c))
            {
                countT[c]++; // Increment count for existing character
            }
            else
            {
                countT[c] = 1; // Initialize count for new character
            }
        }

        int have = 0, need = countT.Count; // have = characters with satisfied frequency, need = total unique characters in t
        int[] res = { -1, -1 }; // Store indices of minimum window [left, right]
        int resLen = int.MaxValue; // Track minimum window length found
        int l = 0; // Left pointer of sliding window

        // Expand window with right pointer
        for (int r = 0; r < s.Length; r++)
        {
            char c = s[r]; // Current character being added to window
            
            // Add character to window frequency count
            if (window.ContainsKey(c))
            {
                window[c]++; // Increment count for existing character
            }
            else
            {
                window[c] = 1; // Initialize count for new character
            }

            // Check if this character's frequency requirement is now satisfied
            if (countT.ContainsKey(c) && window[c] == countT[c])
            {
                have++; // One more character type has satisfied frequency
            }

            // Try to shrink window from left while maintaining valid window
            while (have == need) // Window contains all required characters with correct frequencies
            {
                // Update minimum window if current window is smaller
                if ((r - l + 1) < resLen)
                {
                    resLen = r - l + 1; // Update minimum length
                    res[0] = l; // Update left boundary
                    res[1] = r; // Update right boundary
                }

                // Try to shrink window by removing leftmost character
                char leftChar = s[l];
                window[leftChar]--; // Decrement count for character being removed
                
                // Check if removing this character breaks the frequency requirement
                if (countT.ContainsKey(leftChar) && window[leftChar] < countT[leftChar])
                {
                    have--; // This character type no longer satisfies frequency requirement
                }
                l++; // Move left pointer forward (shrink window)
            }
        }
        
        // Return minimum window substring or empty string if no valid window found
        return resLen == int.MaxValue ? "" : s.Substring(res[0], resLen);
    }
}

/*
TIME COMPLEXITY: O(m + n)
- Building countT: O(n) where n = length of string t
- Sliding window: O(m) where m = length of string s
  - Right pointer visits each character once: O(m)
  - Left pointer moves at most m positions throughout entire algorithm: O(m)
  - Dictionary operations are O(1) on average
- Overall: O(m + n) which achieves the follow-up requirement

SPACE COMPLEXITY: O(m + n)
- countT dictionary: O(k) where k = unique characters in t, worst case O(n)
- window dictionary: O(k) where k = unique characters in s, worst case O(m)
- Other variables: O(1)
- Overall: O(m + n) in worst case

ALGORITHM EXPLANATION:
- Sliding window technique with variable window size
- Expand window until it contains all required characters
- Then shrink window while maintaining validity to find minimum size
- Track best (minimum) valid window found throughout the process

KEY INSIGHTS:
- Use 'have' and 'need' counters to efficiently track window validity
- have = number of character types that satisfy frequency requirements
- need = total number of unique character types in target string
- Window is valid when have == need (all character frequencies satisfied)

SLIDING WINDOW STRATEGY:
- Expand: Add characters to window until valid (have == need)
- Shrink: Remove characters from left while maintaining validity
- Track: Keep minimum valid window throughout the process

EXAMPLE TRACE with s = "ADOBECODEBANC", t = "ABC":
- countT = {A:1, B:1, C:1}, need = 3
- Expand until window contains A, B, C with correct frequencies
- Window "ADOBEC" is first valid window (have = 3)
- Shrink from left: "DOBEC" still valid, "OBEC" not valid
- Continue expanding and shrinking...
- Final minimum window: "BANC" (length 4)

EDGE CASES HANDLED:
- No valid window exists: return ""
- Target string is empty: return ""
- Entire string s is the minimum window: correctly identified
- Target has duplicate characters: frequency tracking handles this

OPTIMIZATION BENEFITS:
- Avoids checking all possible substrings (O(m²) approach)
- Each character processed at most twice (once by each pointer)
- Efficient tracking with have/need counters instead of comparing dictionaries
*/