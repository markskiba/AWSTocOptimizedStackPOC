namespace AWSServerlessWebApi.Messages
{
	/// <summary>  
	/// Response from a registration request 
	/// </summary>
    public class RegistrationPostResponse : BaseResponse {
		public string userSub { get; set; }
		public string username { get; set; }
		public bool userConfirmed { get; set; }
	}
}