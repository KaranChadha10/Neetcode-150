/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 287. Find the Duplicate Number
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
Given an array of integers nums containing n + 1 integers where each integer is 
in the range [1, n] inclusive.

There is only one repeated number in nums, return this repeated number.

You must solve the problem without modifying the array nums and uses only constant 
extra space.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: nums = [1,3,4,2,2]
Output: 2
Explanation: 2 appears twice

Example 2:
Input: nums = [3,1,3,4,2]
Output: 3
Explanation: 3 appears twice

Example 3:
Input: nums = [3,3,3,3,3]
Output: 3
Explanation: All elements are 3

Example 4:
Input: nums = [1,1]
Output: 1

Example 5:
Input: nums = [1,1,2]
Output: 1

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 1 <= n <= 10^5
- nums.length == n + 1
- 1 <= nums[i] <= n
- All the integers in nums appear only once except for precisely one integer 
  which appears two or more times

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Array as Implicit Linked List**: Treat array as a linked list
   - Index i points to index nums[i]
   - Since values are in [1, n] and length is n+1, we can use index 0 as start
   - Duplicate creates a cycle in this "linked list"

2. **Floyd's Cycle Detection (Tortoise and Hare)**:
   - Used to find cycles in linked lists
   - Two pointers: slow (moves 1 step) and fast (moves 2 steps)
   - If cycle exists, they will meet inside the cycle

3. **Finding Cycle Start**:
   - Once intersection found, reset one pointer to start
   - Move both at same speed (1 step each)
   - They meet at the cycle entrance = duplicate number

4. **Why This Works**:
   - Duplicate number means multiple indices point to same value
   - Creates a cycle when following nums[i] as "next" pointer
   - Cycle entrance is the duplicate value

5. **Mathematical Proof**:
   - Let distance to cycle start = F
   - Let distance from cycle start to meeting point = a
   - Let cycle length = C
   - When they meet: 2 × slow_distance = fast_distance
   - This ensures they meet at cycle start after reset

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(n)
- Phase 1 (find intersection): O(n)
  - Both pointers traverse at most n positions
  - They must meet within one cycle traversal
- Phase 2 (find cycle start): O(n)
  - Both traverse at most n positions
- Overall: O(n)

Space Complexity: O(1)
- Only uses two pointer variables
- No additional data structures
- Constant extra space as required

═══════════════════════════════════════════════════════════════════════════════
*/

