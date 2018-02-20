using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack;

namespace AWSServerlessWebApi.Tests {

	public class AppSettingsFixture : IDisposable {
		public AppSettingsFixture() {
			using (var file = File.OpenText("aws-lambda-tools-defaults.json")) {
				var reader = new JsonTextReader(file);
				var jObject = JObject.Load(reader);

				char[] trimChars = {'[',']','"'};

				foreach (var token in jObject.Children()) {
					// TODO: handle multiple children settings
					JToken firstChildToken = token.Values<JToken>().First();
					if (firstChildToken!=null)
						if (firstChildToken.Count()==0) {
					
							string name = token.Path.Trim(trimChars);
							var value = firstChildToken.Value<string>();
							Environment.SetEnvironmentVariable(name, value);
					}
				}
			}
		}

		public void Dispose() {
			// ... clean up
		}
	}

}
