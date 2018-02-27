using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime.CredentialManagement;
using AWSServerlessWebApi.Controllers;
using AWSServerlessWebApi.DAL;
using Microsoft.Extensions.Logging;

namespace AWSServerlessWebApi.Service
{
	/// <summary>
	/// Cognito RegistrationService implementation
	/// </summary>
    public class RegistrationService
    {
		private readonly string COGNITO_APPLICATION_CLIENT_ID_ENVIRONMENT_VARIABLE_LOOKUP = "CognitoClientId";
		private readonly string COGNITO_USER_POOL_ID_ENVIRONMENT_VARIABLE_LOOKUP = "CognitoUserPoolId";

		// custom attributes
		public readonly string CompanyNameAttribute = "custom:company";

		private readonly string COGNITO_REGION_ENVIRONMENT_VARIABLE_LOOKUP = "CognitoRegion";

		public IDataStore DataStore { get; set; }
		private ILogger _logger;

		public AmazonCognitoIdentityProviderClient Cognito { get; set; }

		public string CognitoClientId { get; set; }

		/// <summary>
		/// Registration Service constructor
		/// </summary>
		/// <param name="logger"></param>
		public RegistrationService(ILogger logger) {
			_logger = logger;
			//string join = Environment.GetEnvironmentVariables() is Dictionary<string, string> env? string.Join(";", env?.Select(x => x.Key + "=" + x.Value)): "Environment.GetEnvironmentVariables()=null";
			//_logger.LogInformation($"Environment:{join}");

			// Ensure all required environment variables are provided
			CognitoClientId = Environment.GetEnvironmentVariable(COGNITO_APPLICATION_CLIENT_ID_ENVIRONMENT_VARIABLE_LOOKUP);
			if (CognitoClientId == null)
			{
				throw new Exception($"Missing required environment variable {COGNITO_APPLICATION_CLIENT_ID_ENVIRONMENT_VARIABLE_LOOKUP}");
			}

			CognitoUserPoolId = Environment.GetEnvironmentVariable(COGNITO_USER_POOL_ID_ENVIRONMENT_VARIABLE_LOOKUP);
			if (CognitoUserPoolId == null)
			{
				throw new Exception($"Missing required environment variable {COGNITO_USER_POOL_ID_ENVIRONMENT_VARIABLE_LOOKUP}");
			}

			string region = Environment.GetEnvironmentVariable(COGNITO_REGION_ENVIRONMENT_VARIABLE_LOOKUP);
			if (region == null)
			{
				throw new Exception($"Missing required environment variable {COGNITO_REGION_ENVIRONMENT_VARIABLE_LOOKUP}");
			}

			var config = new AmazonCognitoIdentityProviderConfig()
							 {
								 AuthenticationRegion = region,
								 RegionEndpoint = RegionEndpoint.GetBySystemName(region)
							 };
			Cognito = new AmazonCognitoIdentityProviderClient(config);
			

			//InitializeCustomAttributes().Wait();
		}

		public string CognitoUserPoolId { get; set; }

		/// <summary>
		/// Add custom attributes if not yet setup
		/// </summary>
		private async Task InitializeCustomAttributes() {



				var userPoolRequest = new DescribeUserPoolRequest()
										  {
											  UserPoolId = CognitoUserPoolId
										  };

				var userPoolInfo = await Cognito.DescribeUserPoolAsync(userPoolRequest);

				List<SchemaAttributeType>
					customAttributes = new List<SchemaAttributeType>();

				var companyAttr = new SchemaAttributeType()
									  {
										  AttributeDataType = AttributeDataType.String,
										  Name = CompanyNameAttribute
									  };

				List<SchemaAttributeType> userPoolSchemaAttributes = userPoolInfo.UserPool.SchemaAttributes;
				if (userPoolSchemaAttributes.All(a => a.Name != companyAttr.Name))
					customAttributes.Add(companyAttr);
				if(customAttributes.Count>0)
					await Cognito.AddCustomAttributesAsync(
													   new AddCustomAttributesRequest()
														   {
															   CustomAttributes = customAttributes,
															   UserPoolId = CognitoUserPoolId
														   });

		}

		/// <summary>
		/// Start signup process for new user in Cognito user pool
		/// </summary>
		/// <param name="regReq"></param>
		/// <returns></returns>
		public async Task<SignUpResponse> RegisterUserAsync(RegistrationRequest regReq)
		{
			// Register the user using Cognito
			var signUpRequest = new SignUpRequest
				{
					ClientId = CognitoClientId,
					Password = regReq.Password,
					Username = regReq.UserName,
				};

			var emailAttribute = new AttributeType
				{
					Name = "email",
					Value = regReq.EMailAddress
				};
			signUpRequest.UserAttributes.Add(emailAttribute);
			// Add custom attributes
			signUpRequest.UserAttributes.Add(new AttributeType
				{
				Name = CompanyNameAttribute,
				Value = regReq.CompanyName
												 });

			return await Cognito.SignUpAsync(signUpRequest);
		}
    }
}
