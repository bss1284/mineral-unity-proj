﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using BSS.UI;

namespace BSS.Unit {
	public class Activables : SerializedMonoBehaviour
	{
		public class InitActivable
		{
			public string category;
			public int index;
			public Activable activable;
		}
		public string initPage="Base";
		public List<InitActivable> initActivableList=new List<InitActivable>();

		public Dictionary<string,List<Activable>> activableList;

		private const int MAX_COUNT=9;

		private BaseUnit owner;

		void Awake() {
			owner = GetComponentInParent<BaseUnit> ();
		}
		void Start() {
			foreach (var it in initActivableList) {
				registActivable (it.category, it.index, it.activable);
			}
		}
			
		public string getCategory(Activable activable) {
			foreach (var it in activableList) {
				var act=it.Value.Find(x => x == activable);
				if (act != null) {
					return it.Key;
				}
			}
			return "";
		}
		public int getIndex(Activable activable) {
			foreach (var it in activableList) {
				var index=it.Value.FindIndex(x => x == activable);
				if (index>=0) {
					return index;
				}
			}
			return -1;
		}
		public Activable getActivableOrNull(string category,int index) {
			if (!activableList.ContainsKey (category)) {
				return null;
			}
			return activableList [category] [index];
		}
		public void registActivable(string category,int index,Activable act) {
			if (!activableList.ContainsKey (category)) {
				activableList [category] = new List<Activable> ();
				for (int i = 0; i < MAX_COUNT; i++) {
					activableList[category].Add(null);
				}
			}
			activableList [category][index]= act;
		}
		public void unregistActivable(string category,int index) {
            if (!activableList.ContainsKey(category)) {
                return;
            }
			Destroy (activableList [category] [index]);
			activableList [category] [index] = null;
		}

		public GameObject getContainerOrCreate(string category){
			Transform containerTr = transform.Find (category);
			if (containerTr == null) {
				var temp = new GameObject (category);
				temp.transform.SetParent (gameObject.transform);
				containerTr = temp.transform;
			}
			return containerTr.gameObject;
		}
			
	}
}

