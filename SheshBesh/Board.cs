using System;
using System.Runtime.Serialization;

namespace SheshBesh
{
    public struct SpikeData
    {
        public Spike Spike { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int l1 { get; set; }
        public int l2 { get; set; }
        public int l3 { get; set; }
    }
    
    public class Board
    {
        public Spike[,] Spikes { get; }
        public bool FirstClick { get; private set; }
        public SpikeData srcSpike { get; private set; }
        public bool BlackTurn { get; set; }
        public bool TurnStart { get; set; }
        public int Cube1 { get; set; }
        public int Cube2 { get; set; }
        public int numTurns;
        public Board()
        {
            MainWindow.OnSpikeClicked += OnSpikeClicked;
            FirstClick = true;
            numTurns = -1;
            TurnStart = false;
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
                if (Spikes[row, column].IsEmpty() || Spikes[row, column].Black != BlackTurn) { return; }

                if (numTurns == -1) 
                {
                    Cube1 = cube1;
                    Cube2 = cube2;
                    SetNumTurns(row, column);
                }

                if (cube1 == cube2)
                {
                    int[] locs = GetDoublePreviewLocations(row, column, cube1);
                    for (var i = 0; i < locs.Length; i++)
                    {
                        (int r, int c) = GetSpikeById(locs[i]);
                        if (r == -1 || c == -1) continue;
                        Spikes[r, c].PreviewMode = CheckSpike(r, c) && CheckMoveDirection(r, c);
                    }
                    
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
                
                (int l1, int l2, int l3) = GetPreviewLocation(row, column, Cube1, Cube2);
                
                srcSpike = new SpikeData
                {
                    Column = column,
                    Row = row,
                    Spike = Spikes[row, column],
                    l1 = l1, l2 = l2, l3 = l3
                };

                if (l1 > 0 && l1 < 24) 
                {
                    (int rowl1, int columnl1) = GetSpikeById(l1);
                    Spikes[rowl1, columnl1].PreviewMode = CheckSpike(rowl1, columnl1) && CheckMoveDirection(rowl1, columnl1);
                }

                if (l2 > 0 && l2 < 24) 
                {
                    (int rowl2, int columnl2) = GetSpikeById(l2);
                    Spikes[rowl2, columnl2].PreviewMode = CheckSpike(rowl2, columnl2) && CheckMoveDirection(rowl2, columnl2);
                }

                if (l3 > 0 && l3 < 24) 
                {
                    (int rowl3, int columnl3) = GetSpikeById(l3);
                    Spikes[rowl3, columnl3].PreviewMode = CheckSpike(rowl3, columnl3) && CheckMoveDirection(rowl3, columnl3);
                }

                Spikes[row, column].Marked = true;
                FirstClick = false;
                return;
            }
            
            if (!Spikes[row, column].PreviewMode && !Spikes[row, column].Marked) return;
            
            Spikes[srcSpike.Row, srcSpike.Column].Marked = false;

            if (Spikes[row, column].IsEmpty())
            {
                Spikes[row, column].Black = srcSpike.Spike.Black;
            }

            if (Spikes[row, column].PreviewMode)
            {
                Spikes[srcSpike.Row, srcSpike.Column].SoldiersCount--;
                Spikes[row, column].SoldiersCount++;
            }

            if (cube1 != cube2)
            {
                if (Spikes[row, column].SpikeId == srcSpike.l1)
                {
                    Cube1 = 0;
                    numTurns--;
                }

                if (Spikes[row, column].SpikeId == srcSpike.l2)
                {
                    Cube2 = 0;
                    numTurns--;
                }

                if (Spikes[row, column].SpikeId == srcSpike.l3)
                {
                    Cube1 = 0;
                    Cube2 = 0;
                    numTurns -= 2;
                }
            }
            else
            {
                int turns = Math.Abs(srcSpike.Spike.SpikeId - Spikes[row, column].SpikeId) / cube1;
                numTurns -= turns;
            }
            for (int i = 0; i < Spikes.GetLength(0); i++)
            {
                for (int j = 0; j < Spikes.GetLength(1); j++)
                {
                    Spikes[i, j].PreviewMode = false;
                }
            }

            if (numTurns <= 0) 
            {
                numTurns = -1;
                BlackTurn = !BlackTurn;
            }
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
            return Spikes[row, column].SpikeId <= srcSpike.Spike.SpikeId && !srcSpike.Spike.Black ||
                   Spikes[row, column].SpikeId >= srcSpike.Spike.SpikeId && srcSpike.Spike.Black;
        }

        private (int l1, int l2, int l3) GetPreviewLocation(int row, int column, int cube1, int cube2)
        {
            if (Spikes[row, column].Black)
            {
                int id1 = cube1 == 0 ? -1 : Spikes[row, column].SpikeId + cube1;
                int id2 = cube2 == 0 ? -1 : Spikes[row, column].SpikeId + cube2;
                int id3 = Spikes[row, column].SpikeId + cube1 + cube2;
                return (id1, id2, id3);
            }
            
            if (!Spikes[row, column].Black)
            {
                int id1 = cube1 == 0 ? -1 : Spikes[row, column].SpikeId - cube1;
                int id2 = cube2 == 0 ? -1 : Spikes[row, column].SpikeId - cube2;
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

        private void SetNumTurns(int row, int column)
        {
            if (Cube1==Cube2)
            {
                numTurns = 4;
                return;
            }
            
            numTurns = 0;
            (int l1, int l2, int l3) = GetPreviewLocation(row, column, Cube1, Cube2);
            
            if (l1 > 0 && l1 < 24)
            {
                numTurns++;
            }

            if (l2 > 0 && l2 < 24) 
            {
                numTurns++;
            }

            if (l3 > 0 && l3 < 24)
            {
                numTurns = numTurns < 2 ? numTurns + 1 : numTurns;
            }

        }

        private int[] GetDoublePreviewLocations(int row, int column, int cube)
        {
            int[] locs=new int[numTurns];
            for (int i = 1; i < numTurns + 1; i++)
            {
                locs[i - 1] = Spikes[row, column].SpikeId + (cube * i) * (Spikes[row, column].Black ? 1 : -1);
            }

            return locs;
        }
    }
}