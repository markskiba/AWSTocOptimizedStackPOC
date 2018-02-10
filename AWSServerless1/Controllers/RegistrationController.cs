using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace AWSServerlessWebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Supplier Portal Registration 
	/// </summary>
	[Route("api/[controller]")]
	class RegistrationController : Controller
	{
		private AmazonDynamoDBClient AmazonDynamoDbClient { get; set; }

		ILogger Logger { get; set; }

		public RegistrationController(ILogger<S3ProxyController> logger) {
			Logger = logger;
			AmazonDynamoDbClient = new AmazonDynamoDBClient(); // todo: can this get passed in like logger?
		}

		/// <summary>
		/// Get Registration Info by userId
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		[HttpGet("{userID}")]
		[Authorize]
		public async Task<GetItemResponse> Get(string userID) {
			GetItemResponse response=null;
			try {
				response = await AmazonDynamoDbClient.GetItemAsync(
												  new GetItemRequest(
																	 "Users",
																	 new Dictionary<string, AttributeValue>()
																		 {
																			{ userID, new AttributeValue("Test")}
																		 }
																	 ));
			}
			catch (AmazonS3Exception e)
			{
				this.Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(this.Response.Body);
				writer.Write(e.Message);
			}
			return response;
		}


		/// <summary>
		/// Register user with info passed in via
		/// </summary>
		/// <returns>HttpResponse</returns>
		[HttpPost]
		public async Task<HttpResponseMessage> Post()
		{
			// Copy the request body into a seekable stream required by the AWS SDK for .NET.
			var seekableStream = new MemoryStream();
			await this.Request.Body.CopyToAsync(seekableStream);
			seekableStream.Position = 0;

			var putRequest = new PutObjectRequest
								 {
									 InputStream = seekableStream
								 };

			try
			{
				// TODO: add stream to DynamoDB
				var response = await AmazonDynamoDbClient.PutItemAsync(
													    new PutItemRequest(
																		   "Users",
																		   new Dictionary<string, AttributeValue>
																			   {
																				   {
																					   "UserName",
																					   new AttributeValue("TestUser")
																				   }
																			   }
																		  )
													   );
				Logger.LogInformation($"Put new user to table Users. user: {seekableStream}");
			}
			catch (AmazonDynamoDBException e)
			{
				this.Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(this.Response.Body);
				writer.Write(e.Message);
				return new HttpResponseMessage
						   {
							   ReasonPhrase = (string)e.Message
						   };

			}
			return new HttpResponseMessage
					   {   StatusCode = HttpStatusCode.OK
					   };
		}
	}
}
