// Hexagonal Map 
// https://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;

namespace HexagonalMap.Domain.HexMap
{

    /// <summary>
    /// 立方体座標系
    /// </summary>
    public struct CubeCoordinate
    {
        /// X座標 (flat-toppedの場合、左上-左下方向で０)
        /// Y座標 (flat-toppedの場合、上下方向で０)
        /// Z座標 (flat-toppedの場合、右上-右下方向で０)
        
        
        public int x 
        {
            get { return x; }
            private set { x = value; }
        }
        public int y
        {
            get { return y; }
            private set { y = value; }
        }
        public int z
        {
            get { return z; }
            private set { z = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private CubeCoordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Cube座標から
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static CubeCoordinate FromCube(int x, int y, int z)
        {
            return new CubeCoordinate(x, y, z);
        }
  
        /// <summary>
        /// QR座標から3次元座標を作る
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static CubeCoordinate FromQr(int q, int r)
        {
            var x = q;
            var z = r;
            var y = -x - z;
            
            return new CubeCoordinate(x, y, z);
        }
        
        /// <summary>
        /// QR座標から3次元座標を作る
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static CubeCoordinate FromQr(QRCoordinate input)
        {
            return FromQr(input.q, input.r);
        }

        /// <summary>
        /// 自分をQR座標化する
        /// </summary>
        /// <returns></returns>
        public QRCoordinate ToQr()
        {
            return QRCoordinate.FromCube(this);
        }
        
        /// <summary>
        /// ある座標から６近傍を見つけるための相対座標を返す
        /// </summary>
        /// <returns>それぞれを座標に加算すれば６近傍を表す座標になる</returns>
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
        
        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x + b.x, a.y + b.y, a.z + b.z);   
        }

        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static bool operator ==(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }

        public static bool operator !=(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x != b.x || a.y != b.y || a.z != b.z);
        }

        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }
        
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
    /// QR座標系
    /// </summary>
    public struct QRCoordinate
    {
        public int q
        {
            get { return q; }
            private set { q = value; }
        }

        public int r
        {
            get { return r; }
            private set { r = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        private QRCoordinate(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        /// <summary>
        /// Q,Rの値から作る
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static QRCoordinate FromQr(int q, int r)
        {
            return new QRCoordinate(q, r);
        }

        /// <summary>
        /// Cube座標からQR座標を作る
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static QRCoordinate FromCube(int x, int y, int z)
        {
            return new QRCoordinate(x, z);
        }

        /// <summary>
        /// Cube座標からQR座標を作る
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static QRCoordinate FromCube(CubeCoordinate input)
        {
            return FromCube(input.x, input.y, input.z);
        }

        /// <summary>
        /// 自分をXYZ座標化する
        /// </summary>
        /// <returns></returns>
        public CubeCoordinate ToCube()
        {
            return CubeCoordinate.FromQr(this);
        }

        public static QRCoordinate operator +(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q + b.q, a.r + b.r);
        }

        public static QRCoordinate operator -(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q - b.q, a.r - b.r);
        }

        public static bool operator ==(QRCoordinate a, QRCoordinate b)
        {
            return a.q == b.q && a.r == b.r;
        }

        public static bool operator !=(QRCoordinate a, QRCoordinate b)
        {
            return a.q != b.q || a.r != b.r;
        }

        public override int GetHashCode()
        {
            return q ^ r;
        }

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
    /// 座標位置を表すオブジェクト
    /// </summary>
    /// <remarks>内部的にはCube座標で持つが相互変換可能。実データにこれを関連づけると良い</remarks>
    public class Cell
    {
        public CubeCoordinate Position
        {
            get { return Position; }
            private set { Position = value; }
        }

        public Cell(CubeCoordinate position)
        {
            Position = position;
        }
    }

    /// <summary>
    /// ６角形座標系の平面を表す
    /// </summary>
    /// <remarks>ここにCellを展開することで見つけることが可能</remarks>
    public class Field
    {
        // TODO: セルをハッシュテーブルで保管した方が絶対高速
        private readonly List<Cell> _cells;

        public Field()
        {
            _cells = new List<Cell>();
        }
        
        /// <summary>
        /// セルをフィールドに追加
        /// </summary>
        /// <param name="cell"></param>
        /// <exception cref="ArgumentException">すでにあるセルと座標が重複した時</exception>
        public void AddCell(Cell cell)
        {
            if (GetCellAtCube(cell.Position) != null)
            {
                throw new ArgumentException("追加しようとしたセルの座標には別のセルがすでにいます", new Exception());
            }
            _cells.Add(cell);
        }

        /// <summary>
        /// 中央を０とした時のQR座標からセルを引っ張り出します
        /// </summary>
        /// <param name="position"></param>
        /// <returns>該当位置のセル</returns>
        /// <exception cref="ArgumentOutOfRangeException">対象の座標のセルがない時</exception>
        public Cell GetCellAtQr(QRCoordinate position)
        {
            CubeCoordinate positionRequested = CubeCoordinate.FromQr(position);

            var cell = GetCellAtCube(positionRequested);

            return cell;
        }

        /// <summary>
        /// Cube座標にあるセルを求めます
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">対象の座標のセルがない時</exception>
        public Cell GetCellAtCube(CubeCoordinate position)
        {
            foreach (var cell in _cells)
            {
                if (cell.Position == position)
                {
                    return cell;
                }
            }
            throw new ArgumentOutOfRangeException("その座標はフィールド外です もしくはその座標のセルがフィールドから抜け落ちています", new Exception());   
        }

        /// <summary>
        /// 指定セルの隣のセルを返す
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public List<Cell> FindAdjacentOf(Cell cell)
        {
            List<CubeCoordinate> adjacentRelativePositions = CubeCoordinate.AdjacentRelatives();
            List<CubeCoordinate> positionToFind = new List<CubeCoordinate>();
            List<Cell> cellsToReturn = new List<Cell>();
            
            foreach (var relativePosition in adjacentRelativePositions)
            {
                positionToFind.Add(cell.Position + relativePosition);
            }

            foreach (var positionRequested in positionToFind)
            {
                var cellFound = GetCellAtCube(positionRequested);
                if (cellFound != null) cellsToReturn.Add(cellFound);
            }

            return cellsToReturn;
        }
        
    }
}