namespace Utilities;

public static class DirectionEnumExtensions
{
    public static PolarDirection GetInverse(this PolarDirection direction)
    {
        switch (direction)
        {
            case PolarDirection.NORTH:
                return PolarDirection.SOUTH;
            case PolarDirection.SOUTH:
                return PolarDirection.NORTH;
            case PolarDirection.EAST:
                return PolarDirection.WEST;
            case PolarDirection.WEST:
                return PolarDirection.EAST;
            case PolarDirection.NONE:
                return PolarDirection.NONE;
            default:
                return PolarDirection.NONE;
        }
    }

    public static Direction MapToDirection(this char letter)
    {
        switch (letter)
        {
            case 'U':
                return Direction.UP;
            case 'D':
                return Direction.DOWN;
            case 'L':
                return Direction.LEFT;
            case 'R':
                return Direction.RIGHT;
            case '0':
                return Direction.RIGHT;
            case '1':
                return Direction.DOWN;
            case '2':
                return Direction.LEFT;
            case '3':
                return Direction.UP;
            default:
                return Direction.NONE;
        }
    }
}