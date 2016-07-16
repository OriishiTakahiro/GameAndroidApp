using System;
using Xamarin.Forms;

namespace Hello2 {
	public class Player {

		// singleton instance
		public static Player entity { private set; get; }

		// --  chracter's data --
		public int direction { set;  get; }
		public int x { set; get; }
		public int y { set; get; }
		public bool jumping { private set; get; }
		// --  chracter's data --

		private Player() {
			this.direction = 1;
			this.x = 0;
			this.y = 0;
			this.jumping = false;
		}

		//
		// --- static method --- //
		//

		public static void Init() {
			entity = new Player();
		}


		//
		// --- non static method --- //
		//


		// rotate clock wise
		public void RotateDir() { this.direction = (this.direction + 1) % 4; }

		// switch to jumping condition
		public void Jump() { this.jumping = true; }

		// warp to start panel
		public void ReturnStart() {
			this.x = 0; this.y = 0;
			this.direction = 1;
		}

		// move to forward
		public bool Move() {
			var map = Map.entity.map;

			map[this.y][this.x].ChangeKind((int)map[this.y][this.x].kind);	// change img of panel which previous location.

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

			map[this.y][this.x].img.Source = "d_man_" + Panel.IMAGES[map[this.y][this.x].kind];	// change img of panel which new location.

			return map[this.y][this.x].ActivateEffect();
		}

		// character can move to forward?
		public bool CanMove() {
			var map = Map.entity.map;
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
			var next_panel = Map.entity.map[tmp_y][tmp_x];
			return next_panel.stayable || (next_panel.kind == Panel.Kind.rock && this.jumping);
		}

	}
}