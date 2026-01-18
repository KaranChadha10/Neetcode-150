/*
PROBLEM: Same Tree
Given the roots of two binary trees p and q, write a function to check if they
are the same or not.

Two binary trees are considered the same if they are structurally identical,
and the nodes have the same value.

Example 1:
Input: p = [1,2,3], q = [1,2,3]
Output: true

Visual:
    p:      q:
    1       1
   / \     / \
  2   3   2   3

Both trees are identical in structure and values.

Example 2:
Input: p = [1,2], q = [1,null,2]
Output: false

Visual:
    p:      q:
    1       1
   /         \
  2           2

Same values but different structure.

Example 3:
Input: p = [1,2,1], q = [1,1,2]
Output: false

Visual:
    p:      q:
    1       1
   / \     / \
  2   1   1   2

Same structure but different values at corresponding positions.

Constraints:
- The number of nodes in both trees is in the range [0, 100].
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
    public bool IsSameTree(TreeNode p, TreeNode q) {
        // Base case 1: Both nodes are null
        if (p == null && q == null)
            return true;

        // Base case 2: One is null, other is not
        if (p == null || q == null)
            return false;

        // Base case 3: Values don't match
        if (p.val != q.val)
            return false;

        // Recursive case: Check both subtrees
        return IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
    }
}

/*
TIME COMPLEXITY: O(min(n, m))
- Visit each node once in smaller tree
- n = nodes in tree p, m = nodes in tree q
- Early termination when mismatch found
- Best case: O(1) if roots differ
- Worst case: O(n) when trees are identical

SPACE COMPLEXITY: O(min(h1, h2))
- Recursive call stack depth equals min height
- h1 = height of p, h2 = height of q
- Best case (balanced): O(log n)
- Worst case (skewed): O(n)

ALGORITHM EXPLANATION:
- Use recursive DFS to compare trees simultaneously
- Check three conditions at each step:
  1. Both null -> same (base case success)
  2. One null, one not -> different (structure mismatch)
  3. Values different -> different
- If current nodes match, recursively check children

KEY INSIGHTS:
- "Same tree" means BOTH structure AND values must match
- Null nodes are valid and must match with null
- Short-circuit evaluation: fails fast on first mismatch
- Order matters: compare left with left, right with right

THREE BASE CASES EXPLAINED:

Case 1: Both null
```
p = null, q = null
```
Two empty subtrees are considered the same.
Return true.

Case 2: One null, one not
```
p = [1], q = null  OR  p = null, q = [1]
```
Structural difference - one has node, other doesn't.
Return false.

Case 3: Different values
```
p.val = 5, q.val = 7
```
Same structure at this point but different content.
Return false.

VISUAL TRACE with p=[1,2,3], q=[1,2,3]:

Trees:
    p:      q:
    1       1
   / \     / \
  2   3   2   3

IsSameTree(1, 1)
├── p=1, q=1, both not null
├── p.val(1) == q.val(1) ✓
├── IsSameTree(2, 2)
│   ├── p=2, q=2, both not null
│   ├── p.val(2) == q.val(2) ✓
│   ├── IsSameTree(null, null) = true
│   └── IsSameTree(null, null) = true
│   └── return true && true = true
├── IsSameTree(3, 3)
│   ├── p=3, q=3, both not null
│   ├── p.val(3) == q.val(3) ✓
│   ├── IsSameTree(null, null) = true
│   └── IsSameTree(null, null) = true
│   └── return true && true = true
└── return true && true = true

Result: true

TRACE FOR DIFFERENT TREES p=[1,2], q=[1,null,2]:

Trees:
    p:      q:
    1       1
   /         \
  2           2

IsSameTree(1, 1)
├── p=1, q=1, both not null
├── p.val(1) == q.val(1) ✓
├── IsSameTree(2, null)     <- Left children
│   ├── p=2 (not null), q=null
│   └── return false        <- One null, one not!
└── return false && ... = false  <- Short-circuits

Result: false

EDGE CASES HANDLED:
- Both empty trees (p = null, q = null):
  - First condition: both null
  - Returns true

- One empty, one not:
  - Second condition catches this
  - Returns false

- Single nodes with same value:
  - Compares values, then compares null children
  - Returns true

- Single nodes with different values:
  - Third condition catches this
  - Returns false

ITERATIVE SOLUTION (Using Queue - BFS):
```csharp
public bool IsSameTree(TreeNode p, TreeNode q) {
    Queue<(TreeNode, TreeNode)> queue = new Queue<(TreeNode, TreeNode)>();
    queue.Enqueue((p, q));

    while (queue.Count > 0) {
        var (node1, node2) = queue.Dequeue();

        if (node1 == null && node2 == null) continue;
        if (node1 == null || node2 == null) return false;
        if (node1.val != node2.val) return false;

        queue.Enqueue((node1.left, node2.left));
        queue.Enqueue((node1.right, node2.right));
    }

    return true;
}
```
BFS: O(min(n,m)) time, O(min(n,m)) space

ITERATIVE SOLUTION (Using Stack - DFS):
```csharp
public bool IsSameTree(TreeNode p, TreeNode q) {
    Stack<(TreeNode, TreeNode)> stack = new Stack<(TreeNode, TreeNode)>();
    stack.Push((p, q));

    while (stack.Count > 0) {
        var (node1, node2) = stack.Pop();

        if (node1 == null && node2 == null) continue;
        if (node1 == null || node2 == null) return false;
        if (node1.val != node2.val) return false;

        stack.Push((node1.right, node2.right));
        stack.Push((node1.left, node2.left));
    }

    return true;
}
```
DFS with stack: O(min(n,m)) time, O(min(h1,h2)) space

COMPACT ONE-LINER (Less Readable):
```csharp
public bool IsSameTree(TreeNode p, TreeNode q) {
    if (p == null || q == null) return p == q;
    return p.val == q.val && IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
}
```

PATTERN RECOGNITION:
- "Compare two trees" -> parallel DFS traversal
- "Check structure and values" -> three-way null/value check
- "Structural comparison" -> recursion on corresponding children

COMMON MISTAKES TO AVOID:
- Comparing p.left with q.right (wrong pairing)
- Not handling both-null case correctly
- Forgetting null checks before accessing .val
- Using || instead of && for recursive calls

RELATIONSHIP TO OTHER PROBLEMS:
- Symmetric Tree: Check if tree is mirror of itself
  - Similar but compares left with right
- Subtree of Another Tree: Uses isSameTree as helper
- Merge Two Binary Trees: Similar traversal pattern

SYMMETRIC TREE COMPARISON:
```
Same Tree:              Symmetric Tree:
Compare p.left          Compare root.left
    with q.left             with root.right (mirrored!)

IsSameTree(p.left, q.left)   IsSymmetric(left, right)
IsSameTree(p.right, q.right) ->  left.val == right.val
                                 IsSymmetric(left.left, right.right)
                                 IsSymmetric(left.right, right.left)
```

COMPARISON MATRIX:
| p      | q      | Result |
|--------|--------|--------|
| null   | null   | true   |
| null   | node   | false  |
| node   | null   | false  |
| val=x  | val=x  | check children |
| val=x  | val=y  | false  |

REAL-WORLD APPLICATIONS:
- DOM tree comparison (React virtual DOM diffing)
- File system structure comparison
- XML/JSON document comparison
- Version control tree merging
- Compiler AST comparison

INTERVIEW TIP:
This problem tests:
1. Handling null cases properly
2. Understanding tree structure vs values
3. Recursive thinking with two inputs
4. Short-circuit evaluation awareness

Start by clarifying: "Same means both structure AND values must match"
Then discuss the three cases before coding.
*/
