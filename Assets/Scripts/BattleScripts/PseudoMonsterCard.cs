using UnityEngine;
using System.Collections;

public class PseudoMonsterCard : MonoBehaviour, PseudoCards {

	string monster_name;
	float attack;
	float defense;
	float agility;
	float health;
	float intellect;
	Elemental element;
	PseudoCardType type = PseudoCardType.MONSTER;

	public PseudoMonsterCard(string name, float hp, float atk, float def, float agi, float intelligence, Elemental ele){
		monster_name = name;
		health = hp;
		attack = atk;
		defense = def;
		agility = agi;
		intellect = intelligence;
		element = ele;
	} 

	public string getMonsterName(){
		return monster_name;
	}

	public float getHealth(){
		return health;
	}

	public float getAttack(){
		return attack;
	}

	public float getDefense(){
		return defense;
	}

	public float getAgility(){
		return agility;
	}

	public float getIntellect(){
		return intellect;
	}

	public Elemental getElemental(){
		return element;
	}

	public PseudoCardType getPseudoCardType(){
		return type;
	}
}
