﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Xamarin.Forms;

namespace Hello2 {
	public partial class MyPage : ContentPage {

		private int selected_id = -1;   // selected item on item list
		private bool item_settable;		// allow setting item

		public MyPage(int param) {

			selected_id = param;
			item_settable = true;

			CharaData.Init();

			InitializeComponent();

			GetMapData();

		}

		private async void GetMapData() {
			this.IsBusy = true;
			HttpService client = new HttpService("13.71.155.33", "/mapmaker/get_mapdata", new Dictionary<string, string> { { "id", "" + selected_id } });
			var jmap = JArray.Parse(await client.download());
			var mapdat = MapData.NewMap(jmap);
			mapdat.map[0][0].img.Source = "d_man_" + PanelData.IMAGES[mapdat.map[0][0].kind];

			/*	check each label
						stack.Children.Add(new Label {
							Text = mapdat.map.Select(
								row => row.Select(panel => (int)panel.kind + ",").Aggregate((acc, str) => acc + str)
							).Aggregate((acc, str) => acc + "\n" + str)
						});
			*/

			var entire = new StackLayout { Padding = 10, Orientation = StackOrientation.Horizontal };
			// set map
			entire.Children.Add(NewMapLayout(mapdat));
			// set item list
			entire.Children.Add(NewItemsLayout());
			Content = entire;

			this.IsBusy = false;
		}


		// --- Create StackLayout for Map --
		private StackLayout NewMapLayout(MapData mapdat) {
			var map_layout = new StackLayout { Padding = 10 };
			foreach (var row in mapdat.map) {
				var row_layout = new StackLayout {
					Orientation = StackOrientation.Horizontal
				};
/*	check each image's file name
				foreach (var str in mapdat.map[i].Select(panel => (panel.GetImage().Source as FileImageSource).File.ToString()).ToArray()) {
					layout.Children.Add(new Label { Text = str } );
				}
*/
				foreach (var panel in row) {
					var tap_gesture_rec = new TapGestureRecognizer();
					var tmp_panel = panel;
					tap_gesture_rec.Tapped += (s, e) => {               // callbacked when a image tapped.
						var kind = (int)tmp_panel.kind;
						if( 0<=kind && kind<7 ) tmp_panel.UpdateImg(selected_id);
					};
					panel.img.GestureRecognizers.Add(tap_gesture_rec);    // set callback method on image view
					row_layout.Children.Add(panel.img);
				}
				map_layout.Children.Add(row_layout);
			}
			return map_layout;
		}
		// --- Create StackLayout for Map --


		// --- Create StackLayout for ItemList --
		private StackLayout NewItemsLayout() {

			var items_layout = new StackLayout();
			for (int i = 0; i < 7; i++) {
				var tap_gesture_rec = new TapGestureRecognizer();
				var tmp_i = i;
				tap_gesture_rec.Tapped += (s, e) => {               // callbacked when a image tapped.
					if (item_settable) {
						selected_id = tmp_i;
						foreach (var item in items_layout.Children) {
							if (item as Image != null) (item as Image).BackgroundColor = Color.Transparent;
						}
						(s as Image).BackgroundColor = Color.Aqua;
					}
				};
				var img = new Image { Source = PanelData.IMAGES[(PanelData.Kind)i], HorizontalOptions = LayoutOptions.Start, WidthRequest = 50 };
				img.GestureRecognizers.Add(tap_gesture_rec);    // set callback method on image view
				items_layout.Children.Add(img);
			}

			var start_btn = new Button { Text = "決定" };
			var jewel_counter = new Label { Text = MapData.mapdat.jewel_count + "/" + MapData.mapdat.jewel_max };
			var chara = CharaData.chardat;
			// Start action
			start_btn.Clicked += async (s, e) => {
				item_settable = false;
				var is_goal = false;
				while (!is_goal) {
					await Task.Delay(1000);
					var i = 0;
					for (; i < 3; i++) {
						if (chara.CanMove()) break;
						else chara.RotateDir();
					}
					if (i == 3) break;
					is_goal = chara.Move();
					jewel_counter.Text = MapData.mapdat.jewel_count + "/" + MapData.mapdat.jewel_max;
				}
				if (is_goal && MapData.mapdat.jewel_max == MapData.mapdat.jewel_count) GameClear();
				else GameOver();
			};
			items_layout.Children.Add(start_btn);
			items_layout.Children.Add(jewel_counter);
			return items_layout;
		}
		// --- Create StackLayout for ItemList --

		// --- Transaction for GameOver ---
		private async void GameOver() {
			await DisplayAlert("Game Over", "ゴールにたどり着けませんでした", "終了");
		}
		// --- Transaction for GameOver ---

		// --- Transaction for GameClear ---
		private async void GameClear() {
			await DisplayAlert("Game Clear", "おめでとうございます!! ゴールしました!!", "終了");
		}
		// --- Transaction for GameClear ---
	}

}