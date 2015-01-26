using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc1
{
    public class Inh3_Assoc1_A10 : Inh3_Assoc1_A00
    {
        public Inh3_Assoc1_A10()
        {
            B00s = new HashSet<Inh3_Assoc1_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh3_Assoc1_B00> B00s { get; set; }
    }
}
