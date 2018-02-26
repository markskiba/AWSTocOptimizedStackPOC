using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SupplierPortalService.Tests {
	public class AppSettingsFixture : IDisposable {
		public AppSettingsFixture() {
			using (StreamReader file = File.OpenText("aws-lambda-tools-defaults.json")) {
				var reader = new JsonTextReader(file);
				JObject jObject = JObject.Load(reader);

				char[] trimChars = {'[', ']', '"'};

				foreach (JToken token in jObject.Children()) {
					// TODO: handle multiple children settings
					JToken firstChildToken = token.Values<JToken>().First();
					if (firstChildToken != null) {
						if (!firstChildToken.Any()) {
							string name = token.Path.Trim(trimChars);
							var value = firstChildToken.Value<string>();
							Environment.SetEnvironmentVariable(name, value);
						}
					}
				}
			}
		}

		public void Dispose() {
			// ... clean up
		}
	}
}