// Hexagonal Map 
// https://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;

/*! Hexagonal Coordinates and Hexagonal Field */
namespace HexagonalMap.Domain.HexMap
{

    /// <summary>
    /// Cube coordinates
    /// </summary>
    /// <remarks>Coordinates based on a cube and a plane _x + _y + _z = 0 slicing the cube</remarks>
    public struct CubeCoordinate
    {
        /// <summary>
        /// X (If flat-topped, this value will not change in top-left to bottom-right direction)
        /// </summary>
        public int x { get; private set; }
        
        /// <summary>
        /// Y (If flat-topped, this value will not change in vertical direction)
        /// </summary>
        public int y { get; private set; }
        
        /// <summary>
        /// Z (If flat-topped, this value will not change in top-right to bottom-left direction)
        /// </summary>
        public int z { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_x">X</param>
        /// <param name="_y">Y</param>
        /// <param name="_z">Z</param>
        /// <remarks>Do NOT use this directly. Use fromCube.</remarks>
        /// <exception cref="ArgumentException">if _x + _y + _z != 0</exception>
        private CubeCoordinate(int _x, int _y, int _z): this()
        {
            if (_x + _y + _z != 0)
            {
                throw new ArgumentException("Impossible coordinate. _x, _y, _z must add up to 0.", new Exception());
            }
            x = _x;
            y = _y;
            z = _z;
        }

        /// <summary>
        /// Create a cube coordinate from cube coordinate values
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>Cube coordinate</returns>
        public static CubeCoordinate FromCube(int x, int y, int z)
        {
            return new CubeCoordinate(x, y, z);
        }
  
        /// <summary>
        /// Create a cube coordinate from QR coordinate values
        /// </summary>
        /// <param name="q">Q</param>
        /// <param name="r">R</param>
        /// <returns>Cube coordinate</returns>
        public static CubeCoordinate FromQr(int q, int r)
        {
            var x = q;
            var z = r;
            var y = -x - z;
            
            return new CubeCoordinate(x, y, z);
        }
        
        /// <summary>
        /// Create a cube coordinate from a QR coodinate
        /// </summary>
        /// <param name="input">QR coordinate (as object)</param>
        /// <returns>Cube coordinate</returns>
        public static CubeCoordinate FromQr(QRCoordinate input)
        {
            return FromQr(input.q, input.r);
        }

        /// <summary>
        /// Create QR coordinate from itself
        /// </summary>
        /// <returns>QR coordinate</returns>
        public QRCoordinate ToQr()
        {
            return QRCoordinate.FromCube(this);
        }
        
        /// <summary>
        /// Returns relative position to 6 neighboring position to a given position
        /// </summary>
        /// <returns>List of 6 positions. Add these to the given position to get the actual neighboring positions.</returns>
        public static List<CubeCoordinate> AdjacentRelatives()
        {
            return new List<CubeCoordinate>
            {
                FromCube(1,-1,0),
                FromCube(-1,1,0),
                FromCube(1,0,-1),
                FromCube(-1,0,1),
                FromCube(0,1,-1),
                FromCube(0,-1,1)
            };
        }
        
        /// <summary>
        /// Addition - use this if you need to move from one place to another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x + b.x, a.y + b.y, a.z + b.z);   
        }

        /// <summary>
        /// Deduction - should be useful for measuring the distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        /// <summary>
        /// Equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }

        /// <summary>
        /// Inequality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x != b.x || a.y != b.y || a.z != b.z);
        }

