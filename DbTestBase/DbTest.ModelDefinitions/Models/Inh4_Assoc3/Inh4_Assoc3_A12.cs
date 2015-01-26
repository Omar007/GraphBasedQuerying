using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh4_Assoc3
{
    public class Inh4_Assoc3_A12 : Inh4_Assoc3_A00
    {
        public Inh4_Assoc3_A12()
        {
            D00s = new HashSet<Inh4_Assoc3_D00>();
        }

        public string dA12 { get; set; }

        public virtual ICollection<Inh4_Assoc3_D00> D00s { get; set; }
    }
}
