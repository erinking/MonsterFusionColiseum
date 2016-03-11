using UnityEngine;
using System.Collections;

public enum CardType {Monster, Weapon, Element};
public class Kard {


		public string name;
		public CardType category;
		public Sprite image;
		public float HP;
		public float STR;
		public float DEF;
		public float AGI;
		public float INT;
		public Kard(string name, CardType category, Sprite image, float HP, float STR, float DEF, float AGI, float INT){
			this.name = name;
			this.category = category;
			this.image = image;
			this.HP = HP;
			this.STR = STR;
			this.DEF = DEF;
			this.AGI = AGI;
			this.INT = INT;
		}

		public Kard(string name, CardType category, Sprite image){
			this.name = name;
			this.category = category;
			this.image = image;
			this.HP = 25;
			this.STR = 1;
			this.DEF = 1;
			this.AGI = 1;
			this.INT = 1;
		}

		public Kard(string name, CardType category){
			this.name = name;
			this.category = category;
			this.image = null;
			this.HP = 25;
			this.STR = 1;
			this.DEF = 1;
			this.AGI = 1;
			this.INT = 1;
		}
}
