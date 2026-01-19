/// <summary>
/// This queue is circular. When people are added via AddPerson, then they are added to the
/// back of the queue (per FIFO rules). When GetNextPerson is called, the next person
/// in the queue is saved to be returned and then they are placed back into the back of the queue.
/// Thus, each person stays in the queue and is given turns. When a person is added to the queue,
/// a turns parameter is provided to identify how many turns they will be given. If the turns is 0 or
/// less then they will stay in the queue forever. If a person is out of turns then they will
/// not be added back into the queue.
/// </summary>
public class TakingTurnsQueue
{
    private readonly PersonQueue _people = new();

    public int Length => _people.Length;

    public void AddPerson(string name, int turns)
    {
        var person = new Person(name, turns);
        _people.Enqueue(person);
    }

    public Person GetNextPerson()
    {
        if (_people.IsEmpty())
            throw new InvalidOperationException("No one in the queue.");

        var person = _people.Dequeue();

        // Infinite turns: 0 or less => ALWAYS re-enqueue, DO NOT modify turns
        if (person.Turns <= 0)
        {
            _people.Enqueue(person);
            return person;
        }

        // Finite turns: decrement, and re-enqueue only if still has turns remaining
        person.Turns -= 1;

        if (person.Turns > 0)
            _people.Enqueue(person);

        return person;
    }

    public override string ToString()
    {
        return _people.ToString();
    }
}