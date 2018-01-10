﻿using UnityEngine;
using System.Collections;

namespace BSS.UI {
	public class NoticeBoard : Board
	{

		private System.Action action;

		public void Show(string _text) {
			base.Show ();
			sendToReceiver ("Text", _text);
		}
		public void Show(string _text,System.Action act) {
			action = act;
			Show (_text);
		}
		public override void Close() {
			base.Close ();
			action = null;
		}
		public void activate() {
			if (action != null) {
				action ();
			}
		}
		public void actAndClose() {
			if (action != null) {
				action ();
			}
			Close ();
		}
	}
}
