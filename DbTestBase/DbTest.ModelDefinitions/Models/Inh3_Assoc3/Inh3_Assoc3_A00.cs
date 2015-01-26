
namespace DbTest.ModelDefinitions.Models.Inh3_Assoc3
{
    public abstract class Inh3_Assoc3_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh3_Assoc3_E00 E00 { get; set; }
    }
}
