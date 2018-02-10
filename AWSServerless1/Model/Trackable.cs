using System;
using System.Collections.Generic;
using System.Text;

namespace AWSServerlessWebApi.Model
{
    public class Trackable {
		public List<Update> VersionHistory;
	}

	public class Update {
		public string Author;
		public DateTime Date;
	}
}
