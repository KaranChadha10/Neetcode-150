/*
PROBLEM: Balanced Binary Tree
Given a binary tree, determine if it is height-balanced.

A height-balanced binary tree is a binary tree in which the depth of the two
subtrees of every node never differs by more than one.

Example 1:
Input: root = [3,9,20,null,null,15,7]
Output: true

Visual:
        3
       / \
      9  20
         / \
        15  7

Heights: left=1, right=2
Difference = |1-2| = 1 <= 1, so balanced at root
All subtrees also balanced.

Example 2:
Input: root = [1,2,2,3,3,null,null,4,4]
Output: false

Visual:
          1
         / \
        2   2
       / \
      3   3
     / \
    4   4

At node 1: left height=3, right height=1
Difference = |3-1| = 2 > 1, NOT balanced

Example 3:
Input: root = []
Output: true

Constraints:
- The number of nodes in the tree is in the range [0, 5000].
- -10^4 <= Node.val <= 10^4
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
    public bool IsBalanced(TreeNode root) {
        return CheckHeight(root) != -1; // -1 indicates unbalanced
    }

    private int CheckHeight(TreeNode node) {
        if (node == null) // Base case: null node has height 0
            return 0;

        int leftHeight = CheckHeight(node.left);   // Check left subtree
        if (leftHeight == -1) return -1;           // Left subtree unbalanced

        int rightHeight = CheckHeight(node.right); // Check right subtree
        if (rightHeight == -1) return -1;          // Right subtree unbalanced

        // Check if current node is balanced
        if (Math.Abs(leftHeight - rightHeight) > 1)
            return -1; // Current node unbalanced

        return Math.Max(leftHeight, rightHeight) + 1; // Return height if balanced
    }
}

/*
TIME COMPLEXITY: O(n)
- Visit each node exactly once
- Early termination when unbalanced subtree found
- n = number of nodes in the tree
- Overall: O(n)

SPACE COMPLEXITY: O(h) where h = height of tree
- Recursive call stack depth equals tree height
- Best case (balanced tree): O(log n)
- Worst case (skewed tree): O(n)
- No additional data structures

ALGORITHM EXPLANATION:
- Use DFS to calculate height of each subtree
- Return -1 as sentinel value for unbalanced subtree
- At each node, check if |left height - right height| <= 1
- Early termination: propagate -1 up immediately

KEY INSIGHTS:
- Balanced = every node's subtrees differ in height by at most 1
- Must check EVERY node, not just root
- Use -1 as sentinel to signal "unbalanced" up the call stack
- Combines height calculation with balance check in single pass

WHY USE -1 AS SENTINEL?
- Height is always >= 0 for valid subtrees
- -1 can uniquely indicate "unbalanced"
- Allows early termination without extra boolean
- Propagates failure up efficiently

NAIVE APPROACH (O(n²) - Less Optimal):
```csharp
public bool IsBalanced(TreeNode root) {
    if (root == null) return true;

    int leftHeight = GetHeight(root.left);
    int rightHeight = GetHeight(root.right);

    return Math.Abs(leftHeight - rightHeight) <= 1
           && IsBalanced(root.left)
           && IsBalanced(root.right);
}

private int GetHeight(TreeNode node) {
    if (node == null) return 0;
    return Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
}
```
This recalculates height multiple times -> O(n²)

OPTIMIZED APPROACH (This Solution - O(n)):
- Calculate height and check balance in same traversal
- Use sentinel value to propagate unbalanced state
- Single pass through tree

VISUAL TRACE with [1,2,2,3,3,null,null,4,4]:

Tree Structure:
          1
         / \
        2   2
       / \
      3   3
     / \
    4   4

Execution:
CheckHeight(1)
├── CheckHeight(2-left)
│   ├── CheckHeight(3-left)
│   │   ├── CheckHeight(4-left)
│   │   │   ├── CheckHeight(null) = 0
│   │   │   └── CheckHeight(null) = 0
│   │   │   └── |0-0| = 0 <= 1 ✓
│   │   │   └── return 1
│   │   ├── CheckHeight(4-right)
│   │   │   ├── CheckHeight(null) = 0
│   │   │   └── CheckHeight(null) = 0
│   │   │   └── |0-0| = 0 <= 1 ✓
│   │   │   └── return 1
│   │   └── |1-1| = 0 <= 1 ✓
│   │   └── return 2
│   ├── CheckHeight(3-right)
│   │   ├── CheckHeight(null) = 0
│   │   └── CheckHeight(null) = 0
│   │   └── |0-0| = 0 <= 1 ✓
│   │   └── return 1
│   └── |2-1| = 1 <= 1 ✓
│   └── return 3
├── CheckHeight(2-right)
│   ├── CheckHeight(null) = 0
│   └── CheckHeight(null) = 0
│   └── |0-0| = 0 <= 1 ✓
│   └── return 1
└── |3-1| = 2 > 1 ✗ UNBALANCED!
└── return -1

Result: CheckHeight returns -1, so IsBalanced returns false

BALANCED TREE TRACE with [3,9,20,null,null,15,7]:

Tree Structure:
        3
       / \
      9  20
         / \
        15  7

CheckHeight(3)
├── CheckHeight(9)
│   ├── CheckHeight(null) = 0
│   └── CheckHeight(null) = 0
│   └── |0-0| = 0 <= 1 ✓
│   └── return 1
├── CheckHeight(20)
│   ├── CheckHeight(15)
│   │   └── return 1
│   ├── CheckHeight(7)
│   │   └── return 1
│   └── |1-1| = 0 <= 1 ✓
│   └── return 2
└── |1-2| = 1 <= 1 ✓
└── return 3

Result: CheckHeight returns 3 (not -1), so IsBalanced returns true

EDGE CASES HANDLED:
- Empty tree (root = null):
  - CheckHeight returns 0
  - 0 != -1, so returns true
  - Empty tree is considered balanced

- Single node (root = [1]):
  - left = 0, right = 0
  - |0-0| = 0 <= 1 ✓
  - Returns true

- Linear tree (linked list):
    1
     \
      2
       \
        3
  - At node 1: left=0, right=2
  - |0-2| = 2 > 1 ✗
  - Returns false (correctly unbalanced)

EARLY TERMINATION BENEFIT:
If left subtree is unbalanced:
```
if (leftHeight == -1) return -1; // Don't even check right!
```
This saves time by not exploring the right subtree.

ALTERNATIVE: Using Boolean Flag
```csharp
private bool isBalanced = true;

public bool IsBalanced(TreeNode root) {
    isBalanced = true;
    CheckHeight(root);
    return isBalanced;
}

private int CheckHeight(TreeNode node) {
    if (node == null || !isBalanced) return 0;

    int left = CheckHeight(node.left);
    int right = CheckHeight(node.right);

    if (Math.Abs(left - right) > 1) {
        isBalanced = false;
    }

    return Math.Max(left, right) + 1;
}
```

ALTERNATIVE: Using Tuple
```csharp
public bool IsBalanced(TreeNode root) {
    return Check(root).isBalanced;
}

private (bool isBalanced, int height) Check(TreeNode node) {
    if (node == null) return (true, 0);

    var left = Check(node.left);
    if (!left.isBalanced) return (false, 0);

    var right = Check(node.right);
    if (!right.isBalanced) return (false, 0);

    bool balanced = Math.Abs(left.height - right.height) <= 1;
    int height = Math.Max(left.height, right.height) + 1;

    return (balanced, height);
}
```

PATTERN RECOGNITION:
- "Check property at every node" -> DFS traversal
- "Height comparison" -> similar to max depth
- "Early termination on failure" -> sentinel value pattern
- "Avoid redundant computation" -> combine checks in single pass

COMMON MISTAKES TO AVOID:
- Only checking balance at root
- Recalculating height separately (O(n²) approach)
- Not handling the sentinel value correctly
- Forgetting to check both subtrees

RELATIONSHIP TO OTHER PROBLEMS:
- Maximum Depth: Height calculation is identical
- Diameter: Also uses height calculation
- Validate BST: Similar pattern of checking property at every node

BALANCED TREE PROPERTIES:
For a balanced tree with n nodes:
- Height h = O(log n)
- Search/Insert/Delete = O(log n)
- This is why AVL and Red-Black trees maintain balance

REAL-WORLD APPLICATIONS:
- AVL tree validation
- Red-Black tree checking
- Database index health check
- Search tree optimization verification

INTERVIEW TIP:
This problem tests:
1. Understanding of balanced tree definition
2. Optimization from O(n²) to O(n)
3. Sentinel value pattern for early termination
4. Combining multiple checks in single traversal

Mention the naive O(n²) approach first, then optimize to show problem-solving skills.
*/
