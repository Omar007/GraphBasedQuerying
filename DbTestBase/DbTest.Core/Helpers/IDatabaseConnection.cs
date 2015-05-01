
namespace DbTest.Core
{
    public interface IDatabaseConnection
    {
        string Name { get; }

        string CreateConnectionString(string modelName, Population population);
    }
}
