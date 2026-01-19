/// <summary>
/// A basic implementation of a Queue
/// </summary>
public class PersonQueue
{
    private readonly List<Person> _queue = new();

    public int Length => _queue.Count;

    /// <summary>
    /// Add a person to the queue (enqueue to the back)
    /// </summary>
    public void Enqueue(Person person)
    {
        _queue.Add(person); // back of the queue
    }

    /// <summary>
    /// Remove a person from the queue (dequeue from the front)
    /// </summary>
    public Person Dequeue()
    {
        var person = _queue[0];  // front of the queue
        _queue.RemoveAt(0);
        return person;
    }

    public bool IsEmpty()
    {
        return Length == 0;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", _queue)}]";
    }
}