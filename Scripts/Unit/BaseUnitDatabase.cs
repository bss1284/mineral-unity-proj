﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace BSS.Unit {
	public class BaseUnitDatabase : SerializedScriptableObject
	{
		public Dictionary<string,GameObject> unitPrefabs=new Dictionary<string,GameObject>();
		public Dictionary<string,ActivableInfo> upgradeInfos=new Dictionary<string,ActivableInfo>();
		public Items items;

		public BaseUnit getBaseUnit(string _ID) {
			if (!unitPrefabs.ContainsKey (_ID)) {
				Debug.LogError ("No BaseUnit!");
			}
			return unitPrefabs [_ID].GetComponent<BaseUnit> ();
		}
	}
}

