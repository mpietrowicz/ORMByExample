using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMByExample.Core
{
    public class MigrationsRunner
    {
        public void Run(DatabaseType type, string connectionString)
        {
            using var serviceProvider = CreateServices(type, connectionString);
            using var scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="connectionString"></param>
        private ServiceProvider CreateServices(DatabaseType databaseType, string connectionString)
        {

            if (databaseType == DatabaseType.MSSQL || databaseType == DatabaseType.MSSQL2019)
            {
                return new ServiceCollection()
                    .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSqlServer2016()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(GetType().Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    .BuildServiceProvider(false);
            }

            throw new NotSupportedException(databaseType.ToString());
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}
