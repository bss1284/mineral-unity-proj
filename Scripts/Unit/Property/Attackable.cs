﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace BSS.Unit {
	public class AttackInfo {
		public GameObject attacker;
		public GameObject hitter;
		public float damage;
		public float attackSpeed;
	}
	[RequireComponent (typeof (BaseUnit))]
    public class Attackable : SerializedMonoBehaviour,IItemPropertyApply,IPunObservable
	{
		public static List<Attackable> attackableList = new List<Attackable>();

		public Sprite icon;

		[SerializeField]
		private float _initDamage;
		public float initDamage {
			get {
				return _initDamage;
			}
			private set {
				_initDamage = value;
                BaseEventListener.onPublishInt(damageEvent,(int)damage,gameObject);
			}
		}
        [HideInInspector]
        private float _changeDamage;
        public float changeDamage {
            get {
                return _changeDamage;
            }
            set {
                _changeDamage = value;
                BaseEventListener.onPublishInt(damageEvent, (int)damage, gameObject);
            }
        }
        public float damage {
            get {
                return initDamage + changeDamage;
            }
        }

		[SerializeField]
		private float _initAttackSpeed;
		public float initAttackSpeed {
			get {
				return _initAttackSpeed;
			}
			private set {
				_initAttackSpeed = value;
			}
		}
        [HideInInspector]
        private float _changeAttackSpeed;
        public float changeAttackSpeed {
            get {
                return _changeAttackSpeed;
            }
            set {
                _changeAttackSpeed = value;
            }
        }
        public float attackSpeed {
            get {
                return initAttackSpeed + changeAttackSpeed;
            }
        }


		[SerializeField]
		private float _initRange;
		public float initRange {
			get {
				return _initRange;
			}
			private set {
				_initRange = value;
			}
		}
		[HideInInspector]
		private float _changeRange=0f;
		public float changeRange {
			get {
				return _changeRange;
			}
			set {
				_changeRange=value;
			}
		}
		public float range {
			get {
				return initRange+changeRange;
			}
		}

        [Header("Int")]
        [FoldoutGroup("BaseEvent")]
        public string damageEvent = "Damage";

        [FoldoutGroup("ItemProperty")]
        public string damageProperty = "Damage";
        [FoldoutGroup("ItemProperty")]
        public string attackSpeedProperty = "AttackSpeed";
        [FoldoutGroup("ItemProperty")]
        public string rangeProperty = "Range";


        private GameObject huntTarget;
        private GameObject target;
		

        private bool canAttack = true;
		private BaseUnit owner;

        private Movable _movable;
        private Movable movable {
            get {
                return _movable == null ? GetComponent<Movable>() : _movable;
            }
        }
		[SerializeField]
		private bool isMoving {
			get{
				if (movable == null) {
					return false;
				}
				return movable.isMoving;
			}
		}



		void Awake() {
			owner = GetComponent<BaseUnit> ();
            attackableList.Add(this);
            StartCoroutine(coHuntTarget());
		}

		void OnDestroy()
		{
			attackableList.Remove(this);
		}

 
        public void toAttack(GameObject enemyObj) {
            if (!canAttack || isMoving) {
                return;
            }
            canAttack = false;
            Invoke("enableAttack", 1f / attackSpeed);

            var reacts = GetComponents<IAttackReact>();
            foreach (var it in reacts) {
                it.onAttack((enemyObj));
            }
            enemyObj.GetComponent<BaseUnit>().hitDamage(damage);
		}

        public void toHunt(GameObject enemyObj) {
            huntTarget = enemyObj;
        }
        public void huntStop() {
            huntTarget = null;
        }
        IEnumerator coHuntTarget() {
            while (true) {
                yield return new WaitForSeconds(0.1f);
                if (huntTarget == null) {
                    continue;
                }
                if (UnitUtils.InDistance(gameObject, huntTarget, range)) {
                    toAttack(huntTarget);
                } 
            }
        }

        private void enableAttack() {
            canAttack = true;
        }

        //Interface
        public void applyProperty(string ID, float value) {
            if (ID==damageProperty) {
                changeDamage += value;
            }
            if (ID == attackSpeedProperty) {
                changeAttackSpeed += value;
            }
            if (ID == rangeProperty) {
                changeRange += value;
            }
        }
        public void cancleProperty(string ID, float value) {
            if (ID == damageProperty) {
                changeDamage -= value;
            }
            if (ID == attackSpeedProperty) {
                changeAttackSpeed -= value;
            }
            if (ID == rangeProperty) {
                changeRange -= value;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting && owner.isMine) {
                // We own this player: send the others our data
                stream.SendNext(changeDamage);
                stream.SendNext(changeAttackSpeed);
                stream.SendNext(changeRange);
            } else {
                // Network player, receive data
                changeDamage = (float)stream.ReceiveNext();
                changeAttackSpeed = (float)stream.ReceiveNext();
                changeRange = (float)stream.ReceiveNext();
            }
        }

        //TODO change

        /*
		public void setTarget(BaseUnit enemy){
			target = enemy;
		}

		public bool reFindTarget(float radius){
			var targets = UnitUtils.GetUnitsInCircle (transform.position, radius).FindAll (x => UnitUtils.CheckHostile (owner, x)&& !x.isInvincible);
			if (targets.Count == 0) {
				return false;
			}
			target = targets [0];
			return true;
		}

		IEnumerator coDetectEnemy() {
			while (true) {
				yield return new WaitForSeconds (0.1f);
				if (isMoving || target!=null) {
					continue;
				}
				if (!reFindTarget (range)) {
					if (isOffenssive && reFindTarget (offenseRange)) {
						SendMessage ("onDetectInOffenceRange", this, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
		IEnumerator coAttackLoop() {
			while (true) {
				yield return new WaitForSeconds (1f/attackSpeed);
				if (isMoving  || target == null) {
					continue;
				}
				if (checkRange (target)) {
					attack (target.gameObject);
				} else if (checkRange (target, outRange)) {
					target = null;
				}
			}
		}
			
        private bool targetInRange() {
            if (target!=null) {
                Vector2 targetLocation = target.transform.position;
                Vector2 direction = targetLocation - (Vector2)transform.position;
                if (direction.sqrMagnitude < range * range) {
                    return true;
                }
            }
            return false;
        }
		private bool checkRange(BaseUnit unit,float dis) {
			return UnitUtils.IsUnitInCircle (unit, transform.position, dis);
		}
		private bool checkRange(BaseUnit unit) {
			return UnitUtils.IsUnitInCircle (unit, transform.position, range);
		}
        */


	}
}
