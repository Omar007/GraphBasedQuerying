
namespace DbTest.Core
{
    public class Population
    {
        //public readonly int OCount;
        public readonly int ECount;
        public readonly int ACount;
        public readonly int BCount;
        public readonly int CCount;
        public readonly int DCount;

        private Population(int oCount, int eCount, int aCount, int bCount, int cCount, int dCount)
        {
            //OCount = oCount;
            ECount = eCount;
            ACount = aCount;
            BCount = bCount;
            CCount = cCount;
            DCount = dCount;
        }

        public Population(int eCount, int aCount, int bCount, int cCount, int dCount)
            : this(eCount, eCount, aCount, bCount, cCount, dCount)
        {

        }

        public override string ToString()
        {
            return "E" + ECount
                + ".A" + ACount
                + ".B" + BCount
                + ".C" + CCount
                + ".D" + DCount;
        }
    }
}
