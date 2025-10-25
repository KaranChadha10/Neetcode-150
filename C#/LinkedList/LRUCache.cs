/*
═══════════════════════════════════════════════════════════════════════════════
PROBLEM: 146. LRU Cache
═══════════════════════════════════════════════════════════════════════════════

📝 DESCRIPTION:
Design a data structure that follows the constraints of a Least Recently Used (LRU) 
cache.

Implement the LRUCache class:
- LRUCache(int capacity) Initialize the LRU cache with positive size capacity.
- int get(int key) Return the value of the key if the key exists, otherwise return -1.
- void put(int key, int value) Update the value of the key if the key exists. 
  Otherwise, add the key-value pair to the cache. If the number of keys exceeds 
  the capacity from this operation, evict the least recently used key.

The functions get and put must each run in O(1) average time complexity.

───────────────────────────────────────────────────────────────────────────────
📌 EXAMPLES:

Example 1:
Input:
["LRUCache", "put", "put", "get", "put", "get", "put", "get", "get", "get"]
[[2], [1, 1], [2, 2], [1], [3, 3], [2], [4, 4], [1], [3], [4]]

Output:
[null, null, null, 1, null, -1, null, -1, 3, 4]

Explanation:
LRUCache lRUCache = new LRUCache(2);
lRUCache.put(1, 1); // cache is {1=1}
lRUCache.put(2, 2); // cache is {1=1, 2=2}
lRUCache.get(1);    // return 1, cache is {2=2, 1=1} (1 most recently used)
lRUCache.put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
lRUCache.get(2);    // returns -1 (not found)
lRUCache.put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
lRUCache.get(1);    // return -1 (not found)
lRUCache.get(3);    // return 3
lRUCache.get(4);    // return 4

───────────────────────────────────────────────────────────────────────────────
🎯 CONSTRAINTS:
- 1 <= capacity <= 3000
- 0 <= key <= 10^4
- 0 <= value <= 10^5
- At most 2 * 10^5 calls will be made to get and put

───────────────────────────────────────────────────────────────────────────────
💡 KEY INSIGHTS:

1. **LRU Policy**: Evict the Least Recently Used item when capacity exceeded
   - "Recently used" means accessed by get OR updated by put
   - Need to track access order

2. **O(1) Requirements**: Both get and put must be O(1)
   - Dictionary for O(1) key lookup
   - Doubly linked list for O(1) insertion/removal/reordering

3. **Data Structure Combination**:
   - Hash Map: key → node (O(1) access to any node)
   - Doubly Linked List: maintains usage order
   - Most recent at end (near right dummy)
   - Least recent at start (near left dummy)

4. **Doubly Linked List Benefits**:
   - O(1) removal from anywhere (given node reference)
   - O(1) insertion at end
   - O(1) access to LRU item (always at left.Next)

5. **Dummy Nodes**: Simplify edge cases
   - left dummy: before first real node
   - right dummy: after last real node
   - No special cases for empty list or single element

───────────────────────────────────────────────────────────────────────────────
⏱️ COMPLEXITY ANALYSIS:

Constructor - LRUCache(capacity):
Time Complexity: O(1)
Space Complexity: O(1)
- Just initializes data structures

Get Operation:
Time Complexity: O(1)
- Dictionary lookup: O(1)
- Remove from list: O(1)
- Insert to list: O(1)
Space Complexity: O(1)
- No additional space

Put Operation:
Time Complexity: O(1)
- Dictionary lookup/insert: O(1)
- Remove from list: O(1)
- Insert to list: O(1)
- Eviction: O(1)
Space Complexity: O(1)
- One new node per put (bounded by capacity)

Overall Space: O(capacity)
- Dictionary stores at most capacity entries
- Doubly linked list stores at most capacity nodes

═══════════════════════════════════════════════════════════════════════════════
*/

// ════════════════════════════════════════════════════════════════════════════
// NODE CLASS: Doubly Linked List Node
// ════════════════════════════════════════════════════════════════════════════
/// <summary>
/// Represents a node in the doubly linked list
/// Stores key-value pair and pointers to previous/next nodes
/// </summary>
public class Node
{
    public int Key { get; set; }   // Key for dictionary lookup
    public int Val { get; set; }   // Cached value
    public Node Prev { get; set; } // Pointer to previous node
    public Node Next { get; set; } // Pointer to next node

