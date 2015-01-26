using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh5_Assoc2
{
    public abstract class Inh5_Assoc2_E00
    {
        public Inh5_Assoc2_E00()
        {
            A00s = new HashSet<Inh5_Assoc2_A00>();
        }

        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh5_Assoc2_O O { get; set; }
        public virtual ICollection<Inh5_Assoc2_A00> A00s { get; set; }
    }
}
