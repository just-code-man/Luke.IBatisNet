
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 513043 $
 * $Date: 2007-02-28 15:56:03 -0700 (Wed, 28 Feb 2007) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Imports

using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Luke.IBatisNet.Common;
using Luke.IBatisNet.DataMapper;

#endregion

namespace Luke.IBatisNet.DataAccess.DaoSessionHandlers
{
	/// <summary>
	/// An SqlMappper implementation of the DataAccess Session.
	/// </summary>
	public class SqlMapDaoSession : DaoSession
	{
		#region Fields
		private ISqlMapper _sqlMap = null;
		#endregion

		#region Properties


        /// <summary>
        /// Gets the SQL map.
        /// </summary>
        /// <value>The SQL map.</value>
		public ISqlMapper SqlMap
		{
			get { return _sqlMap; }
		}


        /// <summary>
        /// The data source use by the session.
        /// </summary>
        /// <value></value>
		public override IDataSource DataSource
		{
			get { return _sqlMap.LocalSession.DataSource; }
		}


        /// <summary>
        /// The Connection use by the session.
        /// </summary>
        /// <value></value>
		public override DbConnection Connection
		{
			get { return _sqlMap.LocalSession.Connection; }
		}


        /// <summary>
        /// The Transaction use by the session.
        /// </summary>
        /// <value></value>
		public override DbTransaction Transaction
		{
			get { return _sqlMap.LocalSession.Transaction; }
		}

        /// <summary>
        /// Indicates if a transaction is open  on
        /// the session.
        /// </summary>
        /// <value></value>
        public override bool IsTransactionStart
        {
            get { return _sqlMap.LocalSession.IsTransactionStart; }
        }

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <param name="sqlMap"></param>
		public SqlMapDaoSession(DaoManager daoManager, ISqlMapper sqlMap):base(daoManager) 
		{
			_sqlMap = sqlMap;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Complete (commit) a transaction
		/// </summary>
		/// <remarks>
		/// Use in 'using' syntax.
		/// </remarks>
		public override void Complete()
		{
			_sqlMap.LocalSession.Complete();
		}

		/// <summary>
		/// Opens a database connection.
		/// </summary>
		public override void OpenConnection()
		{
			_sqlMap.OpenConnection();
		}

        public override async Task OpenConnectionAsync()
        {
            _sqlMap.OpenConnection();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Open a connection, on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public override void OpenConnection(string connectionString)
		{
			_sqlMap.OpenConnection(connectionString);
		}

        public override async Task OpenConnectionAsync(string connectionString)
        {
			_sqlMap.OpenConnection(connectionString);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public override void CloseConnection()
		{
			_sqlMap.CloseConnection();
		}

        public override async Task CloseConnectionAsync()
        {
            await _sqlMap.CloseConnectionAsync();
        }

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        public override void BeginTransaction()
		{
			_sqlMap.BeginTransaction();
		}

        public override async Task BeginTransactionAsync()
        {
            await _sqlMap.BeginTransactionAsync();
        }

        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public override void BeginTransaction(string connectionString)
		{
			_sqlMap.BeginTransaction( connectionString );		
		}

        public override async Task BeginTransactionAsync(string connectionString)
        {
            await _sqlMap.BeginTransactionAsync(connectionString);
        }

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        /// <param name="openConnection">Open a connection.</param>
        public override void BeginTransaction(bool openConnection)
		{
			_sqlMap.BeginTransaction(openConnection);
		}

        public override async Task BeginTransactionAsync(bool openConnection)
        {
            await _sqlMap.BeginTransactionAsync(openConnection);
        }

        /// <summary>
        /// Begins a transaction at the data source with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        public override void BeginTransaction(IsolationLevel isolationLevel)
		{
			_sqlMap.BeginTransaction (isolationLevel);
		}

        public override async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            await _sqlMap.BeginTransactionAsync(isolationLevel);
        }

        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        public override void BeginTransaction(string connectionString, IsolationLevel isolationLevel)
		{
			_sqlMap.BeginTransaction ( connectionString, isolationLevel );
		}

        public override async Task BeginTransactionAsync(string connectionString, IsolationLevel isolationLevel)
        {
            await _sqlMap.BeginTransactionAsync(connectionString, isolationLevel);
        }

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="openConnection">Open a connection.</param>
        public override void BeginTransaction(bool openConnection, IsolationLevel isolationLevel)
		{
			_sqlMap.BeginTransaction(openConnection, isolationLevel);
		}

        public override async Task BeginTransactionAsync(bool openConnection, IsolationLevel isolationLevel)
        {
            await _sqlMap.BeginTransactionAsync(openConnection, isolationLevel);
        }

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="openConnection">Open a connection.</param>
        public override void BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel)
		{
			_sqlMap.BeginTransaction( connectionString, openConnection, isolationLevel );
		}

        public override async Task BeginTransactionAsync(string connectionString, bool openConnection, IsolationLevel isolationLevel)
        {
            await _sqlMap.BeginTransactionAsync(connectionString, openConnection, isolationLevel);
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <remarks>
        /// Will close the connection.
        /// </remarks>
        public override void CommitTransaction()
		{
			_sqlMap.CommitTransaction();
		}

        public override async Task CommitTransactionAsync()
        {
            await _sqlMap.CommitTransactionAsync();
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        public override void CommitTransaction(bool closeConnection)
		{
			_sqlMap.CommitTransaction(closeConnection);
		}

        public override async Task CommitTransactionAsync(bool closeConnection)
        {
            await _sqlMap.CommitTransactionAsync(closeConnection);
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <remarks>
        /// Will close the connection.
        /// </remarks>
        public override void RollBackTransaction()
		{
			_sqlMap.RollBackTransaction();
		}

        public override async Task RollBackTransactionAsync()
        {
            await _sqlMap.RollBackTransactionAsync();
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        public override void RollBackTransaction(bool closeConnection)
		{
			_sqlMap.RollBackTransaction(closeConnection);
		}

        public override async Task RollBackTransactionAsync(bool closeConnection)
        {
            await _sqlMap.RollBackTransactionAsync(closeConnection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public override DbCommand CreateCommand(CommandType commandType)
		{
			return _sqlMap.LocalSession.CreateCommand(commandType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override DbParameter CreateDataParameter()
		{
			return _sqlMap.LocalSession.CreateDataParameter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override DbDataAdapter CreateDataAdapter()
		{
			return _sqlMap.LocalSession.CreateDataAdapter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public override DbDataAdapter CreateDataAdapter(DbCommand command)
		{
			return _sqlMap.LocalSession.CreateDataAdapter(command);
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Releasing, or resetting resources.
		/// </summary>
		public override void Dispose()
		{
			_sqlMap.LocalSession.Dispose();
			daoManager.Dispose();
		}

        public override async ValueTask DisposeAsync()
        {
            _sqlMap.LocalSession.Dispose();
            daoManager.Dispose();
        }
        #endregion

    }
}
