using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hello2 {
	public class HttpPost {

		private string uri, path;
		private Dictionary<string, string> parameters;

		public void PostData(string uri, string path, Dictionary<string, string> parameters) {
			this.uri = uri;
			this.path = path;
			this.parameters = parameters;
		}

		public async void Post() {
			var client = new HttpClient();
		}
	}
}

