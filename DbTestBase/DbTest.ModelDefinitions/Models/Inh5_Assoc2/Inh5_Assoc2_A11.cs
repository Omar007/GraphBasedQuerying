using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc2
{
    public class Inh5_Assoc2_A11 : Inh5_Assoc2_A00
    {
        public Inh5_Assoc2_A11()
        {
            C00s = new HashSet<Inh5_Assoc2_C00>();
        }

        public string dA11 { get; set; }

        public virtual ICollection<Inh5_Assoc2_C00> C00s { get; set; }
    }
}
