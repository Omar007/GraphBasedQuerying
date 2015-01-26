
namespace DbTest.ModelDefinitions.Models.Inh4_Assoc2
{
    public abstract class Inh4_Assoc2_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh4_Assoc2_E00 E00 { get; set; }
    }
}
