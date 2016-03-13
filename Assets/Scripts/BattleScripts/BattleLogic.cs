using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleLogic : MonoBehaviour {
	//Player Objects that will come in
	public Player player1;
	public Player player2;


	BattleEngine engine;

	//CPU Stuff
	float CPU_THINK_DELAY = 1.5f;


	public BattleLogic(Player p1, Player p2){
		player1 = p1;
		player2 = p2;
		StartCoroutine (BattleLoop());
	}

	// Use this for initialization
	//ONLY USE THIS FOR TESTING PURPOSES
	void Start () {
		player1 = new Player ("Bob", false);
		player1.STR = 5;
		player1.DEF = 5;
		player1.AGI = 5;
		player1.INT = 5;


		player2 = new Player ("Jill", true);
		player2.STR = 2;
		player2.DEF = 2;
		player2.AGI = 2;
		player2.INT = 2;

		//Debug.Log ("WE ARE USING THIS THINGY!!!!!!!");

		StartCoroutine (BattleLoop ());
	}


	/// <summary>
	/// The Main battle loop. Starts the battle, runs the battle, finishes the battle
	/// </summary>
	/// <returns>The loop.</returns>
	IEnumerator BattleLoop(){
		yield return StartCoroutine (BeginBattle ());

		Player player_going_first;
		Player player_going_second;

		if (player1.AGI < player2.AGI) {
			player_going_first = player2;
			player_going_second = player1;
		}
		else {
			player_going_first = player1;
			player_going_second = player2;
		}

		while (!engine.isBattleOver ()) {
			yield return StartCoroutine (Attack (player_going_first, player_going_second));
		}
	}
		
	/// <summary>
	/// Controls all initial setup, setting health bars, button text, etc.
	/// This method is very finnicky, so please check with me before making any significant
	/// changes to it, ESPECIALLY if you change the GameObject.Find(...) things.
	/// This method also depends on the name of the objects in the scene, so PLEASE do not change those.
	/// </summary>
	/// <returns>The battle.</returns>
	IEnumerator BeginBattle(){
		engine = new BattleEngine (player1, player2);
		PseudoSpellCard p1_attack1 = new PseudoSpellCard ("Basic Attack 1", SpellType.PHYSICAL_ATTACK, 10, Elemental.NONE);
		PseudoSpellCard p1_attack2 = new PseudoSpellCard ("Water Spell 1", SpellType.MAGIC_ATTACK, 20, Elemental.WATER);


		engine.getMonsterControlledByPlayer (player1).addSpell (p1_attack1);
		engine.getMonsterControlledByPlayer (player1).addSpell (p1_attack2);


		//Ask the others about these lines...why doesn't using a variable here work?
		GameObject.Find("Health A").GetComponent<HealthBar>().monster = engine.getMonsterControlledByPlayer (player1);

		//Setting Up buttons for player 1
		GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton>().spell = p1_attack1;
		GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton> ().player_that_owns_this_button = player1;

		GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton>().spell = p1_attack2;
		GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton> ().player_that_owns_this_button = player1;

		GameObject.Find ("Text A").GetComponent<PlayerName> ().player = player1;


		PseudoSpellCard p2_attack1 = new PseudoSpellCard ("Basic Attack 2", SpellType.PHYSICAL_ATTACK, 10, Elemental.NONE);
		PseudoSpellCard p2_attack2 = new PseudoSpellCard ("Fire Spell 1", SpellType.MAGIC_ATTACK, 20, Elemental.WATER);

		engine.getMonsterControlledByPlayer (player2).addSpell (p2_attack1);
		engine.getMonsterControlledByPlayer (player2).addSpell (p2_attack2);

		//Ask the others about this line...why doesn't using a variable here work?
		GameObject.Find ("Health B").GetComponent<HealthBar>().monster = engine.getMonsterControlledByPlayer (player2);

		//Setting up Buttons for player 2
		GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton>().spell = p2_attack1;
		GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton> ().player_that_owns_this_button = player2;

		GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton>().spell = p2_attack2;
		GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton> ().player_that_owns_this_button = player2;


		GameObject.Find ("Text B").GetComponent<PlayerName> ().player = player2;

		yield return null;
	}


	/// <summary>
	/// A turn of battle and damage. first_attacking_player attacks first, followed by
	/// second_attacking_player if the monster is still alive.
	/// </summary>
	/// <param name="first_attacking_player">First attacking player.</param>
	/// <param name="second_attacking_player">Second attacking player.</param>
	IEnumerator Attack(Player first_attacking_player, Player second_attacking_player){
		if (first_attacking_player.isCPU) {
			yield return new WaitForSeconds (CPU_THINK_DELAY);
			yield return StartCoroutine (RandomAttack (first_attacking_player));
		}
		else {//Human player is this one
			yield return StartCoroutine(WaitForPlayerAttack(first_attacking_player));
		}
		if (!engine.isBattleOver ()) {
			if (second_attacking_player.isCPU) {
				yield return new WaitForSeconds (CPU_THINK_DELAY);
				yield return StartCoroutine (RandomAttack (second_attacking_player));
			}
			else {//Human Player is this one
				yield return StartCoroutine(WaitForPlayerAttack(second_attacking_player));
			}
		}

		yield return null;
	}

	/// <summary>
	/// Used exclusively by the computer player, who randomly chooses one of their available moves from their monster
	/// and uses it to attack the opposing player
	/// </summary>
	/// <returns>The attack.</returns>
	/// <param name="attacker">Attacker.</param>
	IEnumerator RandomAttack(Player attacker){
		List<PseudoSpellCard> spells = engine.getMonsterControlledByPlayer (attacker).getSpells ();

		int index = Random.Range(0, spells.Count);

		engine.playerAttacks (attacker, spells [index]);

		yield return null;
	}

	/// <summary>
	/// Waits for player attack. Attacks when the player presses a button
	/// Current Bug, allows for clicking opponents buttons (nothing happens, purely visual)
	/// </summary>
	/// <returns>The for player attack.</returns>
	/// <param name="human">Human.</param>
	IEnumerator WaitForPlayerAttack(Player human){
		yield return StartCoroutine (ResetButtonTriggers ());
		while (true) {
			//This button has been pressed
			if (GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton> ().buttonPressed == true) {
				//The player owns this button, do the attack
				if (GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton> ().player_that_owns_this_button == human) {
					engine.playerAttacks (human, GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton> ().spell);
					break;
				}
			}

			if (GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton> ().buttonPressed == true) {
				//The player owns this button, do the attack
				if (GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton> ().player_that_owns_this_button == human) {
					engine.playerAttacks (human, GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton> ().spell);
					break;
				}
			}

			if (GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton> ().buttonPressed == true) {
				//The player owns this button, do the attack
				if (GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton> ().player_that_owns_this_button == human) {
					engine.playerAttacks (human, GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton> ().spell);
					break;
				}
			}

			if (GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton> ().buttonPressed == true) {
				//The player owns this button, do the attack
				if (GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton> ().player_that_owns_this_button == human) {
					engine.playerAttacks (human, GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton> ().spell);
					break;
				}
			}
			yield return null;
		}
		yield return StartCoroutine (ResetButtonTriggers ());
		yield return null;
	}

	/// <summary>
	/// Resets the button triggers for the SpellButtons.
	/// </summary>
	/// <returns>The button triggers.</returns>
	IEnumerator ResetButtonTriggers(){
		GameObject.Find ("Monster A Normal Attack").GetComponent<SpellButton> ().buttonPressed = false;
		GameObject.Find ("Monster A Special Attack").GetComponent<SpellButton> ().buttonPressed = false;

		GameObject.Find ("Monster B Normal Attack").GetComponent<SpellButton> ().buttonPressed = false;
		GameObject.Find ("Monster B Special Attack").GetComponent<SpellButton> ().buttonPressed = false;

		yield return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
