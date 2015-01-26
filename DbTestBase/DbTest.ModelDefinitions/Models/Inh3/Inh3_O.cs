using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3
{
    public class Inh3_O
    {
        public Inh3_O()
        {
            E00s = new HashSet<Inh3_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh3_E00> E00s { get; set; }
    }
}
