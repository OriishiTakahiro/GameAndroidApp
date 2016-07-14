using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Hello2 {
	public class MapData {

		public static MapData mapdat { private set; get; }

		public PanelData[][] map { private set; get; }
		public int jewel_max { private set; get; }
		public int jewel_count { private set; get; }

		// instantiate Mapdata
		private MapData(JArray mapdat) {

			map = new PanelData[mapdat.Count][];
			int i = 0;
			foreach (var row in mapdat) {
				map[i] = row.Values<int>().Select(val => new PanelData(val)).ToArray();
				i++;
			}

			this.jewel_count = jewel_max = 0;
			foreach (PanelData[] row in map) {
				jewel_max += row.Where(panel => panel.kind == PanelData.Kind.jewel).Count();
			}

		}

		// instantiate and get new mapdata
		public static MapData NewMap(JArray jarr) {
			mapdat = new MapData(jarr);
			return mapdat;
		}

		// reset all panels stayable flag
		public void ResetAllStayable() {
			foreach (PanelData[] row in this.map) {
				foreach (PanelData panel in row) {
					if (panel.kind != PanelData.Kind.rock) panel.ResetStayable();
				}
			}
		}

		public void countupJewel() {
			jewel_count++;
		}

	}
}