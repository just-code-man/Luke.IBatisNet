
#region Apache Notice
/*****************************************************************************
 * $Revision: 476843 $
 * $LastChangedDate: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
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

#region Using

using System;
using System.Data;
using Luke.IBatisNet.DataMapper.Configuration.ParameterMapping;
using Luke.IBatisNet.DataMapper.Configuration.ResultMapping;
#endregion 


namespace Luke.IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Summary description for Int32TypeHandler.
	/// </summary>
    public sealed class Int32TypeHandler : BaseTypeHandler
	{


        /// <summary>
        /// Gets a column value by the name
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			int index = dataReader.GetOrdinal(mapping.ColumnName);

			if (dataReader.IsDBNull(index) == true)
			{
				return DBNull.Value;
			}
			else
			{
				// Don't used dataReader.GetInt32 to fix oracle who alwray return decimal type
				return Convert.ToInt32(dataReader.GetValue(index));
			}
		}

        /// <summary>
        /// Gets a column value by the index
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return DBNull.Value;
			}
			else
			{
				// Don't used dataReader.GetInt32 to fix oracle who alwray return decimal type
				return Convert.ToInt32(dataReader.GetValue(mapping.ColumnIndex));
			}
		}

        /// <summary>
        /// Retrieve ouput database value of an output parameter
        /// </summary>
        /// <param name="outputValue">ouput database value</param>
        /// <param name="parameterType">type used in EnumTypeHandler</param>
        /// <returns></returns>
		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Convert.ToInt32(outputValue);
		}

        /// <summary>
        /// Converts the String to the type that this handler deals with
        /// </summary>
        /// <param name="type">the tyepe of the property (used only for enum conversion)</param>
        /// <param name="s">the String value</param>
        /// <returns>the converted value</returns>
		public override object ValueOf(Type type, string s)
		{
			return Convert.ToInt32(s);
		}


        /// <summary>
        /// Tell us if ot is a 'primitive' type
        /// </summary>
        /// <value></value>
        /// <returns></returns>
		public override bool IsSimpleType
		{
			get { return true; }
		}

        //public override object NullValue
        //{
        //    get { throw new InvalidCastException("Int32TypeHandler could not cast a null value in int32 field."); }
        //}
	}
}
