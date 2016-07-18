using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Hello2 {

	public class Panel {

		public enum Kind { rock = -3, jewel = -2, portal = -1, empty = 0, up = 1, right = 2, down = 3, left = 4, jump = 5, reverse = 6, start = 7, goal = 8 };

		public static Dictionary<Kind, string> IMAGES = new Dictionary<Kind, string> {
			{ Kind.rock , "rock.png"},
			{ Kind.jewel, "jewel.png" },
			{ Kind.portal, "portal.png" },
			{ Kind.empty, "empty.png" },
			{ Kind.up, "up.png" },
			{ Kind.right, "right.png" },
			{ Kind.down, "down.png" },
			{ Kind.left, "left.png" },
			{ Kind.jump, "jump.png" },
			{ Kind.reverse, "reverse.png" },
			{ Kind.start, "start.png" },
			{ Kind.goal, "goal.png" }
		};
		public static Dictionary<Kind, Func<Panel, bool>> EFFECSTS = new Dictionary<Kind, Func<Panel, bool>> {
			{ Kind.rock, panel => { panel.stayable = false; return false; } },
			{ Kind.jewel, panel => { 
					panel.stayable = false;
					panel.SetEmpty();
					Map.entity.CountupJewel();
					return false;
			}},
			{ Kind.portal, panel => {
					panel.stayable = false;
					panel.SetEmpty();
					Player.entity.ReturnStart();
					return false;
			}},
			{ Kind.empty, panel => { panel.stayable = false; return false; } },
			{ Kind.up, panel => { 
					panel.stayable = false;
					Player.entity.direction = 0;
					return false;
			}},
			{ Kind.right, panel => {
					panel.stayable = false;
					Player.entity.direction = 1;
					return false;
			} },
			{ Kind.down, panel => {
					panel.stayable = false;
					Player.entity.direction = 2;
					return false;
			}},
			{ Kind.left, panel => {
					panel.stayable = false;
					Player.entity.direction = 3;
					return true;
			}},
			{ Kind.jump, panel => {
					panel.stayable = false;
					Player.entity.Jump();
					return false;
			}},
			{ Kind.reverse, panel => {
					panel.stayable = false;
					panel.SetEmpty();
					Map.entity.ResetAllStayable();
					return false;
			}},
			{ Kind.start, panel => { return false; } },
			{ Kind.goal, panel => { return true; } },
		};
		public readonly static Dictionary<Kind, int> COSTS = new Dictionary<Kind, int> {
			{ Kind.up, 1 }, { Kind.right, 1}, {Kind.down, 1}, {Kind.left, 1}, { Kind.jump, 3 }, { Kind.reverse, 8 }
		};

		public Kind kind { private set; get; }
		public Image img { private set; get; }
		public bool stayable { private set; get; }

		// constructor 
		public Panel(int kind) {
			this.kind = (Kind)kind;
			this.img = new Image { Source = IMAGES[(Kind)kind], HorizontalOptions = LayoutOptions.Start, WidthRequest = 50 };
			this.stayable = this.kind != Kind.rock;
		}

		// activating effect
		public bool ActivateEffect() {
			return EFFECSTS[this.kind](this);
		}

		public string ChangeKind(int kind) {
			this.kind = (Kind)kind;
			this.img.Source = IMAGES[(Kind)kind];
			return IMAGES[(Kind)kind];
		}

		public void ResetStayable() { this.stayable = true; }

		public int GetCost() {
			if (COSTS.ContainsKey(this.kind)) return COSTS[this.kind];
			else return 0;
		}
		public static int GetCost(int kind) {
			if (COSTS.ContainsKey((Kind)kind)) return COSTS[(Kind)kind];
			else return 0;
		}

		private void SetEmpty() { this.kind = 0; }


	}
}