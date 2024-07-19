#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-11-15 13:22:00 -0700 (Wed, 15 Nov 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - Gilles Bayon
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

using System;
using System.Data;
using System.Data.Common;
using Luke.IBatisNet.DataMapper.Scope;

namespace Luke.IBatisNet.DataMapper.Commands
{
    /// <summary>
    /// Decorate an <see cref="System.Data.IDbCommand"></see>
    /// to auto move to next ResultMap on ExecuteReader call. 
    /// </summary>
    public class DbCommandDecorator : DbCommand
    {
        private DbCommand _innerDbCommand = null;
        private RequestScope _request = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbCommandDecorator"/> class.
        /// </summary>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="request">The request scope</param>
        public DbCommandDecorator(DbCommand dbCommand, RequestScope request)
        {
            _request = request;
            _innerDbCommand = dbCommand;
        }

        public override string CommandText { get => _innerDbCommand.CommandText; set => _innerDbCommand.CommandText = value; }
        public override int CommandTimeout { get => _innerDbCommand.CommandTimeout; set => _innerDbCommand.CommandTimeout = value; }
        public override CommandType CommandType { get => _innerDbCommand.CommandType; set => _innerDbCommand.CommandType = value; }
        public override bool DesignTimeVisible { get => _innerDbCommand.DesignTimeVisible; set => _innerDbCommand.DesignTimeVisible = value; }
        public override UpdateRowSource UpdatedRowSource { get => _innerDbCommand.UpdatedRowSource; set => _innerDbCommand.UpdatedRowSource = value; }
        protected override DbConnection DbConnection { get => _innerDbCommand.Connection; set => _innerDbCommand.Connection = value; }

        protected override DbParameterCollection DbParameterCollection => _innerDbCommand.Parameters;

        protected override DbTransaction DbTransaction { get => _innerDbCommand.Transaction; set => _innerDbCommand.Transaction = value; }

        public override void Cancel()
        {
            _innerDbCommand.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteScalar();
        }

        public override void Prepare()
        {
            _request.Session.OpenConnection();
            _innerDbCommand.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return _innerDbCommand.CreateParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteReader(behavior);
        }


        #region IDbCommand Members

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
           _innerDbCommand.Dispose();
            base.Dispose();
        }

        #endregion
    }
}
