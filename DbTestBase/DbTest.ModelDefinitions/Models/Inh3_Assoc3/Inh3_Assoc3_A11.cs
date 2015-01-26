using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc3
{
    public class Inh3_Assoc3_A11 : Inh3_Assoc3_A00
    {
        public Inh3_Assoc3_A11()
        {
            C00s = new HashSet<Inh3_Assoc3_C00>();
        }

        public string dA11 { get; set; }

        public virtual ICollection<Inh3_Assoc3_C00> C00s { get; set; }
    }
}
