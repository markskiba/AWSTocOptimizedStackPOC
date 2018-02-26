namespace SupplierPortalService.Model
{
	/// <summary>
	/// RegistrationRequest message for Registration service post
	/// </summary>
	public class RegistrationRequest
	{
		public string UserName { get; set; }
		public string EMailAddress { get; set; }
		public string Password { get; set; }
		public string CompanyName { get; set; }
	}
}