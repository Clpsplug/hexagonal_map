// Example Cell Data Object
using HexagonalMap.Domain.HexMap;

namespace HexagonalMap.Domain.Block
{
    public enum States
    {
        Foo,
        Bar,
        Count
    }

    public class Block
    {
        public States _state
        {
            get { return _state; }
            set { _state = value; }
        }

        public Cell _cell
        {
            get { return _cell; }
            private set { _cell = value; }
        }
        
        public Block(States state)
        {
            _state = state;
        }
    }
}