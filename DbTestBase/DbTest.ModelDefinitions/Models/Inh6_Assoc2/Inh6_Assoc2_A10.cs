using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc2
{
    public class Inh6_Assoc2_A10 : Inh6_Assoc2_A00
    {
        public Inh6_Assoc2_A10()
        {
            B00s = new HashSet<Inh6_Assoc2_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh6_Assoc2_B00> B00s { get; set; }
    }
}
