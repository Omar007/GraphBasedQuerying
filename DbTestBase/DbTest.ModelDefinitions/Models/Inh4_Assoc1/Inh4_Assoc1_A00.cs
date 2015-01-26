
namespace DbTest.ModelDefinitions.Models.Inh4_Assoc1
{
    public abstract class Inh4_Assoc1_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh4_Assoc1_E00 E00 { get; set; }
    }
}
