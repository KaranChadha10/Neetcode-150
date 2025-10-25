/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 138. Copy List with Random Pointer
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
A linked list of length n is given such that each node contains an additional 
random pointer, which could point to any node in the list, or null.

Construct a deep copy of the list. The deep copy should consist of exactly n 
brand new nodes, where each new node has its value set to the value of its 
corresponding original node. Both the next and random pointer of the new nodes 
should point to new nodes in the copied list such that the pointers in the 
original list and copied list represent the same list state. 

None of the pointers in the new list should point to nodes in the original list.

For example, if there are two nodes X and Y in the original list, where X.random --> Y, 
then for the corresponding two nodes x and y in the copied list, x.random --> y.

Return the head of the copied linked list.

The linked list is represented in the input/output as a list of n nodes. Each node 
is represented as a pair of [val, random_index] where:
- val: an integer representing Node.val
- random_index: the index of the node (range from 0 to n-1) that the random pointer 
  points to, or null if it does not point to any node.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input: head = [[7,null],[13,0],[11,4],[10,2],[1,0]]
Output: [[7,null],[13,0],[11,4],[10,2],[1,0]]
Explanation:
Node 0: val=7, random=null
Node 1: val=13, random=Node 0
Node 2: val=11, random=Node 4
Node 3: val=10, random=Node 2
Node 4: val=1, random=Node 0

Visual:
7 → 13 → 11 → 10 → 1
↓    ↓    ↓    ↓    ↓
null 7    1    11   7

Example 2:
Input: head = [[1,1],[2,1]]
Output: [[1,1],[2,1]]
Explanation:
Node 0: val=1, random=Node 1
Node 1: val=2, random=Node 1 (self-loop)

Visual:
1 → 2
↓   ↓
2   2 (self)

Example 3:
Input: head = [[3,null],[3,0],[3,null]]
Output: [[3,null],[3,0],[3,null]]

Example 4:
Input: head = []
Output: []
Explanation: The given linked list is empty (null pointer), so return null.

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 0 <= n <= 1000
- -10^4 <= Node.val <= 10^4
- Node.random is null or is pointing to some node in the linked list

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **Deep Copy Challenge**: Must create entirely new nodes, not just copy pointers
   - All pointers in new list must point to new nodes, not original nodes

2. **Random Pointer Problem**: Random pointers can point to any node
   - Can't simply copy as we traverse (random might point forward or backward)
   - Need to map old nodes to new nodes

3. **Hash Map Solution**: Use dictionary to map old nodes → new nodes
   - First pass: create all nodes and store mappings
   - Second pass: connect next and random pointers using mappings

4. **Two-Pass Strategy**:
   - Pass 1: Create all new nodes (ensures all nodes exist before connecting)
   - Pass 2: Connect pointers (now we can lookup any node in the map)

5. **Alternative O(1) Space**: Interweaving approach (more complex)
   - Insert copied nodes between original nodes
   - Connect random pointers
   - Separate the lists

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Time Complexity: O(n)
- First pass: O(n) to create all nodes and build hash map
- Second pass: O(n) to connect next and random pointers
- Overall: O(n) where n = number of nodes

Space Complexity: O(n)
- Hash map storing n old→new node mappings: O(n)
- New copied list: O(n) but doesn't count as extra space (required for output)
- Overall: O(n) auxiliary space for hash map

═══════════════════════════════════════════════════════════════════════════════
*/

/*
// Definition for a Node.
public class Node {
    public int val;
    public Node next;
    public Node random;
    
    public Node(int _val) {
        val = _val;
        next = null;
        random = null;
    }
}
*/

