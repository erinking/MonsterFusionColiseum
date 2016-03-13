using UnityEngine;
using System.Collections;

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
	float elemental_advantage = 2f;
	float elemental_disadvantage = .5f;


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
			determineDebuff (defending_mon, spell);
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

			Debug.Log (attacking_mon.getMonsterOwner() + "'s " + attacking_mon.getMonsterName() + " casts " + spell.getName() + "!!!");

			bool missed = didAttackMiss (spell);
			bool dodged = didOpponentDodge (defending_mon);

			if (missed) {
				Debug.Log (attacking_mon.getMonsterOwner () + "'s " + attacking_mon.getMonsterName () + " spell missed!!!");
			} else if (dodged) {
				Debug.Log (defending_mon.getMonsterOwner () + "'s " + defending_mon.getMonsterName () + " dodged the spell!!!");
			} else {
				Debug.Log ("It hits for " + damage + " damage!!!");
				defending_mon.takeDamage (damage);
			}



		}
		else if (spell.getSpellType () == SpellType.PHYSICAL_ATTACK) {
			float damage = spell.getAttack () + monster1.getAttack ();
			damage *= sameTypeAttackBonus (attacking_mon, spell);
			damage *= elementalAdvantage (attacking_mon, defending_mon);

			float defense = defending_mon.getDefense ();

			damage = Mathf.Max (damage - defense, 1);

			Debug.Log (attacking_mon.getMonsterOwner() + "'s "+ attacking_mon.getMonsterName() + " uses " + spell.getName() + "!!!");

			bool missed = didAttackMiss (spell);
			bool dodged = didOpponentDodge (defending_mon);

			if (missed) {
				Debug.Log (attacking_mon.getMonsterOwner () + "'s " + attacking_mon.getMonsterName () + " attack missed!!!");

			} else if (dodged) {
				Debug.Log (defending_mon.getMonsterOwner () + "'s " + defending_mon.getMonsterName () + " dodged the attack!!!");

			} else {
				Debug.Log("It hits for " + damage + " damage!!!");
				defending_mon.takeDamage (damage);
			}
		}
		else {//Someone messed up. I...I messed up.
			Debug.Log("For Some reason, an unimplemented attack came through");
		}

		Debug.Log ("");

		checkWinCondition ();
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
		float dodge_chance = 4.0f * monster.getAgility ();

		return dodge_chance >= dodge_threshold;
	}

	/// <summary>
	/// Applies the debuff value given by attack to the Monster mon.
	/// </summary>
	/// <param name="mon">Mon.</param>
	/// <param name="attack">Attack.</param>
	void determineDebuff(Monster mon, PseudoSpellCard attack){
		if (attack.getElement () == Elemental.FIRE) {
			mon.addAttack (-attack.getAttack ());
		}
		else if (attack.getElement () == Elemental.WATER) {
			mon.addAgility (-attack.getAttack ());
		}
		else if (attack.getElement () == Elemental.EARTH) {
			mon.addDefense (-attack.getAttack ());
		}
		else if (attack.getElement () == Elemental.WIND) {
			mon.addIntellect (-attack.getAttack ());
		}
		else if (attack.getElement () == Elemental.DARK) {
			mon.addHealth (-attack.getAttack ());
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
	bool isPlayer1Winner (){
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
