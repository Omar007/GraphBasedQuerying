using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using EntityGraph4EF6.Mapping;

namespace EntityGraph4EF6
{
    internal class QueryPlan
    {
        public IDictionary<DbCommand, TypeMapping> CommandTypes { get; private set; }

        public QueryPlan(DbContext context, IDictionary<DbExpression, TypeMapping> queriesAndType)
        {
            CommandTypes = new Dictionary<DbCommand, TypeMapping>();

            foreach (var queryAndType in queriesAndType)
            {
#if DEBUG
                DbQueryCommandTree commandTree = null;
                try
                {
                    commandTree = new DbQueryCommandTree(context.GetObjectContext().MetadataWorkspace, DataSpace.SSpace, queryAndType.Key);
                }
                catch (Exception e)
                {
                    commandTree = new DbQueryCommandTree(context.GetObjectContext().MetadataWorkspace, DataSpace.SSpace, queryAndType.Key, false);
                }
                using (var logWriter = new StreamWriter("./cTree.log"))
                {
                    logWriter.Write(commandTree.ToString());
                }
#else
                var commandTree = new DbQueryCommandTree(context.GetObjectContext().MetadataWorkspace, DataSpace.SSpace, queryAndType.Key);
#endif
                var commandDef = DbProviderServices.GetProviderServices(context.Database.Connection).CreateCommandDefinition(commandTree);
                CommandTypes.Add(commandDef.CreateCommand(), queryAndType.Value);
            }
        }
    }
}