        /// <summary>
        /// Creates hash
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }
        
        /// <summary>
        /// Equality (against any object)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CubeCoordinate))
            {
                return false;
            }

            var coordinate = (CubeCoordinate) obj;
            return coordinate == this;
        }
    }

    /// <summary>
    /// QR coordinate (or Axial coordinate)
    /// </summary>
    /// <remarks>XY coordiante with one of the axis being skewed by 30 degrees. Useful for drawing onto the screen.</remarks>
    public struct QRCoordinate
    {
        /// <summary>
        /// Q (If flat-topped, this value will not change in vertical direction)
        /// </summary>
        public int q { get; private set; }

        /// <summary>
        /// R (If flat-topped, this value will not change in top-left to bottom-right direction)
        /// </summary>
        public int r { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_q">Q</param>
        /// <param name="_r">R</param>
        /// <remarks>Do NOT use this directly. Use fromQR.</remarks>
        private QRCoordinate(int _q, int _r): this()
        {
            q = _q;
            r = _r;
        }

        /// <summary>
        /// Create QR coordinate from QR coordinate values
        /// </summary>
        /// <param name="q">Q</param>
        /// <param name="r">R</param>
        /// <returns>QR coordinate</returns>
        public static QRCoordinate FromQr(int q, int r)
        {
            return new QRCoordinate(q, r);
        }

        /// <summary>
        /// Create QR coordinate from cube coordinate values
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>QR coordinate</returns>
        public static QRCoordinate FromCube(int x, int y, int z)
        {
            return new QRCoordinate(x, z);
        }

        /// <summary>
        /// Create QR coordiante from cube coordinate
        /// </summary>
        /// <param name="input">Cube coordinate (as object)</param>
        /// <returns>QR coordinate</returns>
        public static QRCoordinate FromCube(CubeCoordinate input)
        {
            return FromCube(input.x, input.y, input.z);
        }

        /// <summary>
        /// Create cube coordinate from itself
        /// </summary>
        /// <returns>Cube座標オブジェクト</returns>
        public CubeCoordinate ToCube()
        {
            return CubeCoordinate.FromQr(this);
        }

        /// <summary>
        /// Addition - use this if you need to move from one place to another
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static QRCoordinate operator +(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q + b.q, a.r + b.r);
        }

        /// <summary>
        /// Deduction - should be useful for measuring the distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static QRCoordinate operator -(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q - b.q, a.r - b.r);
        }

        /// <summary>
        /// Equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(QRCoordinate a, QRCoordinate b)
        {
            return a.q == b.q && a.r == b.r;
        }

        /// <summary>
        /// Inequality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(QRCoordinate a, QRCoordinate b)
        {
            return a.q != b.q || a.r != b.r;
        }

        /// <summary>
        /// Creates hash
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return q ^ r;
        }

        /// <summary>
        /// Equality (against any object)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is QRCoordinate))
            {
                return false;
            }

            var coordinate = (QRCoordinate) obj;
            return coordinate == this;
        }
    }
    

    /// <summary>
    /// Abstract class for object that needs hexagonal coordinate
    /// </summary>
    /// <remarks>
    /// If your object needs hexagonal coordinate, inherit this class.
    /// The coordinates are stored in cube coordinate, which you can convert to QR for drawing.
    /// </remarks>
    public abstract class Cell
    {
        /// <summary>
        /// Cube座標での位置
        /// </summary>
        public CubeCoordinate Position { get; protected set; }

        public Cell(CubeCoordinate position)
        {
            this.Position = position;
        }
    }

    /// <summary>
    /// Abstract class for object that represents hexagonal field
    /// </summary>
    /// <remarks>
    /// You can map the object inheriting ICell here.
    /// This class will help you find the neighboring cells.
    /// </remarks>
    public abstract class Field
    {
        /// <summary>
        /// List of cells
        /// </summary>
        /// <remarks>
        /// TODO: Use hash table for performance?
        /// </remarks>
        protected readonly List<Cell> Cells = new List<Cell>();

        /// <summary>
        /// Add cell into the field
        /// </summary>
        /// <param name="cell"></param>
        /// <exception cref="ArgumentException">if the position is already occupied</exception>
        public void AddCell(Cell cell)
        {
            if (GetCellAt(cell.Position) != null)
            {
                throw new ArgumentException("You'll have 2 cells in one position. This isn't allowed.", new Exception());
            }
            Cells.Add(cell);
        }

        /// <summary>
        /// Removes a Cell at the given position from the field.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Removed Cell</returns>
        public Cell RemoveCellAt(CubeCoordinate position)
        {
            var cell = GetCellAt(position);
            Cells.Remove(cell);
            return cell;
        }
        
        /// <summary>
        /// Get cell from Cube coordinates
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">if there's no cell in the given position</exception>
        public Cell GetCellAt(CubeCoordinate position)
        {
            foreach (var cell in Cells)
            {
                if (cell.Position == position)
                {
                    return cell;
                }
            }
            throw new ArgumentOutOfRangeException("That position is either out of field, or does not contain a cell.", new Exception());   
        }
        
        /// <summary>
        /// Get cell from QR coordinates
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Cell found</returns>
        /// <exception cref="ArgumentOutOfRangeException">if there's no cell in the given position</exception>
        public Cell GetCellAt(QRCoordinate position)
        {
            var positionRequested = CubeCoordinate.FromQr(position);

            var cell = GetCellAt(positionRequested);

            return cell;
        }


        /// <summary>
        /// Get the list of neighboring cells (up to 6)
        /// </summary>
        /// <param name="cell">Cell whose neighbor you want to find.</param>
        /// <returns></returns>
        public List<Cell> FindNeighborsOf(Cell cell)
        {
            var adjacentRelativePositions = CubeCoordinate.AdjacentRelatives();
            var positionToFind = new List<CubeCoordinate>();
            var cellsToReturn = new List<Cell>();
            
            foreach (var relativePosition in adjacentRelativePositions)
            {
                positionToFind.Add(cell.Position + relativePosition);
            }

            foreach (var positionRequested in positionToFind)
            {
                var cellFound = GetCellAt(positionRequested);
                if (cellFound != null) cellsToReturn.Add(cellFound);
            }

            return cellsToReturn;
        }
    }
}