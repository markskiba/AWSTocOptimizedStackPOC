using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessWebApi.Model
{
	/// <summary>
	/// Base class for Models
	/// </summary>
    public class BaseModel {
		/// <summary>
		/// ID of object
		/// </summary>
		[DynamoDBHashKey]
		public string ID { get; set; }

		public List<UpdateModel> VersionHistory { get; set; }
	}

}
