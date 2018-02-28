using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Messages;
using AWSServerlessWebApi.Model;
using AWSServerlessWebApi.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AWSServerlessWebApi.Tests {
	public class RegistrationServiceTests : AppSettingsFixture {
		private readonly Mock<IDataStore> _dataStoreMock = new Mock<IDataStore>();
		private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
		public IAmazonCognitoIdentityProvider IdentityProvider;

		public IConfigurationRoot Configuration { get; set; }

		[Fact]
		public async Task TestRegisterPost() {
			var registrationRequest = new RegistrationRequest
										  {
											  CompanyName = "Test Company",
											  EMailAddress = "markwskiba@gmail.com",
											  Password = "1Asdfghi!",
											  UserName = "TestUser4"
										  };
			var regService = new RegistrationService(_loggerMock.Object);
			RegistrationPostResponse response = await regService.RegisterUserAsync(registrationRequest);


			Assert.True(response.success);

			_dataStoreMock.Verify();
		}
	}
}