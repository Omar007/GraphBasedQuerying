
namespace DbTest.ModelDefinitions.Models.Inh5_Assoc3
{
    public class Inh5_Assoc3_B00
    {
        public int Id { get; set; }
        public string dB00 { get; set; }

        public virtual Inh5_Assoc3_A10 A10 { get; set; }
    }
}
