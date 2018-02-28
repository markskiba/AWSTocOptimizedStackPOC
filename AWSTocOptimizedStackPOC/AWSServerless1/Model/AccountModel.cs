using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessWebApi.Model
{
	/// <inheritdoc />
	public class Account : BaseModel {
		/// <summary>
		/// Company Name
		/// </summary>
		[DynamoDBProperty("UserName")]
		public string CompanyName;
	}
}
