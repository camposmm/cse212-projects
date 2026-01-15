using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue multiple items with different priorities. Dequeue should always return
    //           the item with the highest priority first.
    // Expected Result: Items are returned in order of priority (highest -> lowest).
    // Defect(s) Found: (Fill this after running) Example defects could include:
    //                  - Not removing the dequeued item
    //                  - Not selecting the true highest priority
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();

        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 3);
        priorityQueue.Enqueue("C", 2);

        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual("A", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: If there is a tie for highest priority, the queue must remove the one that was
    //           enqueued first (FIFO among ties). Also verify the empty-queue exception.
    // Expected Result: For equal priorities, items come out in insertion order. When empty,
    //                  throws InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: (Fill this after running) Example defects could include:
    //                  - Using >= and breaking FIFO tie rule
    //                  - Throwing wrong exception type or message
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();

        priorityQueue.Enqueue("X", 5);
        priorityQueue.Enqueue("Y", 5);
        priorityQueue.Enqueue("Z", 1);

        // FIFO among ties (X and Y both priority 5)
        Assert.AreEqual("X", priorityQueue.Dequeue());
        Assert.AreEqual("Y", priorityQueue.Dequeue());
        Assert.AreEqual("Z", priorityQueue.Dequeue());

        var ex = Assert.ThrowsException<InvalidOperationException>(() => priorityQueue.Dequeue());
        Assert.AreEqual("The queue is empty.", ex.Message);
    }

    // Add more test cases as needed below.
}