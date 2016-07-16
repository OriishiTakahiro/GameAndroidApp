using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hello2 {
	public class HttpWrapper {

		private const string URI = "http://13.71.155.33/mapmaker";

		private string path;
		private Dictionary<string, string> params_hash;

		public HttpWrapper(string path, Dictionary<string, string> params_hash) {
			this.path = path;
			this.params_hash = params_hash;
		}

		public async Task<string> GetMsg() {
			try {
				var params_str = "";
				if (params_hash != null) {
					params_str = "?";
					foreach (KeyValuePair<string, string> pair in params_hash) {
						params_str += pair.Key + "=" + pair.Value + "&";
					}
				}
				return await new HttpClient().GetStringAsync(URI + path + params_str);
			} catch (HttpRequestException error) {
				return null;
			}
		}

		public async void Post() {
			try {
				var parameters = new FormUrlEncodedContent(this.params_hash);
				await new HttpClient().PostAsync(URI + path, parameters);
			} catch (HttpRequestException error) {
			}
		}

	}
}