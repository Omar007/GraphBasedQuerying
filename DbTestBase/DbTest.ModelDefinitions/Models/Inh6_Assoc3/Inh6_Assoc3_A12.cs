using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc3
{
    public class Inh6_Assoc3_A12 : Inh6_Assoc3_A00
    {
        public Inh6_Assoc3_A12()
        {
            D00s = new HashSet<Inh6_Assoc3_D00>();
        }

        public string dA12 { get; set; }

        public virtual ICollection<Inh6_Assoc3_D00> D00s { get; set; }
    }
}
