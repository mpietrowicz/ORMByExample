using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;

namespace ORMByExample.Core
{
    public enum DatabaseType
    {
        MSSQL,
        MSSQL2019,
        MySQL,
        SQLite,
        PostgreSQL,
        Oracle,
        Firebird,
        DB2,
        Informix,
        Ingres,
        H2,
        SQLAnywhere,
        Sybase,
        VistaDB,
        Access
    }

    public class HibernationSessionFactory
    {
        private static ISessionFactory? _sessionFactoryCurrent;
        private static ISessionFactory? _sessionFactoryMSSQL;
        public static ISessionFactory CreateSessionFactory(DatabaseType databaseType, string connectionString)
        {
            var runner = new MigrationsRunner();
            runner.Run(databaseType,connectionString);
            return _sessionFactoryCurrent ??= _sessionFactoryMSSQL??= Fluently.Configure()
                ?.Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                ?.Mappings(m => m.FluentMappings.AddFromAssemblyOf<HibernationSessionFactory>())
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                ?.BuildSessionFactory() ?? throw new DataException("No SessionFactory",new Exception(databaseType.ToString()),connectionString);
        }

        public static ISession CreateSession()
        {
            return _sessionFactoryCurrent?.OpenSession() ?? throw new Exception("No session factory or session");
        }

        public static IStatelessSession CreateStatelessSession()
        {
            return _sessionFactoryCurrent?.OpenStatelessSession() ?? throw new Exception("No Stateless session factory or session");
        }


    }
}