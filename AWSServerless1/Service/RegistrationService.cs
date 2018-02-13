using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AWSServerlessWebApi.Controllers;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Model;
using Microsoft.Extensions.Logging;

namespace AWSServerlessWebApi.Service
{
    class RegistrationService
    {
		private static AmazonCognitoIdentityProviderClient IdentityClient { get; set; }

		public static async System.Threading.Tasks.Task AddRegistrationAsync(IDataStore ds, ILogger logger, RegistrationRequest regReq) {
			logger.LogInformation($"Registered new user in Cognito. user: {regReq.UserName}");
			// Add to User Table
			var user = new User()
						   {
							   EMail = regReq.EMailAddress,
							   UserName = regReq.UserName
						   };
			ds.AddUser(user);
			logger.LogInformation($"Registered new user to table Users. user: {user.UserName}");

			// Add to Cognito
			// TODO: take out keys!!!!
			var authReq = new AdminInitiateAuthRequest
							  {
								  UserPoolId = "us-east-2_YobCwhRLu",
								  ClientId = "2cphdf4o7aftg5fe6iq0de3fmo",
								  AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
							  };
			authReq.AuthParameters.Add("USERNAME", regReq.UserName);
			authReq.AuthParameters.Add("PASSWORD", regReq.Password);
			authReq.AuthParameters.Add("EMAIL", regReq.EMailAddress);

			AdminInitiateAuthResponse authResp = await IdentityClient.AdminInitiateAuthAsync(authReq);
		}
    }
}
