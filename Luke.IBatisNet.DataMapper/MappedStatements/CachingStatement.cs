#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 602985 $
 * $Date: 2007-12-10 11:25:11 -0700 (Mon, 10 Dec 2007) $
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

#region Using

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Luke.IBatisNet.DataMapper.Commands;
using Luke.IBatisNet.DataMapper.Configuration.Cache;
using Luke.IBatisNet.DataMapper.Configuration.Statements;
using Luke.IBatisNet.DataMapper.Scope;

#endregion

namespace Luke.IBatisNet.DataMapper.MappedStatements
{
    /// <summary>
    /// Summary description for CachingStatement.
    /// </summary>
    public sealed class CachingStatement : IMappedStatement
    {
        private readonly MappedStatement _mappedStatement = null;

        /// <summary>
        /// Event launch on exceute query
        /// </summary>
        public event ExecuteEventHandler Execute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statement"></param>
        public CachingStatement(MappedStatement statement)
        {
            _mappedStatement = statement;
        }

        #region IMappedStatement Members


        /// <summary>
        /// The IPreparedCommand to use
        /// </summary>
        public IPreparedCommand PreparedCommand
        {
            get { return _mappedStatement.PreparedCommand; }
        }

        /// <summary>
        /// Name used to identify the MappedStatement amongst the others.
        /// This the name of the SQL statment by default.
        /// </summary>
        public string Id
        {
            get { return _mappedStatement.Id; }
        }

        /// <summary>
        /// The SQL statment used by this MappedStatement
        /// </summary>
        public IStatement Statement
        {
            get { return _mappedStatement.Statement; }
        }

        /// <summary>
        /// The SqlMap used by this MappedStatement
        /// </summary>
        public ISqlMapper SqlMap
        {
            get { return _mappedStatement.SqlMap; }
        }

        /// <summary>
        /// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
        /// in the keyProperty parameter.  The value at each key will be the value of the property specified
        /// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        ///<exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public IDictionary ExecuteQueryForMap(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary map = new Hashtable();
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForMap");
            if (keyProperty != null)
            {
                cacheKey.Update(keyProperty);
            }
            if (valueProperty != null)
            {
                cacheKey.Update(valueProperty);
            }

            map = this.Statement.CacheModel[cacheKey] as IDictionary;
            if (map == null)
            {
                map = _mappedStatement.RunQueryForMap(request, session, parameterObject, keyProperty, valueProperty, null);
                this.Statement.CacheModel[cacheKey] = map;
            }

            return map;
        }

        public async Task<IDictionary> ExecuteQueryForMapAsync(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary map = new Hashtable();
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForMap");
            if (keyProperty != null)
            {
                cacheKey.Update(keyProperty);
            }
            if (valueProperty != null)
            {
                cacheKey.Update(valueProperty);
            }

            map = this.Statement.CacheModel[cacheKey] as IDictionary;
            if (map == null)
            {
                map = await _mappedStatement.RunQueryForMapAsync(request, session, parameterObject, keyProperty, valueProperty, null);
                this.Statement.CacheModel[cacheKey] = map;
            }

            return map;
        }

        #region ExecuteQueryForMap .NET 2.0

        /// <summary>
        /// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
        /// in the keyProperty parameter.  The value at each key will be the value of the property specified
        /// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        ///<exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary<K, V> map = new Dictionary<K, V>();
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForMap");
            if (keyProperty != null)
            {
                cacheKey.Update(keyProperty);
            }
            if (valueProperty != null)
            {
                cacheKey.Update(valueProperty);
            }

            map = this.Statement.CacheModel[cacheKey] as IDictionary<K, V>;
            if (map == null)
            {
                map = _mappedStatement.RunQueryForDictionary<K, V>(request, session, parameterObject, keyProperty, valueProperty, null);
                this.Statement.CacheModel[cacheKey] = map;
            }

