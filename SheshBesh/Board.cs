namespace SheshBesh
{
    public struct SpikeData
    {
        public Spike Spike { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
    
    public class Board
    {
        public Spike[,] Spikes { get; }
        public bool FirstClick { get; private set; }
        public SpikeData srcSpike { get; private set; }

        public Board()
        {
            MainWindow.OnSpikeClicked += OnOnSpikeClicked;
            FirstClick = true;
            
            Spikes = new Spike[2, 12];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Spikes[i, j] = new Spike();
                }
            }
            
            Spikes[0, 0] = new Spike(5, false);
            Spikes[1, 0] = new Spike(5, true);
            
            Spikes[0, 4] = new Spike(3, false);
            Spikes[1, 4] = new Spike(3, true);
            
            Spikes[0, 6] = new Spike(5, false);
            Spikes[1, 6] = new Spike(5, true);
            
            Spikes[0, 11] = new Spike(2, false);
            Spikes[1, 11] = new Spike(2, true);
        }

        private void OnOnSpikeClicked(int row, int column)
        {
            if (FirstClick)
            {
                if (Spikes[row, column].IsEmpty()) { return; }
                srcSpike = new SpikeData
                {
                    Column = column,
                    Row = row,
                    Spike = Spikes[row, column]
                };
                Spikes[row, column].Marked = true;
                FirstClick = false;
                return;
            }

            Spikes[srcSpike.Row, srcSpike.Column].Marked = false;
            Spikes[srcSpike.Row, srcSpike.Column].SoldiersCount--;
            if (Spikes[row, column].IsEmpty())
            {
                Spikes[row, column].Black = srcSpike.Spike.Black;
            }
            Spikes[row, column].SoldiersCount++;
            FirstClick = true;
        }

        public Spike this[int row, int column] => Spikes[row, column];
    }
}