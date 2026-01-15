using System;
using System.Collections.Generic;
using System.IO;

public class CustomerService
{
    // Messages used by the requirements
    private const string FullQueueMessage = "Maximum Number of Customers in Queue.";
    private const string EmptyQueueMessage = "No Customers in Queue.";

    public static void Run()
    {
        // Helper to capture Console output for tests
        static string CaptureOutput(Action action)
        {
            TextWriter originalOut = Console.Out;
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            try
            {
                action();
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            return sw.ToString();
        }

        static void Assert(string testName, bool condition, string failureMessage)
        {
            if (condition)
            {
                Console.WriteLine($"{testName}: PASS");
            }
            else
            {
                Console.WriteLine($"{testName}: FAIL - {failureMessage}");
            }
        }

        Console.WriteLine("Running CustomerService tests...\n");

        // =========================
        // Test 1: Invalid size defaults to 10
        // =========================
        Console.WriteLine("Test 1");
        CustomerService cs1 = new CustomerService(0);
        string cs1Text = cs1.ToString();
        Assert(
            "Test 1",
            cs1Text.Contains("max_size=10"),
            $"Expected max_size=10, got: {cs1Text}"
        );
        Console.WriteLine("=================\n");

        // =========================
        // Test 2: Enqueue works, Serve dequeues in FIFO order
        // =========================
        Console.WriteLine("Test 2");
        CustomerService cs2 = new CustomerService(2);

        cs2.AddNewCustomer("Ana", "A1", "Password reset");
        cs2.AddNewCustomer("Bob", "B2", "Billing issue");

        Assert(
            "Test 2A",
            cs2.ToString().Contains("size=2"),
            $"Expected size=2, got: {cs2}"
        );

        string serve1 = CaptureOutput(() => cs2.ServeCustomer()).Trim();
        Assert(
            "Test 2B",
            serve1.Contains("Ana (A1)") && serve1.Contains("Password reset"),
            $"Expected to serve Ana first, got: {serve1}"
        );

        string serve2 = CaptureOutput(() => cs2.ServeCustomer()).Trim();
        Assert(
            "Test 2C",
            serve2.Contains("Bob (B2)") && serve2.Contains("Billing issue"),
            $"Expected to serve Bob second, got: {serve2}"
        );

        Assert(
            "Test 2D",
            cs2.ToString().Contains("size=0"),
            $"Expected size=0 after serving all, got: {cs2}"
        );

        Console.WriteLine("=================\n");

        // =========================
        // Test 3: Adding when full prints an error
        // =========================
        Console.WriteLine("Test 3");
        CustomerService cs3 = new CustomerService(1);
        cs3.AddNewCustomer("Carla", "C3", "Login problem");

        string addWhenFull = CaptureOutput(() =>
            cs3.AddNewCustomer("Dan", "D4", "App crash")
        );

        Assert(
            "Test 3",
            addWhenFull.Contains(FullQueueMessage),
            $"Expected full queue message, got: {addWhenFull.Trim()}"
        );

        Console.WriteLine("=================\n");

        // =========================
        // Test 4: Serving when empty prints an error
        // =========================
        Console.WriteLine("Test 4");
        CustomerService cs4 = new CustomerService(3);

        string serveWhenEmpty = CaptureOutput(() => cs4.ServeCustomer());

        Assert(
            "Test 4",
            serveWhenEmpty.Contains(EmptyQueueMessage),
            $"Expected empty queue message, got: {serveWhenEmpty.Trim()}"
        );

        Console.WriteLine("\nDone.");
    }

    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize)
    {
        _maxSize = (maxSize <= 0) ? 10 : maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class. Its real name is CustomerService.Customer
    /// </summary>
    private class Customer
    {
        public Customer(string name, string accountId, string problem)
        {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString()
        {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Requirement: AddNewCustomer shall enqueue a new customer into the queue.
    /// If the queue is full, display an error message.
    /// </summary>
    public void AddNewCustomer(string name, string accountId, string problem)
    {
        // FIX #1: Full check must be >= (not >)
        if (_queue.Count >= _maxSize)
        {
            Console.WriteLine(FullQueueMessage);
            return;
        }

        Customer customer = new Customer(name.Trim(), accountId.Trim(), problem.Trim());
        _queue.Add(customer);
    }

    /// <summary>
    /// Original interactive version (kept), calls the testable overload above.
    /// </summary>
    private void AddNewCustomer()
    {
        Console.Write("Customer Name: ");
        string name = Console.ReadLine()!.Trim();

        Console.Write("Account Id: ");
        string accountId = Console.ReadLine()!.Trim();

        Console.Write("Problem: ");
        string problem = Console.ReadLine()!.Trim();

        AddNewCustomer(name, accountId, problem);
    }

    /// <summary>
    /// Requirement: ServeCustomer shall dequeue the next customer and display details.
    /// If empty, display an error message.
    /// </summary>
    public void ServeCustomer()
    {
        // FIX #2: Must check empty before removing
        if (_queue.Count == 0)
        {
            Console.WriteLine(EmptyQueueMessage);
            return;
        }

        // FIX #3: Read first, then remove (FIFO)
        Customer customer = _queue[0];
        _queue.RemoveAt(0);
        Console.WriteLine(customer);
    }

    public override string ToString()
    {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}