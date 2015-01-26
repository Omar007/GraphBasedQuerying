using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh4
{
    public class Inh4_O
    {
        public Inh4_O()
        {
            E00s = new HashSet<Inh4_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh4_E00> E00s { get; set; }
    }
}
