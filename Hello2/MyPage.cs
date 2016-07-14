using System;

using Xamarin.Forms;

namespace Hello2 {
	public class MyPage : ContentPage {
		public MyPage() {
			Content = new StackLayout {
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


