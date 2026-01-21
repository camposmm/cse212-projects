using System;
using System.Collections.Generic;

public class Maze
{
    private readonly Dictionary<ValueTuple<int, int>, bool[]> _mazeMap;
    private int _currX = 1;
    private int _currY = 1;

    public Maze(Dictionary<ValueTuple<int, int>, bool[]> mazeMap)
    {
        _mazeMap = mazeMap;
    }

    /// <summary>
    /// left = index 0
    /// right = index 1
    /// up = index 2
    /// down = index 3
    /// </summary>
    private bool CanMove(int directionIndex)
    {
        return _mazeMap[(_currX, _currY)][directionIndex];
    }

    public void MoveLeft()
    {
        if (!CanMove(0))
            throw new InvalidOperationException("Can't go that way!");

        _currX -= 1;
    }

    public void MoveRight()
    {
        if (!CanMove(1))
            throw new InvalidOperationException("Can't go that way!");

        _currX += 1;
    }

    public void MoveUp()
    {
        if (!CanMove(2))
            throw new InvalidOperationException("Can't go that way!");

        _currY -= 1;
    }

    public void MoveDown()
    {
        if (!CanMove(3))
            throw new InvalidOperationException("Can't go that way!");

        _currY += 1;
    }

    public string GetStatus()
    {
        return $"Current location (x={_currX}, y={_currY})";
    }
}