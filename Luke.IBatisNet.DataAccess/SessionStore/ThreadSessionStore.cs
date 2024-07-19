using Luke.IBatisNet.Common;
using Luke.IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luke.IBatisNet.DataAccess.SessionStore
{
    public class ThreadSessionStore : AbstractSessionStore
    {
        [ThreadStatic]
        private static readonly Dictionary<string, IDalSession> StaticSessions =
            new Dictionary<string, IDalSession>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CallContextSessionStore"/> class.
        /// </summary>
        /// <param name="sqlMapperId">The SQL mapper id.</param>
        public ThreadSessionStore(string sqlMapperId)
            : base(sqlMapperId)
        {
        }

        /// <summary>
        /// Get the local session
        /// </summary>
        public override IDalSession LocalSession
        {
            get
            {
                IDalSession result;
                StaticSessions.TryGetValue(sessionName, out result);
                return result;
                //             return StaticSessions[sessionName] as SqlMapSession;
            }
        }

        /// <summary>
        /// Store the specified session.
        /// </summary>
        /// <param name="session">The session to store</param>
        public override void Store(IDalSession session)
        {
            StaticSessions[sessionName] = session;
        }

        /// <summary>
        /// Remove the local session.
        /// </summary>
        public override void Dispose()
        {
            StaticSessions[sessionName] = null;
        }

    }
}
