/// Hexagonal Map 
/// https://www.redblobgames.com/grids/hexagons/

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
        public static CubeCoordinate fromCube(int x, int y, int z)
        {
            return new CubeCoordinate(x, y, z);
        }
  
        /// <summary>
        /// QR座標から3次元座標を作る
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static CubeCoordinate fromQR(int q, int r)
        {
            var x = q;
            var z = r;
            var y = -x - z;
            
            return new CubeCoordinate(x, y, z);
        }

        /// <summary>
        /// 自分をQR座標化する
        /// </summary>
        /// <returns></returns>
        public QRCoordinate toQR()
        {
            return QRCoordinate.fromCube(this);
        }
        
        /// <summary>
        /// QR座標から3次元座標を作る
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static CubeCoordinate fromQR(QRCoordinate input)
        {
            return fromQR(input.q, input.r);
        }
        
        /// <summary>
        /// ある座標から６近傍を見つけるための相対座標を返す
        /// </summary>
        /// <returns>それぞれを座標に加算すれば６近傍を表す座標になる</returns>
        public static List<CubeCoordinate> AdjacentRelatives()
        {
            return new List<CubeCoordinate>
            {
                CubeCoordinate.fromCube(1,-1,0),
                CubeCoordinate.fromCube(-1,1,0),
                CubeCoordinate.fromCube(1,0,-1),
                CubeCoordinate.fromCube(-1,0,1),
                CubeCoordinate.fromCube(0,1,-1),
                CubeCoordinate.fromCube(0,-1,1)
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
        public static QRCoordinate fromQR(int q, int r)
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
        public static QRCoordinate fromCube(int x, int y, int z)
        {
            return new QRCoordinate(x, z);
        }

        /// <summary>
        /// Cube座標からQR座標を作る
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static QRCoordinate fromCube(CubeCoordinate input)
        {
            return fromCube(input.x, input.y, input.z);
        }

        /// <summary>
        /// 自分をXYZ座標化する
        /// </summary>
        /// <returns></returns>
        public CubeCoordinate toXYZ()
        {
            return CubeCoordinate.fromQR(this);
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
        public CubeCoordinate position
        {
            get { return position; }
            private set { position = value; }
        }

        public Cell(CubeCoordinate position)
        {
            this.position = position;
        }
    }

    /// <summary>
    /// ６角形座標系の平面を表す
    /// </summary>
    /// <remarks>ここにCellを展開することで見つけることが可能</remarks>
    public class Field
    {
        // TODO: セルをハッシュテーブルで保管した方が絶対高速
        private List<Cell> cells;

        /// <summary>
        /// セルをフィールドに追加
        /// </summary>
        /// <param name="cell"></param>
        /// <exception cref="ArgumentException">すでにあるセルと座標が重複した時</exception>
        public void addCell(Cell cell)
        {
            if (getCellAtCube(cell.position) != null)
            {
                throw new ArgumentException("追加しようとしたセルの座標には別のセルがすでにいます", new Exception());
            }
            cells.Add(cell);
        }

        /// <summary>
        /// 中央を０とした時のQR座標からセルを引っ張り出します
        /// </summary>
        /// <param name="position"></param>
        /// <returns>該当位置のセル</returns>
        /// <exception cref="ArgumentOutOfRangeException">対象の座標のセルがない時</exception>
        public Cell getCellAtQR(QRCoordinate position)
        {
            CubeCoordinate position_requested = CubeCoordinate.fromQR(position);

            var cell = getCellAtCube(position_requested);

            return cell;
        }

        /// <summary>
        /// Cube座標にあるセルを求めます
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">対象の座標のセルがない時</exception>
        public Cell getCellAtCube(CubeCoordinate position)
        {
            foreach (var cell in cells)
            {
                if (cell.position == position)
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
        public List<Cell> findAdjacentOf(Cell cell)
        {
            List<CubeCoordinate> adjacent_relative_positions = CubeCoordinate.AdjacentRelatives();
            List<CubeCoordinate> position_to_find = new List<CubeCoordinate>();
            List<Cell> cells_to_return = new List<Cell>();
            
            foreach (var relative_position in adjacent_relative_positions)
            {
                position_to_find.Add(cell.position + relative_position);
            }

            foreach (var position_requested in position_to_find)
            {
                var cell_found = getCellAtCube(position_requested);
                if (cell_found != null) cells_to_return.Add(cell_found);
            }

            return cells_to_return;
        }
        
    }
}