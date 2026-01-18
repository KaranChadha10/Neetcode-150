/*
PROBLEM: Maximum Depth of Binary Tree
Given the root of a binary tree, return its maximum depth.

A binary tree's maximum depth is the number of nodes along the longest path
from the root node down to the farthest leaf node.

Example 1:
Input: root = [3,9,20,null,null,15,7]
Output: 3

Visual:
      3        <- Level 1
     / \
    9  20      <- Level 2
       / \
      15  7    <- Level 3

Maximum depth = 3

Example 2:
Input: root = [1,null,2]
Output: 2

Visual:
    1          <- Level 1
     \
      2        <- Level 2

Maximum depth = 2

Example 3:
Input: root = []
Output: 0

Constraints:
- The number of nodes in the tree is in the range [0, 10^4].
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
    public int MaxDepth(TreeNode root) {
        if (root == null) // Base case: null node has depth 0
            return 0;

        int leftDepth = MaxDepth(root.left);   // Get depth of left subtree
        int rightDepth = MaxDepth(root.right); // Get depth of right subtree

        return Math.Max(leftDepth, rightDepth) + 1; // Max of children + 1 for current node
    }
}

/*
TIME COMPLEXITY: O(n)
- Visit each node exactly once
- At each node, perform constant time operations
- n = number of nodes in the tree
- Overall: O(n)

SPACE COMPLEXITY: O(h) where h = height of tree
- Recursive call stack depth equals tree height
- Best case (balanced tree): O(log n)
- Worst case (skewed tree/linked list): O(n)
- No additional data structures used

ALGORITHM EXPLANATION:
- Use recursive DFS (Depth-First Search)
- Base case: null node has depth 0
- Recursive case: depth = 1 + max(left depth, right depth)
- Post-order traversal: process children before current node

KEY INSIGHTS:
- Depth = number of nodes from root to deepest leaf
- Each node adds 1 to the depth
- Maximum depth = 1 + max(left subtree depth, right subtree depth)
- Recursion naturally explores all paths

RECURSIVE FORMULA:
maxDepth(node) =
    0                                          if node is null
    1 + max(maxDepth(left), maxDepth(right))   otherwise

VISUAL TRACE with [3,9,20,null,null,15,7]:

Tree Structure:
      3
     / \
    9  20
       / \
      15  7

Execution (post-order):
1. MaxDepth(3)
   ├── MaxDepth(9)
   │   ├── MaxDepth(null) = 0
   │   └── MaxDepth(null) = 0
   │   └── return max(0,0) + 1 = 1
   ├── MaxDepth(20)
   │   ├── MaxDepth(15)
   │   │   ├── MaxDepth(null) = 0
   │   │   └── MaxDepth(null) = 0
   │   │   └── return max(0,0) + 1 = 1
   │   ├── MaxDepth(7)
   │   │   ├── MaxDepth(null) = 0
   │   │   └── MaxDepth(null) = 0
   │   │   └── return max(0,0) + 1 = 1
   │   └── return max(1,1) + 1 = 2
   └── return max(1,2) + 1 = 3

Final Answer: 3

STEP-BY-STEP BREAKDOWN:
Node 9:  max(0, 0) + 1 = 1
Node 15: max(0, 0) + 1 = 1
Node 7:  max(0, 0) + 1 = 1
Node 20: max(1, 1) + 1 = 2
Node 3:  max(1, 2) + 1 = 3 <- Maximum Depth

EDGE CASES HANDLED:
- Empty tree (root = null):
  - Returns 0 immediately

- Single node (root = [1]):
  - left = MaxDepth(null) = 0
  - right = MaxDepth(null) = 0
  - Returns max(0, 0) + 1 = 1

- Completely unbalanced (linked list):
    1
     \
      2
       \
        3
  - Depth = 3, works correctly

ITERATIVE SOLUTION (BFS - Level Order):
```csharp
public int MaxDepth(TreeNode root) {
    if (root == null) return 0;

    Queue<TreeNode> queue = new Queue<TreeNode>();
    queue.Enqueue(root);
    int depth = 0;

    while (queue.Count > 0) {
        int levelSize = queue.Count; // Nodes at current level
        depth++;

        for (int i = 0; i < levelSize; i++) {
            TreeNode node = queue.Dequeue();
            if (node.left != null) queue.Enqueue(node.left);
            if (node.right != null) queue.Enqueue(node.right);
        }
    }

    return depth;
}
```
BFS: O(n) time, O(w) space where w = max width of tree

ITERATIVE SOLUTION (DFS with Stack):
```csharp
public int MaxDepth(TreeNode root) {
    if (root == null) return 0;

    Stack<(TreeNode node, int depth)> stack = new Stack<(TreeNode, int)>();
    stack.Push((root, 1));
    int maxDepth = 0;

    while (stack.Count > 0) {
        var (node, depth) = stack.Pop();
        maxDepth = Math.Max(maxDepth, depth);

        if (node.right != null) stack.Push((node.right, depth + 1));
        if (node.left != null) stack.Push((node.left, depth + 1));
    }

    return maxDepth;
}
```
DFS with stack: O(n) time, O(h) space

COMPARISON OF APPROACHES:
| Approach      | Time | Space    | Notes                    |
|---------------|------|----------|--------------------------|
| Recursive DFS | O(n) | O(h)     | Clean, intuitive         |
| Iterative BFS | O(n) | O(w)     | Counts levels directly   |
| Iterative DFS | O(n) | O(h)     | Explicit stack           |

h = height, w = max width
For balanced tree: h = log(n), w = n/2
For skewed tree: h = n, w = 1

PATTERN RECOGNITION:
- "Maximum depth" -> DFS with max comparison
- "Tree height" -> same as maximum depth
- "Longest path from root" -> recursive subtree comparison
- "Level counting" -> BFS alternative

COMMON MISTAKES TO AVOID:
- Counting edges instead of nodes (depth vs height confusion)
- Forgetting base case (null check)
- Not taking max of both subtrees
- Off-by-one error (forgetting +1 for current node)

DEPTH VS HEIGHT TERMINOLOGY:
- Depth: distance from root to node (root has depth 1 or 0 depending on definition)
- Height: distance from node to deepest leaf
- Maximum depth of tree = height of tree (when root depth = 1)

Note: LeetCode uses depth counting from 1 (root = depth 1)

RELATED PROBLEMS:
- Minimum Depth of Binary Tree
- Balanced Binary Tree (uses height comparison)
- Diameter of Binary Tree
- Binary Tree Level Order Traversal

REAL-WORLD APPLICATIONS:
- Measuring tree balance for self-balancing trees (AVL, Red-Black)
- Estimating worst-case search time in BST
- Calculating recursion stack requirements
- Database index depth analysis

INTERVIEW TIP:
This problem tests:
1. Understanding of tree recursion
2. Base case handling
3. Post-order traversal concept
4. Ability to write clean, simple code

Often used as warm-up before harder tree problems.
The interviewer may ask for iterative solution as follow-up.
*/
