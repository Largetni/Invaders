using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;


namespace Invaders.Model
{
    class Player : Ship
    {
        static readonly Size PlayerSize = new Size(25, 15);
        private double _speedPerPixel = 10;
        private Point _insertionPoint = new Point((InvadersModel.PlayAreaSize.Width-PlayerSize.Width)/2, InvadersModel.PlayAreaSize.Height - PlayerSize.Height * 1.2);
        public Player()
            : base(new Point(PlayerSize.Width/2,0), PlayerSize)
        {
            Location = _insertionPoint;
        }
        override public void Move(Direction direction)
        {
            if (this.Location.X-_speedPerPixel <= 0)
            direction = Direction.Right;
                
            

            if (this.Location.X + PlayerSize.Width+ _speedPerPixel >= InvadersModel.PlayAreaSize.Width)
            direction = Direction.Left;
            
                switch (direction)
            {

                case Direction.Left:
                       Location = new Point(Location.X - _speedPerPixel, Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X + _speedPerPixel, Location.Y);
                    break;
                default:
                    break;
            }

        }
    }
}
