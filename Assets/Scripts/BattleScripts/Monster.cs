using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Monster{

	//Stat fields
	float health;
	float attack;
	float defense;
	float agility;
	float intellect;
	float maxHealth;
	Elemental ele;

	Dictionary<Elemental, float> affinity = new Dictionary<Elemental, float>();

	List<PseudoSpellCard> methods_of_attack = new List<PseudoSpellCard>();

	string monster_name;
	string monster_owner = "Wild";

	//Determines if a monster is CPU controlled
	bool isComputer = false;

	/// <summary>
	/// Initializes a new instance of the <see cref="Monster"/> class.
	/// </summary>
	/// <param name="p">P.</param>
	public Monster(Player p){
		monster_owner = p.name;
		health = p.HP;
		maxHealth = health;

		attack = p.STR;
		defense = p.DEF;
		agility = p.AGI;
		intellect = p.INT;
		ele = ElementCheck.getMonsterElementFromCards(p.myCards);
		isComputer = p.isCPU;

		if (!isComputer) {
			monster_name = "Player-Controlled Monster";	
		} else {
			monster_name = "Computer-Controlled Monster";
		}

		setUpInitialAffinities ();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Monster"/> class. with the given parameters
	/// </summary>
	/// <param name="monster_health">Monster health.</param>
	/// <param name="monster_attack">Monster attack.</param>
	/// <param name="monster_def">Monster def.</param>
	/// <param name="monster_agi">Monster agi.</param>
	/// <param name="monster_int">Monster int.</param>
	/// <param name="monster_element">Monster element.</param>
	/// <param name="isCPU">If set to <c>true</c> is CP.</param>
	public Monster(float monster_health, float monster_attack, float monster_def, float monster_agi, float monster_int, Elemental monster_element, bool isCPU = false){
		health = monster_health;
		maxHealth = health;

		attack = monster_attack;
		defense = monster_def;
		agility = monster_agi;
		intellect = monster_int;
		ele = monster_element;
		isComputer = isCPU;

		if (!isComputer) {
			monster_name = "Player-Controlled Monster";	
		} else {
			monster_name = "Computer-Controlled Monster";
		}

		setUpInitialAffinities ();
	}

	void setUpInitialAffinities(){
		foreach (Elemental element in Enum.GetValues(typeof(Elemental))) {
			affinity [element] = 0.0f;
		}
	}


	/// <summary>
	/// Subtracts damage from the health of the monster, setting the health to
	/// zero if the damge done is greater than the current health
	/// </summary>
	/// <param name="dmg">Dmg.</param>
	public void takeDamage(float dmg){
		health -= dmg;
		if (health < 0) {
			health = 0;
		}
	}


	public float getMaxHealth(){
		return maxHealth;
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
		return ele;
	}

	public void heal(float heal_amount){
		health += heal_amount;
		if (health > maxHealth) {
			health = maxHealth;
		}
	}

	public void addHealth(float amount){
		maxHealth += amount;
		health += amount;
	}

	public void addAttack(float amount){
		attack += amount;
	}

	public void addDefense(float amount){
		defense += amount;
	}

	public void addAgility(float amount){
		agility += amount;
	}

	public void addIntellect(float amount){
		intellect += amount;
	}

	public void addSpell(PseudoSpellCard spell){
		methods_of_attack.Add (spell);
	}

	public List<PseudoSpellCard> getSpells(){
		return methods_of_attack;
	}

	public bool isCPU(){
		return isComputer;
	}

	public string getMonsterName(){
		return monster_name;
	}

	public string getMonsterOwner(){
		return monster_owner;
	}

}
