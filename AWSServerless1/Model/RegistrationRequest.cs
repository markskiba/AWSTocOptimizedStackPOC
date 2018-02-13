using System.ServiceModel.Security;

namespace AWSServerlessWebApi.Controllers
{
	public class RegistrationRequest
	{
		public string UserName { get; set; }
		public string EMailAddress { get; set; }
		public string Password { get; set; }
		public string CompanyName { get; set; }
	}
}