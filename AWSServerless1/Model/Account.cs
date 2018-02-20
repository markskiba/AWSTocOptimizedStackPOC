using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessWebApi.Model
{
	/// <inheritdoc />
	public class Account : Trackable {
		/// <summary>
		/// Company Name
		/// </summary>
		[DynamoDBProperty("UserName")]
		public string CompanyName;
	}
}
