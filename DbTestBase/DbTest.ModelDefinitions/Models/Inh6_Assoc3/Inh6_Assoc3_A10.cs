using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc3
{
    public class Inh6_Assoc3_A10 : Inh6_Assoc3_A00
    {
        public Inh6_Assoc3_A10()
        {
            B00s = new HashSet<Inh6_Assoc3_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh6_Assoc3_B00> B00s { get; set; }
    }
}
