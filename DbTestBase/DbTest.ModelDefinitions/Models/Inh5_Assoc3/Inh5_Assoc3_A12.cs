using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc3
{
    public class Inh5_Assoc3_A12 : Inh5_Assoc3_A00
    {
        public Inh5_Assoc3_A12()
        {
            D00s = new HashSet<Inh5_Assoc3_D00>();
        }

        public string dA12 { get; set; }

        public virtual ICollection<Inh5_Assoc3_D00> D00s { get; set; }
    }
}
