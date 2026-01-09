using System;

public static class ArraySelector
{
    public static void Run()
    {
        var l1 = new[] { 1, 2, 3, 4, 5 };
        var l2 = new[] { 2, 4, 6, 8, 10 };
        var select = new[] { 1, 1, 1, 2, 2, 1, 2, 2, 2, 1 };

        var intResult = ListSelector(l1, l2, select);
        Console.WriteLine("<int[]>{" + string.Join(", ", intResult) + "}"); 
        // <int[]>{1, 2, 3, 2, 4, 4, 6, 8, 10, 5}
    }

    private static int[] ListSelector(int[] list1, int[] list2, int[] select)
    {
        int[] result = new int[select.Length];

        int list1Index = 0;
        int list2Index = 0;

        for (int i = 0; i < select.Length; i++)
        {
            if (select[i] == 1)
            {
                if (list1Index >= list1.Length)
                    throw new ArgumentException("Selector requests more items from list1 than list1 contains.");

                result[i] = list1[list1Index];
                list1Index++;
            }
            else if (select[i] == 2)
            {
                if (list2Index >= list2.Length)
                    throw new ArgumentException("Selector requests more items from list2 than list2 contains.");

                result[i] = list2[list2Index];
                list2Index++;
            }
            else
            {
                throw new ArgumentException("Selector array can only contain 1s and 2s.");
            }
        }

        return result;
    }
}