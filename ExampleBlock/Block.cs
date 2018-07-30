// Example Cell Data Object
using HexagonalMap.Domain.HexMap;

/*! セルデータ一例 */
namespace ExampleBlock.ExampleCellData
{
    /// <summary>
    /// データの例
    /// </summary>
    public enum States
    {
        Foo,
        Bar,
        Count
    }

    /// <summary>
    /// セルデータ
    /// </summary>
    public class Block
    {
        /// <summary>
        /// データ
        /// </summary>
        public States _state
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 座標位置への参照
        /// </summary>
        public Cell _cell
        {
            get { return _cell; }
            private set { _cell = value; }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="state"></param>
        public Block(Cell cell, States state)
        {
            _cell = cell;
            _state = state;
        }
    }
}