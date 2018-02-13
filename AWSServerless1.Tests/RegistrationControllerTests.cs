using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace AWSServerlessWebApi.Tests
{
    public class RegistrationControllerTests 
    {
		//IConfigurationRoot Configuration { get; set; }

		[Fact]
		public async Task TestPost() {
			var lambdaFunction = new LambdaEntryPoint();

			var requestStr = File.ReadAllText("./SampleRequests/RegistrationController-Post.json");
			var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
			var context = new TestLambdaContext();
			var response = await lambdaFunction.FunctionHandlerAsync(request, context);

			Assert.Equal(response.StatusCode, 200);
			//Assert.Equal("[\"value1\",\"value2\"]", response.Body);
			Assert.True(response.Headers.ContainsKey("Content-Type"));
			Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);

		}

		public void Dispose() {
			//throw new NotImplementedException();
		}
	}
}
