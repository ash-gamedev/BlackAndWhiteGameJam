using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnumLayer
{
    Customer
}

public enum EnumTags
{
    Order,
    Chair
}

public enum EnumOrder
{
    Pear,
    Burger,
    Hotdog
}

public enum EnumTileDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}

public enum EnumNeighbour
{
    Top,
    Bottom,
    Left,
    Right
}

public enum EnumSeatPosition
{
    Top,
    Bottom,
    Left,
    Right
}

public enum EnumAnimationState
{
    Idle,
    PlateBreak
}

public enum EnumSoundEffects
{
    TileSet,
    TileRemoved,
    PlateShatter,
    CustomerOrder,
    OrderCorrect,
    OrderIncorrect,
    CustomerPays
}