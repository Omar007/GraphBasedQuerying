
namespace DbTest.ModelDefinitions.Models.Inh6_Assoc2
{
    public abstract class Inh6_Assoc2_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh6_Assoc2_E00 E00 { get; set; }
    }
}
