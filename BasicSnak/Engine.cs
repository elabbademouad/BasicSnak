using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicSnak
{
    public class Engine:INotifyPropertyChanged
    {
        private int _Speed;
        private bool _Pause;
        private bool _GameOver;
        private int _Score;
        private ObservableCollection<CellViewModel> _listEntity;
        public bool NewGame { get; set; }
        public int Score
        {
            get { return _Score; }
            set { _Score = value; OnPropertyChanged("Score"); }
        }
        public List<CellViewModel> Obstacles { get; set; }
        public DirectionEnum FutureDirection { get; set; }
        public bool GameOver
        {
            get { return _GameOver; }
            set { _GameOver = value;OnPropertyChanged("GameOver"); }
        }

        public bool Pause
        {
            get { return _Pause; }
            set { _Pause = value; }
        }
        private BackgroundWorker _backGroundWorker;
        public int Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }
        private ObservableCollection<CellViewModel> _Scene;

        public ObservableCollection<CellViewModel> Scene
        {
            get { return _Scene; }
            set { _Scene = value; }
        }
        private DirectionEnum _Direction;

        public DirectionEnum Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }
        private CellViewModel _Root;
        private CellViewModel _Object;

        public event PropertyChangedEventHandler PropertyChanged;

        public CellViewModel Object
        {
            get { return _Object; }
            set { _Object = value; }
        }

        public CellViewModel Root
        {
            get { return _Root; }
            set { _Root = value; }
        }

        public BackgroundWorker BackGroundWorker { get {return _backGroundWorker; } set { _backGroundWorker = value; } }
        int _i;
        public void Move()
        {
            
            switch (FutureDirection)
            {
                case DirectionEnum.Left:
                    if (Direction != DirectionEnum.Rigth)
                        Direction = FutureDirection;
                    break;
                case DirectionEnum.Rigth:
                    if (Direction != DirectionEnum.Left)
                        Direction = FutureDirection;
                    break;
                case DirectionEnum.Top:
                    if (Direction != DirectionEnum.Bottom)
                        Direction = FutureDirection;
                    break;
                case DirectionEnum.Bottom:
                    if (Direction != DirectionEnum.Top)
                        Direction = FutureDirection;
                    break;
            }
            if(Root==Object)
            {
                _i++;
                CellViewModel _lastEntity;
                if (_listEntity.Count != 0)
                    _lastEntity = _listEntity.Last();
                else
                    _lastEntity = Root;

                var entity= Scene.Where(c => c.Position.X == _lastEntity.Position.X && c.Position.Y == _lastEntity.Position.Y).First();
                entity.Style = StyleEnum.Entity;
                _listEntity.Add(entity);
                Random r = new Random();
                Object = (from c in Scene  orderby r.Next() where c.Style==StyleEnum.Default select c).First();
                Object.Style = StyleEnum.Object;
                Score=Score+10;
                if (Speed > 120)
                    Speed = Speed - 4;

                if (_i==4)
                {
                    var obstacle= (from c in Scene orderby r.Next() where c.Style == StyleEnum.Default select c).Take(4);
                    foreach (var item in obstacle)
                    {
                        item.Style = StyleEnum.Obstacle;
                        Obstacles.Add(item);
                    }
                    _i = 0;
                }
                
            }
            
            Root.Style = StyleEnum.Default;
            var _temp = Root;
            try
            {
                if (Obstacles.Contains(Root))
                    throw new Exception("GameOver");
                switch (Direction)
                {
                    case DirectionEnum.Left:
                        Root = Scene.Where(c => c.Position.X == Root.Position.X - 1 && c.Position.Y == Root.Position.Y).First();
                        Root.Style = StyleEnum.Entity;
                        break;
                    case DirectionEnum.Rigth:
                        Root = Scene.Where(c => c.Position.X == Root.Position.X + 1 && c.Position.Y == Root.Position.Y).First();
                        Root.Style = StyleEnum.Entity;
                        break;
                    case DirectionEnum.Top:
                        Root = Scene.Where(c => c.Position.X == Root.Position.X && c.Position.Y == Root.Position.Y - 1).First();
                        Root.Style = StyleEnum.Entity;
                        break;
                    case DirectionEnum.Bottom:
                        Root = Scene.Where(c => c.Position.X == Root.Position.X && c.Position.Y == Root.Position.Y + 1).First();
                        Root.Style = StyleEnum.Entity;
                        break;
                }
                if (_listEntity.Contains(Root))
                    throw new Exception("GameOver");

            }
            catch (Exception E)
            {
                if(E.Message== "GameOver")
                this.GameOver = true;
                else
                {
                    switch (Direction)
                    {
                        case DirectionEnum.Left:
                            Root = Scene.Where(c => c.Position.X == 29 && c.Position.Y == Root.Position.Y).First();
                            break;
                        case DirectionEnum.Rigth:
                            Root = Scene.Where(c => c.Position.X == 0 && c.Position.Y == Root.Position.Y).First();
                            break;
                        case DirectionEnum.Top:
                            Root = Scene.Where(c => c.Position.Y == 29 && c.Position.X == Root.Position.X).First();
                            break;
                        case DirectionEnum.Bottom:
                            Root = Scene.Where(c => c.Position.Y == 0 && c.Position.X == Root.Position.X).First();
                            break;
                    }
                    
                }
            }

            for (int i = 0; i < _listEntity.Count; i++)
            {
                var _temp2 = _listEntity[i];
                _listEntity[i].Style = StyleEnum.Default;
                _listEntity[i] = _temp;
                _listEntity[i].Style = StyleEnum.Entity;
                _temp = _temp2;

            }
        }
        public Engine()
        {
            _listEntity = new ObservableCollection<CellViewModel>();
            Obstacles = new List<CellViewModel>();
            Scene = new ObservableCollection<CellViewModel>();
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    CellViewModel newCell = new CellViewModel();
                    newCell.Style = StyleEnum.Default;
                    PositionViewModel position = new PositionViewModel();
                    position.X = j;
                    position.Y = i;
                    newCell.Position = position;
                    Scene.Add(newCell);
                }
            }
            BackGroundWorker = new BackgroundWorker();
            BackGroundWorker.WorkerReportsProgress = true;
            BackGroundWorker.WorkerSupportsCancellation = true;
            BackGroundWorker.ProgressChanged += _backGroundWorker_ProgressChanged;
            BackGroundWorker.DoWork += _backGroundWorker_DoWork;
            BackGroundWorker.RunWorkerCompleted += BackGroundWorker_RunWorkerCompleted;

        }

        private void BackGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(NewGame)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            _listEntity.Clear();
            Obstacles.Clear();
            foreach (var item in Scene)
            {
                item.Style = StyleEnum.Default;
            }
            Root= Scene.Where(c => c.Position.X == 15 && c.Position.Y == 15).First();
            Root.Style = StyleEnum.Entity;
            Direction = DirectionEnum.Rigth;
            Object = Scene.Where(c => c.Position.X == 25 && c.Position.Y == 25).First();
            Object.Style = StyleEnum.Object;
            _Speed = 200;
            GameOver = false;
            Pause = false;
            NewGame = false;
            Score = 0;
            BackGroundWorker.RunWorkerAsync();

        }

        private void _backGroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Move();
        }

        private void _backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!GameOver && !Pause&& !NewGame)
            {
                Thread.Sleep(Speed);
                BackGroundWorker.ReportProgress(1);
                
            }
            
        }
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
