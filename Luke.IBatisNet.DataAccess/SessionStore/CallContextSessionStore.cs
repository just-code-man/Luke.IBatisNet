#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2006-10-10 12:57:36 -0600 (Tue, 10 Oct 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006 - Apache Fondation
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

using Luke.IBatisNet.Common;
using Luke.IBatisNet.DataMapper;
using System.Collections.Generic;
using System.Threading;

namespace Luke.IBatisNet.DataAccess.SessionStore
{
	/// <summary>
	/// Provides an implementation of <see cref="ISessionStore"/>
	/// which relies on <c>CallContext</c>
    /// This implementation will first get the current session from the current 
    /// thread. Do NOT use on web scenario (web applications or web services).
	/// </summary>
	public class CallContextSessionStore : AbstractSessionStore
	{
		private static AsyncLocal<Dictionary<string, IDalSession>> _callContext = new AsyncLocal<Dictionary<string, IDalSession>>()
		{
			Value = new Dictionary<string, IDalSession>()
		};

        /// <summary>
        /// Initializes a new instance of the <see cref="CallContextSessionStore"/> class.
        /// </summary>
        /// <param name="daoManagerId">The DaoManager name.</param>
        public CallContextSessionStore(string daoManagerId) : base(daoManagerId)
		{}

		/// <summary>
		/// Get the local session
		/// </summary>
		public override IDalSession LocalSession
		{
			get 
			{
                if (_callContext.Value.ContainsKey(sessionName))
                {
                    return _callContext.Value[sessionName] as ISqlMapSession;
                }
                return null;
            }
		}

		/// <summary>
		/// Store the specified session.
		/// </summary>
		/// <param name="session">The session to store</param>

		public override void Store(IDalSession session)
		{
            _callContext.Value[sessionName] = session;
        }

		/// <summary>
		/// Remove the local session.
		/// </summary>
		public override void Dispose()
		{
            _callContext.Value[sessionName] = null;
        }
	}
}
