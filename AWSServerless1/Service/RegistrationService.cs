using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AWSServerlessWebApi.Controllers;
using AWSServerlessWebApi.DAL;
using AWSServerlessWebApi.Model;
using Microsoft.Extensions.Logging;

namespace AWSServerlessWebApi.Service
{
    public class RegistrationService
    {
		private static readonly string UserPoolIdKey= "UserPoolId";
		private static readonly string ClientIdKey = "ClientIdId";
		private static readonly string AuthFlowKey = "AuthFlow";
		private AmazonCognitoIdentityProviderClient IdentityClient { get; set; }

		public IDataStore DataStore { get; set; }
		public ILogger Logger { get; set; }

		/// <summary>
		/// Register a user with the UserPool and add User/Account object to DataStore
		/// </summary>
		/// <param name="regReq"></param>
		/// <returns></returns>
		public async Task AddRegistrationAsync(RegistrationRequest regReq) {
			Logger.LogInformation($"Registered new user in Cognito. user: {regReq.UserName}");
			// Add to User Table
			var user = new User
						   {
							   EMail = regReq.EMailAddress,
							   UserName = regReq.UserName
						   };
			DataStore.AddUser(user);
			Logger.LogInformation($"Registered new user to table Users. user: {user.UserName}");

			// Add to Cognito
			// TODO: take out keys!!!!
			//var authReq = new AdminInitiateAuthRequest
			//				  {
								  
			//					  UserPoolId = Startup.Configuration[UserPoolIdKey], //"us-east-2_YobCwhRLu",
			//					  ClientId = Startup.Configuration[UserPoolIdKey],//"2cphdf4o7aftg5fe6iq0de3fmo",
			//					  AuthFlow = Startup.Configuration[AuthFlowKey] //AuthFlowType.ADMIN_NO_SRP_AUTH
			//};
			//authReq.AuthParameters.Add("USERNAME", regReq.UserName);
			//authReq.AuthParameters.Add("PASSWORD", regReq.Password);
			//authReq.AuthParameters.Add("EMAIL", regReq.EMailAddress);

			//AdminInitiateAuthResponse authResp = await IdentityClient.AdminInitiateAuthAsync(authReq);
		}
    }
}
