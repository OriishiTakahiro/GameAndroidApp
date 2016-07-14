using System;

namespace Hello2 {

	static private HtttpClient http_client;

	public class HttpGetService {
		public HttpGetService() {
			if(httpclient == null) http_client = new HttpClient();
		}
	}
}