    /// <summary>
    /// Constructor: Creates a new node with given key and value
    /// </summary>
    public Node(int key, int val)
    {
        Key = key;
        Val = val;
        Prev = null;
        Next = null;
    }
}

// ════════════════════════════════════════════════════════════════════════════
// LRU CACHE CLASS: Least Recently Used Cache Implementation
// ════════════════════════════════════════════════════════════════════════════
/// <summary>
/// LRU Cache with O(1) get and put operations
/// Uses HashMap + Doubly Linked List for optimal performance
/// 
/// Data Structure Layout:
/// Dictionary: {key → Node}
/// Linked List: left(dummy) ⟷ LRU ⟷ ... ⟷ MRU ⟷ right(dummy)
///              ↑                                    ↑
///              Least Recently Used            Most Recently Used
/// </summary>
public class LRUCache 
{
    // ════════════════════════════════════════════════════════════════════════
    // PRIVATE FIELDS
    // ════════════════════════════════════════════════════════════════════════
    
    private int cap;                          // Maximum cache capacity
    private Dictionary<int, Node> cache;      // Hash map for O(1) key lookup
    private Node left;                        // Dummy head (before LRU)
    private Node right;                       // Dummy tail (after MRU)
    
    // Why dummy nodes?
    // - Eliminates special cases for empty list
    // - No null checks needed for first/last nodes
    // - Simplifies insertion and removal logic
    
    // List structure:
    // left (dummy) ⟷ node1 ⟷ node2 ⟷ ... ⟷ nodeN ⟷ right (dummy)
    // ↑                                                    ↑
    // LRU position                                    MRU position

    // ════════════════════════════════════════════════════════════════════════
    // CONSTRUCTOR: Initialize LRU Cache
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Initializes the LRU cache with given capacity
    /// Sets up empty doubly linked list with dummy nodes
    /// </summary>
    /// <param name="capacity">Maximum number of items cache can hold</param>
    public LRUCache(int capacity) 
    {
        cap = capacity;
        cache = new Dictionary<int, Node>();
        
        // Create dummy head and tail nodes
        // These are never removed and simplify edge cases
        left = new Node(0, 0);   // Dummy head (LRU side)
        right = new Node(0, 0);  // Dummy tail (MRU side)
        
        // Connect dummy nodes to create empty list
        left.Next = right;
        right.Prev = left;
        
        // Initial state:
        // left ⟷ right
        // (empty cache, ready for insertions)
    }

    // ════════════════════════════════════════════════════════════════════════
    // HELPER METHOD: Remove node from linked list
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Removes a node from its current position in the doubly linked list
    /// Does NOT remove from dictionary - that's caller's responsibility
    /// Time Complexity: O(1)
    /// </summary>
    /// <param name="node">Node to remove from list</param>
    private void Remove(Node node)
    {
        // Get references to adjacent nodes
        Node prev = node.Prev;  // Node before the one we're removing
        Node nxt = node.Next;   // Node after the one we're removing
        
        // Bypass the node being removed
        // Before: prev ⟷ node ⟷ nxt
        // After:  prev ⟷ nxt
        prev.Next = nxt;
        nxt.Prev = prev;
        
        // Note: node's pointers are not cleared
        // This is fine since we'll either reinsert or let GC collect it
    }

    // ════════════════════════════════════════════════════════════════════════
    // HELPER METHOD: Insert node at end (most recently used position)
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Inserts a node at the end of the list (before right dummy)
    /// This marks the node as most recently used
    /// Time Complexity: O(1)
    /// </summary>
    /// <param name="node">Node to insert</param>
    private void Insert(Node node)
    {
        // Get the current last real node (right.Prev)
        Node prev = right.Prev;
        
        // Insert node between prev and right dummy
        // Before: ... ⟷ prev ⟷ right
        // After:  ... ⟷ prev ⟷ node ⟷ right
        
        prev.Next = node;      // prev now points to new node
        node.Prev = prev;      // new node points back to prev
        node.Next = right;     // new node points forward to right dummy
        right.Prev = node;     // right dummy points back to new node
        
        // Visual:
        // left ⟷ ... ⟷ prev ⟷ [node] ⟷ right
        //                         ↑
        //                    Most Recently Used
    }

