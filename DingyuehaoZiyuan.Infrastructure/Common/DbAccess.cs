using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DingyuehaoZiyuan.Infrastructure
{
    /// <summary>数据访问类
    /// </summary>
    public sealed class DbAccess
    {
        /// <summary>构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        /// <param name="providerName">
        /// SQLServer:System.Data.SqlClient
        /// Oracle:System.Data.OracleClient
        /// Access:System.Data.OleDb
        /// </param>
        public DbAccess(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            DbProvider = DbProviderFactories.GetFactory(providerName);
        }

        /// <summary>构造函数
        /// </summary>
        public DbAccess(DbConnection connection)
        {
            DbProvider = DbProviderFactories.GetFactory(connection);
            ConnectionString = connection.ConnectionString;
        }

        /// <summary>构造函数
        /// </summary>
        public DbAccess(ConnectionStringSettings settings)
            : this(settings.ConnectionString, settings.ProviderName)
        {
        }

        /// <summary>构造函数
        /// </summary>
        /// <param name="name">Config配置节的Name</param>
        public DbAccess(string name = "connection")
            : this(ConfigurationManager.ConnectionStrings[name])
        {
        }

        /// <summary>属性,数据库链接字符串
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>属性,ProviderName
        /// </summary>
        public DbProviderFactory DbProvider { get; private set; }

        #region 创建参数

        /// <summary>创建参数
        /// </summary>
        /// <param name="field">参数字段</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="size">参数长度</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string field, DbType dbtype, int size, string value)
        {
            DbParameter parameter = CreateParameter(field, dbtype, value);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>创建参数
        /// </summary>
        /// <param name="field">参数字段</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string field, DbType dbtype, string value)
        {
            DbParameter parameter = CreateParameter(field, value);
            parameter.DbType = dbtype;
            return parameter;
        }

        /// <summary>创建参数
        /// </summary>
        /// <param name="field">参数字段</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public DbParameter CreateParameter(string field, string value)
        {
            var parameter = DbProvider.CreateParameter();
            parameter.ParameterName = field;
            parameter.Value = value;
            return parameter;
        }

        #endregion 创建参数

        #region 执行ExecuteCommand

        /// <summary>执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteCommand(string sql, params DbParameter[] parameters)
        {
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>执行非查询语句,并返回受影响的记录行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响记录行数</returns>
        public int ExecuteCommand(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            var result = 0;
            var command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteNonQuery();
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }

        /// <summary>批量执行SQL语句
        /// </summary>
        /// <param name="sqlList">SQL列表</param>
        /// <returns></returns>
        public int ExecuteCommand(ArrayList sqlList)
        {
            var count = 0;
            Exception executeException = null;

            using (var con = CreateConnection())
            {
                con.Open();
                var sqlTran = con.BeginTransaction();
                try
                {
                    foreach (var item in sqlList)
                    {
                        var command = DbProvider.CreateCommand();
                        command.Connection = con;
                        command.CommandText = item.ToString();
                        command.Transaction = sqlTran;
                        count += command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    executeException = e;
                }
                finally
                {
                    if (executeException == null)
                    {
                        sqlTran.Commit();
                    }
                    else
                    {
                        sqlTran.Rollback();
                        throw executeException;
                    }
                }
                return count;
            }
        }

        #endregion 执行ExecuteCommand

        #region 执行ExecuteScalar

        /// <summary>执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, params DbParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>执行非查询语句,并返回首行首列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            object result;
            var command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                result = command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }

        #endregion 执行ExecuteScalar

        #region 执行ExecuteReader

        /// <summary>执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql, params DbParameter[] parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>执行查询，并以DataReader返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            var command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                command.Connection.Open();
                var result = command.ExecuteReader(CommandBehavior.CloseConnection);
                return result;
            }
            catch
            {
                command.Connection.Close();
                throw;
            }
        }

        #endregion 执行ExecuteReader

        #region 执行查询,并以DataSet返回结果集

        /// <summary>执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql, params DbParameter[] parameters)
        {
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary> 执行查询，并以DataSet返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            var result = new DataSet();

            using (var dataAdapter = DbProvider.CreateDataAdapter())
            {
                dataAdapter.SelectCommand = CreateCommand(sql, CommandType.Text, parameters);
                dataAdapter.Fill(result);
                return result;
            }
        }

        #endregion 执行查询,并以DataSet返回结果集

        #region 执行查询,并以DataView返回结果集

        /// <summary>执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        public DataView GetDataView(string sql, params DbParameter[] parameters)
        {
            DataView dv = GetDataTable(sql, CommandType.Text, parameters).DefaultView;
            return dv;
        }

        /// <summary>执行查询，并以DataView返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataView</returns>
        public DataView GetDataView(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            DataView dv = GetDataTable(sql, cmdtype, parameters).DefaultView;
            return dv;
        }

        #endregion 执行查询,并以DataView返回结果集

        #region 执行查询,并以DataTable返回结果集

        /// <summary>执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql, params DbParameter[] parameters)
        {
            DataTable dt = GetDataTable(sql, CommandType.Text, parameters);
            return dt;
        }

        /// <summary>执行查询，并以DataTable返回结果集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdtype">命令类型</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql, CommandType cmdtype, params DbParameter[] parameters)
        {
            var result = new DataTable();

            using (var dataAdapter = DbProvider.CreateDataAdapter())
            {
                if (dataAdapter != null)
                {
                    dataAdapter.SelectCommand = CreateCommand(sql, CommandType.Text, parameters);
                    dataAdapter.Fill(result);
                }
                return result;
            }
        }

        /// <summary>执行查询,返回以空行填充的指定条数记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="sizeCount">显示记录条数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql, int sizeCount)
        {
            var dt = GetDataTable(sql);

            if (dt.Rows.Count < sizeCount)
            {
                var b = sizeCount - dt.Rows.Count;
                for (var i = 0; i < b; i++)
                {
                    var dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>执行查询，指定行数行数与最大行数
        /// </summary>
        public DataTable GetDataTable(string sql, int startIndex, int maxRecord)
        {
            var result = new DataTable();
            var parameters = new DbParameter[0];
            using (var dataAdapter = DbProvider.CreateDataAdapter())
            {
                dataAdapter.SelectCommand = CreateCommand(sql, CommandType.Text, parameters);
                dataAdapter.Fill(startIndex, maxRecord, result);
                return result;
            }
        }

        #endregion 执行查询,并以DataTable返回结果集

        #region DataTable更新操作

        /// <summary>更新DataTable表
        /// </summary>
        public void DataTableToUpdate(string sql, DataTable table, params DbParameter[] parameters)
        {
            if (table.Rows.Count > 0)
            {
                using (var adapter = DbProvider.CreateDataAdapter())
                {
                    adapter.UpdateCommand = CreateCommand(sql, CommandType.Text, parameters);
                    adapter.Update(table);
                }
            }
        }

        /// <summary>插入DataTable表
        /// </summary>
        public void DataTableToInsert(string sql, DataTable table, params DbParameter[] parameters)
        {
            if (table.Rows.Count > 0)
            {
                using (var adapter = DbProvider.CreateDataAdapter())
                {
                    adapter.InsertCommand = CreateCommand(sql, CommandType.Text, parameters);
                    adapter.Update(table);
                }
            }
        }

        /// <summary>删除DataTable表
        /// </summary>
        public void DataTableToDelete(string sql, DataTable table, params DbParameter[] parameters)
        {
            if (table.Rows.Count > 0)
            {
                using (var adapter = DbProvider.CreateDataAdapter())
                {
                    adapter.DeleteCommand = CreateCommand(sql, CommandType.Text, parameters);
                    adapter.Update(table);
                }
            }
        }

        /// <summary>SqlBulkCopy
        /// </summary>
        public void SqlBulkCopy(DataTable table)
        {
            var bulkCopy = new SqlBulkCopy(ConnectionString) { BulkCopyTimeout = 3600, DestinationTableName = table.TableName };
            bulkCopy.WriteToServer(table);
        }

        #endregion DataTable更新操作

        #region 批量添加datatable数据到数据库

        public int SqlBulkCopyDt(DataTable dt, string tablename)
        {
            int result = 0;
            SqlBulkCopy sbc = new SqlBulkCopy(ConnectionString);
            sbc.BulkCopyTimeout = 3600;
            sbc.DestinationTableName = tablename;
            sbc.WriteToServer(dt);
            result = 1;
            return result;
        }

        #endregion

        #region 辅助方法

        /// <summary>创建执行命令对象
        /// </summary>
        private DbCommand CreateCommand(string sql, CommandType cmdType, params DbParameter[] parameters)
        {
            var command = DbProvider.CreateCommand();
            command.Connection = CreateConnection();
            command.CommandText = sql;
            command.CommandType = cmdType;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            return command;
        }

        /// <summary>创建数据库连接
        /// </summary>
        private DbConnection CreateConnection()
        {
            var con = DbProvider.CreateConnection();
            con.ConnectionString = ConnectionString;
            return con;
        }

        #endregion 辅助方法
    }
}
