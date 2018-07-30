// Hexagonal Map 
// https://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;

/*! 六角座標系・六角平面定義 */
namespace HexagonalMap.Domain.HexMap
{

    /// <summary>
    /// 立方体座標系
    /// </summary>
    /// <remarks>立方体と x + y + z = 0 なる平面との切り口を元にした座標系</remarks>
    public struct CubeCoordinate
    {
        /// <summary>
        /// X座標 (flat-toppedの場合、左上-左下方向で０)
        /// </summary>
        public int x 
        {
            get { return x; }
            private set { x = value; }
        }
        
        /// <summary>
        /// Y座標 (flat-toppedの場合、上下方向で０)
        /// </summary>
        public int y
        {
            get { return y; }
            private set { y = value; }
        }
        
        /// <summary>
        /// Z座標 (flat-toppedの場合、右上-右下方向で０)
        /// </summary>
        public int z
        {
            get { return z; }
            private set { z = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="z">Z座標</param>
        /// <remarks>これはそのまま使えない 生成するにはFromCubeなどを使用する</remarks>
        /// <exception cref="ArgumentException">x + y + z == 0でない時</exception>
        private CubeCoordinate(int x, int y, int z)
        {
            if (x + y + z != 0)
            {
                throw new ArgumentException("Impossible coordinate. x, y, z must add up to 0.", new Exception());
            }
            
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Cube座標の数値からCube座標を生成する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="z">Z座標</param>
        /// <returns>Cube座標オブジェクト</returns>
        public static CubeCoordinate FromCube(int x, int y, int z)
        {
            return new CubeCoordinate(x, y, z);
        }
  
        /// <summary>
        /// QR座標の数値からCube座標を生成する
        /// </summary>
        /// <param name="q">Q座標</param>
        /// <param name="r">R座標</param>
        /// <returns>Cube座標オブジェクト</returns>
        public static CubeCoordinate FromQr(int q, int r)
        {
            var x = q;
            var z = r;
            var y = -x - z;
            
            return new CubeCoordinate(x, y, z);
        }
        
        /// <summary>
        /// QR座標オブジェクトからCube座標を生成する
        /// </summary>
        /// <param name="input">QR座標オブジェクト</param>
        /// <returns>Cube座標オブジェクト</returns>
        public static CubeCoordinate FromQr(QRCoordinate input)
        {
            return FromQr(input.q, input.r);
        }

        /// <summary>
        /// 自分をQR座標化する
        /// </summary>
        /// <returns>QR座標オブジェクト</returns>
        public QRCoordinate ToQr()
        {
            return QRCoordinate.FromCube(this);
        }
        
        /// <summary>
        /// ある座標から６近傍を見つけるための相対座標を返す
        /// </summary>
        /// <returns>６近傍の相対座標。それぞれを座標に加算すれば６近傍を表す座標になる</returns>
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
        /// 加算 セルからセルへ移動するときに使える
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x + b.x, a.y + b.y, a.z + b.z);   
        }

        /// <summary>
        /// 減算 距離計測?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        /// <summary>
        /// 一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x == b.x && a.y == b.y && a.z == b.z);
        }

        /// <summary>
        /// 不一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(CubeCoordinate a, CubeCoordinate b)
        {
            return (a.x != b.x || a.y != b.y || a.z != b.z);
        }

        /// <summary>
        /// ハッシュ生成
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x ^ y ^ z;
        }
        
        /// <summary>
        /// 一致(任意のオブジェクトと)
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
    /// QR座標系
    /// </summary>
    /// <remarks>標準のXY座標の1つの軸が30°傾いた座標。画面への描画を行う際に扱いやすい。</remarks>
    public struct QRCoordinate
    {
        /// <summary>
        /// q座標 (flat-topの場合、上下方向で0)
        /// </summary>
        public int q
        {
            get { return q; }
            private set { q = value; }
        }

        /// <summary>
        /// r座標 (flat-topの場合、左上-右下方向で0)
        /// </summary>
        public int r
        {
            get { return r; }
            private set { r = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="q">Q座標</param>
        /// <param name="r">R座標</param>
        private QRCoordinate(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        /// <summary>
        /// QR座標の数値からQR座標を生成する
        /// </summary>
        /// <param name="q">Q座標</param>
        /// <param name="r">R座標</param>
        /// <returns>QR座標オブジェクト</returns>
        public static QRCoordinate FromQr(int q, int r)
        {
            return new QRCoordinate(q, r);
        }

        /// <summary>
        /// Cube座標の数値からQR座標を生成する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="z">Z座標</param>
        /// <returns>QR座標オブジェクト</returns>
        public static QRCoordinate FromCube(int x, int y, int z)
        {
            return new QRCoordinate(x, z);
        }

        /// <summary>
        /// Cube座標オブジェクトからQR座標を生成する
        /// </summary>
        /// <param name="input">Cube座標オブジェクト</param>
        /// <returns>QR座標オブジェクト</returns>
        public static QRCoordinate FromCube(CubeCoordinate input)
        {
            return FromCube(input.x, input.y, input.z);
        }

        /// <summary>
        /// 自分をXYZ座標化する
        /// </summary>
        /// <returns>Cube座標オブジェクト</returns>
        public CubeCoordinate ToCube()
        {
            return CubeCoordinate.FromQr(this);
        }

        /// <summary>
        /// 加算 セルからセルへ移動するときに使える
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static QRCoordinate operator +(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q + b.q, a.r + b.r);
        }

        /// <summary>
        /// 減算 距離計測?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static QRCoordinate operator -(QRCoordinate a, QRCoordinate b)
        {
            return new QRCoordinate(a.q - b.q, a.r - b.r);
        }

        /// <summary>
        /// 一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(QRCoordinate a, QRCoordinate b)
        {
            return a.q == b.q && a.r == b.r;
        }

        /// <summary>
        /// 不一致
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(QRCoordinate a, QRCoordinate b)
        {
            return a.q != b.q || a.r != b.r;
        }

        /// <summary>
        /// ハッシュ生成
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return q ^ r;
        }

        /// <summary>
        /// 一致(任意のオブジェクトと)
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
    /// 座標位置を表すオブジェクト
    /// </summary>
    /// <remarks>内部的にはCube座標で持つが、それを取り出して相互変換可能。実データにこれを関連づけると良い</remarks>
    public class Cell
    {
        /// <summary>
        /// Cube座標での位置
        /// </summary>
        public CubeCoordinate Position
        {
            get { return Position; }
            private set { Position = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Cube座標で入力。そのほかの座標の場合は変換して入力</param>
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
        /// <summary>
        /// Cellの集まり
        /// </summary>
        /// <remarks>
        /// TODO: セルをハッシュテーブルで保管した方が高速?
        /// </remarks>
        private readonly List<Cell> _cells;

        /// <summary>
        /// Constructor
        /// </summary>
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
        /// QR座標からセルを返す
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
        /// Cube座標にあるセルを返す
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