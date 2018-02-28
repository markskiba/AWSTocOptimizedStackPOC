namespace AWSServerlessWebApi.Messages
{
	/// <summary>
	/// Error reponse class to be embedded in all responses 
	/// </summary>
	public class ErrorResponse
	{
		/// <summary>
		/// 
		/// </summary>
		public ErrorEnum error { get; set; }
		/// <summary>
		/// Error message
		/// </summary>
		public string message { get; set; } 
	} 
}
