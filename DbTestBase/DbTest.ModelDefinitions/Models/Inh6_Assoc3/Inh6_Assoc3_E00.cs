using System.Collections.Generic;

namespace DbTest.ModelDefinitions.Models.Inh6_Assoc3
{
    public abstract class Inh6_Assoc3_E00
    {
        public Inh6_Assoc3_E00()
        {
            A00s = new HashSet<Inh6_Assoc3_A00>();
        }

        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh6_Assoc3_O O { get; set; }
        public virtual ICollection<Inh6_Assoc3_A00> A00s { get; set; }
    }
}
