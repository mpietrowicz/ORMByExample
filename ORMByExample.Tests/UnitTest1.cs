using DotNet.Testcontainers.Builders;
using NHibernate;
using ORMByExample.Core;
using Testcontainers;
using Testcontainers.MsSql;
namespace ORMByExample.Tests
{
    public class UnitTest1 : IClassFixture<ConnectionFixture>
    {
        public ConnectionFixture ConnectionFixture { get; }

        public UnitTest1(ConnectionFixture connectionFixture)
        {
            ConnectionFixture = connectionFixture ?? throw new ArgumentNullException(nameof(connectionFixture));
        }
        [Fact]
        public async Task GetHibernationSessionFactory_Success()
        {
            var factory = await ConnectionFixture.GetConnectionFactoryHibernate(DatabaseType.MSSQL);

           

            Assert.NotNull(factory);
            Assert.True( !factory.IsClosed);
            Assert.NotNull(factory.OpenStatelessSession());
            Assert.NotNull(factory.OpenSession());
        }
    }
    public class ConnectionFixture : IDisposable
    {
        readonly IDictionary<DatabaseType, ISessionFactory> _factories = new Dictionary<DatabaseType, ISessionFactory>();

        public async Task<ISessionFactory> GetConnectionFactoryHibernate(DatabaseType dbDatabaseType = DatabaseType.MSSQL)
        {
            if (_factories.TryGetValue(dbDatabaseType, out var connectionFactory))
            {
                return connectionFactory;
            }
            var connectionString = $"server={Guid.NewGuid():D};user id=sa;password={Guid.NewGuid():D};database={Guid.NewGuid():D}";


            //   var  msSqlConfiguration = new Testcontainers.MsSql.MsSqlConfiguration(Guid.NewGuid().ToString("D"),Guid.NewGuid().ToString("D"),Guid.NewGuid().ToString("D"));
            msSqlTestcontainer = new MsSqlBuilder()
                       .WithPassword(Guid.NewGuid().ToString("D"))
                   ?.WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                   .WithName(Guid.NewGuid().ToString("D"))
                   ?.WithCleanUp(true)
                   ?.Build();


            await msSqlTestcontainer?.StartAsync();
            var factory = HibernationSessionFactory.CreateSessionFactory(DatabaseType.MSSQL, msSqlTestcontainer.GetConnectionString());
            _factories.Add(dbDatabaseType, factory);
            return _factories[dbDatabaseType];
        }

        public MsSqlContainer? msSqlTestcontainer { get; set; }


        public void Dispose()
        {
            foreach (var factory in _factories)
            {
                factory.Value?.Close();
                factory.Value?.Dispose();
            }
            msSqlTestcontainer?.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}