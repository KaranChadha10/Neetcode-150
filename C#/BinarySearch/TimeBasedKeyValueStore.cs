/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 981. Time Based Key-Value Store
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
Design a time-based key-value data structure that can store multiple values for 
the same key at different time stamps and retrieve the key's value at a certain 
timestamp.

Implement the TimeMap class:
- TimeMap() Initializes the object of the data structure.
- void set(String key, String value, int timestamp) Stores the key with the value 
  at the given time timestamp.
- String get(String key, int timestamp) Returns a value such that set was called 
  previously, with timestamp_prev <= timestamp. If there are multiple such values, 
  it returns the value associated with the largest timestamp_prev. If there are no 
  values, it returns "".

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input:
["TimeMap", "set", "get", "get", "set", "get", "get"]
[[], ["foo", "bar", 1], ["foo", 1], ["foo", 3], ["foo", "bar2", 4], ["foo", 4], ["foo", 5]]

Output:
[null, null, "bar", "bar", null, "bar2", "bar2"]

Explanation:
TimeMap timeMap = new TimeMap();
timeMap.set("foo", "bar", 1);  // store key "foo" with value "bar" at timestamp 1
timeMap.get("foo", 1);         // return "bar"
timeMap.get("foo", 3);         // return "bar" (no value at 3, use value at 1)
timeMap.set("foo", "bar2", 4); // store key "foo" with value "bar2" at timestamp 4
timeMap.get("foo", 4);         // return "bar2"
timeMap.get("foo", 5);         // return "bar2" (no value at 5, use value at 4)

Example 2:
Input:
["TimeMap", "set", "set", "get", "get"]
[[], ["love", "high", 10], ["love", "low", 20], ["love", 15], ["love", 25]]

Output:
[null, null, null, "high", "low"]

Explanation:
timeMap.set("love", "high", 10);
timeMap.set("love", "low", 20);
timeMap.get("love", 15);  // return "high" (closest timestamp <= 15 is 10)
timeMap.get("love", 25);  // return "low" (closest timestamp <= 25 is 20)

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 1 <= key.length, value.length <= 100
- key and value consist of lowercase English letters and digits
- 1 <= timestamp <= 10^7
- All the timestamps of set are strictly increasing
- At most 2 * 10^5 calls will be made to set and get

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Strictly Increasing Timestamps**: The problem guarantees timestamps are 
   strictly increasing for set operations. This means we can use a simple list
   without worrying about insertion sort.

2. **Binary Search Optimization**: Since timestamps are sorted, we can use binary
   search to find the closest timestamp <= target in O(log n) time.

3. **Hash Map + List Structure**: 
   - Hash map for O(1) key lookup
   - List of (timestamp, value) pairs for each key
   - Binary search within the list

4. **Floor Function**: We're essentially finding the "floor" timestamp - the 
   largest timestamp that is <= the query timestamp.

5. **Empty String Default**: Return "" when key doesn't exist or no valid 
   timestamp found.

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Constructor - TimeMap():
Time Complexity: O(1)
Space Complexity: O(1)

Set Operation:
Time Complexity: O(1)
- Dictionary lookup/insertion: O(1)
- List append: O(1) amortized
Space Complexity: O(n)
- Stores n timestamp-value pairs across all keys

Get Operation:
Time Complexity: O(log m)
- Dictionary lookup: O(1)
- Binary search on list: O(log m) where m = number of timestamps for that key
Space Complexity: O(1)
- Only uses constant extra space for binary search variables

Overall Space: O(n) where n is total number of set operations

═══════════════════════════════════════════════════════════════════════════════
*/

public class TimeMap
{
    // ════════════════════════════════════════════════════════════════════════
    // DATA STRUCTURE DESIGN
    // ════════════════════════════════════════════════════════════════════════
    // Dictionary: key → List of (timestamp, value) tuples
    // 
    // Structure:
    // {
    //   "foo": [(1, "bar"), (4, "bar2"), (7, "bar3")],
    //   "love": [(10, "high"), (20, "low")]
    // }
    //
    // Why this design?
    // - Dictionary provides O(1) key lookup
    // - List maintains sorted timestamps (guaranteed by problem)
    // - Tuple stores (timestamp, value) pairs together
    // - Binary search works on sorted list for O(log n) retrieval
    
