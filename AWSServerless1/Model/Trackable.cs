using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessWebApi.Model
{
    public class Trackable {
		/// <summary>
		/// ID of object
		/// </summary>
		[DynamoDBHashKey]
		public string ID { get; set; }

		public List<Update> VersionHistory;
	}

	public class Update {
		public string Author;
		public DateTime Date;
	}
}
