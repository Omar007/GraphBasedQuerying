using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5Wide_Assoc1
{
    public abstract class Inh5Wide_Assoc1_E00
    {
        public Inh5Wide_Assoc1_E00()
        {
            A00s = new HashSet<Inh5Wide_Assoc1_A00>();
        }

        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh5Wide_Assoc1_O O { get; set; }
        public virtual ICollection<Inh5Wide_Assoc1_A00> A00s { get; set; }
    }
}
