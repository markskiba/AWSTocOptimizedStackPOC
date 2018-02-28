using System;
using System.Collections.Generic;
using System.Text;

namespace AWSServerlessWebApi.Model
{
	/// <summary>
	/// Document update
	/// </summary>
	public class UpdateModel : BaseModel 
	{
		public string UserName { get; set; }
		public DateTime Date { get; set; }
	}
}
