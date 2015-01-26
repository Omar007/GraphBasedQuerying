using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc3
{
    public class Inh3_Assoc3_A12 : Inh3_Assoc3_A00
    {
        public Inh3_Assoc3_A12()
        {
            D00s = new HashSet<Inh3_Assoc3_D00>();
        }

        public string dA12 { get; set; }

        public virtual ICollection<Inh3_Assoc3_D00> D00s { get; set; }
    }
}
