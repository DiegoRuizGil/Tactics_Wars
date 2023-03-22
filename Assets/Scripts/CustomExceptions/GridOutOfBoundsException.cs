using System;

public class GridOutOfBoundsException : SystemException
{
    public float X { get; }
    public float Y { get; }

    public GridOutOfBoundsException(int x, int y)
        : base($"The coordinates ({x}, {y}) are outside the grid")
    {
        X = x;
        Y = y;
    }

    public GridOutOfBoundsException(float x, float y)
        : base($"The coordinates ({x}, {y}) are outside the grid")
    {
        X = x;
        Y = y;
    }
}
