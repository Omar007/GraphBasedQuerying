
namespace DbTest.ModelDefinitions.Models.Inh6_Assoc3
{
    public abstract class Inh6_Assoc3_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh6_Assoc3_E00 E00 { get; set; }
    }
}
