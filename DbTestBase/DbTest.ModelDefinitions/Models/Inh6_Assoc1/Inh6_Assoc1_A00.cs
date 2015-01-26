
namespace DbTest.ModelDefinitions.Models.Inh6_Assoc1
{
    public abstract class Inh6_Assoc1_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh6_Assoc1_E00 E00 { get; set; }
    }
}
