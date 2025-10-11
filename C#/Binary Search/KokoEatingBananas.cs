/*
PROBLEM: Koko Eating Bananas
Koko loves to eat bananas. There are n piles of bananas, the ith pile has piles[i] bananas. 
The guards have gone and will come back in h hours.

Koko can decide her bananas-per-hour eating speed of k. Each hour, she chooses some pile of 
bananas and eats k bananas from that pile. If the pile has less than k bananas, she eats all 
of them instead and will not eat any more bananas during this hour.

Koko likes to eat slowly but still wants to finish eating all the bananas before the guards return.

Return the minimum integer k such that she can eat all the bananas within h hours.

Example 1:
Input: piles = [3,6,7,11], h = 8
Output: 4
Explanation: With eating speed 4, she can finish all bananas in 8 hours.

Example 2:
Input: piles = [30,11,23,4,20], h = 5
Output: 30
Explanation: With eating speed 30, she finishes each pile in exactly 1 hour.

Example 3:
Input: piles = [30,11,23,4,20], h = 6
Output: 23

Constraints:
- 1 <= piles.length <= 10^4
- piles.length <= h <= 10^9
- 1 <= piles[i] <= 10^9
*/

public class Solution
{
    public int MinEatingSpeed(int[] piles, int h)
    {
        int l = 1; // Minimum possible eating speed (1 banana per hour)
        int r = piles.Max(); // Maximum possible speed (largest pile in one hour)
        int res = r; // Track the minimum valid eating speed found

        while (l <= r) // Binary search on eating speed
        {
            int k = (l + r) / 2; // Current eating speed to test

            // Calculate total hours needed at eating speed k
            long totalTime = 0; // Use long to prevent overflow with large values
            foreach (int p in piles)
            {
                // Time to eat pile p at speed k (round up using ceiling)
                totalTime += (int)Math.Ceiling((double)p / k);
            }
            
            // Check if current speed k allows finishing within h hours
            if (totalTime <= h)
            {
                res = k; // k is valid, update result
                r = k - 1; // Try to find a smaller valid speed (search left)
            }
            else
            {
                l = k + 1; // k is too slow, need faster speed (search right)
            }
        }
        return res; // Return minimum eating speed that works
    }
}

/*
TIME COMPLEXITY: O(n * log(m))
- Binary search on speed range [1, max(piles)]: O(log m) iterations where m = max pile
- For each speed, calculate total time by iterating all piles: O(n) where n = number of piles
- Overall: O(n * log m)

SPACE COMPLEXITY: O(1)
- Only using constant extra space for variables (l, r, res, k, totalTime)
- piles.Max() is O(n) operation but doesn't use extra space
- No additional data structures used

ALGORITHM EXPLANATION:
- Binary search on the answer space (eating speeds from 1 to max pile)
- For each speed k, check if Koko can finish all bananas in h hours
- If yes, try smaller speed (search left for minimum)
- If no, try larger speed (search right)
- Find the minimum k where it's possible to finish in time

KEY INSIGHTS:
- Search space: eating speeds from 1 to max(piles)
- Monotonic property: if speed k works, all speeds > k also work
- Goal: find minimum k that works (leftmost valid speed)
- Time to eat pile p at speed k = ⌈p/k⌉ hours (ceiling division)

BINARY SEARCH ON ANSWER:
- Instead of searching array, we search the "answer space" (possible speeds)
- Valid speeds form a continuous range: [min_valid, max_pile]
- We want the leftmost (minimum) valid speed

CEILING DIVISION LOGIC:
- If pile has 7 bananas and speed is 3:
  - Hour 1: eat 3 (4 remaining)
  - Hour 2: eat 3 (1 remaining)  
  - Hour 3: eat 1
  - Total: 3 hours = ⌈7/3⌉ = ⌈2.33⌉ = 3

EXAMPLE TRACE with piles = [3,6,7,11], h = 8:

Initial: l=1, r=11 (max pile), res=11

Iteration 1: k=6
- Pile 3: ⌈3/6⌉ = 1 hour
- Pile 6: ⌈6/6⌉ = 1 hour
- Pile 7: ⌈7/6⌉ = 2 hours
- Pile 11: ⌈11/6⌉ = 2 hours
- Total: 6 hours ≤ 8 ✓ (valid)
- res = 6, r = 5 (try slower speed)

Iteration 2: k=3
- Pile 3: ⌈3/3⌉ = 1 hour
- Pile 6: ⌈6/3⌉ = 2 hours
- Pile 7: ⌈7/3⌉ = 3 hours
- Pile 11: ⌈11/3⌉ = 4 hours
- Total: 10 hours > 8 ✗ (too slow)
- l = 4 (need faster speed)

Iteration 3: k=4
- Pile 3: ⌈3/4⌉ = 1 hour
- Pile 6: ⌈6/4⌉ = 2 hours
- Pile 7: ⌈7/4⌉ = 2 hours
- Pile 11: ⌈11/4⌉ = 3 hours
- Total: 8 hours ≤ 8 ✓ (valid)
- res = 4, r = 3

Iteration 4: k=3 (already tested, too slow)
- l=4, r=3, l > r → exit

Result: res = 4 ✓

MONOTONIC PROPERTY:
- If speed k works → all speeds > k also work (faster is better)
- If speed k fails → all speeds < k also fail (slower is worse)
- This property allows binary search on answer space

SEARCH SPACE VISUALIZATION:
Speed:     1  2  3  4  5  6  7  8  9  10  11
Works?     ✗  ✗  ✗  ✓  ✓  ✓  ✓  ✓  ✓  ✓   ✓
                    ↑
              Minimum valid speed (answer)

WHY USE CEILING DIVISION:
- Koko can only eat during full hours
- If pile has 7 bananas and speed is 3:
  - Cannot finish in 2.33 hours
  - Must take full 3 hours
- Math.Ceiling rounds up to next integer

EDGE CASES HANDLED:
- Single pile: binary search still works, returns optimal speed
- h equals number of piles: must eat largest pile in 1 hour
- Very large piles: uses long for totalTime to prevent overflow
- Speed of 1: minimum possible speed, always checked

OPTIMIZATION NOTES:
- Start with l=1 (minimum speed) and r=max(piles) (maximum needed speed)
- Could optimize by setting l = total_bananas / h (theoretical minimum)
- Current approach is clean and correct

WHY NOT LINEAR SEARCH:
- Linear search: try speed 1, 2, 3, ... until one works
- Time: O(m * n) where m = max pile
- Binary search: O(log m * n) - much faster for large piles

PATTERN RECOGNITION:
- "Find minimum value that satisfies condition" → binary search on answer
- Similar problems: minimum capacity, minimum days, minimum time
- Key: identify monotonic property in answer space
*/