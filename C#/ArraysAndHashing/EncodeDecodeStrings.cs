/*
PROBLEM: Encode and Decode Strings
Design an algorithm to encode a list of strings to a string. The encoded string 
is then sent over the network and is decoded back to the original list of strings.

Machine 1 (sender) has the function:
string encode(vector<string> strs) {
  // ... your code
  return encoded_string;
}

Machine 2 (receiver) has the function:
vector<string> decode(string s) {
  // ... your code
  return strs;
}

So Machine 1 does: string encoded_string = encode(strs);
and Machine 2 does: vector<string> strs2 = decode(encoded_string);
strs2 in Machine 2 should be the same as strs in Machine 1.

Implement the encode and decode methods.

Example 1:
Input: dummy_input = ["Hello","World"]
Output: ["Hello","World"]
Explanation: 
Machine 1: Codec encoder = new Codec(); encoder.encode(strs); // can be "5#Hello5#World"
Machine 2: Codec decoder = new Codec(); decoder.decode(s); // return ["Hello","World"]

Example 2:
Input: dummy_input = [""]
Output: [""]

Note:
- The string may contain any possible characters out of 256 valid ascii characters. 
- Your algorithm should be generalized enough to work on any possible characters.
- Do not use class member/global/static variables to store states. Your encode and decode algorithms should be stateless.
- Do not rely on any library method such as eval or serialize methods. You should implement your own encode/decode algorithm.
*/

public class Solution
{
    public string Encode(IList<string> strs)
    {
        string res = ""; // Initialize result string to store encoded data
        foreach (string s in strs) // Loop through each string in the input list
        {
            // Format: "length#string" - prepend each string with its length and delimiter
            res += s.Length + "#" + s;
        }
        return res; // Return the encoded string
    }

    public List<string> Decode(string s)
    {
        List<string> res = new List<string>(); // Initialize result list to store decoded strings
        int i = 0; // Pointer to traverse the encoded string
        
        while (i < s.Length) // Process entire encoded string
        {
            int j = i; // Start position to find the length number
            
            while (s[j] != '#') // Find the '#' delimiter
            {
                j++; // Move forward until we hit '#'
            }
            
            // Extract and parse the length number before '#'
            int length = int.Parse(s.Substring(i, j - i));
            
            i = j + 1; // Move past the '#' to start of actual string
            j = i + length; // Calculate end position of current string
            
            // Extract the string using the length we found
            res.Add(s.Substring(i, length));
            
            i = j; // Move to start of next encoded string
        }
        return res; // Return the list of decoded strings
    }
}

/*
TIME COMPLEXITY:
- Encode: O(n * m) where n = number of strings, m = average string length
  - We iterate through each string once and concatenate (string concatenation in C# creates new objects)
- Decode: O(k) where k = length of encoded string
  - We traverse the encoded string once, parsing lengths and extracting substrings

SPACE COMPLEXITY:
- Encode: O(k) where k = total length of all strings combined
  - We create a result string that stores all input strings plus their length prefixes
- Decode: O(k) where k = total length of all original strings
  - We create a result list containing all the decoded strings

OPTIMIZATION NOTE:
- The Encode method uses string concatenation which creates new string objects each time
- For better performance, consider using StringBuilder for the Encode method
*/