using UnityEngine;
using System.Collections;

public class BattleLogic : MonoBehaviour {

	public Monster a;
	public Monster b;
	int speedCap = 901;
	static int startingSpeed = 0;

	bool winner = false;

	//int monsterASpeed = startingSpeed;
	//int monsterBSpeed = startingSpeed;

	// Use this for initialization
	void Start () {
	
	}

	//Simulates damage calculation for Monster a attacking Monster b
	void attack(Monster a, Monster b){
		int attackingWith;
		int defendingWith;



		if (a.intellect > a.attack) {
			attackingWith = a.intellect;
			defendingWith = b.intellect;
		}
		else {
			attackingWith = a.attack;
			defendingWith = b.def;
		}
		int damage = Mathf.Max(attackingWith - defendingWith, 1);

		if (a.weakAgainst (b.ele)) {
			damage = Mathf.Max (damage / 2, 1);
		}
		if (b.weakAgainst (a.ele)) {
			damage *= 2;
		}

		//Debug.Log ("Damage dealt: " + damage);
		b.takeDamage (damage);
	}

	public int getSpeedCap(){
		return speedCap;
	}

	void checkWinCondition(){
		if (a.health == 0 && b.health == 0) {
			Debug.Log ("A TIE?!!!!");
			winner = true;
		} 
		else if (a.health == 0) {
			Debug.Log ("Monster B Wins!");
			winner = true;
		}
		else if (b.health == 0) {
			Debug.Log ("Monster A Wins!");
			winner = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!winner) {
			a.attackBuildup += a.speed;
			b.attackBuildup += b.speed;

			if (a.attackBuildup >= speedCap) {
				a.attackBuildup = startingSpeed;
				//Debug.Log ("A attacks B!");
				attack (a, b);
			}
			if (b.attackBuildup >= speedCap) {
				b.attackBuildup = startingSpeed;
				//Debug.Log ("B attacks A!");
				attack (b, a);
			}
			checkWinCondition ();
		}
	}
}
