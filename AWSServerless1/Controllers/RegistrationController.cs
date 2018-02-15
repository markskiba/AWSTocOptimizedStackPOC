using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3;
using Amazon.S3.Model;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Model;
using AWSServerlessWebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack;


namespace AWSServerlessWebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Supplier Portal Registration 
	/// </summary>
	[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
	public class RegistrationController : Controller
	{
		

		ILogger Logger { get; set; }

		IDataStore DataStore { get; set; }

		private RegistrationService RegService { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public RegistrationController(ILogger<RegistrationController> logger) {
			Logger = logger;
			DataStore = new DynamoDataStore();
			RegService = new RegistrationService();
			// TODO: setup logger and datastore for service
			RegService.DataStore = DataStore;
		}

		/// <summary>
		/// Get Registration Info by userId
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		[HttpGet("{userID}")]
		public JsonResult Get(string userID) {
			JsonResult result;
			try {
				var user = DataStore.GetUserByID(userID);
				result = new JsonResult(user);
			}
			catch (AmazonDynamoDBException e)
			{
				result = new JsonResult(e);
				result.StatusCode = (int)e.StatusCode;
			}
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
				//var seekableStream = new MemoryStream();
				//await this.Request.Body.CopyToAsync(seekableStream);
				//seekableStream.Position = 0;

				var sr = new StreamReader(Request.Body);
				string request = await sr.ReadToEndAsync();
				var registrationRequest = JsonConvert.DeserializeObject<RegistrationRequest>(request);
				//// Add to Identity Server
				await RegService.AddRegistrationAsync(registrationRequest);
			}
			catch (AmazonDynamoDBException e)
			{
				this.Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(this.Response.Body);
				writer.Write(e.Message);
			}
			catch (Exception exc)
			{
				var writer = new StreamWriter(this.Response.Body);
				writer.Write(exc.Message);
			}
		}
	}
}
