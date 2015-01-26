
namespace DbTest.ModelDefinitions.Models.Inh5Wide_Assoc1
{
    public class Inh5Wide_Assoc1_B00
    {
        public int Id { get; set; }
        public string dB00 { get; set; }

        public virtual Inh5Wide_Assoc1_A10 A10 { get; set; }
    }
}
