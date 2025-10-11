public class Solution
{
    public int Search(int[] nums, int target)
    {
        int l = 0, r = nums.Length;

        while (l < r)
        {
            int m = l + (r - l) / 2;
            if (nums[m] >= target)
            {
                r = m;
            }
            else
            {
                l = m + 1;
            }
        }
        return (l < nums.Length && nums[l] == target) ? l : -1;
    }
}