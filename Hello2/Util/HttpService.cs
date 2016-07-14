using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hello2 {
	public class HttpService {

		private string uri, path, params_str;

		public HttpService(string uri, string path, Dictionary<string, string> params_hash) {
			this.uri = uri;
			this.path = path;
			if (params_hash != null) {
				this.params_str = "?";
				foreach (KeyValuePair<string, string> pair in params_hash) {
					params_str += pair.Key + "=" + pair.Value + "&";
				}
			} else this.params_str = "";
		}
		public async Task<String> download() {
			try {
				var response = await new HttpClient().GetStringAsync("http://" + uri + path + params_str);
				return response;
			} catch (HttpRequestException error) {
				return error.ToString();
			}
		}
	}
}