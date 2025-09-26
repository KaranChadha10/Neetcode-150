/*
PROBLEM: Longest Substring Without Repeating Characters
Given a string s, find the length of the longest substring without repeating characters.

Example 1:
Input: s = "abcabcbb"
Output: 3
Explanation: The answer is "abc", with the length of 3.

Example 2:
Input: s = "bbbbb"
Output: 1
Explanation: The answer is "b", with the length of 1.

Example 3:
Input: s = "pwwkew"
Output: 3
Explanation: The answer is "wke", with the length of 3.
Notice that the answer must be a substring, "pwke" is a subsequence and not a substring.

Constraints:
- 0 <= s.length <= 5 * 10^4
- s consists of English letters, digits, symbols and spaces.
*/

public class Solution
{
    public int LengthOfLongestSubstring(string s)
    {
        HashSet<char> charSet = new HashSet<char>(); // Track characters in current window
        int l = 0; // Left pointer of sliding window
        int res = 0; // Track maximum length found

        for (int r = 0; r < s.Length; r++) // Right pointer expands the window
        {
            // Shrink window from left while duplicate character exists
            while (charSet.Contains(s[r]))
            {
                charSet.Remove(s[l]); // Remove leftmost character from set
                l++; // Move left pointer forward
            }
            
            charSet.Add(s[r]); // Add current character to set
            res = Math.Max(res, r - l + 1); // Update maximum length if current window is larger
        }
        return res; // Return length of longest substring without repeating characters
    }
}

/*
TIME COMPLEXITY: O(n)
- Right pointer (r) traverses the string once: O(n)
- Left pointer (l) moves at most n positions throughout the entire algorithm
- Each character is added and removed from HashSet at most once
- HashSet operations (Contains, Add, Remove) are O(1) on average
- Overall: O(n) where n is the length of the input string

SPACE COMPLEXITY: O(min(m, n))
- HashSet stores at most min(m, n) characters
- Where m is the size of character set (e.g., 128 for ASCII) and n is string length
- In worst case (all unique characters), we store all characters in the string
- Best case: O(1) if all characters are the same

ALGORITHM EXPLANATION:
- Sliding window technique with two pointers (left and right)
- Expand window by moving right pointer and adding characters to set
- When duplicate found, shrink window from left until duplicate is removed
- Track maximum window size throughout the process

KEY INSIGHTS:
- Use HashSet for O(1) duplicate detection
- Sliding window maintains the invariant: no duplicate characters in current window
- We don't need to restart from scratch when duplicate found - just shrink from left
- Right pointer never goes backward, left pointer never goes backward

SLIDING WINDOW TECHNIQUE:
- Expand: Add s[r] to window if it doesn't create duplicates
- Shrink: Remove s[l] from window until no duplicates exist
- Track: Keep maximum window size seen so far

EXAMPLE TRACE with "abcabcbb":
- r=0: "a", charSet={a}, window="a", maxLen=1
- r=1: "b", charSet={a,b}, window="ab", maxLen=2  
- r=2: "c", charSet={a,b,c}, window="abc", maxLen=3
- r=3: "a" duplicate! Remove s[l=0]='a', l=1, add "a", window="bca", maxLen=3
- r=4: "b" duplicate! Remove s[l=1]='b', l=2, add "b", window="cab", maxLen=3
- r=5: "c" duplicate! Remove s[l=2]='c', l=3, add "c", window="abc", maxLen=3
- r=6: "b" duplicate! Remove s[l=3]='a', Remove s[l=4]='b', l=5, add "b", window="cb", maxLen=3
- r=7: "b" duplicate! Remove s[l=5]='c', l=6, add "b", window="bb", maxLen=3
- Result: 3

WHY SLIDING WINDOW WORKS:
- Any substring containing duplicates cannot be optimal
- When we find duplicate, we can safely skip all substrings starting before the first occurrence
- This avoids checking all possible substrings (which would be O(n³))
*/