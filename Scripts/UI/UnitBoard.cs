﻿using UnityEngine;
using System.Collections;
using BSS.Unit;

namespace BSS.UI {
	public class UnitBoard : Board
	{
		public BaseUnit selectUnit;

		public void Show(GameObject obj) {
			base.Show ();
			if (obj.GetComponent<BaseUnit> () != null) {
				setSelectUnit (obj.GetComponent<BaseUnit>());
			}
		}

		public void Show(BaseUnit unit) {
			base.Show ();
			setSelectUnit (unit);
		}
		public void CloseCheck(GameObject obj) {
			if (selectUnit != null && selectUnit.gameObject.GetInstanceID () == obj.GetInstanceID ()) {
				Close ();
			}
		}

		public virtual void setSelectUnit(BaseUnit unit) {
			selectUnit = unit;
		}
		public void setSelectUnit(GameObject obj) {
			var _unit=obj.GetComponent<BaseUnit> ();
			setSelectUnit (_unit);
		}
		public virtual void clearSelectUnit() {
			selectUnit = null;
			sendBoolToReceiver ("All", false);
		}
	}
}

