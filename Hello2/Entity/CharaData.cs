using System;
using Xamarin.Forms;

namespace Hello2 {
	public class CharaData {

		// singleton instance
		public static CharaData chardat { private set; get; }

		// --  chracter's data --
		public int direction { set;  get; }
		public int x { set; get; }
		public int y { set; get; }
		public bool jumping { private set; get; }
		// --  chracter's data --

		private CharaData() {
			this.direction = 1;
			this.x = 0;
			this.y = 0;
			this.jumping = false;
		}

		public static CharaData GetCharDat() {
			if (chardat == null) chardat = new CharaData();
			return chardat;
		}

		public static void Init() {
			chardat = new CharaData();
		}

		// rotate clock wise
		public void RotateDir() { this.direction = (this.direction + 1) % 4; }

		// switch to jumping condition
		public void Jump() { this.jumping = true; }

		// warp to start panel
		public void ReturnStart() {
			this.x = 0; this.y = 0;
		}

		// move to forward
		public bool Move() {
			var map = MapData.mapdat.map;

			map[this.y][this.x].UpdateImg((int)map[this.y][this.x].kind);	// change img of panel which previous location.

			switch (this.direction) {
				case 0: // up
					this.y--;
					break;
				case 1: // right
					this.x++;
					break;
				case 2: // left
					this.x--;
					break;
				case 3: // down
					this.y++;
					break;
			}
			if (jumping) jumping = false;

			map[this.y][this.x].img.Source = "d_man_" + PanelData.IMAGES[map[this.y][this.x].kind];	// change img of panel which new location.

			return map[this.y][this.x].ActivateEffect();
		}

		// character can move to forward?
		public bool CanMove() {
			var map = MapData.mapdat.map;
			int tmp_x = this.x, tmp_y = this.y;
			switch (this.direction) {
				case 0: // up
					if (0 < tmp_y) tmp_y--;
					else return false;
					break;
				case 1: // right
					if (tmp_x < map[tmp_y].Length - 1) tmp_x++;
					else return false;
					break;
				case 2: // left
					if (0 < tmp_x) tmp_x--;
					else return false;
					break;
				case 3: // down
					if (tmp_y < map.Length - 1) tmp_y++;
					else return false;
					break;
			}
			var next_panel = MapData.mapdat.map[tmp_y][tmp_x];
			return next_panel.stayable || (next_panel.kind==PanelData.Kind.rock && this.jumping);
		}

	}

}

