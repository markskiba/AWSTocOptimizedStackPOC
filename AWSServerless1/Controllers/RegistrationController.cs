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


namespace AWSServerlessWebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Supplier Portal Registration 
	/// </summary>
	[Route("api/[controller]")]
	class RegistrationController : Controller
	{
		

		ILogger Logger { get; set; }

		IDataStore DataStore { get; set; }

		public RegistrationController(ILogger logger) {
			Logger = logger;
			DataStore = new DynamoDataStore();
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
				var user = DataStore.GetUserByID(userID);
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
		/// <returns>void</returns>
		[HttpPost]
		public async void Post([FromBody]string value)
		{
			try
			{
				var registrationRequest = JsonConvert.DeserializeObject<RegistrationRequest>(value);
				// Add to Identity Server
				await RegistrationService.AddRegistrationAsync(DataStore, Logger, registrationRequest);
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
