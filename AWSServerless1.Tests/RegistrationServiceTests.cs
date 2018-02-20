using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Lambda.Core;
using AWSServerlessWebApi.Controllers;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Model;
using Xunit;
using AWSServerlessWebApi.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceStack;

namespace AWSServerlessWebApi.Tests
{
    public class RegistrationServiceTests : AppSettingsFixture
    {
		private Mock<IDataStore> _dataStoreMock = new Moq.Mock<IDataStore>();
		private Mock<ILogger> loggerMock= new Moq.Mock<ILogger>();
		public IAmazonCognitoIdentityProvider IdentityProvider;


		public RegistrationServiceTests() {
			//RegService = new RegistrationService(loggerMock.Object);
			//var builder = new ConfigurationBuilder()
			//	.SetBasePath(Directory.GetCurrentDirectory())
			//	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			//this.Configuration = builder.Build();

			//RegService.Cognito = new AmazonCognitoIdentityProviderClient(new AmazonCognitoIdentityProviderConfig(){});


		}

		public IConfigurationRoot Configuration { get; set; }

		[Fact]
        public async Task TestRegister() {
				
			var registrationRequest = new RegistrationRequest
										  {
											  CompanyName = "Test Company",
											  EMailAddress = "markwskiba@gmail.com",
											  Password = "1Asdfghi!",
											  UserName = "TestUser"
										  };
			var regService = new RegistrationService((ILogger)loggerMock.Object);
			var response = await regService.RegisterUserAsync(registrationRequest);


			var user = new User
							{
								EMail = "TestUser@Company.com",
								UserName = "TestUser"
							};
			_dataStoreMock.Verify();
			
			//Assert.Equal(_dataStoreMock.ToUrn());
			
		}
    }
}
