using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace AWSServerlessWebApi.Tests
{
    public class RegistrationControllerTests : AppSettingsFixture
	{
		//IConfigurationRoot Configuration { get; set; }
		public RegistrationControllerTests() {
		}

		[Fact]
		public async Task TestPost() {
			var lambdaFunction = new LambdaEntryPoint();

			var requestStr = File.ReadAllText("./SampleRequests/RegistrationController-Post.json");
			var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
			var context = new TestLambdaContext();
			var response = await lambdaFunction.FunctionHandlerAsync(request, context);

			Assert.Equal(200, response.StatusCode);
		}

		// TODO Round trip test with get

		[Fact]
		public async Task TestGet()
		{
			var lambdaFunction = new LambdaEntryPoint();

			var requestStr = File.ReadAllText("./SampleRequests/RegistrationController-Get.json");
			var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
			var context = new TestLambdaContext();
			var response = await lambdaFunction.FunctionHandlerAsync(request, context);

			Assert.Equal(200, response.StatusCode);
			//Assert.Equal("[\"value1\",\"value2\"]", response.Body);
			//Assert.True(response.Headers.ContainsKey("Content-Type"));
			//Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);

		}


	}
}
