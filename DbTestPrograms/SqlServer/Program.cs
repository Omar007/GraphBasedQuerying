using System.Collections.Generic;
using DbTest.Core;
using DbTest.ModelDefinitions;
using SqlServer.DbHelpers;

namespace SqlServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var populations = new List<Population>
            {
                new Population(100, 75, 5, 5, 5),
                new Population(200, 75, 5, 5, 5),
                new Population(300, 75, 5, 5, 5),
                new Population(400, 75, 5, 5, 5),
                new Population(500, 75, 5, 5, 5),
                new Population(600, 75, 5, 5, 5),
                new Population(700, 75, 5, 5, 5), 
                new Population(800, 75, 5, 5, 5),
                new Population(900, 75, 5, 5, 5),
                new Population(1000, 75, 5, 5, 5),
            };

            new Main(args).Run(new LocalDbConnection(), populations);
        }
    }
}
