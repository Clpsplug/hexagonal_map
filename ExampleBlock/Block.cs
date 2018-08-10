// Example Cell Data Object

using HexagonalMap.Domain.HexMap;

/*! Example cell data using HexMap */
namespace HexagonalMap.ExampleBlock
{
    /// <summary>
    /// Example Enum
    /// </summary>
    public enum States
    {
        Foo,
        Bar,
        Count
    }

    /// <summary>
    /// Example cell data
    /// </summary>
    public class Block: Cell
    {
        /// <summary>
        /// data
        /// </summary>
        public States State { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="state"></param>
        public Block(States state)
        {
            State = state;
        }
    }
}