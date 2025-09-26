/*
PROBLEM: Best Time to Buy and Sell Stock
You are given an array prices where prices[i] is the price of a given stock on the ith day.

You want to maximize your profit by choosing a single day to buy one stock and choosing 
a different day in the future to sell that stock.

Return the maximum profit you can achieve from this transaction. If you cannot achieve 
any profit, return 0.

Example 1:
Input: prices = [7,1,5,3,6,4]
Output: 5
Explanation: Buy on day 2 (price = 1) and sell on day 5 (price = 6), profit = 6-1 = 5.
Note that buying on day 2 and selling on day 1 is not allowed because you must buy before you sell.

Example 2:
Input: prices = [7,6,4,3,1]
Output: 0
Explanation: In this case, no transactions are done and the max profit = 0.

Constraints:
- 1 <= prices.length <= 10^5
- 0 <= prices[i] <= 10^4
*/

public class Solution
{
    public int MaxProfit(int[] prices)
    {
        int maxP = 0; // Track maximum profit found so far
        int minBuy = prices[0]; // Track minimum price seen so far (best buying opportunity)

        foreach (int sell in prices) // Consider each day as a potential selling day
        {
            // Calculate profit if we sell on current day (with best buy price so far)
            maxP = Math.Max(maxP, sell - minBuy);
            
            // Update minimum buy price if current price is lower
            minBuy = Math.Min(minBuy, sell);
        }
        return maxP; // Return maximum profit achievable
    }
}

/*
TIME COMPLEXITY: O(n)
- Single pass through the prices array
- Each day is processed exactly once
- Constant time operations in each iteration
- Overall: O(n) where n is the number of days (length of prices array)

SPACE COMPLEXITY: O(1)
- Only using constant extra space for two variables (maxP, minBuy)
- No additional data structures needed
- Space usage doesn't depend on input size

ALGORITHM EXPLANATION:
- Greedy approach: keep track of minimum price seen so far and maximum profit
- For each day, calculate profit if we sell today using the best buy price found previously
- Update both the minimum buy price and maximum profit as we iterate
- Key insight: we don't need to remember all previous prices, just the minimum

KEY INSIGHTS:
- We must buy before we sell, so we process days sequentially
- At each day, we have two choices: update min buy price or calculate profit
- Minimum buy price gives us the best profit potential for future selling days
- We can do both operations in the same iteration efficiently

GREEDY STRATEGY:
- Always keep track of the lowest price seen so far (best buying opportunity)
- For each current price, calculate what profit we'd get if we sold today
- This ensures we consider all valid buy-sell combinations in one pass

EXAMPLE TRACE with [7,1,5,3,6,4]:
- Day 0: price=7, maxP=0, minBuy=7
- Day 1: price=1, maxP=max(0,1-7)=0, minBuy=min(7,1)=1
- Day 2: price=5, maxP=max(0,5-1)=4, minBuy=min(1,5)=1
- Day 3: price=3, maxP=max(4,3-1)=4, minBuy=min(1,3)=1
- Day 4: price=6, maxP=max(4,6-1)=5, minBuy=min(1,6)=1
- Day 5: price=4, maxP=max(5,4-1)=5, minBuy=min(1,4)=1
- Result: maxP = 5 (buy at 1, sell at 6)

WHY THIS WORKS:
- By maintaining minBuy, we ensure we're always using the best possible buy price
- By checking profit at each day, we consider all possible sell days
- The combination gives us the optimal buy-sell pair in a single pass
- No need to check all possible pairs (which would be O(n²))
*/