/*
PROBLEM: Diameter of Binary Tree
Given the root of a binary tree, return the length of the diameter of the tree.

The diameter of a binary tree is the length of the longest path between any two nodes
in a tree. This path may or may not pass through the root.

The length of a path between two nodes is represented by the number of edges between them.

Example 1:
Input: root = [1,2,3,4,5]
Output: 3

Visual:
        1
       / \
      2   3
     / \
    4   5

Longest path: 4 -> 2 -> 1 -> 3 OR 5 -> 2 -> 1 -> 3
Path length = 3 edges

Example 2:
Input: root = [1,2]
Output: 1

Visual:
    1
   /
  2

Longest path: 2 -> 1
Path length = 1 edge

Constraints:
- The number of nodes in the tree is in the range [1, 10^4].
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
    private int diameter = 0; // Global variable to track maximum diameter

    public int DiameterOfBinaryTree(TreeNode root) {
        Height(root); // Calculate heights and update diameter
        return diameter;
    }

    private int Height(TreeNode node) {
        if (node == null) // Base case: null node has height 0
            return 0;

        int leftHeight = Height(node.left);   // Height of left subtree
        int rightHeight = Height(node.right); // Height of right subtree

        // Diameter through this node = left height + right height
        diameter = Math.Max(diameter, leftHeight + rightHeight);

        // Return height of this subtree (max child height + 1)
        return Math.Max(leftHeight, rightHeight) + 1;
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
- Worst case (skewed tree): O(n)
- Only one integer variable for global state

ALGORITHM EXPLANATION:
- Use DFS to calculate height of each subtree
- At each node, potential diameter = left height + right height
- Track maximum diameter seen across all nodes
- Return height to parent for its calculation

KEY INSIGHTS:
- Diameter = longest path between ANY two nodes
- Path may or may NOT pass through root
- At each node, longest path through it = left height + right height
- Must check diameter at every node, not just root

CRITICAL OBSERVATION:
The diameter through a node = left_height + right_height

Why? The longest path through a node:
- Goes down to deepest node in left subtree
- Comes back up through current node
- Goes down to deepest node in right subtree

Number of EDGES = left_height + right_height

WHY EDGES NOT NODES?
- Height of subtree = number of edges to deepest leaf
- Left height + right height = total edges in path
- This is why we return 0 for null (not -1)

VISUAL TRACE with [1,2,3,4,5]:

Tree Structure:
        1
       / \
      2   3
     / \
    4   5

Step-by-step execution:

Height(1)
├── Height(2)
│   ├── Height(4)
│   │   ├── Height(null) = 0
│   │   └── Height(null) = 0
│   │   └── diameter = max(0, 0+0) = 0
│   │   └── return max(0,0) + 1 = 1
│   ├── Height(5)
│   │   ├── Height(null) = 0
│   │   └── Height(null) = 0
│   │   └── diameter = max(0, 0+0) = 0
│   │   └── return max(0,0) + 1 = 1
│   └── diameter = max(0, 1+1) = 2  <- Updated!
│   └── return max(1,1) + 1 = 2
├── Height(3)
│   ├── Height(null) = 0
│   └── Height(null) = 0
│   └── diameter = max(2, 0+0) = 2
│   └── return max(0,0) + 1 = 1
└── diameter = max(2, 2+1) = 3  <- Updated!
└── return max(2,1) + 1 = 3

Final diameter = 3

DIAMETER CALCULATION AT EACH NODE:
Node 4: left=0, right=0, diameter through 4 = 0+0 = 0
Node 5: left=0, right=0, diameter through 5 = 0+0 = 0
Node 2: left=1, right=1, diameter through 2 = 1+1 = 2 <- Path: 4-2-5
Node 3: left=0, right=0, diameter through 3 = 0+0 = 0
Node 1: left=2, right=1, diameter through 1 = 2+1 = 3 <- Path: 4-2-1-3 or 5-2-1-3

Maximum diameter = 3

EDGE CASES HANDLED:
- Single node (root = [1]):
  - left height = 0, right height = 0
  - diameter = 0 (no edges)
  - Returns 0

- Linear tree (linked list):
      1
       \
        2
         \
          3
  - Diameter = 2 (path: 1-2-3)

- Diameter not through root:
        1
       /
      2
     / \
    3   4
   /     \
  5       6

  Diameter = 4 (path: 5-3-2-4-6)
  Does NOT go through root 1!

WHY USE INSTANCE VARIABLE?
- Need to track global maximum across all recursive calls
- Function returns height, not diameter
- Can't return two values easily in C#
- Alternative: use tuple or out parameter

ALTERNATIVE: Using Tuple (No Instance Variable)
```csharp
public int DiameterOfBinaryTree(TreeNode root) {
    return GetHeightAndDiameter(root).diameter;
}

private (int height, int diameter) GetHeightAndDiameter(TreeNode node) {
    if (node == null) return (0, 0);

    var left = GetHeightAndDiameter(node.left);
    var right = GetHeightAndDiameter(node.right);

    int height = Math.Max(left.height, right.height) + 1;
    int diameterThroughNode = left.height + right.height;
    int diameter = Math.Max(diameterThroughNode,
                           Math.Max(left.diameter, right.diameter));

    return (height, diameter);
}
```

ALTERNATIVE: Using ref Parameter
```csharp
public int DiameterOfBinaryTree(TreeNode root) {
    int diameter = 0;
    Height(root, ref diameter);
    return diameter;
}

private int Height(TreeNode node, ref int diameter) {
    if (node == null) return 0;

    int left = Height(node.left, ref diameter);
    int right = Height(node.right, ref diameter);

    diameter = Math.Max(diameter, left + right);
    return Math.Max(left, right) + 1;
}
```

PATTERN RECOGNITION:
- "Longest path in tree" -> check path through each node
- "Global maximum tracking" -> instance variable or ref parameter
- "Path between two nodes" -> height-based calculation
- "May not pass through root" -> check every node as potential path center

COMMON MISTAKES TO AVOID:
- Only checking diameter through root
- Confusing nodes vs edges (diameter counts edges)
- Returning diameter instead of height from helper function
- Not updating global max before recursing further

RELATIONSHIP TO OTHER PROBLEMS:
- Maximum Depth: Height calculation is the same
- Balanced Binary Tree: Also compares left/right heights
- Binary Tree Maximum Path Sum: Similar pattern but with node values

HEIGHT VS DEPTH CLARIFICATION:
In this solution:
- Height(null) = 0
- Height(leaf) = 1
- Height measures edges from node to deepest descendant + 1

This works because:
- leftHeight + rightHeight = number of edges in path through node

REAL-WORLD APPLICATIONS:
- Network diameter (max latency between any two nodes)
- Social network analysis (max degrees of separation)
- File system depth analysis
- Game tree evaluation

INTERVIEW TIP:
This problem tests:
1. Understanding that diameter may not pass through root
2. Ability to track global state during recursion
3. Relationship between height and path length
4. Clean code with helper function pattern

Key insight to mention: "The diameter through any node equals
the sum of heights of its left and right subtrees"
*/