    // ════════════════════════════════════════════════════════════════════════
    // GET: Retrieve value by key
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Returns the value associated with key if it exists, otherwise -1
    /// Accessing a key marks it as recently used (moves to end of list)
    /// Time Complexity: O(1)
    /// </summary>
    /// <param name="key">Key to look up</param>
    /// <returns>Value if found, -1 otherwise</returns>
    public int Get(int key) 
    {
        // ────────────────────────────────────────────────────────────────────
        // Check if key exists in cache
        // ────────────────────────────────────────────────────────────────────
        if (cache.ContainsKey(key))
        {
            // Key found! Update usage order
            Node node = cache[key];
            
            // ────────────────────────────────────────────────────────────────
            // Move node to end (mark as most recently used)
            // ────────────────────────────────────────────────────────────────
            // This is the key LRU behavior:
            // - Accessing an item updates its "last used" time
            // - We represent this by moving to end of list
            
            Remove(node);   // Remove from current position
            Insert(node);   // Reinsert at end (MRU position)
            
            // Dictionary still has same reference, no update needed
            return node.Val;
        }
        
        // Key not found
        return -1;
    }
    
    // ════════════════════════════════════════════════════════════════════════
    // PUT: Insert or update key-value pair
    // ════════════════════════════════════════════════════════════════════════
    /// <summary>
    /// Inserts or updates key-value pair in cache
    /// If key exists, updates value and marks as recently used
    /// If new key and cache full, evicts least recently used item
    /// Time Complexity: O(1)
    /// </summary>
    /// <param name="key">Key to insert/update</param>
    /// <param name="value">Value to store</param>
    public void Put(int key, int value) 
    {
        // ────────────────────────────────────────────────────────────────────
        // If key exists, remove old node
        // ────────────────────────────────────────────────────────────────────
        // We always create a new node (simpler than updating existing)
        // So if key exists, remove its old node from list
        if (cache.ContainsKey(key))
        {
            Remove(cache[key]);
        }
        
        // ────────────────────────────────────────────────────────────────────
        // Create new node and add to cache
        // ────────────────────────────────────────────────────────────────────
        Node newNode = new Node(key, value);
        cache[key] = newNode;      // Add/update in dictionary
        Insert(newNode);           // Add to end of list (MRU position)

        // ────────────────────────────────────────────────────────────────────
        // Evict LRU item if capacity exceeded
        // ────────────────────────────────────────────────────────────────────
        // After insertion, if we're over capacity, remove the LRU item
        // LRU item is always at left.Next (first real node after dummy)
        if (cache.Count > cap)
        {
            // Get the least recently used node (leftmost real node)
            Node lru = left.Next;
            
            // Remove from both data structures
            Remove(lru);              // Remove from linked list
            cache.Remove(lru.Key);    // Remove from dictionary
            
            // Important: Remove from dictionary using lru.Key
            // We can't use the original key parameter (might be different)
        }
    }
}

