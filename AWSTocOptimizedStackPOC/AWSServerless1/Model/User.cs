using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessWebApi.Model
{
	///<summary>
	/// User table data
	///</summary> 
	/// <inheritdoc />
	[DynamoDBTable("Users")]
	public class User: Trackable {


		/// <summary>
		/// User Name
		/// </summary>
		[DynamoDBProperty("UserName")]
		public string UserName { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		[DynamoDBProperty("EMail")]
		public string EMail { get; set; }


	}
}
