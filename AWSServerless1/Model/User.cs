using System;
using System.Collections.Generic;
using System.Text;

namespace AWSServerlessWebApi.Model
{
	///<summary>
	/// User table data
	///</summary> 
	/// <inheritdoc />
	public class User: Trackable {
		/// <summary>
		/// User Name
		/// </summary>
		public string UserName;

		/// <summary>
		/// Password
		/// </summary>
		public string Password;


	}
}
