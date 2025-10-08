/*
PROBLEM: Car Fleet
There are n cars going to the same destination along a one-lane road. The destination is target miles away.

You are given two integer arrays position and speed, both of length n, where position[i] is the 
position of the ith car and speed[i] is the speed of the ith car (in miles per hour).

A car can never pass another car ahead of it, but it can catch up to it and drive bumper to bumper 
at the same speed. The faster car will slow down to match the slower car's speed. The distance between 
these two cars is ignored (i.e., they are assumed to have the same position).

A car fleet is some non-empty set of cars driving at the same position and same speed. Note that a 
single car is also a car fleet.

If a car catches up to a car fleet right at the destination point, it will still be considered as 
one car fleet.

Return the number of car fleets that will arrive at the destination.

Example 1:
Input: target = 12, position = [10,8,0,5,3], speed = [2,4,1,1,3]
Output: 3
Explanation:
- Car at position 10 with speed 2: arrives at time 1.0
- Car at position 8 with speed 4: arrives at time 1.0 (catches car ahead, forms fleet)
- Car at position 5 with speed 1: arrives at time 7.0
- Car at position 3 with speed 3: arrives at time 3.0 (catches car ahead, forms fleet)
- Car at position 0 with speed 1: arrives at time 12.0
Three fleets arrive: [10,8], [5,3], [0]

Example 2:
Input: target = 10, position = [3], speed = [3]
Output: 1
Explanation: There is only one car, hence there is only one fleet.

Example 3:
Input: target = 100, position = [0,2,4], speed = [4,2,1]
Output: 1
Explanation:
- Car at 0: time = 100/4 = 25
- Car at 2: time = 98/2 = 49  
- Car at 4: time = 96/1 = 96
All cars form one fleet as they catch up to the slowest car.

Constraints:
- n == position.length == speed.length
- 1 <= n <= 10^5
- 0 < target <= 10^6
- 0 <= position[i] < target
- All the values of position are unique.
- 0 < speed[i] <= 10^6
*/

public class Solution
{
    public int CarFleet(int target, int[] position, int[] speed)
    {
        // Create pairs of [position, speed] for each car
        int[][] pair = new int[position.Length][];
        for (int i = 0; i < position.Length; i++)
        {
            pair[i] = new int[] { position[i], speed[i] };
        }

        // Sort cars by position in descending order (closest to target first)
        Array.Sort(pair, (a, b) => b[0].CompareTo(a[0]));
        
        Stack<double> stack = new Stack<double>(); // Stack stores arrival times (time to reach target)
        
        // Process each car from closest to target to furthest
        foreach (var p in pair)
        {
            // Calculate time for this car to reach target
            double arrivalTime = (double)(target - p[0]) / p[1];
            stack.Push(arrivalTime);
            
            // Check if current car catches up to the fleet ahead
            // If current car arrives <= than car ahead, they form same fleet
            if (stack.Count >= 2 && stack.Peek() <= stack.ElementAt(1))
            {
                stack.Pop(); // Remove current car (merges into fleet ahead)
            }
        }
        
        return stack.Count; // Number of elements in stack = number of fleets
    }
}

/*
TIME COMPLEXITY: O(n log n)
- Creating pairs array: O(n)
- Sorting pairs by position: O(n log n)
- Processing each car and stack operations: O(n)
- Overall: O(n log n) where n is the number of cars

SPACE COMPLEXITY: O(n)
- Pairs array: O(n) to store position and speed for each car
- Stack: O(n) in worst case (all cars form separate fleets)
- Sorting may use O(log n) space depending on implementation
- Overall: O(n)

ALGORITHM EXPLANATION:
- Sort cars by position (closest to target first)
- Calculate arrival time for each car
- If a car behind catches up (arrives earlier or same time), they form a fleet
- Use stack to track distinct fleet arrival times
- Each distinct time in stack represents one fleet

KEY INSIGHTS:
- Cars closer to target are processed first
- A car can only catch up to cars ahead of it, never behind
- If car B (behind) arrives before car A (ahead), B slows down to match A
- Fleet formation: multiple cars arriving at same time or catching up
- Stack tracks "leading cars" of each fleet (cars that don't catch others)

FLEET FORMATION RULES:
- Car behind with arrival_time <= car_ahead → forms fleet (merge)
- Car behind with arrival_time > car_ahead → separate fleet (independent)
- Once cars form fleet, they travel together at slowest car's pace

EXAMPLE TRACE with target=12, position=[10,8,0,5,3], speed=[2,4,1,1,3]:

After sorting by position (descending):
[10,2], [8,4], [5,1], [3,3], [0,1]

Process car at position 10:
- Arrival time = (12-10)/2 = 1.0
- Stack: [1.0]

Process car at position 8:
- Arrival time = (12-8)/4 = 1.0
- 1.0 <= 1.0 → catches up to car ahead, forms fleet
- Pop, Stack: [1.0] (represents fleet of cars at positions 10 and 8)

Process car at position 5:
- Arrival time = (12-5)/1 = 7.0
- 7.0 > 1.0 → won't catch up, separate fleet
- Stack: [1.0, 7.0]

Process car at position 3:
- Arrival time = (12-3)/3 = 3.0
- 3.0 <= 7.0 → catches up to car at position 5, forms fleet
- Pop, Stack: [1.0, 7.0] (represents fleet of cars at positions 5 and 3)

Process car at position 0:
- Arrival time = (12-0)/1 = 12.0
- 12.0 > 7.0 → won't catch up, separate fleet
- Stack: [1.0, 7.0, 12.0]

Result: 3 fleets

WHY PROCESS FROM FRONT TO BACK:
- Cars can only catch cars ahead, not behind
- Processing front to back simulates natural flow toward target
- Each car compares with immediate car ahead (top of stack)
- Stack naturally represents "fleet leaders" (cars not caught by anyone behind)

STACK INTERPRETATION:
- Each element in stack = arrival time of a fleet leader
- Fleet leader = first car of a fleet (others caught up to it)
- Stack size = number of independent fleets
- Popping = car merges into fleet ahead

CATCH-UP CONDITION:
If time_behind <= time_ahead:
- Car behind reaches target at same time or earlier
- Must slow down to match car ahead (can't pass)
- They form one fleet

If time_behind > time_ahead:
- Car behind is too slow to catch up
- Remains separate fleet

EDGE CASES HANDLED:
- Single car: Returns 1 (one fleet)
- All cars same speed: Depends on positions
- Car already at target: time = 0 (catches everyone)
- All cars form one fleet: Stack size = 1
- All separate fleets: Stack size = n

ALTERNATIVE APPROACH:
- Could use array instead of stack for slightly better performance
- But stack provides cleaner semantics for fleet merging concept
*/