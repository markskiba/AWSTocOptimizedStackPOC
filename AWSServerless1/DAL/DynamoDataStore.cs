using System;
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
		private AmazonDynamoDBClient AmazonDynamoDbClient { get; set; }

		public DynamoDataStore() {
			AmazonDynamoDbClient = new AmazonDynamoDBClient(); // todo: can this get passed in like logger?
			Context = new DynamoDBContext(AmazonDynamoDbClient);
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public User GetUserByID(string userID) {
			var userSearch = Context.QueryAsync<User>(userID,QueryOperator.Equal,null);
			// TODO: fill user from response
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
