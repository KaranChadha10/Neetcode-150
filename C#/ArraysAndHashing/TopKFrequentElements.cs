// Question: Given an integer array nums and an integer k, 
// return the k most frequent elements. 
// You may return the answer in any order.

public class Solution {
    public int[] TopKFrequent(int[] nums, int k) {
        // Step 1: Count frequency of each number
        Dictionary<int, int> count = new Dictionary<int, int>();

        // Step 2: Create "bucket" array where index = frequency
        List<int>[] freq = new List<int>[nums.Length + 1];
        for(int i = 0; i < freq.Length; i++){
            freq[i] = new List<int>();
        }

        // Fill frequency map
        foreach(int n in nums){
            if(count.ContainsKey(n)){
                count[n]++;
            } else {
                count[n] = 1;
            }
        }

        // Step 3: Place numbers into buckets by their frequency
        foreach(var entry in count){
            freq[entry.Value].Add(entry.Key);
        }

        // Step 4: Gather top k frequent numbers (from high freq → low freq)
        int[] res = new int[k];
        int index = 0;

        for(int i = freq.Length - 1; i > 0 && index < k; i--){
            foreach(int n in freq[i]){
                res[index++] = n;

                // Stop once we have k elements
                if(index == k){
                    return res;
                }
            }
        }

        return res;
    }
}

/*
Time Complexity: O(n)
 - Counting frequencies takes O(n).
 - Bucket placement takes O(n).
 - Collecting top k also takes O(n) in worst case.
 Overall → O(n).

Space Complexity: O(n)
 - Dictionary stores up to n unique elements.
 - Bucket array size = n + 1.
 Overall → O(n).

---------------------------------------------------
Dry Run Example:
nums = [1,1,1,2,2,3], k = 2

Step 1: Frequency count
count = { 1:3, 2:2, 3:1 }

Step 2: Bucket array (size = nums.Length+1 = 7)
Index = frequency
freq[1] = [3]
freq[2] = [2]
freq[3] = [1]

Step 3: Iterate from high freq to low
- i = 6 → empty
- i = 5 → empty
- i = 4 → empty
- i = 3 → [1] → res = [1]
- i = 2 → [2] → res = [1,2] → done (k=2)

Answer = [1,2]
*/
