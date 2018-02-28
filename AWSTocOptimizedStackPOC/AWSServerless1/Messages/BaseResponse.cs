using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Runtime.Internal;

namespace AWSServerlessWebApi.Messages
{

	/// <summary>
	/// Base reponse class that is parent of messages
	/// </summary>
    public class BaseResponse {
		public BaseResponse() {
			error = new ErrorResponse();
		}
		public bool success { get; set; }
		public string statusCode { get; set; }

		public ErrorResponse error { get; set; }
	}
}
