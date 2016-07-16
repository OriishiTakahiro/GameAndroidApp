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
		public Hello2Page() {
			InitializeComponent();
			loadData();
		}
		private async void loadData() {
			this.IsBusy = true;
			var client = new HttpWrapper("13.71.155.33", "/mapmaker/get_maplist", null);
			string response = await client.GetMsg();
			var jarr = JArray.Parse(response);

			ListView maplist = ListViewComponent;
			maplist.ItemTemplate = new DataTemplate(typeof(MapCell));
			maplist.ItemsSource = jarr.ToObject<List<MapCaption>>();

			maplist.ItemSelected += async (sender, e) => {
				if (e.SelectedItem == null) return;
				await Navigation.PushModalAsync( new MyPage((e.SelectedItem as MapCaption).id) );
			};
			this.IsBusy = false;
		}
	}
}

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

	public override string ToString() {
		return string.Format("[MapData: id={0}, cleared={1}, min_cost={2}, name={3}, creater={4}, created_at={5}]", id, cleared, min_cost, name, creater, created_at);
	}
}

public class MapCell : ViewCell {

	private Label[] labels;

	public MapCell() {

		labels = new Label[6];

		labels[0] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[0].SetBinding(Label.TextProperty, "id");

		labels[1] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[1].SetBinding(Label.TextProperty, "name");

		labels[2] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[2].SetBinding(Label.TextProperty, "creater");

		labels[3] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[3].SetBinding(Label.TextProperty, new Binding("cleared", stringFormat: "{0}回"));

		labels[4] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[4].SetBinding(Label.TextProperty, new Binding("min_cost", stringFormat: "{0}Pt"));

		labels[5] = new Label { Style = Device.Styles.TitleStyle, FontSize = 20 };
		labels[5].SetBinding(Label.TextProperty, "created_at");

		View = new StackLayout {
			Orientation = StackOrientation.Horizontal,
			Padding = 5,
			Children = { labels[0], labels[1], labels[2], labels[3], labels[4], labels[5] }
		};

	}

}