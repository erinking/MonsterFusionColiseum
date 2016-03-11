using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
		public string name;
		public Kard myCharacter;
		public Kard myWeapon;
		public Kard myElement;
		public bool isCPU;
		public List<Kard> myCards;
		public float HP;
		public float STR;
		public float DEF;
		public float AGI;
		public float INT;
		public Player(string name, bool isCPU){
			this.name = name;
			this.isCPU = isCPU;
			this.myCards = new List<Kard>();
			this.HP = 100;
			this.STR = 0;
			this.DEF = 0;
			this.AGI = 0;
			this.INT = 0;
		}
	}

