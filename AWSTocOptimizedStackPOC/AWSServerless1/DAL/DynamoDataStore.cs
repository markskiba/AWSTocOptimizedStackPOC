using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using AWSServerlessWebApi.Model;
//using ServiceStack;
using DynamoDBContextConfig = Amazon.DynamoDBv2.DataModel.DynamoDBContextConfig;

namespace AWSServerlessWebApi.DAL
{
	/// <summary>
	/// DynamoDB DataStore implementation 
	/// </summary>
	public class DynamoDataStore : IDataStore {
		/// <summary>
		/// Context of Data Store
		/// </summary>
		public DynamoDBContext Context { get; set; }
		private AmazonDynamoDBClient DbClient { get; set; }

		IDataStore DataStore448636956 { get; set; }

		// This const is the name of the environment variable that the serverless.template will use to set
		// the name of the DynamoDB table used to store blog posts.

		private const string USER_TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP = "UserTable";

		public const string ID_QUERY_STRING_NAME = "Id";

		private IDataStore DataStore { get; set; }
		private IDynamoDBContext DDBContext { get; set; }

		public DynamoDataStore() {
			// Check to see if a table name was passed in through environment variables and if so 
			// add the table mapping.
			var tableName = Environment.GetEnvironmentVariable(USER_TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP);
			if (!String.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(User)] = new TypeMapping(typeof(User), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DDBContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<bool> InitializeTables() {
			List<string> currentTables = DbClient.ListTablesAsync().Result.TableNames;
			bool tablesAdded = false;
			if (!currentTables.Contains("Users"))
			{
				Console.WriteLine("Table Actors does not exist, creating");
				await DbClient.CreateTableAsync(new CreateTableRequest
				{
					TableName = "Users",
					ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
					KeySchema = new List<KeySchemaElement>
									{
															   new KeySchemaElement
																   {
																	   AttributeName = "UserID",
																	   KeyType = KeyType.HASH
																   }
														   },
					AttributeDefinitions = new List<AttributeDefinition>
											   {
																		  new AttributeDefinition { AttributeName = "UserName", AttributeType = ScalarAttributeType.S },
																		  new AttributeDefinition { AttributeName = "EMailAddress", AttributeType = ScalarAttributeType.S }
																	  }
				});
				tablesAdded = true;
			}
			return tablesAdded;
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public User GetUserByID(string userID)
		{
			//var userSearch = Context.QueryAsync<User>(userID,QueryOperator.Equal,null);
			//return (userSearch.ConvertTo<User>());
			return new User();
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public void AddUser (User user) {
			Context.SaveAsync(user);
		}
	}
}
