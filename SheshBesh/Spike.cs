namespace SheshBesh
{
    public class Spike
    {
        public int SoldiersCount { get; set; }
        public bool Black { get; set; }
        public bool Marked { get; set; }

        public Spike()
        {
            SoldiersCount = 0;
            Black = true;
            Marked = false;
        }

        public Spike(int soldiersCount, bool black)
        {
            SoldiersCount = soldiersCount;
            Black = black;
            Marked = false;
        }

        public bool IsEmpty()
        {
            return SoldiersCount == 0;
        }

        public override string ToString()
        {
            return $"SoldiersCount = {SoldiersCount}, {(Black ? "Black": "White")} Player";
        }
    }
}