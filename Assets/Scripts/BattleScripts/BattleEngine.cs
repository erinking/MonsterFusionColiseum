﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Battle engine simulates a fight between two given players, creating the monsters
/// From the attributes and using method calls to simulate attacks. There are no checks
/// in this class, aside to determine if a monster's health has gone to zero (i.e. you could
/// repeatedly call playerAttacks after a monster's health has been depleted)
/// </summary>
public class BattleEngine {
	Monster monster1;
	Monster monster2;

	Player player1;
	Player player2;

	bool player1_is_the_winner;
	bool winner_determined = false;

	float same_type_attack_bonus = 1.1f;
	float elemental_advantage = 1.0f;
	float elemental_disadvantage = 1.0f;

	string damage_done_to_player_1;
	string damage_done_to_player_2;

	const int HEALTH_DEBUFF_MULTIPLIER = 20;
	const float SPELL_BASE_DMG = 20;

	const float PERCENT_DODGE_INCREASE_PER_AGI_POINT = 1.0f;

	const string DODGE_TEXT = "Dodge";
	const string MISS_TEXT = "Miss";

	/// <summary>
	/// Initializes a new instance of the <see cref="BattleEngine"/> class.
	/// </summary>
	/// <param name="p1">P1.</param>
	/// <param name="p2">P2.</param>
	public BattleEngine (Player p1, Player p2){
		player1 = p1;
		monster1 = new Monster(p1);

		player2 = p2;
		monster2 = new Monster(p2);
	}

	/// <summary>
	/// Gets the monster controlled by player.
	/// </summary>
	/// <returns>The monster controlled by player.</returns>
	/// <param name="p">P.</param>
	public Monster getMonsterControlledByPlayer(Player p){
		if (p == player1) {
			//Debug.Log ("Returning: " + monster1.getMonsterOwner () + "'s monster");
			return monster1;
		}
		if (p == player2) {
			//Debug.Log ("Returning: " + monster2.getMonsterOwner () + "'s monster");
			return monster2;
		}
		Debug.Log ("Something terrible has happened while trying to get a monster controlled by a player");
		return null;
	}

	/// <summary>
	/// Simulates attaking_player attacking with their monster, using spell
	/// </summary>
	/// <param name="attacking_player">Attacking player.</param>
	/// <param name="spell">Spell.</param>
	public void playerAttacks(Player attacking_player, PseudoSpellCard spell){
		Monster attacking_monster;
		Monster defending_monster;
		if (attacking_player == player1) {
			attacking_monster = monster1;
			defending_monster = monster2;
		}
		else {
			attacking_monster = monster2;
			defending_monster = monster1;
		}

		attack (attacking_monster, defending_monster, spell);
	}

