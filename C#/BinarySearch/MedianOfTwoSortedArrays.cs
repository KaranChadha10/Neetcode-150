/*
PROBLEM: Median of Two Sorted Arrays
Given two sorted arrays nums1 and nums2 of size m and n respectively, return the median 
of the two sorted arrays.

The overall run time complexity should be O(log(min(m,n))).

Example 1:
Input: nums1 = [1,3], nums2 = [2]
Output: 2.00000
Explanation: merged array = [1,2,3] and median is 2.

Example 2:
Input: nums1 = [1,2], nums2 = [3,4]
Output: 2.50000
Explanation: merged array = [1,2,3,4] and median is (2 + 3) / 2 = 2.5.

Constraints:
- nums1.length == m
- nums2.length == n
- 0 <= m <= 1000
- 0 <= n <= 1000
- 1 <= m + n <= 2000
- -10^6 <= nums1[i], nums2[i] <= 10^6
*/

public class Solution
{
    public double FindMedianSortedArrays(int[] nums1, int[] nums2)
    {
        int[] A = nums1; // First array
        int[] B = nums2; // Second array
        int total = A.Length + B.Length; // Total number of elements
        int half = (total + 1) / 2; // Number of elements in left partition (ceiling division)

        // Ensure A is the smaller array for optimization (binary search on smaller array)
        if (B.Length < A.Length)
        {
            int[] temp = A;
            A = B;
            B = temp;
        }

        int l = 0; // Left boundary: minimum elements we can take from A
        int r = A.Length; // Right boundary: maximum elements we can take from A
        
        while (l <= r) // Binary search on number of elements to take from A
        {
            int i = (l + r) / 2; // Number of elements from A in left partition
            int j = half - i; // Number of elements from B in left partition

            // Get boundary elements from both arrays
            // Use int.MinValue/MaxValue for out-of-bounds (edge cases)
            int Aleft = i > 0 ? A[i - 1] : int.MinValue; // Rightmost element of A's left partition
            int Aright = i < A.Length ? A[i] : int.MaxValue; // Leftmost element of A's right partition
            int Bleft = j > 0 ? B[j - 1] : int.MinValue; // Rightmost element of B's left partition
            int Bright = j < B.Length ? B[j] : int.MaxValue; // Leftmost element of B's right partition

            // Check if we found the correct partition
            if (Aleft <= Bright && Bleft <= Aright)
            {
                // Correct partition found!
                // All elements in left partition ≤ all elements in right partition
                
                if (total % 2 != 0) // Odd total: median is max of left partition
                {
                    return Math.Max(Aleft, Bleft);
                }
                // Even total: median is average of max(left) and min(right)
                return (Math.Max(Aleft, Bleft) + Math.Min(Aright, Bright)) / 2.0;
            }
            else if (Aleft > Bright) // Too many elements from A in left partition
            {
                r = i - 1; // Take fewer elements from A
            }
            else // Too few elements from A in left partition (Bleft > Aright)
            {
                l = i + 1; // Take more elements from A
            }
        }
        return -1; // Should never reach here with valid input
    }
}

