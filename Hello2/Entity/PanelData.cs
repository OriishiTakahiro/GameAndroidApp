﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Hello2 {

	public class PanelData {

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
		public static Dictionary<Kind, Func<PanelData, bool>> EFFECSTS = new Dictionary<Kind, Func<PanelData, bool>> {
			{ Kind.rock, panel => { panel.stayable = false; return false; } },
			{ Kind.jewel, panel => { 
					panel.stayable = false;
					panel.SetEmpty();
					MapData.mapdat.countupJewel();
					return false;
			}},
			{ Kind.portal, panel => {
					panel.stayable = false;
					panel.SetEmpty();
					CharaData.GetCharDat().ReturnStart();
					return false;
			}},
			{ Kind.empty, panel => { panel.stayable = false; return false; } },
			{ Kind.up, panel => { 
					panel.stayable = false;
					CharaData.GetCharDat().direction = 0;
					return false;
			}},
			{ Kind.right, panel => {
					panel.stayable = false;
					CharaData.GetCharDat().direction = 1;
					return false;
			} },
			{ Kind.down, panel => {
					panel.stayable = false;
					CharaData.GetCharDat().direction = 2;
					return false;
			}},
			{ Kind.left, panel => {
					panel.stayable = false;
					CharaData.GetCharDat().direction = 3;
					return true;
			}},
			{ Kind.jump, panel => {
					panel.stayable = false;
					CharaData.GetCharDat().Jump();
					return false;
			}},
			{ Kind.reverse, panel => {
					panel.stayable = false;
					panel.SetEmpty();
					MapData.mapdat.ResetAllStayable();
					return false;
			}},
			{ Kind.start, panel => { return false; } },
			{ Kind.goal, panel => { return true; } },
		};


		public Kind kind { private set; get; }
		public Image img { private set; get; }
		public bool stayable { private set; get; }

		// constructor 
		public PanelData(int kind) {
			this.kind = (Kind)kind;
			this.img = new Image { Source = IMAGES[(Kind)kind], HorizontalOptions = LayoutOptions.Start, WidthRequest = 50 };
			this.stayable = this.kind != Kind.rock;
		}

		// activating effect
		public bool ActivateEffect() {
			return EFFECSTS[this.kind](this);
		}

		public string UpdateImg(int kind) {
			this.kind = (Kind)kind;
			this.img.Source = IMAGES[(Kind)kind];
			return IMAGES[(Kind)kind];
		}

		public void ResetStayable() { this.stayable = true; }

		private void SetEmpty() { this.kind = 0; }
	}
}