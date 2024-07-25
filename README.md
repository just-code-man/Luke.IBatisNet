# Luke.IBatisNet
A cross-platform version of [IBatisNet](https://github.com/yYang365/IBatisNet)



## How to use？



Add `providers.config`

```
<?xml version="1.0" encoding="utf-8"?>
<providers
xmlns="http://ibatis.apache.org/providers"
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<clear/>
	<provider
	  name="MySql"
	  description="MySQL, MySQL provider"
	  enabled="true"
	  assemblyName="MySql.Data, Version=8.0.30.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d"
	  connectionClass="MySql.Data.MySqlClient.MySqlConnection"
	  commandClass="MySql.Data.MySqlClient.MySqlCommand"
	  parameterClass="MySql.Data.MySqlClient.MySqlParameter"
	  parameterDbTypeClass="MySql.Data.MySqlClient.MySqlDbType"
	  parameterDbTypeProperty="MySqlDbType"
	  dataAdapterClass="MySql.Data.MySqlClient.MySqlDataAdapter"
	  commandBuilderClass="MySql.Data.MySqlClient.MySqlCommandBuilder"
	  usePositionalParameters="false"
	  useParameterPrefixInSql="true"
	  useParameterPrefixInParameter="true"
	  parameterPrefix="?"
	  allowMARS="false"
  />
</providers>
```



Add `SqlMap.config`

```
<?xml version="1.0" encoding="utf-8"?>
<sqlMapConfig
  xmlns="http://ibatis.apache.org/dataMapper"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<settings>
		<setting useStatementNamespaces="false"/>
		<setting cacheModelsEnabled="true"/>
		<setting validateSqlMap="false"/>
	</settings>
	<providers resource="providers.config"/>
	<database>
		<provider name="MySql"/>
		<dataSource name="YourDbConnactionName" connectionString="YourconnectionString"/>
	</database>
	<sqlMaps>
		<sqlMap resource="SqlMappers/TestMapper.xml"/>
	</sqlMaps>
</sqlMapConfig>
```



Add `SqlMappers/TestMapper.xml`

```
<?xml version="1.0" encoding="utf-8" ?>
<sqlMap namespace="" xmlns="http://ibatis.apache.org/mapping" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<statements>
		<select id="TestSql" resultClass="System.Data.DataTable" parameterClass="System.String">
			SELECT * FROM sys_user
			<dynamic prepend="where">
				<isNotEmpty prepend="and" property="">
					name like '%$name$%'
				</isNotEmpty>
			</dynamic>
		</select>
	</statements>
</sqlMap>
```



Program.cs

```C#
  DomSqlMapBuilder builder = new DomSqlMapBuilder();
  ISqlMapper mapper = builder.Configure("SqlMap.config");

  DataTable dt = mapper.QueryForDataTable("TestSql", "b");
  
  dt = await mapper.QueryForDataTableAsync("TestSql", "b");

  //mapper.Insert("XXX");
  //mapper.Update("XXX");
  //mapper.Delete("XXX");
```

