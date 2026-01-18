/*
PROBLEM: Invert Binary Tree
Given the root of a binary tree, invert the tree, and return its root.

Example 1:
Input: root = [4,2,7,1,3,6,9]
Output: [4,7,2,9,6,3,1]

Visual:
        4                    4
       / \                  / \
      2   7      =>        7   2
     / \ / \              / \ / \
    1  3 6  9            9  6 3  1

Example 2:
Input: root = [2,1,3]
Output: [2,3,1]

Visual:
      2              2
     / \    =>      / \
    1   3          3   1

Example 3:
Input: root = []
Output: []

Constraints:
- The number of nodes in the tree is in the range [0, 100].
- -100 <= Node.val <= 100
*/

/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */

public class Solution {
    public TreeNode InvertTree(TreeNode root) {
        if (root == null) // Base case: empty tree or leaf's child
            return null;

        // Swap the left and right children
        TreeNode temp = root.left;
        root.left = root.right;
        root.right = temp;

        // Recursively invert left and right subtrees
        InvertTree(root.left);
        InvertTree(root.right);

        return root; // Return the root of inverted tree
    }
}

/*
TIME COMPLEXITY: O(n)
- Visit each node exactly once
- At each node, perform constant time swap operation
- n = number of nodes in the tree
- Overall: O(n)

SPACE COMPLEXITY: O(h) where h = height of tree
- Recursive call stack depth equals tree height
- Best case (balanced tree): O(log n)
- Worst case (skewed tree): O(n)
- No additional data structures used

ALGORITHM EXPLANATION:
- Use recursive DFS (Depth-First Search)
- At each node, swap its left and right children
- Recursively apply same operation to both subtrees
- Base case: null node returns immediately

KEY INSIGHTS:
- Inverting a tree = mirror image of original tree
- Each node's left child becomes right, right becomes left
- Recursion naturally handles all levels of the tree
- Order of operations: swap first, then recurse (pre-order)

THREE APPROACHES TO INVERT:
1. Pre-order (swap, then recurse) - This solution
2. Post-order (recurse, then swap) - Also valid
3. Level-order (BFS with queue) - Iterative approach

PRE-ORDER VS POST-ORDER:
Pre-order (this solution):
```
swap(node.left, node.right)
invert(node.left)
invert(node.right)
```

Post-order:
```
invert(node.left)
invert(node.right)
swap(node.left, node.right)
```

Both work! The order of swap doesn't matter because we're
swapping references, not the actual subtrees.

VISUAL TRACE with [4,2,7,1,3,6,9]:

Initial Tree:
        4
       / \
      2   7
     / \ / \
    1  3 6  9

Step 1: At node 4
- Swap children: 2 <-> 7
        4
       / \
      7   2
     / \ / \
    6  9 1  3

Step 2: Recurse left (node 7)
- Swap children: 6 <-> 9
        4
       / \
      7   2
     / \ / \
    9  6 1  3

Step 3: Recurse to node 9 (leaf) - no children to swap
Step 4: Recurse to node 6 (leaf) - no children to swap

Step 5: Recurse right (node 2)
- Swap children: 1 <-> 3
        4
       / \
      7   2
     / \ / \
    9  6 3  1

Step 6: Recurse to node 3 (leaf) - no children to swap
Step 7: Recurse to node 1 (leaf) - no children to swap

Final Inverted Tree:
        4
       / \
      7   2
     / \ / \
    9  6 3  1

RECURSION CALL STACK TRACE:
InvertTree(4)
├── swap(2, 7) -> 4 has children (7, 2)
├── InvertTree(7)
│   ├── swap(6, 9) -> 7 has children (9, 6)
│   ├── InvertTree(9) -> returns (leaf)
│   └── InvertTree(6) -> returns (leaf)
├── InvertTree(2)
│   ├── swap(1, 3) -> 2 has children (3, 1)
│   ├── InvertTree(3) -> returns (leaf)
│   └── InvertTree(1) -> returns (leaf)
└── return 4

EDGE CASES HANDLED:
- Empty tree (root = null):
  - Returns null immediately

- Single node (root = [1]):
  - Swap null with null (no effect)
  - Recurse on null children (base case)
  - Returns the single node

- Unbalanced tree:
  - Algorithm works the same
  - Null children are handled by base case

ITERATIVE SOLUTION (BFS with Queue):
```csharp
public TreeNode InvertTree(TreeNode root) {
    if (root == null) return null;

    Queue<TreeNode> queue = new Queue<TreeNode>();
    queue.Enqueue(root);

    while (queue.Count > 0) {
        TreeNode node = queue.Dequeue();

        // Swap children
        TreeNode temp = node.left;
        node.left = node.right;
        node.right = temp;

        // Add children to queue
        if (node.left != null) queue.Enqueue(node.left);
        if (node.right != null) queue.Enqueue(node.right);
    }

    return root;
}
```
Iterative: O(n) time, O(n) space (queue can hold n/2 nodes at leaf level)

PATTERN RECOGNITION:
- "Transform entire tree" -> DFS or BFS traversal
- "Mirror/Invert" -> swap left and right at each node
- "Recursive structure" -> tree problems often use recursion

COMMON MISTAKES TO AVOID:
- Forgetting base case (null check)
- Trying to swap values instead of node references
- Returning wrong node (should return root)
- Overcomplicating with unnecessary variables

WHY SWAP REFERENCES NOT VALUES?
Swapping just values would require:
- Swap all values in left subtree with right subtree
- Much more complex, O(n) swaps per level

Swapping references:
- Just change two pointers per node
- O(1) per node, total O(n)

REAL-WORLD APPLICATIONS:
- Mirror image generation
- UI layout transformations (RTL to LTR)
- Game development (flipping sprites/levels)
- Data structure transformations

RELATED PROBLEMS:
- Symmetric Tree (check if tree is mirror of itself)
- Same Tree (compare two trees)
- Maximum Depth of Binary Tree
- Binary Tree Level Order Traversal

INTERVIEW TIP:
This is a famous problem - it's simple but tests understanding of:
1. Tree traversal
2. Recursion
3. Reference manipulation
4. Base case handling

The interviewer may ask for both recursive and iterative solutions.
*/
