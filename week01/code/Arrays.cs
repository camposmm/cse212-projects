using System.Collections.Generic;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // TODO Problem 1 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.

        // PLAN:
        // 1) Create a new double array of size 'length'.
        // 2) Loop from i = 0 to i < length.
        // 3) Compute the next multiple as number * (i + 1).
        // 4) Store it in the array at index i.
        // 5) Return the array.

        double[] results = new double[length];

        for (int i = 0; i < length; i++)
        {
            results[i] = number * (i + 1);
        }

        return results;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // TODO Problem 2 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.

        // PLAN:
        // 1) Rotating right by 'amount' means the last 'amount' elements move to the front.
        // 2) Find where the "tail" starts: splitIndex = data.Count - amount.
        // 3) Make two slices:
        //    tail = data.GetRange(splitIndex, amount)  (last 'amount' items)
        //    head = data.GetRange(0, splitIndex)       (everything before tail)
        // 4) Clear the original list.
        // 5) Add tail first, then head back into data.

        int n = data.Count;

        // amount is guaranteed 1..n by the assignment/tests, but this makes it safe anyway
        int rotate = amount % n;

        // If rotate is 0 (e.g., amount == n), list stays the same (matches TestRotateListRight_Rotate9)
        if (rotate == 0)
            return;

        int splitIndex = n - rotate;

        List<int> tail = data.GetRange(splitIndex, rotate);
        List<int> head = data.GetRange(0, splitIndex);

        data.Clear();
        data.AddRange(tail);
        data.AddRange(head);
    }
}