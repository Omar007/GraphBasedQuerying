using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc1
{
    public abstract class Inh3_Assoc1_E00
    {
        public Inh3_Assoc1_E00()
        {
            A00s = new HashSet<Inh3_Assoc1_A00>();
        }

        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh3_Assoc1_O O { get; set; }
        public virtual ICollection<Inh3_Assoc1_A00> A00s { get; set; }
    }
}
