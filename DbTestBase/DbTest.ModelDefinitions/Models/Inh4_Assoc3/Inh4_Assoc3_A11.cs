using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh4_Assoc3
{
    public class Inh4_Assoc3_A11 : Inh4_Assoc3_A00
    {
        public Inh4_Assoc3_A11()
        {
            C00s = new HashSet<Inh4_Assoc3_C00>();
        }

        public string dA11 { get; set; }

        public virtual ICollection<Inh4_Assoc3_C00> C00s { get; set; }
    }
}