    private Dictionary<string, List<Tuple<int, string>>> keyStore;

    // ════════════════════════════════════════════════════════════════════════
    // CONSTRUCTOR: Initialize the TimeMap data structure
    // ════════════════════════════════════════════════════════════════════════
    public TimeMap()
    {
        // Initialize empty dictionary to store key → timestamp-value pairs
        keyStore = new Dictionary<string, List<Tuple<int, string>>>();
    }

    // ════════════════════════════════════════════════════════════════════════
    // SET: Store a key-value pair at a specific timestamp
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Stores the key with the value at the given timestamp.
    /// Timestamps are guaranteed to be strictly increasing for each key.
    /// </summary>
    /// <param name="key">The key to store</param>
    /// <param name="value">The value associated with the key</param>
    /// <param name="timestamp">The timestamp (strictly increasing)</param>
    public void Set(string key, string value, int timestamp)
    {
        // ────────────────────────────────────────────────────────────────────
        // Check if this is the first time we're seeing this key
        // ────────────────────────────────────────────────────────────────────
        if (!keyStore.ContainsKey(key))
        {
            // Initialize a new list for this key
            keyStore[key] = new List<Tuple<int, string>>();
        }
        
        // ────────────────────────────────────────────────────────────────────
        // Append the (timestamp, value) pair to the list
        // ────────────────────────────────────────────────────────────────────
        // Since timestamps are strictly increasing, we can simply append
        // No need for insertion sort or maintaining sorted order manually
        // The list remains sorted automatically!
        keyStore[key].Add(Tuple.Create(timestamp, value));
    }

