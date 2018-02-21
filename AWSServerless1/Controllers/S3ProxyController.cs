using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AWSServerlessWebApi.Controllers {
	/// <inheritdoc />
	/// <summary>
	/// ASP.NET Core controller acting as a S3 Proxy.
	/// </summary>
	[Route("api/[controller]")]
	public class S3ProxyController : Controller {
		public S3ProxyController(ILogger<S3ProxyController> logger, IAmazonS3 s3Client) {
			Logger = logger;
			S3Client = s3Client;

			BucketName = Startup.Configuration[Startup.AppS3BucketKey];
			if (string.IsNullOrEmpty(BucketName)) {
				logger.LogCritical("Missing configuration for S3 bucket. The AppS3Bucket configuration must be set to a S3 bucket.");
				throw new Exception("Missing configuration for S3 bucket. The AppS3Bucket configuration must be set to a S3 bucket.");
			}

			logger.LogInformation($"Configured to use bucket {BucketName}");
		}

		private IAmazonS3 S3Client { get; set; }
		private ILogger Logger { get; set; }

		private string BucketName { get; set; }

		/// <summary>
		/// Get
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<JsonResult> Get() {
			ListObjectsV2Response listResponse = await S3Client.ListObjectsV2Async(
																				   new ListObjectsV2Request
																					   {
																						   BucketName = BucketName
																					   });

			try {
				Response.ContentType = "text/json";
				return new JsonResult(
									  listResponse.S3Objects,
									  new JsonSerializerSettings
										  {
											  Formatting = Formatting.Indented
										  });
			}
			catch (AmazonS3Exception e) {
				Response.StatusCode = (int)e.StatusCode;
				return new JsonResult(e.Message);
			}
		}

		/// <summary>
		/// Get by key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpGet("{key}")]
		public async Task Get(string key) {
			try {
				GetObjectResponse getResponse = await S3Client.GetObjectAsync(
																			  new GetObjectRequest
																				  {
																					  BucketName = BucketName,
																					  Key = key
																				  });

				Response.ContentType = getResponse.Headers.ContentType;
				getResponse.ResponseStream.CopyTo(Response.Body);
			}
			catch (AmazonS3Exception e) {
				Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(Response.Body);
				writer.Write(e.Message);
			}
		}

		/// <summary>
		/// Put by key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpPut("{key}")]
		public async Task Put(string key) {
			// Copy the request body into a seekable stream required by the AWS SDK for .NET.
			var seekableStream = new MemoryStream();
			await Request.Body.CopyToAsync(seekableStream);
			seekableStream.Position = 0;

			var putRequest = new PutObjectRequest
								 {
									 BucketName = BucketName,
									 Key = key,
									 InputStream = seekableStream
								 };

			try {
				PutObjectResponse response = await S3Client.PutObjectAsync(putRequest);
				Logger.LogInformation($"Uploaded object {key} to bucket {BucketName}. Request Id: {response.ResponseMetadata.RequestId}");
			}
			catch (AmazonS3Exception e) {
				Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(Response.Body);
				writer.Write(e.Message);
			}
		}

		/// <summary>
		/// Delete by key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpDelete("{key}")]
		public async Task Delete(string key) {
			var deleteRequest = new DeleteObjectRequest
									{
										BucketName = BucketName,
										Key = key
									};

			try {
				DeleteObjectResponse response = await S3Client.DeleteObjectAsync(deleteRequest);
				Logger.LogInformation($"Deleted object {key} from bucket {BucketName}. Request Id: {response.ResponseMetadata.RequestId}");
			}
			catch (AmazonS3Exception e) {
				Response.StatusCode = (int)e.StatusCode;
				var writer = new StreamWriter(Response.Body);
				writer.Write(e.Message);
			}
		}
	}
}