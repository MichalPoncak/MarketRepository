﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWebAPI.Common
{
    public interface IDataAccessHelper
    {
        DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType);
        int ExecuteNonQuery(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        IDataReader GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
    }

    public class DataAccessHelper : IDataAccessHelper
    {
        protected string ConnectionString { get; set; }

        public DataAccessHelper()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            var connectionStrings = configuration.GetConnectionString("DefaultConnection");
            var decryptedConnectionString = ConfigurationHelper.DecryptConnectionString(connectionStrings, "fundadmin@admin");
            this.ConnectionString = decryptedConnectionString;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        public virtual int ExecuteNonQuery(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue;
            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    cmd.CommandTimeout = 1200;

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteNonQuery();
                }

                return returnValue;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public IDataReader GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader dr;

            DbConnection connection = this.GetConnection();
            {
                DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return dr;
        }
    }
}
