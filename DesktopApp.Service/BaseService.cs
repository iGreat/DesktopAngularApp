using AutoMapper;
using DesktopApp.Common;
using SqlSugar;
using System;

namespace DesktopApp.Service
{
    public abstract class BaseService
    {
        private readonly ConnectionConfig _connectionConfig;

        protected readonly IMapper BaseMapper;

        protected BaseService(Action<IMapperConfigurationExpression> mapperConfig)
        {
            _connectionConfig = new ConnectionConfig
            {
                IsAutoCloseConnection = true,
                ConnectionString = AppConfig.ConnectionString,
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.SystemTable
            };

            BaseMapper = new Mapper(new MapperConfiguration(mapperConfig));
        }

        protected void Execute(Action<ISqlSugarClient> action)
        {
            using (var dbClient = new SqlSugarClient(_connectionConfig))
            {
                action(dbClient);
            }
        }

        protected T Execute<T>(Func<ISqlSugarClient, T> func)
        {
            using (var dbClient = new SqlSugarClient(_connectionConfig))
            {
                return func(dbClient);
            }
        }
    }
}
