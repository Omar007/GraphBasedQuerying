using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc2
{
    public class Inh5_Assoc2_O
    {
        public Inh5_Assoc2_O()
        {
            E00s = new HashSet<Inh5_Assoc2_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh5_Assoc2_E00> E00s { get; set; }
    }
}
