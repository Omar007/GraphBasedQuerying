using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc3
{
    public class Inh5_Assoc3_A11 : Inh5_Assoc3_A00
    {
        public Inh5_Assoc3_A11()
        {
            C00s = new HashSet<Inh5_Assoc3_C00>();
        }

        public string dA11 { get; set; }

        public virtual ICollection<Inh5_Assoc3_C00> C00s { get; set; }
    }
}
