using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6
{
    public class Inh6_O
    {
        public Inh6_O()
        {
            E00s = new HashSet<Inh6_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh6_E00> E00s { get; set; }
    }
}
