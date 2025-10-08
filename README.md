# 🚀 LeetCode 150 - Problem Solutions

A comprehensive collection of solutions to the **NeetCode 150** problem set, implemented in **C#**. This repository contains well-commented, optimized solutions with detailed explanations of algorithms, time/space complexity analysis, and problem-solving strategies.

## 📚 About This Repository

This repository follows the [NeetCode 150](https://neetcode.io/practice) curated list of essential coding interview problems. Each solution includes:

- ✅ **Detailed inline comments** explaining every step
- ⏱️ **Time and space complexity analysis**
- 📝 **Problem description and examples**
- 🎯 **Key insights and algorithm explanations**
- 💡 **Edge cases and optimization notes**

## 🎯 Progress Tracker

**Total Problems Solved: 32/150**

| Category | Problems Solved | Total Problems |
|----------|----------------|----------------|
| Arrays & Hashing | 9 | 9 |
| Two Pointers | 5 | 5 |
| Sliding Window | 6 | 6 |
| Stack | 8 | 8 |
| Binary Search | 0 | 7 |
| Linked List | 0 | 6 |
| Trees | 0 | 15 |
| Tries | 0 | 3 |
| Heap / Priority Queue | 0 | 3 |
| Backtracking | 0 | 9 |
| Graphs | 0 | 13 |
| Advanced Graphs | 0 | 6 |
| 1-D Dynamic Programming | 0 | 12 |
| 2-D Dynamic Programming | 0 | 11 |
| Greedy | 0 | 8 |
| Intervals | 0 | 6 |
| Math & Geometry | 0 | 8 |
| Bit Manipulation | 0 | 7 |

## 📂 Repository Structure

```
LeetCode150/
├── ArraysAndHashing/
│   ├── ContainsDuplicate.cs
│   ├── ValidAnagram.cs
│   ├── TwoSum.cs
│   ├── GroupAnagrams.cs
│   ├── TopKFrequentElements.cs
│   ├── EncodeDecodeStrings.cs
│   ├── ProductExceptSelf.cs
│   ├── ValidSudoku.cs
│   └── LongestConsecutiveSequence.cs
├── TwoPointers/
│   ├── ValidPalindrome.cs
│   ├── TwoSumII.cs
│   ├── ThreeSum.cs
│   ├── ContainerWithMostWater.cs
│   └── TrappingRainWater.cs
├── SlidingWindow/
│   ├── BestTimeToBuySellStock.cs
│   ├── LongestSubstringWithoutRepeating.cs
│   ├── LongestRepeatingCharacterReplacement.cs
│   ├── PermutationInString.cs
│   ├── MinimumWindowSubstring.cs
│   └── SlidingWindowMaximum.cs
├── Stack/
│   ├── ValidParentheses.cs
│   ├── MinStack.cs
│   ├── EvaluateReversePolishNotation.cs
│   ├── GenerateParentheses.cs
│   ├── DailyTemperatures.cs
│   ├── CarFleet.cs
│   └── LargestRectangleInHistogram.cs
└── README.md
```

---

## 📖 Problem Solutions

### 1️⃣ Arrays & Hashing (9/9)

| # | Problem | Difficulty | Solution | Topics |
|---|---------|------------|----------|--------|
| 1 | [Contains Duplicate](https://leetcode.com/problems/contains-duplicate/) | 🟢 Easy | [C#](./ArraysAndHashing/ContainsDuplicate.cs) | Hash Set |
| 2 | [Valid Anagram](https://leetcode.com/problems/valid-anagram/) | 🟢 Easy | [C#](./ArraysAndHashing/ValidAnagram.cs) | Hash Map, Sorting |
| 3 | [Two Sum](https://leetcode.com/problems/two-sum/) | 🟢 Easy | [C#](./ArraysAndHashing/TwoSum.cs) | Hash Map |
| 4 | [Group Anagrams](https://leetcode.com/problems/group-anagrams/) | 🟡 Medium | [C#](./ArraysAndHashing/GroupAnagrams.cs) | Hash Map, Sorting |
| 5 | [Top K Frequent Elements](https://leetcode.com/problems/top-k-frequent-elements/) | 🟡 Medium | [C#](./ArraysAndHashing/TopKFrequentElements.cs) | Bucket Sort, Heap |
| 6 | [Encode and Decode Strings](https://leetcode.com/problems/encode-and-decode-strings/) | 🟡 Medium | [C#](./ArraysAndHashing/EncodeDecodeStrings.cs) | String Manipulation |
| 7 | [Product of Array Except Self](https://leetcode.com/problems/product-of-array-except-self/) | 🟡 Medium | [C#](./ArraysAndHashing/ProductExceptSelf.cs) | Prefix/Suffix Product |
| 8 | [Valid Sudoku](https://leetcode.com/problems/valid-sudoku/) | 🟡 Medium | [C#](./ArraysAndHashing/ValidSudoku.cs) | Hash Set, Matrix |
| 9 | [Longest Consecutive Sequence](https://leetcode.com/problems/longest-consecutive-sequence/) | 🟡 Medium | [C#](./ArraysAndHashing/LongestConsecutiveSequence.cs) | Hash Set, Union Find |

**Key Patterns:**
- Hash Map for O(1) lookups
- Frequency counting and bucketing
- Array manipulation without division

---

### 2️⃣ Two Pointers (5/5)

| # | Problem | Difficulty | Solution | Topics |
|---|---------|------------|----------|--------|
| 10 | [Valid Palindrome](https://leetcode.com/problems/valid-palindrome/) | 🟢 Easy | [C#](./TwoPointers/ValidPalindrome.cs) | Two Pointers, String |
| 11 | [Two Sum II](https://leetcode.com/problems/two-sum-ii-input-array-is-sorted/) | 🟡 Medium | [C#](./TwoPointers/TwoSumII.cs) | Two Pointers, Binary Search |
| 12 | [3Sum](https://leetcode.com/problems/3sum/) | 🟡 Medium | [C#](./TwoPointers/ThreeSum.cs) | Two Pointers, Sorting |
| 13 | [Container With Most Water](https://leetcode.com/problems/container-with-most-water/) | 🟡 Medium | [C#](./TwoPointers/ContainerWithMostWater.cs) | Two Pointers, Greedy |
| 14 | [Trapping Rain Water](https://leetcode.com/problems/trapping-rain-water/) | 🔴 Hard | [C#](./TwoPointers/TrappingRainWater.cs) | Two Pointers, Dynamic Programming |

**Key Patterns:**
- Opposite direction pointers
- Sorted array optimization
- Greedy decision making

---

### 3️⃣ Sliding Window (6/6)

| # | Problem | Difficulty | Solution | Topics |
|---|---------|------------|----------|--------|
| 15 | [Best Time to Buy and Sell Stock](https://leetcode.com/problems/best-time-to-buy-and-sell-stock/) | 🟢 Easy | [C#](./SlidingWindow/BestTimeToBuySellStock.cs) | Sliding Window, Greedy |
| 16 | [Longest Substring Without Repeating Characters](https://leetcode.com/problems/longest-substring-without-repeating-characters/) | 🟡 Medium | [C#](./SlidingWindow/LongestSubstringWithoutRepeating.cs) | Sliding Window, Hash Set |
| 17 | [Longest Repeating Character Replacement](https://leetcode.com/problems/longest-repeating-character-replacement/) | 🟡 Medium | [C#](./SlidingWindow/LongestRepeatingCharacterReplacement.cs) | Sliding Window, Hash Map |
| 18 | [Permutation in String](https://leetcode.com/problems/permutation-in-string/) | 🟡 Medium | [C#](./SlidingWindow/PermutationInString.cs) | Sliding Window, Hash Map |
| 19 | [Minimum Window Substring](https://leetcode.com/problems/minimum-window-substring/) | 🔴 Hard | [C#](./SlidingWindow/MinimumWindowSubstring.cs) | Sliding Window, Hash Map |
| 20 | [Sliding Window Maximum](https://leetcode.com/problems/sliding-window-maximum/) | 🔴 Hard | [C#](./SlidingWindow/SlidingWindowMaximum.cs) | Sliding Window, Deque, Monotonic Queue |

**Key Patterns:**
- Variable/fixed window size
- Hash map for character frequency
- Expand and shrink technique

---

### 4️⃣ Stack (8/8)

| # | Problem | Difficulty | Solution | Topics |
|---|---------|------------|----------|--------|
| 21 | [Valid Parentheses](https://leetcode.com/problems/valid-parentheses/) | 🟢 Easy | [C#](./Stack/ValidParentheses.cs) | Stack, String |
| 22 | [Min Stack](https://leetcode.com/problems/min-stack/) | 🟡 Medium | [C#](./Stack/MinStack.cs) | Stack, Design |
| 23 | [Evaluate Reverse Polish Notation](https://leetcode.com/problems/evaluate-reverse-polish-notation/) | 🟡 Medium | [C#](./Stack/EvaluateReversePolishNotation.cs) | Stack, Math |
| 24 | [Generate Parentheses](https://leetcode.com/problems/generate-parentheses/) | 🟡 Medium | [C#](./Stack/GenerateParentheses.cs) | Stack, Backtracking |
| 25 | [Daily Temperatures](https://leetcode.com/problems/daily-temperatures/) | 🟡 Medium | [C#](./Stack/DailyTemperatures.cs) | Stack, Monotonic Stack |
| 26 | [Car Fleet](https://leetcode.com/problems/car-fleet/) | 🟡 Medium | [C#](./Stack/CarFleet.cs) | Stack, Sorting |
| 27 | [Largest Rectangle in Histogram](https://leetcode.com/problems/largest-rectangle-in-histogram/) | 🔴 Hard | [C#](./Stack/LargestRectangleInHistogram.cs) | Stack, Monotonic Stack |
| 28 | [Online Stock Span](https://leetcode.com/problems/online-stock-span/) | 🟡 Medium | [C#](./Stack/OnlineStockSpan.cs) | Stack, Monotonic Stack, Design |

**Key Patterns:**
- LIFO operations
- Monotonic stack for next greater/smaller element
- Backtracking with stack

---

## 🛠️ Technologies Used

- **Language:** C# (.NET 6.0+)
- **IDE:** Visual Studio / Visual Studio Code / Rider
- **Version Control:** Git & GitHub

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- Any C# IDE (Visual Studio, VS Code, Rider)

### Clone the Repository

```bash
git clone https://github.com/yourusername/leetcode-150.git
cd leetcode-150
```

### Running Solutions

Each solution is a self-contained C# class. You can:

1. **Copy individual solutions** into your own project
2. **Create a console application** and call the solution methods
3. **Use LeetCode's online judge** to test the solutions

Example:
```csharp
public class Program
{
    public static void Main()
    {
        var solution = new Solution();
        int[] nums = { 2, 7, 11, 15 };
        int target = 9;
        int[] result = solution.TwoSum(nums, target);
        Console.WriteLine($"[{result[0]}, {result[1]}]");
    }
}
```

## 💡 Solution Features

Each solution file includes:

### 1. Problem Description
```csharp
/*
PROBLEM: Two Sum
Given an array of integers nums and an integer target, return indices 
of the two numbers such that they add up to target.
...
*/
```

### 2. Detailed Comments
```csharp
// Calculate the complement needed to reach target
int complement = target - nums[i];
```

### 3. Complexity Analysis
```csharp
/*
TIME COMPLEXITY: O(n)
- Single pass through array
- Hash map operations are O(1)

SPACE COMPLEXITY: O(n)
- Hash map stores up to n elements
*/
```

### 4. Algorithm Explanation
- Key insights and intuition
- Step-by-step approach
- Edge cases handled

## 📈 Learning Resources

- [NeetCode.io](https://neetcode.io/) - Problem list and video explanations
- [LeetCode](https://leetcode.com/) - Practice platform
- [Big-O Cheat Sheet](https://www.bigocheatsheet.com/) - Algorithm complexity reference

## 🤝 Contributing

While this is a personal learning repository, suggestions and improvements are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/improvement`)
3. Commit your changes (`git commit -am 'Add improvement'`)
4. Push to the branch (`git push origin feature/improvement`)
5. Open a Pull Request

## 📝 Notes

- Solutions prioritize **clarity and understanding** over brevity
- Comments explain **why**, not just **what**
- Each solution includes **alternative approaches** when applicable
- **Time/space complexity** is always analyzed

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Your Name**
- GitHub: [@yourusername](https://github.com/yourusername)
- LinkedIn: [Your LinkedIn](https://linkedin.com/in/yourprofile)
- LeetCode: [@yourusername](https://leetcode.com/yourusername)

## ⭐ Show Your Support

If you found this repository helpful, please give it a ⭐️!

---

**Happy Coding! 💻✨**

*Last Updated: October 2025*