using System;
using System.IO;
using Amazon.DynamoDBv2;
using AWSServerlessWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace AWSServerlessWebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Supplier Portal Registration 
	/// </summary>
	[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
	public class RegistrationController : Controller
	{
		private ILogger Logger { get; }


		private RegistrationService RegService { get; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public RegistrationController(ILogger<RegistrationController> logger) {
			Logger = logger;

			RegService = new RegistrationService(logger);
		}

		/// <summary>
		/// Get Registration Info by userId
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		[HttpGet("{userID}")]
		public JsonResult Get(string userID) {
			JsonResult result=null;
			//try {
			//	var user = DataStore.GetUserByID(userID);
			//	result = new JsonResult(user);
			//}
			//catch (AmazonDynamoDBException e)
			//{
			//	result = new JsonResult(e);
			//	result.StatusCode = (int)e.StatusCode;
			//}
			return result;
		}


		/// <summary>
		/// Register user with info passed in via
		/// </summary>
		/// <returns>void</returns>
		[HttpPost]
		public async void Post() 
		{
			try
			{
				// Validate object by deserializing into POCO
				var sr = new StreamReader(Request.Body);
				string request = await sr.ReadToEndAsync();
				var registrationRequest = JsonConvert.DeserializeObject<RegistrationRequest>(request);

				//// Add to Identity Server
				await RegService.RegisterUserAsync(registrationRequest);
			}
			catch (AmazonDynamoDBException e)
			{
				Logger.LogCritical(e.Message, e);
			}
			catch (Exception exc)
			{
				Logger.LogCritical(exc.Message, exc);
			}
		}
	}
}
