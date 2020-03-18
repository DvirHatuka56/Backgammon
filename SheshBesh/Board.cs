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
            MainWindow.OnSpikeClicked += OnSpikeClicked;
            FirstClick = true;
            
            Spikes = new Spike[2, 12];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Spikes[i, j] = new Spike();
                }
            }

            for (int i = 0; i < 12; i++)
            {
                Spikes[0, i].SpikeId = 11 - i;
            }
            for (int i = 0; i < 12; i++)
            {
                Spikes[1, i].SpikeId = 12 + i;
            }
            
            Spikes[0, 0].SoldiersCount = 5;
            Spikes[0, 0].Black = true;
            Spikes[1, 0].SoldiersCount = 5;
            Spikes[1, 0].Black = false;
            
            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;
            
            Spikes[0, 4].SoldiersCount = 3;
            Spikes[0, 4].Black = false;
            Spikes[1, 4].SoldiersCount = 3;
            Spikes[1, 4].Black = true;
            
            Spikes[0, 6].SoldiersCount = 5;
            Spikes[0, 6].Black = false;
            Spikes[1, 6].SoldiersCount = 5;
            Spikes[1, 6].Black = true;
            
            Spikes[0, 11].SoldiersCount = 2;
            Spikes[0, 11].Black = true;
            Spikes[1, 11].SoldiersCount = 2;
            Spikes[1, 11].Black = false;
        }

        private void OnSpikeClicked(int row, int column, int cube1, int cube2)
        {
            if (FirstClick)
            {
                if (Spikes[row, column].IsEmpty())
                {
                    return;
                }

                int l1, l2, l3;
                (l1, l2, l3) = GetPreviewLocation(row, column, cube1, cube2);
                
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

            if (!CheckSpike(row, column)) return;
            if (!CheckMoveDirection(row, column)) return;
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

        private bool CheckSpike(int row, int column)
        {
            return Spikes[row, column].Black == srcSpike.Spike.Black ||
                   Spikes[row, column].IsEmpty() ||
                   Spikes[row, column].SoldiersCount <= 1;
        }

        private bool CheckMoveDirection(int row, int column)
        {
            return Spikes[row, column].SpikeId < srcSpike.Spike.SpikeId && !srcSpike.Spike.Black ||
                   Spikes[row, column].SpikeId > srcSpike.Spike.SpikeId && srcSpike.Spike.Black;
        }

        private (int l1, int l2, int l3) GetPreviewLocation(int row, int column, int cube1, int cube2)
        {
            if (Spikes[row, column].Black)
            {
                int id1 = Spikes[row, column].SpikeId + cube1;
                int id2 = Spikes[row, column].SpikeId + cube2;
                int id3 = Spikes[row, column].SpikeId + cube1 + cube2;
                return (id1, id2, id3);
            }
            
            if (!Spikes[row, column].Black)
            {
                int id1 = Spikes[row, column].SpikeId - cube1;
                int id2 = Spikes[row, column].SpikeId - cube2;
                int id3 = Spikes[row, column].SpikeId - (cube1 + cube2);
                return (id1, id2, id3);
            }

            return (0, 0, 0);

        }

        private (int row, int column) GetSpikeById(int spikeId)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (Spikes[i, j].SpikeId == spikeId)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }
    }
}