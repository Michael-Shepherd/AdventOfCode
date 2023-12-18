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
}