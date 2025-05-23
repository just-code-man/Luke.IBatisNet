
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 515691 $
 * $Date: 2007-03-07 11:39:07 -0700 (Wed, 07 Mar 2007) $
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
using Luke.IBatisNet.Common;
using Luke.IBatisNet.Common.Logging;
using Luke.IBatisNet.DataMapper.Exceptions;

#endregion


namespace Luke.IBatisNet.DataMapper
{
	/// <summary>
	/// Summary description for SqlMapSession.
	/// </summary>
	[Serializable]
    public class SqlMapSession : ISqlMapSession
	{
		#region Fields
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		private ISqlMapper _sqlMapper = null;
		private IDataSource _dataSource = null;
			
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlMapper"></param>
		public SqlMapSession(ISqlMapper sqlMapper) 
		{
			_dataSource = sqlMapper.DataSource;
			_sqlMapper = sqlMapper;
		}
		#endregion

        #region IDalSession Members

        #region Fields
        private bool _isTransactionOpen = false;
		/// <summary>
		/// Changes the vote to commit (true) or to abort (false) in transsaction
		/// </summary>
		private bool _consistent = false;

		/// <summary>
		/// Holds value of connection
		/// </summary>
		private DbConnection _connection = null;

		/// <summary>
		/// Holds value of transaction
		/// </summary>
		private DbTransaction _transaction = null;	
		#endregion

		#region Properties


        /// <summary>
        /// Gets the SQL mapper.
        /// </summary>
        /// <value>The SQL mapper.</value>
		public ISqlMapper SqlMapper
		{
			get { return _sqlMapper; }
		}


        /// <summary>
        /// The data source use by the session.
        /// </summary>
        /// <value></value>
		public IDataSource DataSource
		{
			get { return _dataSource; }
		}


        /// <summary>
        /// The Connection use by the session.
        /// </summary>
        /// <value></value>
		public DbConnection Connection
		{
			get { return _connection; }
		}


        /// <summary>
        /// The Transaction use by the session.
        /// </summary>
        /// <value></value>
		public DbTransaction Transaction
		{
			get { return _transaction; }
		}

        /// <summary>
        /// Indicates if a transaction is open  on
        /// the session.
        /// </summary>
        public bool IsTransactionStart
        {
            get { return _isTransactionOpen; }
        }

		/// <summary>
		/// Changes the vote for transaction to commit (true) or to abort (false).
		/// </summary>
		private bool Consistent
		{
			set { _consistent = value; }
		}

		#endregion

		#region Methods
		/// <summary>
		/// Complete (commit) a transaction
		/// </summary>
		/// <remarks>
		/// Use in 'using' syntax.
		/// </remarks>
		public void Complete()
		{
			this.Consistent = true;
		}

		/// <summary>
		/// Open the connection
		/// </summary>
		public void OpenConnection()
		{
			this.OpenConnection(_dataSource.ConnectionString);
		}

        public async Task OpenConnectionAsync()
        {
            await this.OpenConnectionAsync(_dataSource.ConnectionString);
        }

        /// <summary>
        /// Create the connection
        /// </summary>
        public void CreateConnection()
        {
            CreateConnection(_dataSource.ConnectionString);
        }
	    
        /// <summary>
        /// Create the connection
        /// </summary>
        public void CreateConnection(string connectionString)
        {
            _connection = _dataSource.DbProvider.CreateConnection();
            _connection.ConnectionString = connectionString;
        }

