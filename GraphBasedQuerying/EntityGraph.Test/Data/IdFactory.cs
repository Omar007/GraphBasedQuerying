
namespace EntityGraph.Test
{
    public class IdFactory
    {
        public static bool AutoGenerateKeys { get; set; }
        private static int Id { get; set; }

        public static int Assign
        {
            get
            {
                if (AutoGenerateKeys)
                {
                    Id--;
                    return Id;
                }
                return 0;
            }
        }
    }
}
