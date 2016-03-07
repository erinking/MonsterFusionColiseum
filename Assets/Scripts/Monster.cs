using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
	public int health;
	public int attack;
	public int def;
	public int speed;
	public int intellect;
	int maxHealth;
	public Elemental ele;

	public int attackBuildup = 0; //This is like the final fantasy ability bar


	//Returns true if the monster is weak to Elemental a 
	public bool weakAgainst(Elemental a){
		if (this.ele == Elemental.FIRE) {
			return a == Elemental.WATER;
		}
		if (this.ele == Elemental.WATER) {
			return a == Elemental.WIND;
		}
		if (this.ele == Elemental.WIND) {
			return a == Elemental.EARTH;
		}
		if (this.ele == Elemental.EARTH) {
			return a == Elemental.FIRE;
		}
		if (this.ele == Elemental.DARK) {
			return false;
		}
		return false;
	}


	//Subtracts damage from the health of the monster
	public void takeDamage(int dmg){
		health -= dmg;
		if (health < 0) {
			health = 0;
		}
	}


	public int getMaxHealth(){
		return maxHealth;
	}

	public int getHealth(){
		return health;
	}

	public int getAtk(){
		return attack;
	}

	public int getDef(){
		return def;
	}

	public int getSpd(){
		return speed;
	}

	public int getIntellect(){
		return intellect;
	}

	public Elemental getElemental(){
		return ele;
	}
		

	// Use this for initialization
	void Start () {
		maxHealth = health;
		/*List<Elemental> blah = new List<Elemental> ();
		foreach (Elemental g in System.Enum.GetValues(typeof(Elemental))) {
			blah.Add (g);
		}
		int randInt = (int) (Random.value * System.Enum.GetNames (typeof(Elemental)).Length) % System.Enum.GetNames (typeof(Elemental)).Length; 
		ele = blah[randInt];
		*/

		Debug.Log (this.gameObject.name + " has element: " + ele);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
