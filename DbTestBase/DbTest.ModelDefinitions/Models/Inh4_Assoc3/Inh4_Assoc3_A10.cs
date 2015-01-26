using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh4_Assoc3
{
    public class Inh4_Assoc3_A10 : Inh4_Assoc3_A00
    {
        public Inh4_Assoc3_A10()
        {
            B00s = new HashSet<Inh4_Assoc3_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh4_Assoc3_B00> B00s { get; set; }
    }
}
