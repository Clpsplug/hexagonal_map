# Hexagonal Map Coordinates Helper

[Red Blob Games Website](https://www.redblobgames.com/grids/hexagons/)で
説明されている立方体座標・QR座標のC#での実装  
This C# code implements Cube- and QR-based coordinates 
described in [Red Blob Games Website.](https://www.redblobgames.com/grids/hexagons/)

# Usage

## Coordinates

There are 2 coordinates supported - Cube and QR(Axial.)  
Constructors of those value objects are private, and
you must make through specific methods, `fromCube` and `fromQR`
respectively.

The supported coordinates makes creating concentric hexagonal map
easy. Better supports for map that spreads in the form of a rectaingle 
are planned through offset coordinates.

**Note** that whether your hexagonal map is pointy-topped or flat-topped
is completely up to you.
These coordinates should work fine under both circumstances.

**Also note** that you MUST NOT directly change the value for the 
coordinates. You can't. If you want to modify the value and save it
somewhere else, just create new coordinate object with modified value.

### Cube Coordinates

> Let's take a cube grid and slice out a diagonal plane at `x + y + z = 0.`
> This is a _weird_ idea but it helps us make hex grid algorithms simpler. 
> In particular, we can reuse standard operations from cartesian coordinates: 
> adding coordinates, subtracting coordinates, multiplying or dividing by a scalar, 
> and distances.  
>  \- Red Blob Games

Use `HexagonalMap.Domain.HexMap.CubeCoordinate` to represent
the cube coordinates.

```c#
// The center
var center = CubeCoordinates.fromCube(0,0,0);
// Top hexagon (in flat-top system)
var top = CubeCoordinates.fromCube(0,1,-1);
// You can access each coordinate via:
top.x // -> 0
top.y // -> 1
top.z // -> -1
// Note that in Cube coordinate, x + y + z is always 0. If they aren't, it will yield an exception.
var fail = CubeCoordinates.fromCube(1,2,3); // -> ArgumentException
```

Addition and Deduction are implemented. This is important
when finding the 6 neighbours.

```c#
// CubeCoordinates has a method to return a list of relative positions
// of the 6 neighbours. 
var relativeNeighbours = CubeCoordinates.AdjacentRelatives();
var origin = CubeCoordinates.fromCube(0,0,0);
foreach (var relative in relativeNeighbours) {
    origin + relative // -> is the coordinate for one of the neighbour of origin.
}
```

Find if two coordinate match simply using `==`
(or don't match using `!=`)

```c#
var a = CubeCoordinates.fromCube(0,0,0);
var b = CubeCoordinates.fromCube(0,0,0);
var c = CubeCoordinates.fromCube(1,0,-1);

a == b // -> true
a != b // -> false
a == c // -> false
a != c // -> true
```

### QR Coordinates (Axial Coordinates)

> The axial coordinate system, sometimes called 
> “trapezoidal” or “oblique” or “skewed”, 
> is built by taking two of the three coordinates 
> from a cube coordinate system.   
> \- Red Blob Games

This coordinate is extremely useful for drawing, since you can convert
the hexagons' position with little math.

Use `HexagonalMap.Domain.HexMap.QRCoordinate` to represent
the QR coordinates.  

```
// The center
var center = QRCoordinates.fromQR(0,0);
// Top hexagon (in flat-top system)
var top = QRCoordinates.fromQR(0, -1);
// Get values of each coordinate
top.q // -> 0
top.r // -> -1
```

`QRCoordinates` cannot give you the relative coordinate of 6 neighbours,
but addition, deduction, equality checks are implemented.
See Cube Coordinates for usage.

## Converting Coordinates

Cube Coordinates and QR Coordinates can be converted into each other.
Simply call:

```c#
var qr = QRCoordinates.fromQR(0,0);
var cube = CubeCoordinates.fromCube(0,0,0);
QRCoordinates.fromCube(cube); // -> returns a QR coordinate for the given Cube coordinate
CubeCoordinates.fromQR(qr); // -> returns a Cube coordinate for the given QR coordinate
qr.toCube(); // -> converts itself (QR) into Cube coordinate
cube.toQR(); // -> converts itself (Cube) into QR coordinate
```

You can also directly create those coordinates with values for other system:

```
var qr = QRCoordinates.fromCube(0,0,0); // -> is a QR coordinate that is equal to Cube coordinate (0,0,0)
var cube = CubeCoordinates.fromQR(0,0); // -> is a Cube coordinate that is equal to QR coordinate (0,0)
```

## Field and Cells

### Cells

If any of your object in your games needs to know its hexagonal position,
inherit `HexagonalMap.Domain.HexMap.Cell`.  
In your constructor, make sure to pass the position of the cell to `Cell`.  
Example usage can be found in `HexagonalMap.ExampleBlock.Block`.

```c#
public class Block: Cell
{
    public int Score { get; set; }

    // Be sure to include `: base(position)` or it won't compile!
    public Block (CubeCoordinate position, int score): base(position)
    {
        Score = score;
    }
}
```

### Field

`HexagonalMap.Domain.HexMap.Field` can hold objects inheriting `Cell`.

If you need a hexagonal field, inherit `Field` to your class.  
This will help you finding cells in your game fields.  
`Field` has a parameterless constructor, so you don't need to worry about
adding `base()` to your class.  

```c#
public class Map: Field
{
    public int randomParam;
    public Map(int value)
    {
        randomParam = value;
    }
}
```

Here are few things to remember:

1. You need to add your `Cell`s into the field before the field can find them.  

```c#
var map = new Map(5);
var block = new Block(CubeCoordinate.fromCube(0,0,0), 50);
map.AddCell(block);
```

2. Since you can't change your `Cell`s position directly, if you want cells to move
to another place, you need to remove it from the map, create new `Cell`
with different location, and add it back to the `Field`.
3. `Field` cannot have more than 1 cell at the same spot. Forcing so will
get you an `ArgumentException`. You can use it to detect collision, if you like.

```c#
// Moving a cell at the center...
// Remove it from the field first. RemoveCellAt gives you the removed cell.
var removedCell = map.RemoveCellAt(CubeCoordinate.fromCube(0,0,0));
// Recreate your cell data with the new location
var movedCell = new Block(CubeCoordinate.fromCube(1,0,-1), block.Score);
// Adding back might yield exception. If it does, resolve the situation first.
try {
    map.AddCell(movedCell);
}
catch (ArgumentException e)
{
    // Collision! It might be that your cell bumped into an enemy.
}
```


Inheriting `Field` grants you following benefits:

1. You can find a cell at a given coordinate.

```c#
var foundByCube = map.FindCellAt(CubeCoordinate.fromCube(0,0,0));
var foundByQR = map.FindCellAt(QRCoordinate.fromQR(0,0));
```

2. You can find the 6 neighboring cells (if it exists on the `Field`.)

```c#
map.FindNeighborsOf(foundByCube); // -> returns list of cells up to 6.
```

# Glitchy behaviors?

Open the issue in this repository! If you provide some reproducable codes,
things will go smoother!
