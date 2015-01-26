
namespace DbTest.ModelDefinitions.Models.Inh3_Assoc3
{
    public class Inh3_Assoc3_B00
    {
        public int Id { get; set; }
        public string dB00 { get; set; }

        public virtual Inh3_Assoc3_A10 A10 { get; set; }
    }
}
