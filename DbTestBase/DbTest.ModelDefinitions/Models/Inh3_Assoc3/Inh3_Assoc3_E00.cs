using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh3_Assoc3
{
    public abstract class Inh3_Assoc3_E00
    {
        public Inh3_Assoc3_E00()
        {
            A00s = new HashSet<Inh3_Assoc3_A00>();
        }

        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh3_Assoc3_O O { get; set; }
        public virtual ICollection<Inh3_Assoc3_A00> A00s { get; set; }
    }
}
