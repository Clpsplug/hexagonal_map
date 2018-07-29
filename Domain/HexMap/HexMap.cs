﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;

namespace src.Domain.HexMap
{
    /// <summary>
    ///  中心型６角形座標を３次元座標として扱うための座標系
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
        /// XYZ座標から
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static CubeCoordinate fromXYZ(int x, int y, int z)
        {
            return new CubeCoordinate(x, y, z);
        }
  
        /// <summary>
        /// QR座標から3次元座標を作る
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <remarks>https://www.redblobgames.com/grids/hexagons/</remarks>
        /// <returns></returns>
        public static CubeCoordinate fromQR(int q, int r)
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
        public static CubeCoordinate fromQR(QRCoordinate input)
        {
            return fromQR(input.q, input.r);
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
    }

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
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static QRCoordinate fromQR(int q, int r)
        {
            return new QRCoordinate(q, r);
        }

        /// <summary>
        /// XYZ座標から
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static QRCoordinate fromXYZ(int x, int y, int z)
        {
            return new QRCoordinate(x, z);
        }

        /// <summary>
        /// XYZ座標から
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static QRCoordinate fromXYZ(CubeCoordinate input)
        {
            return fromXYZ(input.x, input.y, input.z);
        }
    }
    

    /// <summary>
    /// ブロック１個分のデータ
    /// </summary>
    public class Cell
    {
        public CubeCoordinate position
        {
            get { return position; }
            private set { position = value; }
        }

        [NotNull]
        public Block.Block data
        {
            get { return data; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                data = value;
            }
        }

        public Cell(CubeCoordinate position, Block.Block data)
        {
            this.position = position;
            this.data = data;
        }
        
        
    }

    public class Field
    {
        private List<Cell> cells;

        /// <summary>
        /// セルをフィールドに追加
        /// </summary>
        /// <param name="cell"></param>
        /// <exception cref="ArgumentException">すでにあるセルと座標が重複した時</exception>
        public void addCell(Cell cell)
        {
            if (findCell(cell.position) != null)
            {
                throw new ArgumentException("追加しようとしたセルの座標には別のセルがすでにいます", new Exception());
            }
            cells.Add(cell);
        }
        
        /// <summary>
        /// 中央を０とした時のQR座標からセルを引っ張り出します
        /// </summary>
        /// <param name="q">中央を０とした時のq座標</param>
        /// <param name="r">中央を０とした時のr座標</param>
        /// <returns>該当位置のセル</returns>
        /// <exception cref="ArgumentOutOfRangeException">対象の座標のセルがない時</exception>
        public Cell getCellAt(int q, int r)
        {
            CubeCoordinate position_requested = CubeCoordinate.fromQR(q, r);

            var cell = findCell(position_requested);

            if (cell == null)
            {
                throw new ArgumentOutOfRangeException("セルがフィールド外にいます もしくはその座標のセルがフィールドから抜け落ちています", new Exception());   
            }

            return cell;
        }

        /// <summary>
        /// XYZ座標にあるセルを求めます
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Cell findCell(CubeCoordinate position)
        {
            foreach (var cell in cells)
            {
                if (cell.position == position)
                {
                    return cell;
                }
            }
            return null;
        }
    }
}