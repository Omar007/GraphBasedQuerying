using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc3
{
    public class Inh6_Assoc3_A11 : Inh6_Assoc3_A00
    {
        public Inh6_Assoc3_A11()
        {
            C00s = new HashSet<Inh6_Assoc3_C00>();
        }

        public string dA11 { get; set; }

        public virtual ICollection<Inh6_Assoc3_C00> C00s { get; set; }
    }
}