/*
TIME COMPLEXITY: O(log(min(m,n)))
- Binary search on the smaller array (A)
- Each iteration: O(1) operations
- Number of iterations: log(length of smaller array)
- Significantly better than O(m+n) merge approach
- Overall: O(log(min(m,n))) - required by problem

SPACE COMPLEXITY: O(1)
- Only using constant extra space for variables
- No additional arrays or data structures
- Array swapping is just reference swap, not copying
- Space usage independent of input size

ALGORITHM EXPLANATION:
- Use binary search to find the correct partition point
- Partition both arrays such that:
  1. Left partition has exactly half (rounded up) total elements
  2. All elements in left ≤ all elements in right
- Median is derived from boundary elements of partitions
- Search on smaller array for efficiency

KEY INSIGHTS:
- Don't merge arrays - find partition directly
- Median separates lower and upper half of sorted sequence
- If we partition both arrays correctly:
  - Left partition: first half of merged array
  - Right partition: second half of merged array
- Median is at the boundary of these partitions

PARTITION CONCEPT:
Array A: [a1, a2, | a3, a4]  (i=2 elements in left)
Array B: [b1, b2, b3, | b4, b5, b6]  (j=3 elements in left)

Left partition: {a1, a2, b1, b2, b3}  (5 elements)
Right partition: {a3, a4, b4, b5, b6}  (5 elements)

Valid partition condition:
- max(a2, b3) ≤ min(a3, b4)
- i.e., Aleft ≤ Bright AND Bleft ≤ Aright

WHY BINARY SEARCH ON SMALLER ARRAY:
- Fewer iterations needed
- j = half - i must be valid (j ≥ 0 and j ≤ B.Length)
- Searching on smaller array ensures j stays in bounds
- Optimization from O(log(max(m,n))) to O(log(min(m,n)))

PARTITION SIZE CALCULATION:
- total = m + n
- half = (total + 1) / 2  (ceiling division)
- For odd total: left has one more element
- For even total: left and right have equal elements
- Using (total + 1) / 2 works for both cases

BOUNDARY ELEMENT HANDLING:
- i=0: No elements from A in left → Aleft = int.MinValue
- i=A.Length: All A elements in left → Aright = int.MaxValue
- j=0: No elements from B in left → Bleft = int.MinValue
- j=B.Length: All B elements in left → Bright = int.MaxValue
- These sentinels ensure comparisons work at boundaries

MEDIAN CALCULATION:
Odd total (e.g., 5 elements):
- Left has 3, right has 2
- Median = max(Aleft, Bleft)  (rightmost of left partition)

Even total (e.g., 6 elements):
- Left has 3, right has 3
- Median = (max(Aleft, Bleft) + min(Aright, Bright)) / 2
- Average of rightmost left and leftmost right

EXAMPLE TRACE with nums1 = [1,3], nums2 = [2]:
Setup:
- A = [1,3], B = [2] (A is smaller, no swap)
- total = 3, half = 2
- l = 0, r = 2

Iteration 1:
- i = (0+2)/2 = 1  (take 1 element from A)
- j = 2-1 = 1  (take 1 element from B)
- Aleft = A[0] = 1, Aright = A[1] = 3
- Bleft = B[0] = 2, Bright = int.MaxValue
- Check: 1 ≤ MaxValue? Yes, 2 ≤ 3? Yes ✓
- Valid partition found!
- Odd total: return max(1, 2) = 2.0 ✓

Merged view: [1, 2 | 3]
             └─┬─┘
          Left partition (size 2)

EXAMPLE TRACE with nums1 = [1,2], nums2 = [3,4]:
Setup:
- A = [1,2], B = [3,4] (same size, A stays)
- total = 4, half = 2
- l = 0, r = 2

Iteration 1:
- i = (0+2)/2 = 1  (take 1 element from A)
- j = 2-1 = 1  (take 1 element from B)
- Aleft = A[0] = 1, Aright = A[1] = 2
- Bleft = B[0] = 3, Bright = B[1] = 4
- Check: 1 ≤ 4? Yes, 3 ≤ 2? No ✗
- Bleft > Aright → need more elements from A
- l = 1+1 = 2

Iteration 2:
- l = 2, r = 2
- i = (2+2)/2 = 2  (take 2 elements from A)
- j = 2-2 = 0  (take 0 elements from B)
- Aleft = A[1] = 2, Aright = int.MaxValue
- Bleft = int.MinValue, Bright = B[0] = 3
- Check: 2 ≤ 3? Yes, MinValue ≤ MaxValue? Yes ✓
- Valid partition found!
- Even total: return (max(2, MinValue) + min(MaxValue, 3)) / 2 = (2+3)/2 = 2.5 ✓

Merged view: [1, 2 | 3, 4]
             └─┬─┘   └─┬─┘
          Left (2)  Right (2)

PARTITION CORRECTNESS CONDITIONS:
1. Aleft ≤ Bright (left of A ≤ right of B)
2. Bleft ≤ Aright (left of B ≤ right of A)
Both must be true for valid partition

ADJUSTMENT LOGIC:
- If Aleft > Bright: too many elements from A → decrease i
- If Bleft > Aright: too few elements from A → increase i

EDGE CASES HANDLED:
- Empty array: swapping ensures we search on non-empty one
- All elements from one array in one partition: sentinels handle it
- Arrays of different sizes: j calculation handles it
- Single element arrays: works correctly
- Duplicate elements: comparison logic handles them

WHY THIS IS HARD:
- Requires thinking in terms of partitions, not merging
- Boundary cases need careful handling
- Off-by-one errors are easy to make
- Need to understand median for odd/even totals

PATTERN RECOGNITION:
- "Median of sorted arrays" → partition-based binary search
- "O(log(m+n))" requirement → can't merge, must use binary search
- Two sorted arrays → binary search on one, derive other position

ALTERNATIVE APPROACHES:
1. Merge both arrays: O(m+n) time, O(m+n) space ✗ Too slow
2. Two pointers merge until middle: O(m+n) time, O(1) space ✗ Too slow
3. Binary search on partition (this): O(log(min(m,n))) ✓ Optimal
*/