﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWSServerlessWebApi.Model;
using ServiceStack;

namespace AWSServerlessWebApi.DAL
{
	public class DynamoDataStore : IDataStore {
		public DynamoDBContext Context { get; set; }
		private AmazonDynamoDBClient DbClient { get; set; }

		public DynamoDataStore() {
			DbClient = new AmazonDynamoDBClient(); // todo: can this get passed in like logger?
			Context = new DynamoDBContext(DbClient);
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
		public User GetUserByID(string userID) {
			var userSearch = Context.QueryAsync<User>(userID,QueryOperator.Equal,null);
			return (userSearch.ConvertTo<User>());
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