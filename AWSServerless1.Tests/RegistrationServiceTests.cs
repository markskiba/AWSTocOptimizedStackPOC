using System;
using System.Linq;
using System.Threading.Tasks;
using AWSServerlessWebApi.Controllers;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Model;
using Xunit;
using AWSServerlessWebApi.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Moq;
using ServiceStack;

namespace AWSServerlessWebApi.Tests
{
    public class RegistrationServiceTests
    {
		private Mock<IDataStore> _dataStoreMock = new Moq.Mock<IDataStore>();
		private Mock<ILogger> loggerMock= new Moq.Mock<ILogger>();
		private RegistrationService RegService { get; set; }


		public RegistrationServiceTests() {
			RegService = new RegistrationService
							 {
								 DataStore = _dataStoreMock.Object,
								 Logger = loggerMock.Object
							 };
		}

		[Fact]
        public async Task TestRegister() {
			var registrationRequest = new RegistrationRequest
										  {
											  CompanyName = "Test Company",
											  EMailAddress = "TestUser@Company.com",
											  Password = "1Asdfghi!",
											  UserName = "TestUser"
										  };
			await RegService.AddRegistrationAsync(registrationRequest);

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