        /// <summary>
        /// Open a connection, on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public void OpenConnection(string connectionString)
		{
			if (_connection == null)
			{
                CreateConnection(connectionString);
				try
				{
					_connection.Open();
					if (_logger.IsDebugEnabled)
					{
						_logger.Debug( string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description) );
					}
				}
				catch(Exception ex)
				{
					throw new DataMapperException( string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex );
				}
			}
			else if (_connection.State != ConnectionState.Open)
			{
				try
				{
					_connection.Open();
					if (_logger.IsDebugEnabled)
					{
						_logger.Debug(string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description) );
					}
				}
				catch(Exception ex)
				{
					throw new DataMapperException(string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex );
				}
			}
		}

        public async Task OpenConnectionAsync(string connectionString)
        {
            if (_connection == null)
            {
                CreateConnection(connectionString);
                try
                {
                    await _connection.OpenAsync();
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.Debug(string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description));
                    }
                }
                catch (Exception ex)
                {
                    throw new DataMapperException(string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex);
                }
            }
            else if (_connection.State != ConnectionState.Open)
            {
                try
                {
                    await _connection.OpenAsync();
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.Debug(string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description));
                    }
                }
                catch (Exception ex)
                {
                    throw new DataMapperException(string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex);
                }
            }
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public void CloseConnection()
		{
			if ( (_connection != null) && (_connection.State != ConnectionState.Closed) )
			{
				_connection.Close();
				if (_logger.IsDebugEnabled)
				{

					_logger.Debug(string.Format("Close Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description));
				}
				_connection.Dispose();
			}
			_connection = null;
		}

        public async Task CloseConnectionAsync()
        {
            if ((_connection != null) && (_connection.State != ConnectionState.Closed))
            {
                await _connection.CloseAsync();
                if (_logger.IsDebugEnabled)
                {

                    _logger.Debug(string.Format("Close Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description));
                }
                await _connection.DisposeAsync();
            }
            _connection = null;
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
		{
			this.BeginTransaction(_dataSource.ConnectionString);
		}

        public async Task BeginTransactionAsync()
        {
            await this.BeginTransactionAsync(_dataSource.ConnectionString);
        }


        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        public void BeginTransaction(string connectionString)
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
			{
				this.OpenConnection( connectionString );
			}
			_transaction = _connection.BeginTransaction();
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Begin Transaction.");
			}
			_isTransactionOpen = true;
		}

        public async Task BeginTransactionAsync(string connectionString)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                await this.OpenConnectionAsync(connectionString);
            }
            _transaction = await _connection.BeginTransactionAsync();
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Begin Transaction.");
            }
            _isTransactionOpen = true;
        }

        /// <summary>
        /// Begins a database transaction
        /// </summary>
        /// <param name="openConnection">Open a connection.</param>
        public void BeginTransaction(bool openConnection)
		{
			if (openConnection)
			{
				this.BeginTransaction();
			}
			else
			{
                if (_connection == null || _connection.State != ConnectionState.Open)
				{
                    this.OpenConnection();
                }
				_transaction = _connection.BeginTransaction();
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Begin Transaction.");
				}
				_isTransactionOpen = true;
			}
		}

        public async Task BeginTransactionAsync(bool openConnection)
        {
            if (openConnection)
            {
                await this.BeginTransactionAsync();
            }
            else
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    await this.OpenConnectionAsync();
                }
                _transaction = await _connection.BeginTransactionAsync();
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Begin Transaction.");
                }
                _isTransactionOpen = true;
            }
        }

        /// <summary>
        /// Begins a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">
        /// The isolation level under which the transaction should run.
        /// </param>
        public void BeginTransaction(IsolationLevel isolationLevel)
		{
			this.BeginTransaction(_dataSource.ConnectionString, isolationLevel);
		}

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            await this.BeginTransactionAsync(_dataSource.ConnectionString, isolationLevel);
        }

        /// <summary>
        /// Open a connection and begin a transaction on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        public void BeginTransaction(string connectionString, IsolationLevel isolationLevel)
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
			{
				this.OpenConnection( connectionString );
			}
			_transaction = _connection.BeginTransaction(isolationLevel);
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Begin Transaction.");
			}
			_isTransactionOpen = true;			
		}

        public async Task BeginTransactionAsync(string connectionString, IsolationLevel isolationLevel)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                await this.OpenConnectionAsync(connectionString);
            }
            _transaction = await _connection.BeginTransactionAsync(isolationLevel);
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Begin Transaction.");
            }
            _isTransactionOpen = true;
        }

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="openConnection">Open a connection.</param>
        public void BeginTransaction(bool openConnection, IsolationLevel isolationLevel)
		{
			this.BeginTransaction(_dataSource.ConnectionString, openConnection, isolationLevel);
		}

        public async Task BeginTransactionAsync(bool openConnection, IsolationLevel isolationLevel)
        {
            await this.BeginTransactionAsync(_dataSource.ConnectionString, openConnection, isolationLevel);
        }

        /// <summary>
        /// Begins a transaction on the current connection
        /// with the specified IsolationLevel value.
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level for this connection.</param>
        /// <param name="connectionString">The connection string</param>
        /// <param name="openConnection">Open a connection.</param>
        public void BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel)
		{
			if (openConnection)
			{
				this.BeginTransaction( connectionString, isolationLevel );
			}
			else
			{
				if (_connection == null || _connection.State != ConnectionState.Open)
				{
					throw new DataMapperException("SqlMapSession could not invoke StartTransaction(). A Connection must be started. Call OpenConnection() first.");
				}
				_transaction = _connection.BeginTransaction(isolationLevel);
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Begin Transaction.");
				}
				_isTransactionOpen = true;
			}			
		}

        public async Task BeginTransactionAsync(string connectionString, bool openConnection, IsolationLevel isolationLevel)
        {
            if (openConnection)
            {
                await this.BeginTransactionAsync(connectionString, isolationLevel);
            }
            else
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    throw new DataMapperException("SqlMapSession could not invoke StartTransaction(). A Connection must be started. Call OpenConnection() first.");
                }
                _transaction =  await _connection.BeginTransactionAsync(isolationLevel);
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Begin Transaction.");
                }
                _isTransactionOpen = true;
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <remarks>
        /// Will close the connection.
        /// </remarks>
        public void CommitTransaction()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Commit Transaction.");
			}
			_transaction.Commit();
			_transaction.Dispose();
            _transaction = null;
            _isTransactionOpen = false;

			if (_connection.State != ConnectionState.Closed)
			{
				this.CloseConnection();
			}
		}

        public async Task CommitTransactionAsync()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Commit Transaction.");
            }
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
            _isTransactionOpen = false;

            if (_connection.State != ConnectionState.Closed)
            {
                await this.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        public void CommitTransaction(bool closeConnection)
		{
			if (closeConnection)
			{
				this.CommitTransaction();
			}
			else
			{
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Commit Transaction.");
				}				
                _transaction.Commit();
				_transaction.Dispose();
                _transaction = null;
                _isTransactionOpen = false;
			}
		}

        public async Task CommitTransactionAsync(bool closeConnection)
        {
            if (closeConnection)
            {
                await this.CommitTransactionAsync();
            }
            else
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Commit Transaction.");
                }
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
                _isTransactionOpen = false;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <remarks>
        /// Will close the connection.
        /// </remarks>
        public void RollBackTransaction()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("RollBack Transaction.");
			}
			_transaction.Rollback();
			_transaction.Dispose();
			_transaction = null;
            _isTransactionOpen = false;
			if (_connection.State != ConnectionState.Closed)
			{
				this.CloseConnection();
			}
		}

        public async Task RollBackTransactionAsync()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("RollBack Transaction.");
            }
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
            _isTransactionOpen = false;
            if (_connection.State != ConnectionState.Closed)
            {
                await this.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <param name="closeConnection">Close the connection</param>
        public void RollBackTransaction(bool closeConnection)
		{
			if (closeConnection)
			{
				this.RollBackTransaction();
			}
			else
			{
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("RollBack Transaction.");
				}
				_transaction.Rollback();
				_transaction.Dispose();
				_transaction = null;
                _isTransactionOpen = false;
			}
		}

        public async Task RollBackTransactionAsync(bool closeConnection)
        {
            if (closeConnection)
            {
                await this.RollBackTransactionAsync();
            }
            else
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("RollBack Transaction.");
                }
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                _isTransactionOpen = false;
            }
        }

        /// <summary>
        /// Create a command object
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(CommandType commandType)
		{
			DbCommand command = _dataSource.DbProvider.CreateCommand();
            command.CommandTimeout = _dataSource.DbProvider.DbCommandTimeout;
			command.CommandType = commandType;
			command.Connection = _connection;
			
			// Assign transaction
			if (_transaction != null)
			{
				try
				{
					command.Transaction = _transaction;
				}
				catch 
				{}
			}
			// Assign connection timeout
			if (_connection!= null)
			{
				try // MySql provider doesn't suppport it !
				{
					command.CommandTimeout = _connection.ConnectionTimeout;
				}
				catch(NotSupportedException e)
				{
					if (_logger.IsInfoEnabled)
					{
						_logger.Info(e.Message);
					}
				}
			}

//			if (_logger.IsDebugEnabled)
//			{
//				command = IDbCommandProxy.NewInstance(command);
//			}

			return command;
		}

        /// <summary>
        /// Create an IDataParameter
        /// </summary>
        /// <returns>An IDataParameter.</returns>
        public DbParameter CreateDataParameter()
		{
			return _dataSource.DbProvider.CreateDataParameter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DbDataAdapter CreateDataAdapter()
		{
			return _dataSource.DbProvider.CreateDataAdapter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public DbDataAdapter CreateDataAdapter(DbCommand command)
		{
			DbDataAdapter dataAdapter = null;

			dataAdapter = _dataSource.DbProvider.CreateDataAdapter();
			dataAdapter.SelectCommand = command;

			return dataAdapter;
		}
		#endregion

		#endregion
	
		#region IDisposable Members

		/// <summary>
		/// Releasing, or resetting resources.
		/// </summary>
		public void Dispose()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Dispose SqlMapSession");
			}
			if (_isTransactionOpen == false)
			{
				if (_connection.State != ConnectionState.Closed)
				{
					_sqlMapper.CloseConnection();
				}
			}
			else
			{
				if (_consistent)
				{
					_sqlMapper.CommitTransaction();
                    _isTransactionOpen = false;
				}
				else
				{
					if (_connection.State != ConnectionState.Closed)
					{
						_sqlMapper.RollBackTransaction();
                        _isTransactionOpen = false;
					}
				}
			}
		}

        public async ValueTask DisposeAsync()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Dispose SqlMapSession");
            }
            if (_isTransactionOpen == false)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    await _sqlMapper.CloseConnectionAsync();
                }
            }
            else
            {
                if (_consistent)
                {
                    await _sqlMapper.CommitTransactionAsync();
                    _isTransactionOpen = false;
                }
                else
                {
                    if (_connection.State != ConnectionState.Closed)
                    {
                        await _sqlMapper.RollBackTransactionAsync();
                        _isTransactionOpen = false;
                    }
                }
            }
        }

        #endregion
    }
}
