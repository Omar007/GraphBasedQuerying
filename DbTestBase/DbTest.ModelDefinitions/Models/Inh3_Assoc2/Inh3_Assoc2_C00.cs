
namespace DbTest.ModelDefinitions.Models.Inh3_Assoc2
{
    public class Inh3_Assoc2_C00
    {
        public int Id { get; set; }
        public string dC00 { get; set; }

        public virtual Inh3_Assoc2_A11 A11 { get; set; }
    }
}
