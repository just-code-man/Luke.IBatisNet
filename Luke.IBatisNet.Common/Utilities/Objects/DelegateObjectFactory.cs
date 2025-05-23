#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-02-19 12:37:22 +0100 (Sun, 19 Feb 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Luke.IBatisNet.Common.Logging;

namespace Luke.IBatisNet.Common.Utilities.Objects
{
    /// <summary>
    /// A <see cref="IObjectFactory"/> implementation that can create objects via DynamicMethod.
    /// </summary>
    public sealed class DelegateObjectFactory : IObjectFactory
    {
        private IDictionary _cachedfactories = new HybridDictionary();
		private object _padlock = new object();
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region IObjectFactory members
        
        /// <summary>
        /// Create a new <see cref="IFactory"/> instance for a given type
        /// </summary>
        /// <param name="typeToCreate">The type instance to build</param>
        /// <param name="types">The types of the constructor arguments</param>
        /// <returns>Returns a new see <see cref="IFactory"/> instance.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
		public IFactory CreateFactory(Type typeToCreate, Type[] types)
		{
			string key = GenerateKey(typeToCreate, types);

			IFactory factory = _cachedfactories[key] as IFactory;
			if (factory == null)
			{
				lock (_padlock)
				{
					factory = _cachedfactories[key] as IFactory;
					if (factory == null) // double-check
					{
                        if (typeToCreate.IsAbstract)
                        {
                            if (_logger.IsInfoEnabled)
                            {
                                _logger.Info("Create a stub IFactory for abstract type " + typeToCreate.Name);
                            }
                            factory = new AbstractFactory(typeToCreate);
                        }
					    else
                        {
                             factory = new DelegateFactory(typeToCreate, types);
                        }
						_cachedfactories[key] = factory;
					}
				}
			}
			return factory;
		}

		/// <summary>
		/// Generates the key for a cache entry.
		/// </summary>
		/// <param name="typeToCreate">The type instance to build.</param>
		/// <param name="arguments">The types of the constructor arguments</param>
		/// <returns>The key for a cache entry.</returns>
		private string GenerateKey(Type typeToCreate, object[] arguments)
		{
			StringBuilder cacheKey = new StringBuilder();
			cacheKey.Append(typeToCreate.ToString());
			cacheKey.Append(".");
			if ((arguments != null) && (arguments.Length != 0)) 
			{
				for (int i=0; i<arguments.Length; i++) 
				{
					cacheKey.Append(".").Append(arguments[i]);
				}
			}
			return cacheKey.ToString();
		}
		#endregion
    }
}
