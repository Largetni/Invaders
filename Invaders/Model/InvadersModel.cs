using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Invaders.Model
{
    class InvadersModel
    {
        public readonly static Size PlayAreaSize = new Size(400, 300);
        public const int MaximumPlayerShots = 3;
        public const int InitialStarCount = 50;

        private readonly Random _random = new Random();

        public int Score { get; private set; }
        public int Wave { get; private set; }
        public int Lives { get; private set; }

        public bool GameOver { get; private set; }

        private DateTime? _playerDied = null;
        public bool PlayerDying { get { return _playerDied.HasValue; } }

        private Player _player;

        private readonly List<Invader> _invaders = new List<Invader>();
        private readonly List<Shot> _playerShots = new List<Shot>();
        private readonly List<Shot> _invaderShots = new List<Shot>();
        private readonly List<Point> _stars = new List<Point>();

        private Direction _invaderDirection = Direction.Left;
       // private bool _justMoved = false;
        private DateTime _lastUpdated = System.DateTime.MinValue;
       // private bool _justMovedDown=false;
        public InvadersModel() 
        { 
            EndGame(); 
        }

        public void EndGame()
        {
            GameOver = true;
        }

        public void StartGame()
        {
            GameOver = false;
            foreach (Invader invader in _invaders)
                OnShipChanged(invader, true);
            _invaders.Clear();

            foreach (Shot invaderShot in _invaderShots)
                OnShotMoved(invaderShot,true);

            _invaderShots.Clear();

            foreach (Shot playerShot in _playerShots)
                OnShotMoved(playerShot, true);
            _playerShots.Clear();

            foreach (Point star in _stars)
                OnStarChanged(star,true);
            _stars.Clear();
            for (int i = 0; i < InitialStarCount; i++)
            {
                _stars.Add(new Point((int)_random.Next(0,(int)PlayAreaSize.Width),(int)_random.Next(0,(int)PlayAreaSize.Height)));
            }

            _player = new Player();
            OnShipChanged(_player,false);
            Lives = 2;
            Wave = 0;
            NextWave();
           

        }
        public void NextWave()
        {
           int columnCount=11; 
            double span = 1.4 * Invader.InvaderSize.Width;
            Point waveInsertioPoint = new Point((PlayAreaSize.Width / 2) - ((span*columnCount/2)), span*6);
            for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y), Invader.InvaderSize, InvaderType.Star, 10));
            }
             for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y - span), Invader.InvaderSize, InvaderType.Satellite, 20));
            }
            for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y - 2 * span), Invader.InvaderSize, InvaderType.Satellite, 20));
            }
            for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y - 3 * span), Invader.InvaderSize, InvaderType.Saucer, 30));
            }
            for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y - 4 * span), Invader.InvaderSize, InvaderType.Bug, 40));
            }
            for (int i = 0; i < columnCount; i++)
            {
                _invaders.Add(new Invader(new Point(waveInsertioPoint.X + ( i * span ), waveInsertioPoint.Y - 5 * span), Invader.InvaderSize, InvaderType.Spaceship, 50));
            }
            
            Wave++;
        }

        public void FireShot()
        {
            if (GameOver || PlayerDying || _lastUpdated == System.DateTime.MinValue)
                return;

            if (_playerShots.Count < MaximumPlayerShots)
            {
                Shot shotFired = new Shot(new Point(_player.Location.X + (_player.Size.Width / 2) - 1, _player.Location.Y),
                    Direction.Up);
                _playerShots.Add(shotFired);
                OnShotMoved(shotFired, false);
            }
        }
        
        public void MovePlayer(Direction direction)
        { 
            if(!PlayerDying)
            {
                _player.Move(direction);
                OnShipChanged(_player, false);
            }
        }

        public void Twinkle()
        {
            
            if ((_random.Next(0, 1) == 0)&&( _stars.Count()>=8))
            {
                Point starToRemove = _stars[(int)_random.Next(0, _stars.Count())];
                OnStarChanged(starToRemove, true);
                _stars.Remove(starToRemove);
            }
            if ((_random.Next(0, 1) == 1) && (_stars.Count() < 75))
            {
                Point starToAdd = new Point(_random.Next(0, (int)PlayAreaSize.Width), _random.Next(0, (int)PlayAreaSize.Height));
                _stars.Add(starToAdd);
                OnStarChanged(starToAdd, false);
            }
        }
        

        private void MoveShots()
        {

            List<Shot> playerShots = _playerShots.ToList();
            foreach (Shot shot in playerShots)
            {
                shot.Move();
                if (shot.Location.Y < 0)
                {
                    OnShotMoved(shot, true);
                    _playerShots.Remove(shot);
                }
                else
                    OnShotMoved(shot, false);

            }
            List<Shot> invadersShots = _invaderShots.ToList();
            foreach (Shot shot in invadersShots)
            {
                shot.Move();
                OnShotMoved(shot, false);
                if (shot.Location.Y > PlayAreaSize.Height|| shot.Location.Y<0)
                {
                    OnShotMoved(shot, true);
                    _invaderShots.Remove(shot);
                }
                
            }
        }
        public void Update(bool paused)
        {
            if (!paused)
            {
                
               
                    if (_invaders.Count() == 0)
                    {
                        NextWave();
                    }

                    if (!PlayerDying)
                    {
                        MoveInvaders();
                        MoveShots();


                        CheckForInvaderCollsion();
                        CheckForPlayerCollision();
                        ReturnFire();

                    }
                    if (PlayerDying && TimeSpan.FromSeconds(2.5) < DateTime.Now - _playerDied)
                    {
                        _playerDied = null;
                        OnShipChanged(_player, false);
                    }
                    Twinkle();
            }
        }

       private void MoveInvaders() //moja metoda
        {
            TimeSpan timeSinceMoved = DateTime.Now - _lastUpdated;
            double moveSpeed =  _invaders.Count()*30/(Wave);
            if (timeSinceMoved > TimeSpan.FromMilliseconds(moveSpeed))
            {
                _lastUpdated = DateTime.Now;
                if (_invaderDirection == Direction.Right && !IsInvaderNearTheEnd())
                {
                    foreach (Invader invader in _invaders)
                    {
                        invader.Move(_invaderDirection);
                        OnShipChanged(invader, false);
                    }
                }
                else if (_invaderDirection == Direction.Right && IsInvaderNearTheEnd())
                {
                    foreach (Invader invader in _invaders)
                    {
                        invader.Move(Direction.Down);
                        OnShipChanged(invader, false);
                        
                    }
                    foreach (Invader invader in _invaders)
                    {
                       ;
                        invader.Move(Direction.Left);
                        OnShipChanged(invader, false);
                    }

                    _invaderDirection = Direction.Left;
                
                }
                    
                if (_invaderDirection == Direction.Left && !IsInvaderNearTheEnd())
                {
                    foreach (Invader invader in _invaders)
                    {

                        invader.Move(_invaderDirection);
                        OnShipChanged(invader, false);
                    }

                }
                else if (_invaderDirection == Direction.Left && IsInvaderNearTheEnd())
                {
                    foreach (Invader invader in _invaders)
                    {

                        invader.Move(Direction.Down);
                        OnShipChanged(invader, false);
                        invader.Move(Direction.Right);
                        OnShipChanged(invader, false);
                    }
                    foreach (Invader invader in _invaders)
                    {

                        invader.Move(Direction.Down);
                        OnShipChanged(invader, false);
                        invader.Move(Direction.Right);
                        OnShipChanged(invader, false);
                    }
                    _invaderDirection = Direction.Right;
                }
                foreach (Invader invader in _invaders)
                {
                    if (invader.Location.Y >= PlayAreaSize.Height - 20 - Invader.InvaderSize.Height)
                        EndGame();
                }
            }
        }
        
        private bool IsInvaderNearTheEnd()  //moja metoda
        {
            if (_invaderDirection == Direction.Right)
            {
                var nearInvader = from invader in _invaders
                                  where invader.Location.X + Invader.InvaderSize.Width > (PlayAreaSize.Width - 2 * Invader.HorizontalPixelsPerMove)
                                  select invader;
                if (nearInvader.Count() != 0)
                { return true; }
                else return false;
            }
            else if (_invaderDirection == Direction.Left)
            {
                var nearInvader = from invader in _invaders
                                  where invader.Area.Left <  2 * Invader.HorizontalPixelsPerMove 
                                  select invader;
                if (nearInvader.Count() != 0)
                { return true; }
                else return false;
            }
            else return false;
        
        }
        
        private void CheckForPlayerCollision()
        {
            List<Shot> invadersShots = _invaderShots.ToList();
            foreach (Shot shot in invadersShots)
            {
                if (CheckCollsion(_player.Area, new Rect(shot.Location, Shot.ShotSize)))
                {
                    Lives -= 1;
                    _playerDied = DateTime.Now;
                    if (Lives < 0)
                    {
                        EndGame();
                    }
                }

            }
        }

        private void CheckForInvaderCollsion()
        {
         
                List<Shot> playerShots = _playerShots.ToList();
                List<Shot> invadersShots = _invaderShots.ToList();
                List<Invader> invaders = _invaders.ToList();
                foreach (Shot shot in playerShots)
                {

                    var invadersHit = from invader in invaders
                                      where CheckCollsion(invader.Area, new Rect(shot.Location, Shot.ShotSize))
                                      select invader;


                    foreach (Invader deadInvader in invadersHit)
                    {
                        Score += deadInvader.Score;
                        OnShipChanged(deadInvader, true);
                        _invaders.Remove(deadInvader);
                        OnShotMoved(shot, true);
                        _playerShots.Remove(shot);
                       
                    }
                }
        
            
            
        }
        private static bool CheckCollsion(Rect r1, Rect r2)
        {
            r1.Intersect(r2);
            if (r1.Width > 0 || r1.Height > 0)
                return true;
            return false;
        
        }





        private void ReturnFire() 
        {
            
            if (_invaderShots.Count() >= Wave )
            {
                return;
            }
            else if (_random.Next(10) > 10 - Wave)
            {
                return;
            }
            else if(_invaders.Count()>0)
           
            {
                var invadersColumn = from invader in _invaders                 
                                     group invader by invader.Location.X
                                         into invaderColumn
                                         orderby invaderColumn.Key ascending
                                         select invaderColumn;


                var randomColumn = invadersColumn.ElementAt(_random.Next(invadersColumn.Count()));
                var shootingInvader = randomColumn.First();

                double newShotX = shootingInvader.Location.X + (shootingInvader.Size.Width / 2);
                double newShotY = shootingInvader.Location.Y + (shootingInvader.Size.Height );
                Shot newInvaderShot = new Shot(new Point(newShotX, newShotY), Direction.Down);
                _invaderShots.Add(newInvaderShot);
                OnShotMoved(newInvaderShot,false);
            }

        }

        public event EventHandler<StarChangedEventArgs> StarChanged;

        private void OnStarChanged(Point star, bool disappeared)
        {
            EventHandler<StarChangedEventArgs> starChanged = StarChanged;
            if (starChanged != null)
            {
                starChanged(this, new StarChangedEventArgs(star, disappeared));
            }
        }

        public event EventHandler<ShotMovedEventArgs> ShotMoved;


        private void OnShotMoved(Shot shot, bool disappeared)
        {
            EventHandler<ShotMovedEventArgs> shotMoved = ShotMoved;
            if (shotMoved != null)
            {
                shotMoved(this, new ShotMovedEventArgs(shot,disappeared));
            }
        }

        public event EventHandler<ShipChangedEventArgs> ShipChanged;

        private void OnShipChanged(Ship ship, bool killed)
        {
            EventHandler<ShipChangedEventArgs> shipChanged = ShipChanged;
            if (shipChanged != null)
            {
                shipChanged(this, new ShipChangedEventArgs(ship, killed));
            }
        }


        internal void UpdateAllShipsAndStars()
        {
            foreach (Shot shot in _playerShots)
                OnShotMoved(shot, false);
            foreach (Invader ship in _invaders)
                OnShipChanged(ship, false);
            OnShipChanged(_player, false);
            foreach (Point point in _stars)
                OnStarChanged(point, false);
        }
    }
}
