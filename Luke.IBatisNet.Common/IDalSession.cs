
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
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
#endregion


namespace Luke.IBatisNet.Common
{
	/// <summary>
	/// A template for a session in the iBATIS.NET framwork.
	/// Holds the connection, the transaction ...
	/// </summary>
	public interface IDalSession : IDisposable, IAsyncDisposable
	{
		/// <summary>
		/// The data source use by the session.
		/// </summary>
		IDataSource DataSource
		{
			get;
		}

		/// <summary>
		/// The Connection use by the session.
		/// </summary>
		DbConnection Connection
		{
			get; 
		}

		/// <summary>
		/// The Transaction use by the session.
		/// </summary>
		DbTransaction Transaction
		{
			get;
		}

        /// <summary>
        /// Indicates if a transaction is open  on
        /// the session.
        /// </summary>
        bool IsTransactionStart
        {
            get;
        }

		/// <summary>
		/// Complete (commit) a transsaction
		/// </summary>
		void Complete();

        /// <summary>
        /// Open a connection.
        /// </summary>
        void OpenConnection();

		Task OpenConnectionAsync();

        /// <summary>
        /// Open a connection, on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        void OpenConnection(string connectionString);

        Task OpenConnectionAsync(string connectionString);

        /// <summary>
        /// close a connection
        /// </summary>
        void CloseConnection();

        Task CloseConnectionAsync();

        /// <summary>
        /// Open a connection and begin a transaction
        /// </summary>
        void BeginTransaction();

        Task BeginTransactionAsync();

        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        void BeginTransaction(string connectionString);

        Task BeginTransactionAsync(string connectionString);

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        /// <param name="openConnection">Open a connection.</param>
        void BeginTransaction(bool openConnection);

        Task BeginTransactionAsync(bool openConnection);

        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        void BeginTransaction(string connectionString, IsolationLevel isolationLevel);

        Task BeginTransactionAsync(string connectionString, IsolationLevel isolationLevel);

        /// <summary>
        /// Open a connection and begin a transaction at the data source 
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        void BeginTransaction(IsolationLevel isolationLevel);

        Task BeginTransactionAsync(IsolationLevel isolationLevel);

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="openConnection">Open a connection.</param>
        void BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel);

        Task BeginTransactionAsync(string connectionString, bool openConnection, IsolationLevel isolationLevel);

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="openConnection">Open a connection.</param>
        void BeginTransaction(bool openConnection, IsolationLevel isolationLevel);

        Task BeginTransactionAsync(bool openConnection, IsolationLevel isolationLevel);

        /// <summary>
        /// Commit a transaction and close the associated connection
        /// </summary>
        void CommitTransaction();

        Task CommitTransactionAsync();

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        void CommitTransaction(bool closeConnection);

        Task CommitTransactionAsync(bool closeConnection);

        /// <summary>
        /// Rollbak a transaction and close the associated connection
        /// </summary>
        void RollBackTransaction();

        Task RollBackTransactionAsync();

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        void RollBackTransaction(bool closeConnection);

        Task RollBackTransactionAsync(bool closeConnection);

        /// <summary>
        /// Create a command
        /// </summary>
        /// <param name="commandType">The type of the command</param>
        /// <returns>An IDbCommand.</returns>
        DbCommand CreateCommand(CommandType commandType);

		/// <summary>
		/// Create an DataParameter
		/// </summary>
		/// <returns>An IDbDataParameter.</returns>
		DbParameter CreateDataParameter();

		/// <summary>
		/// Create a DataAdapter
		/// </summary>
		/// <param name="command">The statement or stored procedure 
		/// used to select records in the data source.</param>
		/// <returns>An IDbDataAdapter.</returns>
		DbDataAdapter CreateDataAdapter(DbCommand command);

		/// <summary>
		/// Create a DataAdapter
		/// </summary>
		/// <returns>An IDbDataAdapter.</returns>
		DbDataAdapter CreateDataAdapter();
	}
}
