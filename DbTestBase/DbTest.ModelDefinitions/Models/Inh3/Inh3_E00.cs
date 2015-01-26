
namespace DbTest.ModelDefinitions.Models.Inh3
{
    public abstract class Inh3_E00
    {
        public int Id { get; set; }
        public string dE00 { get; set; }

        public virtual Inh3_O O { get; set; }
    }
}
