using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc3
{
    public class Inh5_Assoc3_A10 : Inh5_Assoc3_A00
    {
        public Inh5_Assoc3_A10()
        {
            B00s = new HashSet<Inh5_Assoc3_B00>();
        }

        public string dA10 { get; set; }

        public virtual ICollection<Inh5_Assoc3_B00> B00s { get; set; }
    }
}
