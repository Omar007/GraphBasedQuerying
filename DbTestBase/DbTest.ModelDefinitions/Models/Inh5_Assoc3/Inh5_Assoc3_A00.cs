
namespace DbTest.ModelDefinitions.Models.Inh5_Assoc3
{
    public abstract class Inh5_Assoc3_A00
    {
        public int Id { get; set; }
        public string dA00 { get; set; }

        public virtual Inh5_Assoc3_E00 E00 { get; set; }
    }
}
