using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
namespace Invaders.Model
{
    class Invader:Ship
    {
        public static Size InvaderSize= new Size(15,15);
        public InvaderType InvaderType { get; private set; }
        public int Score;

        public const double HorizontalPixelsPerMove = 5;
        public const double VerticalPixelsPerMove = 15;

        public Invader(Point location, Size size,InvaderType type, int score) : base(location, size) 
        {
            InvaderType = type;
            Score = score;
        }

        override public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    Location = new Point(Location.X,Location.Y + VerticalPixelsPerMove);
                    break;
                case Direction.Left:
                    Location = new Point(Location.X-HorizontalPixelsPerMove,Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X+HorizontalPixelsPerMove, Location.Y);
                    break;
                default:
                    break;
            }

        }

    }
}
