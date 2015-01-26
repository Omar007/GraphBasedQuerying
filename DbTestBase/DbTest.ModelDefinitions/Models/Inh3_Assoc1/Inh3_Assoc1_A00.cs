
namespace DbTest.ModelDefinitions.Models.Inh3_Assoc1
{
    public abstract class Inh3_Assoc1_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh3_Assoc1_E00 E00 { get; set; }
    }
}