public class Solution
{
    public int FindDuplicate(int[] nums)
    {
        // ════════════════════════════════════════════════════════════════════
        // PHASE 1: Find intersection point in the cycle
        // ════════════════════════════════════════════════════════════════════
        // Use Floyd's Cycle Detection algorithm (Tortoise and Hare)
        // Treat array as implicit linked list where nums[i] points to nums[nums[i]]
        //
        // Why this works:
        // - Array has n+1 elements with values [1, n]
        // - By pigeonhole principle, there must be a duplicate
        // - Duplicate creates a cycle in the implicit linked list
        // - We start from index 0 (which is never a target since values ≥ 1)
        
        int slow = 0;  // Tortoise: moves 1 step at a time
        int fast = 0;  // Hare: moves 2 steps at a time
        
        // ────────────────────────────────────────────────────────────────────
        // Move pointers until they meet inside the cycle
        // ────────────────────────────────────────────────────────────────────
        while (true)
        {
            // Move slow pointer one step: index → nums[index]
            slow = nums[slow];
            
            // Move fast pointer two steps: index → nums[nums[index]]
            fast = nums[nums[fast]];
            
            // Check if they meet (intersection point inside cycle)
            if (slow == fast)
            {
                break;  // Exit first loop to start phase 2
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // PHASE 2: Find the entrance to the cycle (the duplicate number)
        // ════════════════════════════════════════════════════════════════════
        // Mathematical insight:
        // - Distance from start to cycle entrance = F
        // - Distance from cycle entrance to meeting point = a
        // - When slow and fast meet: slow traveled F+a, fast traveled F+a+nC
        // - Since fast travels 2x speed: 2(F+a) = F+a+nC
        // - Simplifying: F+a = nC, therefore F = nC-a
        // - This means: distance from start to entrance = 
        //              distance from meeting point to entrance (going around)
        
        int slow2 = 0;  // Start from beginning
        // slow is already at meeting point
        
        // ────────────────────────────────────────────────────────────────────
        // Move both pointers at same speed until they meet
        // ────────────────────────────────────────────────────────────────────
        while (true)
        {
            // Move both pointers one step at a time
            slow = nums[slow];      // Continue from meeting point
            slow2 = nums[slow2];    // Start from beginning
            
            // When they meet, this is the cycle entrance = duplicate number
            if (slow == slow2)
            {
                return slow;  // This is the duplicate!
            }
        }
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🐛 BUG FIXES APPLIED:
═══════════════════════════════════════════════════════════════════════════════

Original Code Issues:

1. ❌ CRITICAL BUG: First loop had "return true"
   - Function signature returns int, not bool
   - Would cause compilation error
   - Should use "break" to exit loop and continue to phase 2
   
   Original:
```csharp
   if (slow == fast) {
       return true;  // ❌ WRONG!
   }
```
   
   Fixed:
```csharp
   if (slow == fast) {
       break;  // ✓ CORRECT
   }
```

2. ❌ LOGIC BUG: Second loop unreachable
   - First loop had return statement
   - Second loop could never execute
   - Algorithm requires TWO phases
   
3. ❌ INCORRECT RETURN: First loop returned wrong value
   - Meeting point is NOT the duplicate
   - Meeting point is somewhere inside the cycle
   - Need phase 2 to find cycle entrance

Summary of Fixes:
✓ Changed "return true" to "break"
✓ Made second loop reachable
✓ Returns correct duplicate value from phase 2

═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example: nums = [1,3,4,2,2]
═══════════════════════════════════════════════════════════════════════════════

Array Visualization:
───────────────────────────────────────────────────────
Index: 0  1  2  3  4
Value: 1  3  4  2  2

Implicit Linked List (index → nums[index]):
  0 → 1 → 3 → 2 → 4 → 2 (cycle!)
              ↑_________↓

Visual Graph:
      0
      ↓
      1
      ↓
      3
      ↓
      2 ← ← ← ←
      ↓         ↑
      4 → → → → ↑
      
Cycle starts at index 2 (value 2)

PHASE 1: Find Intersection Point
───────────────────────────────────────────────────────

Step 0: slow=0, fast=0

Step 1:
  slow = nums[0] = 1
  fast = nums[nums[0]] = nums[1] = 3
  slow ≠ fast, continue

Step 2:
  slow = nums[1] = 3
  fast = nums[nums[3]] = nums[2] = 4
  slow ≠ fast, continue

Step 3:
  slow = nums[3] = 2
  fast = nums[nums[4]] = nums[2] = 4
  slow ≠ fast, continue

Step 4:
  slow = nums[2] = 4
  fast = nums[nums[4]] = nums[2] = 4
  slow == fast! Meeting point found at index 4 ✓
  Break out of first loop

Position after Phase 1:
  slow = 4 (meeting point)
  fast = 4 (not used anymore)

PHASE 2: Find Cycle Entrance
───────────────────────────────────────────────────────

Initialize: slow2 = 0, slow = 4

Step 1:
  slow = nums[4] = 2
  slow2 = nums[0] = 1
  slow ≠ slow2, continue

Step 2:
  slow = nums[2] = 4
  slow2 = nums[1] = 3
  slow ≠ slow2, continue

Step 3:
  slow = nums[4] = 2
  slow2 = nums[3] = 2
  slow == slow2! Found cycle entrance ✓

RESULT: 2 ✓

Verification:
  Array: [1,3,4,2,2]
  Duplicate: 2 appears at indices 3 and 4
  Answer: 2 ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 DETAILED WALKTHROUGH - Example: nums = [3,1,3,4,2]
═══════════════════════════════════════════════════════════════════════════════

Array:
Index: 0  1  2  3  4
Value: 3  1  3  4  2

Implicit Linked List:
  0 → 3 → 4 → 2 → 3 (cycle!)
      ↑___________↓

Visual:
      0
      ↓
      3 ← ← ← ←
      ↓         ↑
      4 → 2 → → ↑

PHASE 1: Find Intersection
───────────────────────────────────────────────────────

Step 0: slow=0, fast=0

Step 1:
  slow = nums[0] = 3
  fast = nums[nums[0]] = nums[3] = 4
  Continue...

Step 2:
  slow = nums[3] = 4
  fast = nums[nums[4]] = nums[2] = 3
  Continue...

Step 3:
  slow = nums[4] = 2
  fast = nums[nums[3]] = nums[4] = 2
  slow == fast! Meeting at index 2 ✓
  Break

PHASE 2: Find Entrance
───────────────────────────────────────────────────────

slow = 2, slow2 = 0

Step 1:
  slow = nums[2] = 3
  slow2 = nums[0] = 3
  slow == slow2! Found entrance ✓

RESULT: 3 ✓

═══════════════════════════════════════════════════════════════════════════════
💡 WHY FLOYD'S ALGORITHM WORKS - MATHEMATICAL PROOF:
═══════════════════════════════════════════════════════════════════════════════

Let's define:
- F = distance from start (index 0) to cycle entrance
- a = distance from cycle entrance to meeting point
- b = distance from meeting point back to cycle entrance
- C = cycle length = a + b

Phase 1 - Finding Intersection:
───────────────────────────────────────────────────────

When slow enters the cycle:
  - slow has traveled: F steps
  - fast has traveled: 2F steps
  - fast is already (2F - F) = F steps into the cycle

They meet when:
  - slow position in cycle: a
  - fast position in cycle: a + nC (n complete cycles ahead)
  - Since fast travels 2x speed: 2(F + a) = F + a + nC
  - Simplifying: F + a = nC
  - Therefore: F = nC - a = b + (n-1)C

Phase 2 - Finding Cycle Entrance:
───────────────────────────────────────────────────────

Since F = b + (n-1)C:
  - Starting from meeting point, moving b steps reaches entrance
  - Starting from index 0, moving F steps reaches entrance
  - Since F = b + (n-1)C, after F steps from start, we've:
    - Traveled F steps = b + (n-1)C
    - Which is b steps + (n-1) complete cycles
    - This lands exactly at cycle entrance!

Therefore:
  - Both pointers moving at same speed will meet at cycle entrance
  - Cycle entrance index contains the duplicate value!

Visual Proof:
───────────────────────────────────────────────────────

Start (0) → → → → F steps → Entrance
                              ↓
Meeting ← ← b steps ← ← ← ← Entrance

Since F = b + cycles, both paths lead to same point!

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Minimum size (n=1):
   nums = [1,1]
   Cycle: 0 → 1 → 1 (self-loop)
   Result: 1 ✓

✅ All same values:
   nums = [3,3,3,3,3]
   Multiple paths to 3, creates cycle
   Result: 3 ✓

✅ Duplicate at beginning:
   nums = [2,5,9,6,9,3,8,9,7,1]
   Result: 9 ✓

✅ Large array:
   nums with n=100,000
   Algorithm still O(n) time, O(1) space ✓

✅ Duplicate appears twice:
   nums = [1,3,4,2,2]
   Result: 2 ✓

✅ Duplicate appears many times:
   nums = [2,5,9,2,9,2,8,2,7,1]
   Result: 2 (or 9) ✓

✅ Sequential values:
   nums = [1,2,3,4,5,6,7,8,9,9]
   Result: 9 ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Returning at first meeting (original bug)
   - First meeting is NOT the duplicate
   - It's just a point inside the cycle
   - Need phase 2 to find cycle entrance

❌ Using return true instead of break (original bug)
   - Wrong return type (should be int)
   - Makes second loop unreachable
   - Phase 2 never executes

❌ Modifying the array
   - Problem requires not modifying array
   - Marking visited elements violates constraint
   - Use Floyd's algorithm instead

❌ Using extra space (hash set/array)
   - Problem requires O(1) space
   - Hash set would be O(n) space
   - Array marking would be O(n) space

❌ Starting pointers at different positions
   - Both must start at index 0
   - Otherwise algorithm doesn't work
   - Index 0 is safe (never pointed to since values ≥ 1)

❌ Moving pointers incorrectly in phase 2
   - Both must move at SAME speed (1 step)
   - Not different speeds like phase 1
   - Same speed ensures meeting at entrance

❌ Comparing indices instead of values
   - We're finding duplicate VALUE, not index
   - Return slow (the value), not the index

❌ Forgetting array is 0-indexed
   - Values are [1, n] but indices are [0, n]
   - Index 0 is never a target (safe starting point)

❌ Not understanding why it's a cycle
   - Duplicate means two indices point to same value
   - Following nums[i] as "next" creates cycle
   - Cycle entrance value is the duplicate

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Floyd's Cycle Detection (Tortoise and Hare)**:
   - Detect cycles in O(n) time, O(1) space
   - Two pointers at different speeds
   - Classic algorithm for linked lists

2. **Array as Implicit Data Structure**:
   - Array can represent linked list
   - nums[i] is like node.next
   - Values as pointers to indices

3. **Cycle Finding in Functional Graphs**:
   - Each node has exactly one outgoing edge
   - Following edges creates a path
   - Duplicate creates cycle

4. **Two-Phase Algorithm**:
   - Phase 1: Detect cycle exists
   - Phase 2: Find cycle properties
   - Common in cycle problems

5. **Mathematical Problem Reduction**:
   - Constraint: n+1 elements, values [1,n]
   - Pigeonhole principle: must have duplicate
   - Reduction to cycle detection problem

═══════════════════════════════════════════════════════════════════════════════
🔄 ALTERNATIVE APPROACHES:
═══════════════════════════════════════════════════════════════════════════════

APPROACH 1: Hash Set (Violates Space Constraint)
───────────────────────────────────────────────────────
```csharp
public int FindDuplicate(int[] nums) {
    HashSet<int> seen = new HashSet<int>();
    foreach (int num in nums) {
        if (seen.Contains(num)) return num;
        seen.Add(num);
    }
    return -1;
}
```
✓ Time: O(n)
✗ Space: O(n) - violates constraint
✓ Simple and intuitive
Use: When space constraint doesn't apply

APPROACH 2: Sorting (Violates Modification Constraint)
───────────────────────────────────────────────────────
```csharp
public int FindDuplicate(int[] nums) {
    Array.Sort(nums);
    for (int i = 1; i < nums.Length; i++) {
        if (nums[i] == nums[i-1]) return nums[i];
    }
    return -1;
}
```
✗ Time: O(n log n)
✗ Modifies array - violates constraint
✓ Simple logic
Use: When modification allowed

APPROACH 3: Binary Search (Advanced)
───────────────────────────────────────────────────────
```csharp
public int FindDuplicate(int[] nums) {
    int left = 1, right = nums.Length - 1;
    while (left < right) {
        int mid = left + (right - left) / 2;
        int count = 0;
        foreach (int num in nums) {
            if (num <= mid) count++;
        }
        if (count > mid) right = mid;
        else left = mid + 1;
    }
    return left;
}
```
✗ Time: O(n log n)
✓ Space: O(1)
✗ Doesn't modify array
✓ Alternative O(1) space solution
Use: When asked for different O(1) space approach

APPROACH 4: Floyd's Algorithm (Current - Optimal)
───────────────────────────────────────────────────────
✓ Time: O(n)
✓ Space: O(1)
✓ Doesn't modify array
✓ Meets all constraints
✓ Most efficient solution

Comparison:
- Floyd's is only approach meeting ALL constraints
- Best time complexity: O(n)
- Required constant space: O(1)
- Industry standard for this problem

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Cycle Detection in Systems**:
   - Detect infinite loops in programs
   - Find cycles in dependency graphs
   - Deadlock detection in concurrent systems

2. **Network Analysis**:
   - Detect routing loops
   - Find cycles in network topology
   - Traffic pattern analysis

3. **Data Validation**:
   - Find duplicate IDs in constrained space
   - Verify data integrity
   - Detect corruption in sequential data

4. **Memory Leak Detection**:
   - Find circular references
   - Detect reference cycles in garbage collection
   - Memory debugging tools

5. **Graph Algorithms**:
   - Cycle detection in directed graphs
   - Finding strongly connected components
   - Path analysis

6. **Cryptography**:
   - Detect collisions in hash functions
   - Find patterns in pseudorandom sequences
   - Cycle analysis in number theory

7. **Game Development**:
   - Detect stuck states in game logic
   - Find repetitive patterns in AI behavior
   - Level completion validation

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Linked List Cycle (LeetCode 141)**:
   - Detect if linked list has cycle
   - Same Floyd's algorithm
   - Foundation for this problem

2. **Linked List Cycle II (LeetCode 142)**:
   - Find where cycle begins
   - Exactly same algorithm as this problem
   - Direct application

3. **Happy Number (LeetCode 202)**:
   - Detect cycle in number sequence
   - Uses Floyd's algorithm
   - Different problem domain, same technique

4. **Find All Duplicates in Array (LeetCode 442)**:
   - Find all duplicates in [1,n] range
   - Can use index marking
   - Related constraints

5. **Missing Number (LeetCode 268)**:
   - Find missing number in [0,n]
   - Different problem but similar constraints
   - Various O(1) space solutions

6. **First Missing Positive (LeetCode 41)**:
   - Find smallest missing positive
   - Array as implicit data structure
   - O(1) space challenge

7. **Set Mismatch (LeetCode 645)**:
   - Find duplicate and missing number
   - Similar array constraints
   - Combination problem

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Solution Analysis:

Time Complexity: O(n) - Optimal
✓ Cannot do better than O(n)
✓ Must examine all elements at least once
✓ Floyd's achieves theoretical minimum

Space Complexity: O(1) - Optimal
✓ Only uses pointer variables
✓ No additional data structures
✓ Meets problem constraint

Why This Is Optimal:
1. **Lower Bound**: Must be Ω(n)
   - Need to examine array to find duplicate
   - Can't know duplicate without looking at values

2. **Upper Bound**: O(n)
   - Floyd's algorithm achieves O(n)
   - Matches lower bound
   - Therefore optimal!

3. **Space**: O(1) required
   - Problem explicitly requires constant space
   - Floyd's uses only pointers
   - No way to improve space further

Micro-optimizations:
- Could cache nums[slow] to avoid double lookup
- Minimal practical benefit
- Hurts code readability
- Not recommended

Best Practice:
✓ Current implementation is optimal
✓ Clear and correct
✓ Meets all constraints
✓ No further optimization needed

When to Consider Alternatives:
- If modification allowed: sorting O(n log n)
- If extra space allowed: hash set O(n) space but simpler
- For interviews: Always use Floyd's for this problem

═══════════════════════════════════════════════════════════════════════════════
*/