/*
═══════════════════════════════════════════════════════════════════════════════
🎯 ALGORITHM WALKTHROUGH - Example Operations (capacity = 2)
═══════════════════════════════════════════════════════════════════════════════

Initial State:
───────────────────────────────────────────────────────
cache = {}
List: left ⟷ right

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 1: put(1, 1)                                                     ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 1 not in cache
- Create Node(1, 1)
- Add to dictionary: cache = {1 → Node(1,1)}
- Insert at end: left ⟷ Node(1,1) ⟷ right
- Count = 1, cap = 2, no eviction

State:
  cache = {1 → Node(1,1)}
  List: left ⟷ [1:1] ⟷ right
        ↑       LRU/MRU   ↑

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 2: put(2, 2)                                                     ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 2 not in cache
- Create Node(2, 2)
- Add to dictionary: cache = {1 → Node(1,1), 2 → Node(2,2)}
- Insert at end: left ⟷ Node(1,1) ⟷ Node(2,2) ⟷ right
- Count = 2, cap = 2, no eviction

State:
  cache = {1 → Node(1,1), 2 → Node(2,2)}
  List: left ⟷ [1:1] ⟷ [2:2] ⟷ right
        ↑       LRU        MRU      ↑

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 3: get(1)                                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 1 found in cache
- Get node: Node(1,1)
- Remove from list: left ⟷ [2:2] ⟷ right
- Insert at end: left ⟷ [2:2] ⟷ [1:1] ⟷ right
- Return value: 1

State:
  cache = {1 → Node(1,1), 2 → Node(2,2)}
  List: left ⟷ [2:2] ⟷ [1:1] ⟷ right
        ↑       LRU        MRU      ↑
  
Order changed: 1 is now most recently used!

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 4: put(3, 3)                                                     ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 3 not in cache
- Create Node(3, 3)
- Add to dictionary: cache = {1→Node(1,1), 2→Node(2,2), 3→Node(3,3)}
- Insert at end: left ⟷ [2:2] ⟷ [1:1] ⟷ [3:3] ⟷ right
- Count = 3 > cap = 2, EVICTION NEEDED!
  
Eviction:
  - LRU = left.Next = Node(2,2)
  - Remove from list: left ⟷ [1:1] ⟷ [3:3] ⟷ right
  - Remove from dict: cache = {1→Node(1,1), 3→Node(3,3)}

State:
  cache = {1 → Node(1,1), 3 → Node(3,3)}
  List: left ⟷ [1:1] ⟷ [3:3] ⟷ right
        ↑       LRU        MRU      ↑
  
Key 2 evicted (was LRU)!

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 5: get(2)                                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 2 not in cache (was evicted)
- Return: -1

State: unchanged

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 6: put(4, 4)                                                     ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 4 not in cache
- Create Node(4, 4)
- Add to dictionary
- Insert at end: left ⟷ [1:1] ⟷ [3:3] ⟷ [4:4] ⟷ right
- Count = 3 > cap = 2, EVICTION!

Eviction:
  - LRU = left.Next = Node(1,1)
  - Remove from list: left ⟷ [3:3] ⟷ [4:4] ⟷ right
  - Remove from dict: cache = {3→Node(3,3), 4→Node(4,4)}

State:
  cache = {3 → Node(3,3), 4 → Node(4,4)}
  List: left ⟷ [3:3] ⟷ [4:4] ⟷ right
        ↑       LRU        MRU      ↑

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 7: get(1)                                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 1 not in cache (was evicted)
- Return: -1

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 8: get(3)                                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 3 found
- Move to end: left ⟷ [4:4] ⟷ [3:3] ⟷ right
- Return: 3

State:
  cache = {3 → Node(3,3), 4 → Node(4,4)}
  List: left ⟷ [4:4] ⟷ [3:3] ⟷ right
        ↑       LRU        MRU      ↑

╔════════════════════════════════════════════════════════════════════════════╗
║ Operation 9: get(4)                                                        ║
╚════════════════════════════════════════════════════════════════════════════╝

- Key 4 found
- Move to end: left ⟷ [3:3] ⟷ [4:4] ⟷ right
- Return: 4

Final State:
  cache = {3 → Node(3,3), 4 → Node(4,4)}
  List: left ⟷ [3:3] ⟷ [4:4] ⟷ right
        ↑       LRU        MRU      ↑

═══════════════════════════════════════════════════════════════════════════════
💡 WHY HASH MAP + DOUBLY LINKED LIST?
═══════════════════════════════════════════════════════════════════════════════

The Problem:
───────────────────────────────────────────────────────
Need to support O(1):
1. Access any element by key
2. Update usage order when accessed
3. Find least recently used element
4. Remove any element
5. Add element as most recently used

Solutions Comparison:
───────────────────────────────────────────────────────

APPROACH 1: Array/List Only
✗ get: O(n) - need to search
✗ Finding LRU: O(n) - need to scan all timestamps
Not acceptable!

APPROACH 2: Hash Map Only  
✓ get: O(1) - direct access
✗ Finding LRU: O(n) - need to find minimum timestamp
Not acceptable!

APPROACH 3: Linked List Only
✗ get: O(n) - need to search
✓ LRU at head: O(1)
Not acceptable!

APPROACH 4: Hash Map + Array
✓ get: O(1)
✗ Reordering: O(n) - need to shift elements
Not acceptable!

APPROACH 5: Hash Map + Doubly Linked List ✓
✓ get: O(1) - hash map lookup
✓ Finding LRU: O(1) - always at left.Next
✓ Remove any node: O(1) - have direct reference
✓ Insert at end: O(1) - simple pointer updates
✓ Update order: O(1) - remove + insert

THIS IS THE ONLY O(1) SOLUTION!

Why Doubly Linked vs Singly Linked?
───────────────────────────────────────────────────────
Singly Linked: Need O(n) to find previous node for removal
Doubly Linked: Have Prev pointer, O(1) removal!

Why Dummy Nodes?
───────────────────────────────────────────────────────
Without dummies:
- Special case for empty list
- Special case for removing head
- Special case for removing tail
- Null checks everywhere

With dummies:
- No special cases!
- Never null
- Simpler code
- Easier to maintain

═══════════════════════════════════════════════════════════════════════════════
✅ EDGE CASES & VALIDATION:
═══════════════════════════════════════════════════════════════════════════════

✅ Capacity 1:
   put(1,1), put(2,2)
   Result: Only key 2 remains ✓

✅ Update existing key:
   put(1,1), put(1,2)
   Result: Key 1 has value 2, count stays 1 ✓

✅ Get moves to end:
   put(1,1), put(2,2), get(1), put(3,3)
   Result: Key 1 stays, key 2 evicted ✓

✅ Multiple gets on same key:
   put(1,1), get(1), get(1), get(1)
   Result: Key 1 stays at MRU position ✓

✅ Eviction on update:
   Capacity 2: put(1,1), put(2,2), put(1,10), put(3,3)
   Result: Keys 1 and 3 remain ✓

✅ All operations on same key:
   put(1,1), get(1), put(1,2), get(1)
   Result: Always returns correct value ✓

✅ Immediate eviction:
   Capacity 1: put(1,1), put(2,2)
   Result: Only key 2 ✓

✅ Sequential operations:
   Alternating gets and puts maintain correct order ✓

═══════════════════════════════════════════════════════════════════════════════
🎓 COMMON MISTAKES TO AVOID:
═══════════════════════════════════════════════════════════════════════════════

❌ Not updating order on get
   - Get must mark item as recently used
   - Must remove and reinsert at end

❌ Forgetting to remove from dictionary on eviction
   - Must remove from both list AND dictionary
   - Memory leak otherwise

❌ Removing from dictionary before getting key
   - Need lru.Key before removing from dict
   - After removal, node is inaccessible

❌ Using singly linked list
   - Can't remove node in O(1) without previous pointer
   - Need doubly linked for O(1) removal

❌ Not handling key update
   - When putting existing key, must remove old node
   - Otherwise have two nodes with same key

❌ Wrong eviction condition
   - Check if count > cap (after insertion)
   - Not before insertion

❌ No dummy nodes
   - Leads to complex null checks
   - Special cases for empty list

❌ Wrong insertion position
   - Must insert at end (before right dummy)
   - Not at beginning

❌ Comparing Count with cap incorrectly
   - Check > not >=
   - We want to keep cap items, not cap-1

❌ Not removing old node on put of existing key
   - Creates duplicate nodes in list
   - Dictionary has wrong node reference

═══════════════════════════════════════════════════════════════════════════════
🌟 PATTERN RECOGNITION:
═══════════════════════════════════════════════════════════════════════════════

This problem demonstrates several important patterns:

1. **HashMap + Doubly Linked List**:
   - Combine two data structures for optimal performance
   - HashMap for O(1) access
   - DLL for O(1) ordering operations

2. **Cache Implementation**:
   - LRU is one of many cache eviction policies
   - Others: LFU, FIFO, Random
   - LRU balances temporal locality with simplicity

3. **Design Pattern**:
   - Dummy nodes pattern
   - Separation of concerns (remove/insert helpers)
   - Clean API design

4. **Order Maintenance**:
   - Implicit ordering through data structure
   - No timestamps needed
   - Position in list represents age

5. **Composite Data Structures**:
   - Neither structure alone is sufficient
   - Combination provides both benefits
   - Common in advanced data structures

═══════════════════════════════════════════════════════════════════════════════
🏢 REAL-WORLD APPLICATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **Operating Systems**:
   - Page replacement in virtual memory
   - Disk cache management
   - File system buffer cache

2. **Web Browsers**:
   - Browser cache for web pages
   - Image caching
   - DNS cache

3. **Databases**:
   - Query result caching
   - Buffer pool management
   - Connection pooling

4. **CDN (Content Delivery Networks)**:
   - Edge server caching
   - Popular content stays cached
   - Optimize bandwidth usage

5. **Application Caches**:
   - Redis, Memcached
   - API response caching
   - Session storage

6. **Mobile Apps**:
   - Image caching
   - API response caching
   - Offline data management

7. **Search Engines**:
   - Query result caching
   - Frequently searched terms
   - Personalized results

═══════════════════════════════════════════════════════════════════════════════
📚 RELATED PROBLEMS & VARIATIONS:
═══════════════════════════════════════════════════════════════════════════════

1. **LFU Cache (LeetCode 460)**:
   - Least Frequently Used instead of Least Recently
   - More complex: need frequency tracking
   - HashMap + multiple DLLs

2. **Design HashMap (LeetCode 706)**:
   - Implement HashMap from scratch
   - Hash functions, collision handling
   - Foundation for this problem

3. **All O(1) Data Structure (LeetCode 432)**:
   - Support inc, dec, getMax, getMin all in O(1)
   - Similar pattern: HashMap + DLL
   - More complex state management

4. **Design Browser History (LeetCode 1472)**:
   - Back/forward navigation
   - Similar doubly linked list usage
   - Simpler than LRU

5. **Snapshot Array (LeetCode 1146)**:
   - Set, get, and snap operations
   - Version control pattern
   - Different caching strategy

6. **Time Based Key-Value Store (LeetCode 981)**:
   - Temporal data with binary search
   - Different access pattern
   - Complementary to LRU

7. **Design In-Memory File System (LeetCode 588)**:
   - Trie + caching
   - Complex data structure design
   - Similar design principles

═══════════════════════════════════════════════════════════════════════════════
🚀 OPTIMIZATION INSIGHTS:
═══════════════════════════════════════════════════════════════════════════════

Current Implementation:

Time Complexity: O(1) - Optimal
✓ All operations constant time
✓ Cannot improve asymptotically

Space Complexity: O(capacity)
✓ Stores exactly as many items as needed
✓ Minimal overhead (prev/next pointers)

Potential Micro-optimizations:

1. **Node Pooling**:
   - Reuse removed nodes instead of creating new
   - Reduces GC pressure
   - Marginal benefit in C#

2. **Struct vs Class for Node**:
   - Could use struct to reduce heap allocations
   - Complex because of reference semantics needed
   - Not recommended

3. **Avoiding Re-insertion on Update**:
   - Current: Remove old node, create new
   - Alternative: Update value in place if already at end
   - Adds complexity for minimal gain

4. **Single Dictionary Entry**:
   - Current: Store Node reference
   - Alternative: Store both value and Node
   - More memory, no performance gain

Best Practices:
✓ Current implementation is optimal
✓ Clean separation of concerns
✓ Easy to understand and maintain
✓ Production-ready code

When to Consider Alternatives:
- LFU: When frequency matters more than recency
- FIFO: When simpler policy acceptable
- Random: When simplicity priority over hit rate
- Adaptive: Combine policies based on workload

Performance Characteristics:
- Hit rate: ~70-90% for typical workloads
- Best for: Temporal locality (recent items accessed again)
- Worst for: Sequential scans (everything accessed once)

Real-World Tuning:
- Capacity: Balance memory vs hit rate
- Eviction policy: LRU good default
- Monitoring: Track hit/miss ratio
- Adaptive sizing: Adjust capacity dynamically

═══════════════════════════════════════════════════════════════════════════════
*/

/**
 * Your LRUCache object will be instantiated and called as such:
 * LRUCache obj = new LRUCache(capacity);
 * int param_1 = obj.Get(key);
 * obj.Put(key,value);
 */