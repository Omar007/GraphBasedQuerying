using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5Wide
{
    public class Inh5Wide_O
    {
        public Inh5Wide_O()
        {
            E00s = new HashSet<Inh5Wide_E00>();
        }

        public int Id { get; set; }
        public string dO { get; set; }

        public virtual ICollection<Inh5Wide_E00> E00s { get; set; }
    }
}