	/// <summary>
	/// Uses attacking_mon to attack defending_mon with the current spell,
	/// if necessary. Will add functionality for heals and debuffs later.
	/// </summary>
	/// <param name="attacking_mon">Attacking mon.</param>
	/// <param name="defending_mon">Defending mon.</param>
	/// <param name="spell">Spell.</param>
	void attack(Monster attacking_mon, Monster defending_mon, PseudoSpellCard spell){
		if (spell.getSpellType () == SpellType.DEBUFF) {
			bool missed = didAttackMiss (spell);
			bool dodged = didOpponentDodge (defending_mon);

			if (missed) {
				Debug.Log (attacking_mon.getMonsterOwner () + "'s " + attacking_mon.getMonsterName () + " attack missed!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, MISS_TEXT);

			} else if (dodged) {
				Debug.Log (defending_mon.getMonsterOwner () + "'s " + defending_mon.getMonsterName () + " dodged the attack!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, DODGE_TEXT);

			} else {
				string debuff_info = determineDebuff (defending_mon, spell);
				setDamageTextForDamageDoneToMonster (defending_mon, debuff_info);
				float damage = SPELL_BASE_DMG;
				defending_mon.takeDamage (damage);
			}


		}
		else if (spell.getSpellType () == SpellType.HEAL) {
			Debug.Log ("HEALs haven't been implemented yet.");
		}
		else if (spell.getSpellType () == SpellType.MAGIC_ATTACK) {
			float damage = spell.getAttack () + attacking_mon.getIntellect ();
			damage *= sameTypeAttackBonus (attacking_mon, spell);
			damage *= elementalAdvantage (attacking_mon, defending_mon);

			float defense = defending_mon.getIntellect ();

			damage = Mathf.Max (damage - defense, 1);

			//Debug.Log (attacking_mon.getMonsterOwner() + "'s " + attacking_mon.getMonsterName() + " casts " + spell.getName() + "!!!");

			bool missed = didAttackMiss (spell);
			bool dodged = didOpponentDodge (defending_mon);

			if (missed) {
				Debug.Log (attacking_mon.getMonsterOwner () + "'s " + attacking_mon.getMonsterName () + " spell missed!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, MISS_TEXT);
			} else if (dodged) {
				Debug.Log (defending_mon.getMonsterOwner () + "'s " + defending_mon.getMonsterName () + " dodged the spell!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, DODGE_TEXT);

			} else {
				Debug.Log ("It hits for " + damage + " damage!!!");
				defending_mon.takeDamage (damage);
				setDamageTextForDamageDoneToMonster (defending_mon, damage.ToString());
			}



		}
		else if (spell.getSpellType () == SpellType.PHYSICAL_ATTACK) {
			float damage = spell.getAttack () + monster1.getAttack ();
			damage *= sameTypeAttackBonus (attacking_mon, spell);
			damage *= elementalAdvantage (attacking_mon, defending_mon);

			float defense = defending_mon.getDefense ();

			damage = Mathf.Max (damage - defense, 1);

			//Debug.Log (attacking_mon.getMonsterOwner() + "'s "+ attacking_mon.getMonsterName() + " uses " + spell.getName() + "!!!");

			bool missed = didAttackMiss (spell);
			bool dodged = didOpponentDodge (defending_mon);

			if (missed) {
				Debug.Log (attacking_mon.getMonsterOwner () + "'s " + attacking_mon.getMonsterName () + " attack missed!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, MISS_TEXT);

			} else if (dodged) {
				Debug.Log (defending_mon.getMonsterOwner () + "'s " + defending_mon.getMonsterName () + " dodged the attack!!!");
				setDamageTextForDamageDoneToMonster (defending_mon, DODGE_TEXT);

			} else {
				Debug.Log("It hits for " + damage + " damage!!!");
				defending_mon.takeDamage (damage);
				setDamageTextForDamageDoneToMonster (defending_mon, damage.ToString ());
			}
		}
		else {//Someone messed up. I...I messed up.
			Debug.Log("For Some reason, an unimplemented attack came through");
		}

		//Debug.Log ("");

		checkWinCondition ();
	}

	void setDamageTextForDamageDoneToMonster(Monster player_hit, string text){
		if (player_hit == monster1) {
			damage_done_to_player_1 = text;
		}
		else {
			damage_done_to_player_2 = text;
		}
	}

	public string getDamageTextForDamageDoneToPlayer(Player p){
		if (p == player1) {
			return damage_done_to_player_1;
		}
		else {
			return damage_done_to_player_2;
		}
	}

	/// <summary>
	/// Returns true if the attack missed
	/// </summary>
	/// <returns><c>true</c>, The spell's base accuracy did not surposs the threshold, <c>false</c> otherwise.</returns>
	/// <param name="spell">Spell.</param>
	bool didAttackMiss(PseudoSpellCard spell){
		float accuracy_threshold = Random.Range (0.0f, 100.0f);

		return accuracy_threshold > spell.getAccuracy ();
	}

	/// <summary>
	/// Returns true if the opponent dodged
	/// </summary>
	/// <returns><c>true</c>, If the defending monster surpassed the dodge threshold <c>false</c> otherwise.</returns>
	/// <param name="monster">Monster.</param>
	bool didOpponentDodge(Monster monster){
		float dodge_threshold = Random.Range (0.0f, 100.0f);
		float dodge_chance = PERCENT_DODGE_INCREASE_PER_AGI_POINT * monster.getAgility ();

		return dodge_chance >= dodge_threshold;
	}


	string determineDebuffBase(Monster mon, PseudoSpellCard attack, Elemental element){
		string debuffed_stat;
		if (element == Elemental.FIRE) {
			mon.addAttack (-attack.getAttack ());
			debuffed_stat = "-ATK";
		}
		else if (element == Elemental.WATER) {
			mon.addAgility (-attack.getAttack ());
			debuffed_stat = "-AGI";
		} 
		else if (element == Elemental.EARTH) {
			mon.addDefense (-attack.getAttack ());
			debuffed_stat = "-DEF";
		}
		else if (element == Elemental.WIND) {
			mon.addIntellect (-attack.getAttack ());
			debuffed_stat = "-INT";
		}
		else if (element == Elemental.DARK) {
			mon.addHealth (-attack.getAttack ()*HEALTH_DEBUFF_MULTIPLIER);
			debuffed_stat = "-HP";
		}
		else {
			debuffed_stat = "???";
			Debug.Log ("AAAAAAAH!!!!!");
		}
		return debuffed_stat;
	}

	/// <summary>
	/// Applies the debuff value given by attack to the Monster mon.
	/// </summary>
	/// <param name="mon">Mon.</param>
	/// <param name="attack">Attack.</param>
	string determineDebuff(Monster mon, PseudoSpellCard attack){
		if(ElementCheck.baseElements.Contains(attack.getElement())){
			return determineDebuffBase (mon, attack, attack.getElement ());
		}
		else {//Tier 2
			List<Elemental> makeup = ElementCheck.getElementalMakeup(attack.getElement());
			string debuffs = determineDebuffBase (mon, attack, makeup[0]);
			determineDebuffBase(mon, attack, makeup[1]);
			return debuffs;
		}

	}


	/// <summary>
	/// Determines the elemental multiplier associated with an attack
	/// </summary>
	/// <returns>The multiplier for the damage calculation.</returns>
	/// <param name="mon1">Mon1.</param>
	/// <param name="mon2">Mon2.</param>
	float elementalAdvantage(Monster mon1, Monster mon2){
		float multiplier = 1;
		if (ElementCheck.isStrongAgainst (mon1.getElemental (), mon2.getElemental ())) {
			multiplier = elemental_advantage;
		}
		else if (ElementCheck.isWeakAgainst (mon1.getElemental (), mon2.getElemental ())) {
			multiplier = elemental_disadvantage;
		}
		return multiplier;
	}

	/// <summary>
	/// Determines if damaged is boosted due to the element of the spell and the monster matching up
	/// </summary>
	/// <returns>The type attack bonus.</returns>
	/// <param name="mon">Mon.</param>
	/// <param name="attack">Attack.</param>
	float sameTypeAttackBonus(Monster mon, PseudoSpellCard attack){
		float multiplier = 1;
		if (attack.getElement () == monster1.getElemental () && attack.getElement() != Elemental.NONE) {
			Debug.Log ("STAB!");
			multiplier = same_type_attack_bonus;
		}
		return multiplier;
	}

	/// <summary>
	/// Returns true if the battle is over, given by the function checkWinCondition()
	/// </summary>
	/// <returns><c>true</c>, if battle the battle is over, <c>false</c> otherwise.</returns>
	public bool isBattleOver(){
		return winner_determined;
	}

	/// <summary>
	/// Returns true if player1's monster won the battle
	/// </summary>
	/// <returns><c>true</c>, if player1 was the winner of the battle, <c>false</c> otherwise.</returns>
	public bool isPlayer1Winner (){
		return player1_is_the_winner;
	}

	/// <summary>
	/// Checks the win condition, setting winner_determined and player1_is_the_winner if necessary.
	/// </summary>
	void checkWinCondition(){
		if (monster1.getHealth () == 0 && monster2.getHealth () == 0) {
			Debug.Log ("Somehow, there was a tie. Defaulting to Player 1 winning");
			player1_is_the_winner = true;
			winner_determined = true;
		}
		else if (monster1.getHealth () == 0) {
			Debug.Log (monster2.getMonsterOwner () + "'s " + monster2.getMonsterName () + " wins!");
			player1_is_the_winner = false;
			winner_determined = true;
		}
		else if (monster2.getHealth () == 0) {
			Debug.Log (monster1.getMonsterOwner () + "'s " + monster1.getMonsterName () + " wins!");
			player1_is_the_winner = true;
			winner_determined = true;
		}
	}
}
