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
    Coffee,
    Croissant,
    Donut,
    Cookie,
    Sandwich
}

public enum EnumTileDirection
{
    Up,
    Down,
    Left,
    Right,
    None,
    Garbage
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
    PlateBreak,
    PlateTrash
}

public enum EnumSoundEffects
{
    TileRemove,
    TileSet,
    PlateShatter,
    CustomerOrder,
    OrderCorrect,
    OrderIncorrect,
    CustomerPays,
    LevelWinSound,
    LevelLostSound,
    OrderPlaced,
    Trash
}

public enum EnumDifficulty
{
    Easy = 0,
    Normal = 1,
    Hard = 2
}

public enum EnumTutorialStep
{
    PlaceTile,
    RemoveTile,
    CustomerOrder,
    NewCustomerOrder
}

public enum EnumTutorialAnimation
{
    PlaceTile,
    RemoveTile
}