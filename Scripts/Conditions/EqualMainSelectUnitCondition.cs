﻿using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace BSS {
	public class EqualMainSelectUnitCondition : Condition
	{
		[BoxGroup("NeedParameter")]
		public string ParameterType="GameObject";
		public override bool validate (object target) {
			if (BaseSelect.instance.mainSelectable == null) {
				return false;
			}
			GameObject comparisonObj = BaseSelect.instance.mainSelectable.owner.gameObject;
			GameObject targetObj= (target as GameObject);

			return targetObj.GetInstanceID () == comparisonObj.GetInstanceID ();
		}
	}
}