    // ════════════════════════════════════════════════════════════════════════
    // GET: Retrieve the value for a key at or before a specific timestamp
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Returns the value associated with the largest timestamp_prev where
    /// timestamp_prev <= timestamp. Returns "" if no such value exists.
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="timestamp">The target timestamp</param>
    /// <returns>The value at the closest timestamp <= target, or ""</returns>
    public string Get(string key, int timestamp)
    {
        // ────────────────────────────────────────────────────────────────────
        // EDGE CASE: Key doesn't exist in our data structure
        // ────────────────────────────────────────────────────────────────────
        if (!keyStore.ContainsKey(key))
        {
            return "";  // No data for this key
        }

        // ────────────────────────────────────────────────────────────────────
        // Get the list of (timestamp, value) pairs for this key
        // ────────────────────────────────────────────────────────────────────
        var value = keyStore[key];
        
        // ────────────────────────────────────────────────────────────────────
        // BINARY SEARCH: Find the rightmost timestamp <= target timestamp
        // ────────────────────────────────────────────────────────────────────
        // Goal: Find the largest timestamp that is <= our query timestamp
        // This is known as finding the "floor" value
        
        int left = 0;              // Left boundary of search space
        int right = value.Count - 1;  // Right boundary of search space
        string result = "";        // Default to empty string if no valid timestamp found

        while (left <= right)
        {
            // Calculate middle index (avoid overflow)
            int mid = left + (right - left) / 2;
            
            // ────────────────────────────────────────────────────────────
            // Check if current timestamp is valid (≤ target)
            // ────────────────────────────────────────────────────────────
            if (value[mid].Item1 <= timestamp)
            {
                // ════════════════════════════════════════════════════════
                // VALID TIMESTAMP FOUND
                // ════════════════════════════════════════════════════════
                // This timestamp is valid (≤ target)
                // Save it as a potential answer
                result = value[mid].Item2;
                
                // But there might be a LARGER valid timestamp to the right
                // Continue searching in the right half
                left = mid + 1;
            }
            else
            {
                // ════════════════════════════════════════════════════════
                // TIMESTAMP TOO LARGE
                // ════════════════════════════════════════════════════════
                // Current timestamp is > target timestamp
                // Search in the left half for smaller timestamps
                right = mid - 1;
            }
        }
        
        // ────────────────────────────────────────────────────────────────────
        // Return the result:
        // - If we found valid timestamps, result contains the value at the
        //   largest valid timestamp
        // - If no valid timestamp found, result remains ""
        // ────────────────────────────────────────────────────────────────────
        return result;
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example Operations
═══════════════════════════════════════════════════════════════════════════════

Initial State:
───────────────────────────────────────────────────────
keyStore = {}

╔════════════════════════════════════════════════════════════════════╗
║ Operation 1: set("foo", "bar", 1)                                 ║
╚════════════════════════════════════════════════════════════════════╝

Check if "foo" exists: NO
Create new list for "foo"
Add (1, "bar") to list

State:
keyStore = {
    "foo": [(1, "bar")]
}

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Operation 2: get("foo", 1)                                        ║
╚════════════════════════════════════════════════════════════════════╝

Key exists: YES
List: [(1, "bar")]

Binary Search for timestamp <= 1:
  left=0, right=0
  mid=0: timestamp=1 <= 1? YES
    result = "bar"
    left = 1
  left > right, exit loop

Return: "bar" ✓

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Operation 3: get("foo", 3)                                        ║
╚════════════════════════════════════════════════════════════════════╝

Key exists: YES
List: [(1, "bar")]

Binary Search for timestamp <= 3:
  left=0, right=0
  mid=0: timestamp=1 <= 3? YES
    result = "bar"
    left = 1
  left > right, exit loop

Return: "bar" ✓
(No exact match at 3, but 1 is the largest timestamp <= 3)

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Operation 4: set("foo", "bar2", 4)                                ║
╚════════════════════════════════════════════════════════════════════╝

Key exists: YES
Add (4, "bar2") to existing list

State:
keyStore = {
    "foo": [(1, "bar"), (4, "bar2")]
}

Timeline visualization:
timestamp: 1       4
value:     "bar"   "bar2"

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Operation 5: get("foo", 4)                                        ║
╚════════════════════════════════════════════════════════════════════╝

Key exists: YES
List: [(1, "bar"), (4, "bar2")]

Binary Search for timestamp <= 4:
  left=0, right=1
  
  Iteration 1:
    mid=0: timestamp=1 <= 4? YES
      result = "bar"
      left = 1
  
  Iteration 2:
    left=1, right=1
    mid=1: timestamp=4 <= 4? YES
      result = "bar2"
      left = 2
  
  left > right, exit loop

Return: "bar2" ✓

───────────────────────────────────────────────────────

╔════════════════════════════════════════════════════════════════════╗
║ Operation 6: get("foo", 5)                                        ║
╚════════════════════════════════════════════════════════════════════╝

Key exists: YES
List: [(1, "bar"), (4, "bar2")]

Binary Search for timestamp <= 5:
  left=0, right=1
  
  Iteration 1:
    mid=0: timestamp=1 <= 5? YES
      result = "bar"
      left = 1
  
  Iteration 2:
    left=1, right=1
    mid=1: timestamp=4 <= 5? YES
      result = "bar2"
      left = 2
  
  left > right, exit loop

Return: "bar2" ✓
(No exact match at 5, but 4 is the largest timestamp <= 5)

═══════════════════════════════════════════════════════════════════════════════
🎯 DETAILED BINARY SEARCH EXAMPLE - Multiple Timestamps
═══════════════════════════════════════════════════════════════════════════════

Setup:
───────────────────────────────────────────────────────
set("foo", "v1", 1)
set("foo", "v2", 3)
set("foo", "v3", 5)
set("foo", "v4", 7)
set("foo", "v5", 9)

List: [(1,"v1"), (3,"v2"), (5,"v3"), (7,"v4"), (9,"v5")]
Index:    0        1        2        3        4

Timeline:
  1    3    5    7    9
  v1   v2   v3   v4   v5

╔════════════════════════════════════════════════════════════════════╗
║ Query: get("foo", 6)                                              ║
╚════════════════════════════════════════════════════════════════════╝

Find largest timestamp <= 6

Iteration 1:
  left=0, right=4
  mid=2: timestamp=5 <= 6? YES
    result = "v3"
    left = 3
    Search space: [3,4] (indices)

Iteration 2:
  left=3, right=4
  mid=3: timestamp=7 <= 6? NO
    right = 2
    Search space: empty

left > right, exit

Return: "v3" ✓

Explanation:
- timestamp=5 is valid (≤6), so we save "v3"
- timestamp=7 is too large (>6), so we eliminate right half
- "v3" at timestamp=5 is the largest valid timestamp

╔════════════════════════════════════════════════════════════════════╗
║ Query: get("foo", 0)                                              ║
╚════════════════════════════════════════════════════════════════════╝

Find largest timestamp <= 0

Iteration 1:
  left=0, right=4
  mid=2: timestamp=5 <= 0? NO
    right = 1
    
Iteration 2:
  left=0, right=1
  mid=0: timestamp=1 <= 0? NO
    right = -1

left > right, exit

Return: "" ✓

Explanation:
- No timestamp is <= 0
- result remains empty string

╔════════════════════════════════════════════════════════════════════╗
║ Query: get("foo", 100)                                            ║
╚════════════════════════════════════════════════════════════════════╝

Find largest timestamp <= 100

The binary search will eventually find timestamp=9 (the rightmost),
which is <= 100.

Return: "v5" ✓

═══════════════════════════════════════════════════════════════════════════════
💡 WHY THIS BINARY SEARCH WORKS (Finding Floor Value)
═══════════════════════════════════════════════════════════════════════════════

Goal: Find the RIGHTMOST element where timestamp <= target

Traditional binary search finds exact matches.
This modified binary search finds the "floor" - the largest element ≤ target.

Key Modification:
1. When we find a valid element (timestamp ≤ target):
   - Save it as potential answer
   - Continue searching RIGHT (left = mid + 1)
   - We might find a larger valid timestamp

2. When element is too large (timestamp > target):
   - Search LEFT (right = mid - 1)
   - Smaller timestamps might be valid

Why it works:
- The list is sorted
- We explore all valid candidates
- We keep the largest valid one
- We terminate when no more candidates exist

Alternative approach (not used here):
Could binary search for the leftmost element > target, then return previous element.
Current approach is more intuitive.

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Key doesn't exist:
   get("nonexistent", 10) → ""
   Handled by checking ContainsKey

✅ Timestamp before all stored timestamps:
   set("foo", "bar", 5)
   get("foo", 3) → ""
   No valid timestamp found, returns ""

✅ Timestamp after all stored timestamps:
   set("foo", "bar", 5)
   get("foo", 10) → "bar"
   Returns most recent value

✅ Exact timestamp match:
   set("foo", "bar", 5)
   get("foo", 5) → "bar"
   Binary search finds exact match

✅ Timestamp between stored values:
   set("foo", "bar", 5)
   set("foo", "baz", 10)
   get("foo", 7) → "bar"
   Returns value at timestamp 5

✅ Single value for key:
   set("foo", "bar", 5)
   get("foo", 5) → "bar"
   get("foo", 6) → "bar"
   get("foo", 4) → ""

✅ Multiple keys:
   set("foo", "bar", 1)
   set("baz", "qux", 2)
   get("foo", 1) → "bar"
   get("baz", 2) → "qux"
   Keys are independent

✅ Same key, multiple timestamps:
   set("foo", "v1", 1)
   set("foo", "v2", 2)
   set("foo", "v3", 3)
   get("foo", 2) → "v2"
   get("foo", 3) → "v3"
   get("foo", 1) → "v1"

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Not handling key doesn't exist case
   - Must check ContainsKey before accessing dictionary
   - Should return "" for nonexistent keys

❌ Using wrong binary search logic
   - Need to find floor (largest ≤ target), not exact match
   - Must save result when valid timestamp found
   - Must continue searching right for larger valid timestamps

❌ Sorting the list on each insertion
   - Unnecessary! Problem guarantees strictly increasing timestamps
   - Can simply append, list stays sorted automatically

❌ Linear search instead of binary search
   - O(n) per get operation instead of O(log n)
   - Fine for small datasets, slow for large ones

❌ Not initializing result to ""
   - If no valid timestamp found, must return ""
   - Empty string is the default, not null

❌ Wrong comparison: using < instead of <=
   - Must include equality case
   - Timestamp equal to target is valid

❌ Not handling empty list case
   - When key exists but list is empty (shouldn't happen with proper set)
   - Binary search handles this naturally (left > right immediately)

❌ Using Item1 incorrectly
   - Item1 is timestamp, Item2 is value
   - Don't confuse them!

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Hash Map + List Combination**:
   - Hash map for fast key lookup
   - List for ordered data per key
   - Common in time-series or versioned data

2. **Binary Search for Floor Value**:
   - Find largest element ≤ target
   - Modified binary search pattern
   - Used in range queries, snapshots

3. **Time-Series Data Structure**:
   - Multiple values over time for same key
   - Query historical data
   - Common in databases, caching

4. **Strictly Increasing Property**:
   - Leverage sorted guarantee
   - Avoid expensive sorting operations
   - Append-only optimization

5. **Snapshot/Versioning Pattern**:
   - Store historical states
   - Query past states
   - Used in version control, databases

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Time-Series Databases**:
   - InfluxDB, TimescaleDB
   - Store metrics over time
   - Query historical data

2. **Version Control Systems**:
   - Git commit history
   - File versions at different times
   - Retrieve state at specific commit

3. **Stock Price Tracking**:
   - Store stock prices at different timestamps
   - Query price at or before specific time
   - Historical analysis

4. **Cache with Expiration**:
   - Store cache entries with timestamps
   - Invalidate old entries
   - Time-based cache eviction

5. **Event Logging Systems**:
   - Application logs with timestamps
   - Query logs before/after specific time
   - Debugging and monitoring

6. **Distributed Systems**:
   - Vector clocks
   - Logical timestamps
   - Consistency models

7. **Database Snapshots**:
   - Point-in-time recovery
   - Snapshot isolation
   - Temporal queries

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **LRU Cache (LeetCode 146)**:
   - Design cache with eviction policy
   - Similar design problem
   - Uses HashMap + LinkedList

2. **Snapshot Array (LeetCode 1146)**:
   - Set, snap, and get operations
   - Similar timestamp concept
   - Efficient snapshot storage

3. **Design Hit Counter (LeetCode 362)**:
   - Count hits in time window
   - Time-based data structure
   - Sliding window with timestamps

4. **Logger Rate Limiter (LeetCode 359)**:
   - Rate limiting with timestamps
   - Check message frequency
   - Time-based filtering

5. **Time Based Ranking (Interview)**:
   - Leaderboard with time decay
   - Recent scores weighted higher
   - Time-series aggregation

6. **Temporal Tables (Database Concept)**:
   - Track row history
   - Query data at past time
   - Similar to this problem

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Solution Analysis:

Strengths:
✓ O(log n) get operations
✓ O(1) set operations
✓ Simple and clean implementation
✓ Leverages sorted timestamp guarantee

Potential Optimizations:

1. **Space Optimization** (if many keys with few timestamps):
   - Use array instead of List for very small collections
   - Saves overhead of List<T> wrapper
   - Only beneficial for < ~10 elements

2. **Bulk Loading**:
   - If loading historical data
   - Can use AddRange for better performance
   - Reduces allocation overhead

3. **Caching Last Access**:
   - If queries are often for recent timestamps
   - Cache last returned (timestamp, value)
   - Check cache before binary search
   - Good for temporal locality

4. **Skip List Alternative**:
   - Could use skip list instead of list + binary search
   - O(log n) average case for both operations
   - More complex to implement
   - No significant advantage here

5. **Segment Tree** (for range queries):
   - If we need range queries (not in this problem)
   - O(log n) for both point and range queries
   - Overkill for simple point queries

Trade-offs:
- Current solution is near-optimal for the problem constraints
- Any optimization would add complexity without major gains
- Simplicity and readability are valuable
- O(log n) get is already very fast

Real-World Considerations:
- In production, might use existing time-series DB
- Persistent storage for large datasets
- Distributed storage for scale
- Compression for space efficiency

═══════════════════════════════════════════════════════════════════════════════
*/

/**
 * Your TimeMap object will be instantiated and called as such:
 * TimeMap obj = new TimeMap();
 * obj.Set(key,value,timestamp);
 * string param_2 = obj.Get(key,timestamp);
 */