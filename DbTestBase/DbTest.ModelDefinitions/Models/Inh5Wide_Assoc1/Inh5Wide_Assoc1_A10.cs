using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5Wide_Assoc1
{
    public class Inh5Wide_Assoc1_A10 : Inh5Wide_Assoc1_A00
    {
        public Inh5Wide_Assoc1_A10()
        {
            B00s = new HashSet<Inh5Wide_Assoc1_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh5Wide_Assoc1_B00> B00s { get; set; }
    }
}
