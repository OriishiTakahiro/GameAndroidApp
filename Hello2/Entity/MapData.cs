using System;
using System.Linq;
using Newtonsoft.Json.Linq;

using Xamarin.Forms;

namespace Hello2 {
	public class MapData {

		public static MapData entity { private set; get; }

		public Panel[][] map { private set; get; }
		public int jewel_max { private set; get; }
		public int jewel_count { private set; get; }

		public int[][] backup;

		// instantiate entitya
		private MapData(JArray mapdat) {

			map = new Panel[mapdat.Count][];
			int i = 0;
			foreach (var row in mapdat) {
				map[i] = row.Values<int>().Select(val => new Panel(val)).ToArray();
				i++;
			}

			this.jewel_count = jewel_max = 0;
			foreach (Panel[] row in map) {
				jewel_max += row.Where(panel => panel.kind == Panel.Kind.jewel).Count();
			}

		}

		//
		// --- static method --- //
		//

		// instantiate and get new mapdata
		public static MapData New(JArray jarr) {
			entity = new MapData(jarr);
			return entity;
		}


		//
		// --- non static method --- //
		//

		// reset all panels stayable flag
		public void ResetAllStayable() {
			foreach (Panel[] row in this.map) {
				foreach (Panel panel in row) {
					if (panel.kind != Panel.Kind.rock) panel.ResetStayable();
				}
			}
		}

		public void Backup() { this.backup = this.map.Select( row => row.Select(panel => (int)panel.kind).ToArray() ).ToArray(); }

		public void ReproductionFromBackup() {
			for (var i = 0; i < this.map.Length; i ++) {
				for (var j = 0; j < this.map[i].Length; j++) {
					var tmp = this.map[i][j];
					if( (int)tmp.kind != this.backup[i][j] || tmp.kind == Panel.Kind.goal) tmp.ChangeKind(this.backup[i][j]);
				}
			}
			this.jewel_count = 0;
			this.ResetAllStayable();
		}

		// methods for jewel
		public string GetJewelMsg() { return $"宝石 : {this.jewel_count}/{this.jewel_max}"; }
		public void countupJewel() { jewel_count++; }

	}
}