public class Solution 
{
    public Node CopyRandomList(Node head) 
    {
        // ════════════════════════════════════════════════════════════════════
        // EDGE CASE: Empty list
        // ════════════════════════════════════════════════════════════════════
        // Handle null input early to avoid unnecessary processing
        if (head == null) return null;
        
        // ════════════════════════════════════════════════════════════════════
        // STEP 1: Create hash map to store old node → new node mappings
        // ════════════════════════════════════════════════════════════════════
        // This map allows us to look up the copied version of any original node
        // Key: original node, Value: copied node
        Dictionary<Node, Node> oldToCopy = new Dictionary<Node, Node>();
       
        // ════════════════════════════════════════════════════════════════════
        // PASS 1: Create all new nodes and build the mapping
        // ════════════════════════════════════════════════════════════════════
        // Why create all nodes first?
        // - Random pointers can point to ANY node (forward or backward)
        // - We need all nodes to exist before we can connect their pointers
        // - This ensures we can lookup any node when setting random pointers
        
        Node cur = head;
        while (cur != null) 
        {
            // Create a new node with the same value as current node
            Node copy = new Node(cur.val);
            
            // Store the mapping: original node → copied node
            oldToCopy[cur] = copy;
            
            // Move to next node in original list
            cur = cur.next;
        }

        // ════════════════════════════════════════════════════════════════════
        // PASS 2: Connect next and random pointers in the copied nodes
        // ════════════════════════════════════════════════════════════════════
        // Now that all nodes exist, we can safely set their pointers
        // by looking up the corresponding copied nodes in our map
        
        cur = head;
        while (cur != null) 
        {
            // Get the copied version of the current node
            Node copy = oldToCopy[cur];
            
            // ────────────────────────────────────────────────────────────────
            // Connect the NEXT pointer
            // ────────────────────────────────────────────────────────────────
            // If current node has a next, point copied node's next to the 
            // copied version of that next node (found in hash map)
            // Otherwise, set to null (end of list)
            copy.next = cur.next != null ? oldToCopy[cur.next] : null;
            
            // ────────────────────────────────────────────────────────────────
            // Connect the RANDOM pointer
            // ────────────────────────────────────────────────────────────────
            // If current node has a random pointer, point copied node's random 
            // to the copied version of that random node (found in hash map)
            // Otherwise, set to null
            copy.random = cur.random != null ? oldToCopy[cur.random] : null;
            
            // Move to next node in original list
            cur = cur.next;
        }

        // ════════════════════════════════════════════════════════════════════
        // Return the head of the copied list
        // ════════════════════════════════════════════════════════════════════
        // The copied head is the value in our map corresponding to original head
        return oldToCopy[head];
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example: [[7,null],[13,0],[11,4],[10,2],[1,0]]
═══════════════════════════════════════════════════════════════════════════════

Original List Structure:
───────────────────────────────────────────────────────
Node 0: val=7,  next=Node 1, random=null
Node 1: val=13, next=Node 2, random=Node 0
Node 2: val=11, next=Node 3, random=Node 4
Node 3: val=10, next=Node 4, random=Node 2
Node 4: val=1,  next=null,   random=Node 0

Visual (next pointers shown with →, random pointers shown below):
7 → 13 → 11 → 10 → 1
↓    ↓    ↓    ↓    ↓
null 7    1    11   7

PASS 1: Create New Nodes and Build Mapping
───────────────────────────────────────────────────────

Iteration 1: cur = Node(7)
  - Create copy: Node'(7)
  - oldToCopy[Node(7)] = Node'(7)
  - Map: {Node(7) → Node'(7)}

Iteration 2: cur = Node(13)
  - Create copy: Node'(13)
  - oldToCopy[Node(13)] = Node'(13)
  - Map: {Node(7) → Node'(7), Node(13) → Node'(13)}

Iteration 3: cur = Node(11)
  - Create copy: Node'(11)
  - oldToCopy[Node(11)] = Node'(11)
  - Map: {Node(7) → Node'(7), Node(13) → Node'(13), Node(11) → Node'(11)}

Iteration 4: cur = Node(10)
  - Create copy: Node'(10)
  - oldToCopy[Node(10)] = Node'(10)
  - Map: {..., Node(10) → Node'(10)}

Iteration 5: cur = Node(1)
  - Create copy: Node'(1)
  - oldToCopy[Node(1)] = Node'(1)
  - Map: {Node(7) → Node'(7), Node(13) → Node'(13), 
          Node(11) → Node'(11), Node(10) → Node'(10), 
          Node(1) → Node'(1)}

After Pass 1:
  All new nodes created: Node'(7), Node'(13), Node'(11), Node'(10), Node'(1)
  But no connections yet (all next and random pointers are null)

PASS 2: Connect Next and Random Pointers
───────────────────────────────────────────────────────

Iteration 1: cur = Node(7)
  - copy = oldToCopy[Node(7)] = Node'(7)
  - copy.next = oldToCopy[Node(13)] = Node'(13)
  - copy.random = null (Node(7).random is null)
  
  Node'(7) state: next=Node'(13), random=null ✓

Iteration 2: cur = Node(13)
  - copy = oldToCopy[Node(13)] = Node'(13)
  - copy.next = oldToCopy[Node(11)] = Node'(11)
  - copy.random = oldToCopy[Node(7)] = Node'(7)
  
  Node'(13) state: next=Node'(11), random=Node'(7) ✓

Iteration 3: cur = Node(11)
  - copy = oldToCopy[Node(11)] = Node'(11)
  - copy.next = oldToCopy[Node(10)] = Node'(10)
  - copy.random = oldToCopy[Node(1)] = Node'(1)
  
  Node'(11) state: next=Node'(10), random=Node'(1) ✓

Iteration 4: cur = Node(10)
  - copy = oldToCopy[Node(10)] = Node'(10)
  - copy.next = oldToCopy[Node(1)] = Node'(1)
  - copy.random = oldToCopy[Node(11)] = Node'(11)
  
  Node'(10) state: next=Node'(1), random=Node'(11) ✓

Iteration 5: cur = Node(1)
  - copy = oldToCopy[Node(1)] = Node'(1)
  - copy.next = null (Node(1).next is null)
  - copy.random = oldToCopy[Node(7)] = Node'(7)
  
  Node'(1) state: next=null, random=Node'(7) ✓

FINAL RESULT:
───────────────────────────────────────────────────────
Copied List:
7' → 13' → 11' → 10' → 1'
↓     ↓     ↓     ↓     ↓
null  7'    1'    11'   7'

All pointers correctly point to NEW nodes, not original nodes ✓

═══════════════════════════════════════════════════════════════════════════════
🎯 DETAILED EXAMPLE - Self-Loop: [[1,1],[2,1]]
═══════════════════════════════════════════════════════════════════════════════

Original List:
───────────────────────────────────────────────────────
Node 0: val=1, next=Node 1, random=Node 1
Node 1: val=2, next=null,   random=Node 1 (points to itself!)

Visual:
1 → 2
↓   ↓
2   2 (self-loop)

PASS 1: Create Nodes
───────────────────────────────────────────────────────
oldToCopy = {Node(1) → Node'(1), Node(2) → Node'(2)}

PASS 2: Connect Pointers
───────────────────────────────────────────────────────

For Node(1):
  - Node'(1).next = oldToCopy[Node(2)] = Node'(2)
  - Node'(1).random = oldToCopy[Node(2)] = Node'(2)

For Node(2):
  - Node'(2).next = null
  - Node'(2).random = oldToCopy[Node(2)] = Node'(2) (self-loop!)

Result:
1' → 2'
↓    ↓
2'   2' (self-loop preserved) ✓

═══════════════════════════════════════════════════════════════════════════════
💡 WHY TWO PASSES ARE NECESSARY:
═══════════════════════════════════════════════════════════════════════════════

Problem: Random pointers can point to ANY node in the list

Example showing why single pass doesn't work:
───────────────────────────────────────────────────────
List: A → B → C
      ↓   ↓   ↓
      C   A   B

Single Pass Attempt:
  At node A: Want to set random=C', but C' doesn't exist yet!
  We're processing nodes left-to-right, but random can point forward

Two Pass Solution:
  Pass 1: Create A', B', C' (all nodes now exist)
  Pass 2: Connect pointers (can now lookup any node)

This is why we need the hash map and two passes!

═══════════════════════════════════════════════════════════════════════════════
🔄 ALTERNATIVE APPROACH: O(1) Space - Interweaving Method
═══════════════════════════════════════════════════════════════════════════════

More complex but uses O(1) extra space (not counting output):

STEP 1: Insert copied nodes between original nodes
───────────────────────────────────────────────────────
Original: A → B → C
After:    A → A' → B → B' → C → C'

Each original.next points to its copy, copy.next points to next original

STEP 2: Set random pointers for copied nodes
───────────────────────────────────────────────────────
If A.random = C, then A'.random = A.random.next = C.next = C'
We can find the copy of any node by going: node.random.next

STEP 3: Separate the lists
───────────────────────────────────────────────────────
Restore original list: A → B → C
Extract copied list:   A' → B' → C'

Implementation:
```csharp
// Step 1: Interweave
Node cur = head;
while (cur != null) {
    Node copy = new Node(cur.val);
    copy.next = cur.next;
    cur.next = copy;
    cur = copy.next;
}

// Step 2: Set random pointers
cur = head;
while (cur != null) {
    if (cur.random != null) {
        cur.next.random = cur.random.next;
    }
    cur = cur.next.next;
}

// Step 3: Separate lists
cur = head;
Node copyHead = head.next;
while (cur != null) {
    Node copy = cur.next;
    cur.next = copy.next;
    cur = cur.next;
    if (cur != null) {
        copy.next = cur.next;
    }
}
return copyHead;
```

Comparison:
✓ Time: O(n) for both approaches
✗ Space: O(1) vs O(n) (hash map approach)
✓ Simplicity: Hash map is much clearer
✗ Complexity: Interweaving is tricky to implement correctly

For interviews: Hash map approach is preferred unless explicitly asked for O(1) space.

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Empty list (null head):
   Input: null
   Output: null
   Handled by: Early return check ✓

✅ Single node with no random:
   Input: [[1,null]]
   Output: [[1,null]]
   copy.next = null, copy.random = null ✓

✅ Single node with self-loop:
   Input: [[1,0]]
   Node 0 random points to itself
   Output: [[1,0]]
   copy.random = oldToCopy[cur] = copy (points to itself) ✓

✅ All nodes have random=null:
   Input: [[1,null],[2,null],[3,null]]
   Output: [[1,null],[2,null],[3,null]]
   All copy.random = null ✓

✅ All random pointers point to head:
   Input: [[1,0],[2,0],[3,0]]
   Output: [[1,0],[2,0],[3,0]]
   All copy.random = oldToCopy[head] ✓

✅ Random pointers form a cycle:
   Input: [[1,1],[2,0]]
   Node 0 → Node 1 → null
   Node 0.random = Node 1
   Node 1.random = Node 0
   Forms a cycle through random pointers
   Correctly copied ✓

✅ Random pointer to last node:
   Input: [[1,2],[2,2],[3,null]]
   Multiple nodes point to last node
   All correctly point to copied last node ✓

✅ Complex graph structure:
   Random pointers create complex connections
   Hash map handles any structure ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Single pass without hash map
   - Can't set random pointers that point forward
   - Need to know all nodes exist first

❌ Copying nodes while connecting pointers
   - Creates incomplete mapping
   - Random pointers might point to non-existent nodes

❌ Forgetting to handle null pointers
   - cur.next might be null (end of list)
   - cur.random might be null (no random connection)
   - Must use ternary operator or null checks

❌ Returning original head instead of copied head
   - Must return oldToCopy[head], not head
   - Common copy-paste error

❌ Not handling empty list
   - head == null should return null
   - Dictionary lookup on null will throw exception

❌ Modifying original list
   - Should not change original list structure
   - Only read from it, write to copy

❌ Forgetting dictionary lookup can fail
   - In this problem, all nodes in random pointers are in the list
   - But good practice to check containsKey if unsure

❌ Creating nodes in second pass
   - All nodes must exist before connecting pointers
   - Otherwise random pointers to forward nodes fail

❌ Using reference equality incorrectly
   - Must use hash map, not comparing values
   - Multiple nodes can have same value

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **Deep Copy Pattern**:
   - Clone data structure with complex pointers
   - Hash map for old→new node mapping
   - Multi-pass processing

2. **Graph Cloning**:
   - Linked list is a directed graph
   - Each node can have multiple edges (next + random)
   - Applicable to general graph cloning

3. **Two-Pass Algorithm**:
   - Pass 1: Create all nodes
   - Pass 2: Connect relationships
   - Common when relationships can point in any direction

4. **Hash Map as Index**:
   - Map objects to their copies
   - O(1) lookup for any node
   - Enables efficient pointer translation

5. **Pointer Manipulation**:
   - Careful handling of next/random pointers
   - Null checking at every step
   - Preserving structure in copy

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Object Cloning in OOP**:
   - Deep copy vs shallow copy
   - Objects with circular references
   - Clone method implementation

2. **Undo/Redo Systems**:
   - Save state of complex data structures
   - Version control systems
   - Document editing history

3. **Game Development**:
   - Clone game states for AI simulation
   - Save/load game functionality
   - Multiplayer state synchronization

4. **Graph Databases**:
   - Clone subgraphs for analysis
   - Create snapshots
   - Duplicate data structures

5. **Memory Management**:
   - Garbage collection algorithms
   - Reference counting
   - Memory pooling

6. **Serialization/Deserialization**:
   - Convert objects to/from storage
   - Network transmission
   - Persistence layers

7. **Cache Invalidation**:
   - Clone data for cache entries
   - Prevent mutation of cached data
   - Thread-safe copies

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Clone Graph (LeetCode 133)**:
   - Similar concept but with general graph
   - Multiple neighbors instead of just next
   - Same hash map approach applies

2. **Serialize and Deserialize Binary Tree (LeetCode 297)**:
   - Convert tree to string and back
   - Must preserve structure
   - Similar deep copy concept

3. **Deep Copy with Arbitrary Pointer**:
   - Variations with different pointer types
   - Multiple random pointers
   - Same two-pass technique

4. **Flatten a Multilevel Doubly Linked List (LeetCode 430)**:
   - Different pointer structure
   - Similar traversal concepts
   - Pointer manipulation

5. **Copy List with Random Pointer II**:
   - Modify to return mappings
   - Track which nodes were copied
   - Additional metadata

6. **Clone N-ary Tree (LeetCode 1490)**:
   - Tree with variable children
   - Similar recursive/iterative approaches
   - Hash map for mapping

7. **Deep Copy of Linked List**:
   - Without random pointers (simpler)
   - Can do in single pass
   - Foundation for this problem

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Solution (Hash Map):

Strengths:
✓ O(n) time complexity - optimal
✓ Clear and easy to understand
✓ Easy to implement correctly
✓ Handles all cases naturally

Weaknesses:
✗ O(n) space for hash map
✗ Extra memory overhead

Alternative Optimization (Interweaving):

Strengths:
✓ O(1) auxiliary space
✓ Still O(n) time
✓ No hash map needed

Weaknesses:
✗ More complex to implement
✗ Temporarily modifies structure
✗ More bug-prone
✗ Harder to understand

When to Use Each:

Hash Map Approach:
- Default choice for interviews
- When clarity is priority
- When space is not critical
- Production code (maintainability)

Interweaving Approach:
- When asked specifically for O(1) space
- When memory is extremely constrained
- To demonstrate advanced technique
- Follow-up question in interview

Micro-optimizations:
1. Use ContainsKey before Get if concerned about exceptions
2. Could use KeyValuePair iteration instead of repeated lookups
3. Could combine null checks differently

But these don't change asymptotic complexity and hurt readability.

Best Practice:
- Use hash map approach by default
- Mention O(1) space alternative if asked
- Prioritize correctness and clarity over micro-optimizations

═══════════════════════════════════════════════════════════════════════════════
*/