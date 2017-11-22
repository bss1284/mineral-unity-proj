﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSS.LobbyItemSystem;
using BSS.Unit;
using Sirenix.OdinInspector;

namespace BSS {
	public class BSDatabase : SerializedMonoBehaviour
	{

		public static BSDatabase instance;
		public LobbyItemDatabase lobbyItemDatabase;
		public ActivableDatabase activableDatabase;
		public BaseUnitDatabase baseUnitDatabase;

		void Awake() {
			if (instance == null) {
				instance = this;
			} else {
				Destroy (gameObject);
				return;
			}
			DontDestroyOnLoad (gameObject);
		}
	}
}

