using Xamarin.Forms;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Hello2 {

	public partial class Hello2Page : ContentPage {

		public static Hello2Page page { private set; get; }

		public Hello2Page() {
			InitializeComponent();
			page = this;
			loadData();
		}
		private async void loadData() {
			this.IsBusy = true;
			// get maplist from web API
			var client = new HttpWrapper("/get_maplist", null);
			string response = await client.GetMsg();
			if (response != null) {	// if a tablet successed getting map list
				var jarr = JArray.Parse(response);
				ListView maplist = ListViewComponent;
				// setting list view for map list
				maplist.ItemTemplate = new DataTemplate(typeof(MapCell));
				maplist.ItemsSource = jarr.ToObject<List<MapCaption>>();
				maplist.ItemSelected += async (sender, e) => {
					if (e.SelectedItem == null) return;
					await Navigation.PushModalAsync(new MyPage((e.SelectedItem as MapCaption).id));
				};
			} else {
				await DisplayAlert("Error", "マップ一覧の取得に失敗しました.", "OK");
			}
			this.IsBusy = false;
		}
	}

	// declear a set of field for using as ItemSource.
	class MapCaption {

		public int id { get; private set; }
		public int cleared { get; private set; }
		public int min_cost { get; private set; }
		public string name { get; private set; }
		public string creater { get; private set; }
		public string created_at { get; private set; }

		public MapCaption(int id, string name, string creater, int cleared, int min_cost, string created_at) {
			this.id = id;
			this.name = name;
			this.creater = creater;
			this.cleared = cleared;
			this.min_cost = min_cost;
			this.created_at = created_at;
		}

	}

	// declear customized ViewCell for map list
	public class MapCell : ViewCell {

		private Label[] labels;

		public MapCell() {

			labels = new Label[6];

			labels[0] = new Label { WidthRequest = Hello2Page.page.Width * 0.05 };
			labels[0].SetBinding(Label.TextProperty, "id");

			labels[1] = new Label { WidthRequest = Hello2Page.page.Width * 0.2 };
			labels[1].SetBinding(Label.TextProperty, "name");

			labels[2] = new Label { WidthRequest = Hello2Page.page.Width * 0.2 };
			labels[2].SetBinding(Label.TextProperty, "creater");

			labels[3] = new Label { WidthRequest = Hello2Page.page.Width * 0.075 };
			labels[3].SetBinding(Label.TextProperty, new Binding("cleared", stringFormat: "{0}回"));

			labels[4] = new Label { WidthRequest = Hello2Page.page.Width * 0.075 };
			labels[4].SetBinding(Label.TextProperty, new Binding("min_cost", stringFormat: "{0}"));

			labels[5] = new Label { WidthRequest = Hello2Page.page.Width * 0.3 };
			labels[5].SetBinding(Label.TextProperty, "created_at");

			// set common properties for each label
			Action<Label> SetLabelProperty = (Label label) => {
				label.Style = Device.Styles.TitleStyle;
				label.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
			};

			foreach (var label in labels) {
				SetLabelProperty(label);
			}

			View = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Padding = 5,
				Children = { labels[0], labels[1], labels[2], labels[3], labels[4], labels[5] }
			};

		}

	}

}