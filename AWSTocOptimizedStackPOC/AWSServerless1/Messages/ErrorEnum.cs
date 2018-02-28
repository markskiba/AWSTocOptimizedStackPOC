namespace AWSServerlessWebApi.Messages
{
	/// <summary>
	/// Response Error enumerations
	/// </summary>
    public enum ErrorEnum
    {
		/// <summary>
		/// No Error
		/// </summary>
		Ok,
		/// <summary>
		/// Unknown error 
		/// </summary>
		Unknown,
		/// <summary>
		/// User Already Exists
		/// </summary>
		UserAlreadyExists,
		/// <summary>
		/// Email already exists 
		/// </summary>
		EmailAleadyExists
    }
}