            return map;
        }

        public async Task<IDictionary<K, V>> ExecuteQueryForDictionaryAsync<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            IDictionary<K, V> map = new Dictionary<K, V>();
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForMap");
            if (keyProperty != null)
            {
                cacheKey.Update(keyProperty);
            }
            if (valueProperty != null)
            {
                cacheKey.Update(valueProperty);
            }

            map = this.Statement.CacheModel[cacheKey] as IDictionary<K, V>;
            if (map == null)
            {
                map = await _mappedStatement.RunQueryForDictionaryAsync<K, V>(request, session, parameterObject, keyProperty, valueProperty, null);
                this.Statement.CacheModel[cacheKey] = map;
            }

            return map;
        }

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <param name="rowDelegate"></param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        /// <exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate<K, V> rowDelegate)
        {
            return _mappedStatement.ExecuteQueryForDictionary<K, V>(session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }

        public async Task<IDictionary<K, V>> ExecuteQueryForDictionaryAsync<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate<K, V> rowDelegate)
        {
            return await _mappedStatement.ExecuteQueryForDictionaryAsync<K, V>(session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }
        #endregion

        /// <summary>
        /// Execute an update statement. Also used for delete statement.
        /// Return the number of row effected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The number of row effected.</returns>
        public int ExecuteUpdate(ISqlMapSession session, object parameterObject)
        {
            return _mappedStatement.ExecuteUpdate(session, parameterObject);
        }

        public async Task<int> ExecuteUpdateAsync(ISqlMapSession session, object parameterObject)
        {
            return await _mappedStatement.ExecuteUpdateAsync(session, parameterObject);
        }

        /// <summary>
        /// Execute an insert statement. Fill the parameter object with 
        /// the ouput parameters if any, also could return the insert generated key
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="parameterObject">The parameter object used to fill the statement.</param>
        /// <returns>Can return the insert generated key.</returns>
        public object ExecuteInsert(ISqlMapSession session, object parameterObject)
        {
            return _mappedStatement.ExecuteInsert(session, parameterObject);
        }

        public async Task<object> ExecuteInsertAsync(ISqlMapSession session, object parameterObject)
        {
            return await _mappedStatement.ExecuteInsertAsync(session, parameterObject);
        }

        #region ExecuteQueryForList

        /// <summary>
        /// Executes the SQL and and fill a strongly typed collection.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        public void ExecuteQueryForList(ISqlMapSession session, object parameterObject, IList resultObject)
        {
            _mappedStatement.ExecuteQueryForList(session, parameterObject, resultObject);
        }

        public async Task ExecuteQueryForListAsync(ISqlMapSession session, object parameterObject, IList resultObject)
        {
            await _mappedStatement.ExecuteQueryForListAsync(session, parameterObject, resultObject);
        }

        /// <summary>
        /// Executes the SQL and retuns a subset of the rows selected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        public IList ExecuteQueryForList(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList list = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForList");
            cacheKey.Update(skipResults);
            cacheKey.Update(maxResults);

            list = this.Statement.CacheModel[cacheKey] as IList;
            if (list == null)
            {
                list = _mappedStatement.RunQueryForList(request, session, parameterObject, skipResults, maxResults);
                this.Statement.CacheModel[cacheKey] = list;
            }

            return list;
        }

        public async Task<IList> ExecuteQueryForListAsync(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList list = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForList");
            cacheKey.Update(skipResults);
            cacheKey.Update(maxResults);

            list = this.Statement.CacheModel[cacheKey] as IList;
            if (list == null)
            {
                list = await _mappedStatement.RunQueryForListAsync(request, session, parameterObject, skipResults, maxResults);
                this.Statement.CacheModel[cacheKey] = list;
            }

            return list;
        }
        /// <summary>
        /// Executes the SQL and retuns all rows selected. This is exactly the same as
        /// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>A List of result objects.</returns>
        public IList ExecuteQueryForList(ISqlMapSession session, object parameterObject)
        {
            return this.ExecuteQueryForList(session, parameterObject, MappedStatement.NO_SKIPPED_RESULTS, MappedStatement.NO_MAXIMUM_RESULTS);
        }

        public async Task<IList> ExecuteQueryForListAsync(ISqlMapSession session, object parameterObject)
        {
            return await this.ExecuteQueryForListAsync(session, parameterObject, MappedStatement.NO_SKIPPED_RESULTS, MappedStatement.NO_MAXIMUM_RESULTS);
        }
        #endregion

        #region ExecuteQueryForList .NET 2.0

        /// <summary>
        /// Executes the SQL and and fill a strongly typed collection.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        public void ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, IList<T> resultObject)
        {
            _mappedStatement.ExecuteQueryForList(session, parameterObject, resultObject);
        }

        public async Task ExecuteQueryForListAsync<T>(ISqlMapSession session, object parameterObject, IList<T> resultObject)
        {
            await _mappedStatement.ExecuteQueryForListAsync(session, parameterObject, resultObject);
        }

        /// <summary>
        /// Executes the SQL and retuns a subset of the rows selected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        public IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList<T> list = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForList");
            cacheKey.Update(skipResults);
            cacheKey.Update(maxResults);

            list = this.Statement.CacheModel[cacheKey] as IList<T>;
            if (list == null)
            {
                list = _mappedStatement.RunQueryForList<T>(request, session, parameterObject, skipResults, maxResults);
                this.Statement.CacheModel[cacheKey] = list;
            }

            return list;
        }

        public async Task<IList<T>> ExecuteQueryForListAsync<T>(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList<T> list = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForList");
            cacheKey.Update(skipResults);
            cacheKey.Update(maxResults);

            list = this.Statement.CacheModel[cacheKey] as IList<T>;
            if (list == null)
            {
                list = await _mappedStatement.RunQueryForListAsync<T>(request, session, parameterObject, skipResults, maxResults);
                this.Statement.CacheModel[cacheKey] = list;
            }

            return list;
        }
        /// <summary>
        /// Executes the SQL and retuns all rows selected. This is exactly the same as
        /// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>A List of result objects.</returns>
        public IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject)
        {
            return this.ExecuteQueryForList<T>(session, parameterObject, MappedStatement.NO_SKIPPED_RESULTS, MappedStatement.NO_MAXIMUM_RESULTS);
        }

        public async Task<IList<T>> ExecuteQueryForListAsync<T>(ISqlMapSession session, object parameterObject)
        {
            return await this.ExecuteQueryForListAsync<T>(session, parameterObject, MappedStatement.NO_SKIPPED_RESULTS, MappedStatement.NO_MAXIMUM_RESULTS);
        }
        #endregion

        #region ExecuteQueryForObject

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The object</returns>
        public object ExecuteQueryForObject(ISqlMapSession session, object parameterObject)
        {
            return this.ExecuteQueryForObject(session, parameterObject, null);
        }

        public async Task<object> ExecuteQueryForObjectAsync(ISqlMapSession session, object parameterObject)
        {
            return await this.ExecuteQueryForObjectAsync(session, parameterObject, null);
        }

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        public object ExecuteQueryForObject(ISqlMapSession session, object parameterObject, object resultObject)
        {
            object obj = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForObject");

            obj = this.Statement.CacheModel[cacheKey];
            // check if this query has alreay been run 
            if (obj == CacheModel.NULL_OBJECT)
            {
                // convert the marker object back into a null value 
                obj = null;
            }
            else if (obj == null)
            {
                obj = _mappedStatement.RunQueryForObject(request, session, parameterObject, resultObject);
                this.Statement.CacheModel[cacheKey] = obj;
            }

            return obj;
        }

        public async Task<object> ExecuteQueryForObjectAsync(ISqlMapSession session, object parameterObject, object resultObject)
        {
            object obj = null;
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForObject");

            obj = this.Statement.CacheModel[cacheKey];
            // check if this query has alreay been run 
            if (obj == CacheModel.NULL_OBJECT)
            {
                // convert the marker object back into a null value 
                obj = null;
            }
            else if (obj == null)
            {
                obj = await _mappedStatement.RunQueryForObjectAsync(request, session, parameterObject, resultObject);
                this.Statement.CacheModel[cacheKey] = obj;
            }

            return obj;
        }

        #region ExecuteQueryForObject .NET 2.0

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The object</returns>
        public T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject)
        {
            return this.ExecuteQueryForObject<T>(session, parameterObject, default(T));
        }

        public async Task<T> ExecuteQueryForObjectAsync<T>(ISqlMapSession session, object parameterObject)
        {
            return await this.ExecuteQueryForObjectAsync<T>(session, parameterObject, default(T));
        }

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        public T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject, T resultObject)
        {
            T obj = default(T);
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForObject");

            object cacheObjet = this.Statement.CacheModel[cacheKey];
            // check if this query has alreay been run 
            if (cacheObjet is T)
            {
                obj = (T)cacheObjet;
            }
            else if (cacheObjet == CacheModel.NULL_OBJECT)
            {
                // convert the marker object back into a null value 
                obj = default(T);
            }
            else
            {
                obj = (T)_mappedStatement.RunQueryForObject(request, session, parameterObject, resultObject);
                this.Statement.CacheModel[cacheKey] = obj;
            }

            return obj;
        }

        public async Task<T> ExecuteQueryForObjectAsync<T>(ISqlMapSession session, object parameterObject, T resultObject)
        {
            T obj = default(T);
            RequestScope request = this.Statement.Sql.GetRequestScope(this, parameterObject, session);

            _mappedStatement.PreparedCommand.Create(request, session, this.Statement, parameterObject);

            CacheKey cacheKey = this.GetCacheKey(request);
            cacheKey.Update("ExecuteQueryForObject");

            object cacheObjet = this.Statement.CacheModel[cacheKey];
            // check if this query has alreay been run 
            if (cacheObjet is T)
            {
                obj = (T)cacheObjet;
            }
            else if (cacheObjet == CacheModel.NULL_OBJECT)
            {
                // convert the marker object back into a null value 
                obj = default(T);
            }
            else
            {
                obj = (T)(await _mappedStatement.RunQueryForObjectAsync(request, session, parameterObject, resultObject));
                this.Statement.CacheModel[cacheKey] = obj;
            }

            return obj;
        }
        #endregion

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="rowDelegate"></param>
        public IList ExecuteQueryForRowDelegate(ISqlMapSession session, object parameterObject, RowDelegate rowDelegate)
        {
            return _mappedStatement.ExecuteQueryForRowDelegate(session, parameterObject, rowDelegate);
        }

        public async Task<IList> ExecuteQueryForRowDelegateAsync(ISqlMapSession session, object parameterObject, RowDelegate rowDelegate)
        {
            return await _mappedStatement.ExecuteQueryForRowDelegateAsync(session, parameterObject, rowDelegate);
        }

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="rowDelegate"></param>
        public IList<T> ExecuteQueryForRowDelegate<T>(ISqlMapSession session, object parameterObject, RowDelegate<T> rowDelegate)
        {
            return _mappedStatement.ExecuteQueryForRowDelegate<T>(session, parameterObject, rowDelegate);
        }

        public async Task<IList<T>> ExecuteQueryForRowDelegateAsync<T>(ISqlMapSession session, object parameterObject, RowDelegate<T> rowDelegate)
        {
            return await _mappedStatement.ExecuteQueryForRowDelegateAsync<T>(session, parameterObject, rowDelegate);
        }

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <param name="rowDelegate"></param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        /// <exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public IDictionary ExecuteQueryForMapWithRowDelegate(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate rowDelegate)
        {
            return _mappedStatement.ExecuteQueryForMapWithRowDelegate(session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }

        public async Task<IDictionary> ExecuteQueryForMapWithRowDelegateAsync(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate rowDelegate)
        {
            return await _mappedStatement.ExecuteQueryForMapWithRowDelegateAsync(session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }

        #endregion
        #endregion
        /// <summary>
        /// Gets a percentage of successful cache hits achieved
        /// </summary>
        /// <returns>The percentage of hits (0-1), or -1 if cache is disabled.</returns>
        public double GetDataCacheHitRatio()
        {
            if (_mappedStatement.Statement.CacheModel != null)
            {
                return _mappedStatement.Statement.CacheModel.HitRatio;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>the cache key</returns>
        private CacheKey GetCacheKey(RequestScope request)
        {
            CacheKey cacheKey = new CacheKey();
            int count = request.IDbCommand.Parameters.Count;
            for (int i = 0; i < count; i++)
            {
                IDataParameter dataParameter = (IDataParameter)request.IDbCommand.Parameters[i];
                if (dataParameter.Value != null)
                {
                    cacheKey.Update(dataParameter.Value);
                }
            }

            cacheKey.Update(_mappedStatement.Id);
            cacheKey.Update(_mappedStatement.SqlMap.DataSource.ConnectionString);
            cacheKey.Update(request.IDbCommand.CommandText);

            CacheModel cacheModel = _mappedStatement.Statement.CacheModel;
            if (!cacheModel.IsReadOnly && !cacheModel.IsSerializable)
            {
                // read/write, nonserializable cache models need to use per-session caching
                cacheKey.Update(request.Session);
            }
            return cacheKey;
        }
    }
}
