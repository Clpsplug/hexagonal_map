namespace src.Domain.Block
{
    public enum States
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
        Trash,
        Count
    }

    public class Block
    {
        public States _state
        {
            get { return _state; }
            set { _state = value; }
        }

        public Block(States state)
        {
            _state = state;
        }
    }
}