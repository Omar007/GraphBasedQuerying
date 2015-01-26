using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc2
{
    public class Inh3_Assoc2_O
    {
        public Inh3_Assoc2_O()
        {
            E00s = new HashSet<Inh3_Assoc2_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh3_Assoc2_E00> E00s { get; set; }
    }
}
