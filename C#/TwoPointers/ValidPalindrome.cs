/*
PROBLEM: Valid Palindrome
A phrase is a palindrome if, after converting all uppercase letters into lowercase letters 
and removing all non-alphanumeric characters, it reads the same forward and backward.

Given a string s, return true if it is a palindrome, or false otherwise.

Example 1:
Input: s = "A man, a plan, a canal: Panama"
Output: true
Explanation: "amanaplanacanalpanama" is a palindrome.

Example 2:
Input: s = "race a car"
Output: false
Explanation: "raceacar" is not a palindrome.

Example 3:
Input: s = " "
Output: true
Explanation: s is an empty string "" after removing non-alphanumeric characters.
Since an empty string reads the same forward and backward, it is a palindrome.

Constraints:
- 1 <= s.length <= 2 * 10^5
- s consists only of printable ASCII characters.
*/

public class Solution
{
    public bool IsPalindrome(string s)
    {
        int l = 0, r = s.Length - 1; // Two pointers: left starts at beginning, right at end

        while (l < r) // Continue until pointers meet in the middle
        {
            // Skip non-alphanumeric characters from left side
            while (l < r && !AlphaNum(s[l]))
            {
                l++; // Move left pointer forward
            }
            
            // Skip non-alphanumeric characters from right side
            while (r > l && !AlphaNum(s[r]))
            {
                r--; // Move right pointer backward
            }
            
            // Compare characters (convert to lowercase for case-insensitive comparison)
            if (char.ToLower(s[l]) != char.ToLower(s[r]))
            {
                return false; // Characters don't match - not a palindrome
            }
            
            l++; // Move left pointer forward to next character
            r--; // Move right pointer backward to next character
        }
        return true; // All characters matched - it's a palindrome
    }

    // Helper method to check if a character is alphanumeric (letter or digit)
    public bool AlphaNum(char c)
    {
        return (c >= 'A' && c <= 'Z' ||  // Uppercase letters
                c >= 'a' && c <= 'z' ||  // Lowercase letters  
                c >= '0' && c <= '9');   // Digits
    }
}

/*
TIME COMPLEXITY: O(n)
- We traverse the string at most once with two pointers
- Each character is visited at most once by either left or right pointer
- The AlphaNum helper method runs in O(1) constant time
- Overall: O(n) where n is the length of the input string

SPACE COMPLEXITY: O(1)
- Only using constant extra space for the two pointers (l, r)
- No additional data structures or string manipulation needed
- The AlphaNum method uses no extra space

ALGORITHM EXPLANATION:
- Two-pointer approach: start from both ends and move towards center
- Skip non-alphanumeric characters on both sides
- Compare characters in case-insensitive manner
- If any pair doesn't match, it's not a palindrome
- If we successfully compare all valid characters, it's a palindrome

KEY INSIGHTS:
- We don't need to create a new cleaned string, saving space
- Two-pointer technique allows us to check palindrome property efficiently
- Case-insensitive comparison handled with char.ToLower()
- Custom AlphaNum method avoids dependency on built-in char methods
*/