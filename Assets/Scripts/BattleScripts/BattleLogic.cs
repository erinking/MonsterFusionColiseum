using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleLogic : MonoBehaviour {
	//Player Objects that will come in
	public Player player1;
	public Player player2;


	BattleEngine engine;

	const string PLAYER_1_TEXT = "Text A";
	const string PLAYER_1_MONSTER_NAME = "Monster A";
	const string PLAYER_1_HEALTH_BAR_NAME = "Health A";
	const string PLAYER_1_HEALTH_POINTS_NAME = "Health Points A";
	const string PLAYER_1_NORMAL_ATTACK_BUTTON_NAME = "Monster A Normal Attack";
	const string PLAYER_1_SPECIAL_ATTACK_BUTTON_NAME = "Monster A Special Attack";

	PlayerName Player_1_Title_Text;
	HealthBar Player_1_Health_Bar;
	MonsterSprite Player_1_Monster;
	HealthPoints Player_1_Health_Points;
	SpellButton	Player_1_Normal_Attack_Button;
	SpellButton Player_1_Special_Attack_Button;

	const float PLAYER_1_DAMAGE_X = -381.5f;
	const float PLAYER_1_DAMAGE_Y = 26.0f;
	Vector3 PLAYER_1_DAMAGE_POSITION;

	const string PLAYER_2_TEXT = "Text B";
	const string PLAYER_2_MONSTER_NAME = "Monster B";
	const string PLAYER_2_HEALTH_BAR_NAME = "Health B";
	const string PLAYER_2_HEALTH_POINTS_NAME = "Health Points B";
	const string PLAYER_2_NORMAL_ATTACK_BUTTON_NAME = "Monster B Normal Attack";
	const string PLAYER_2_SPECIAL_ATTACK_BUTTON_NAME = "Monster B Special Attack";

	const float PLAYER_2_DAMAGE_X = 381.5f;
	const float PLAYER_2_DAMAGE_Y = 26.0f;
	Vector3 PLAYER_2_DAMAGE_POSITION;


	PlayerName Player_2_Title_Text;
	HealthBar Player_2_Health_Bar;
	MonsterSprite Player_2_Monster;
	HealthPoints Player_2_Health_Points;
	SpellButton Player_2_Normal_Attack_Button;
	SpellButton Player_2_Special_Attack_Button;

	//CPU Stuff
	const float CPU_THINK_DELAY = 1.5f;

	//Basic Attack
	PseudoSpellCard BASIC_ATTACK;

	/*public BattleLogic(Player p1, Player p2){
		player1 = p1;
		player2 = p2;
		BASIC_ATTACK = new PseudoSpellCard("Basic Attack", SpellType.PHYSICAL_ATTACK, 10, Elemental.NONE);

		PLAYER_1_DAMAGE_POSITION = new Vector3 (PLAYER_1_DAMAGE_X, PLAYER_1_DAMAGE_Y);
		PLAYER_2_DAMAGE_POSITION = new Vector3 (PLAYER_2_DAMAGE_X, PLAYER_2_DAMAGE_Y);


	}*/

	public IEnumerator StartBattleFromMain(Player p1, Player p2){
		player1 = p1;
		player2 = p2;
		BASIC_ATTACK = new PseudoSpellCard("Basic Attack", SpellType.PHYSICAL_ATTACK, 50, Elemental.NONE);

		PLAYER_1_DAMAGE_POSITION = new Vector3 (PLAYER_1_DAMAGE_X, PLAYER_1_DAMAGE_Y);
		PLAYER_2_DAMAGE_POSITION = new Vector3 (PLAYER_2_DAMAGE_X, PLAYER_2_DAMAGE_Y);

		yield return StartCoroutine (BattleLoop ());
	}

	public bool getWinner(){
		return engine.isPlayer1Winner ();
	}

	// Use this for initialization
	//ONLY USE THIS FOR TESTING PURPOSES
	void Start () {
		/*player1 = new Player ("Bob", false);
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

		PLAYER_1_DAMAGE_POSITION = new Vector3 (PLAYER_1_DAMAGE_X, PLAYER_1_DAMAGE_Y);
		PLAYER_2_DAMAGE_POSITION = new Vector3 (PLAYER_2_DAMAGE_X, PLAYER_2_DAMAGE_Y);


		BASIC_ATTACK = new PseudoSpellCard("Basic Attack", SpellType.PHYSICAL_ATTACK, 10, Elemental.NONE);
		StartCoroutine (BattleLoop ());*/
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
		yield return StartCoroutine (DisableButtons (player1));
		yield return StartCoroutine (DisableButtons (player2));
		yield return StartCoroutine (WaitForDamageToDisappear ());
	}

	IEnumerator WaitForDamageToDisappear(){
		GameObject a = GameObject.Find ("DamageIndicator(Clone)");
		while (a != null) {
			yield return new WaitForSeconds (0.1f);
		}
		a = GameObject.Find ("DamageIndicator(Clone)");
		while (a != null) {
			yield return new WaitForSeconds (0.1f);
		}
		yield return null;
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
		PseudoSpellCard p1_attack1 = BASIC_ATTACK;
		PseudoSpellCard p1_attack2 = ElementCheck.getSpellFromCards(player1.myCards);


		engine.getMonsterControlledByPlayer (player1).addSpell (p1_attack1);
		engine.getMonsterControlledByPlayer (player1).addSpell (p1_attack2);


		//Ask the others about these lines...why doesn't using a variable here work?
		Player_1_Health_Bar = GameObject.Find(PLAYER_1_HEALTH_BAR_NAME).GetComponent<HealthBar>();
		Player_1_Health_Bar.monster = engine.getMonsterControlledByPlayer (player1);

		Player_1_Health_Points = GameObject.Find (PLAYER_1_HEALTH_POINTS_NAME).GetComponent<HealthPoints> ();
		Player_1_Health_Points.monster = engine.getMonsterControlledByPlayer (player1);


		Player_1_Monster = GameObject.Find (PLAYER_1_MONSTER_NAME).GetComponent<MonsterSprite> ();
		Player_1_Monster.player = player1;

		//Setting Up buttons for player 1
		Player_1_Normal_Attack_Button = GameObject.Find (PLAYER_1_NORMAL_ATTACK_BUTTON_NAME).GetComponent<SpellButton>();
		Player_1_Normal_Attack_Button.spell = p1_attack1;
		Player_1_Normal_Attack_Button = GameObject.Find (PLAYER_1_NORMAL_ATTACK_BUTTON_NAME).GetComponent<SpellButton> ();
		Player_1_Normal_Attack_Button.player_that_owns_this_button = player1;
		Player_1_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = p1_attack1.getName ();

		Player_1_Special_Attack_Button = GameObject.Find (PLAYER_1_SPECIAL_ATTACK_BUTTON_NAME).GetComponent<SpellButton> ();
		Player_1_Special_Attack_Button.spell = p1_attack2;
		Player_1_Special_Attack_Button.player_that_owns_this_button = player1;
		Player_1_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = p1_attack2.getName ();



		Player_1_Title_Text = GameObject.Find (PLAYER_1_TEXT).GetComponent<PlayerName> ();
		Player_1_Title_Text.player = player1;


		PseudoSpellCard p2_attack1 = BASIC_ATTACK;
		PseudoSpellCard p2_attack2 = ElementCheck.getSpellFromCards (player2.myCards);

		engine.getMonsterControlledByPlayer (player2).addSpell (p2_attack1);
		engine.getMonsterControlledByPlayer (player2).addSpell (p2_attack2);

		//Ask the others about this line...why doesn't using a variable here work?
		Player_2_Health_Bar = GameObject.Find (PLAYER_2_HEALTH_BAR_NAME).GetComponent<HealthBar>();
		Player_2_Health_Bar.monster = engine.getMonsterControlledByPlayer (player2);

		Player_2_Health_Points = GameObject.Find (PLAYER_2_HEALTH_POINTS_NAME).GetComponent<HealthPoints> ();
		Player_2_Health_Points.monster = engine.getMonsterControlledByPlayer (player2);

		Player_2_Monster = GameObject.Find (PLAYER_2_MONSTER_NAME).GetComponent<MonsterSprite> ();
		Player_2_Monster.player = player2;


		//Setting up Buttons for player 2
		Player_2_Normal_Attack_Button = GameObject.Find (PLAYER_2_NORMAL_ATTACK_BUTTON_NAME).GetComponent<SpellButton>();
		Player_2_Normal_Attack_Button.spell = p2_attack1;
		Player_2_Normal_Attack_Button.player_that_owns_this_button = player2;
		Player_2_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = p2_attack1.getName ();


		Player_2_Special_Attack_Button = GameObject.Find (PLAYER_2_SPECIAL_ATTACK_BUTTON_NAME).GetComponent<SpellButton> ();
		Player_2_Special_Attack_Button.spell = p2_attack2;
		Player_2_Special_Attack_Button.player_that_owns_this_button = player2;
		Player_2_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = p2_attack2.getName ();

		Player_2_Title_Text = GameObject.Find (PLAYER_2_TEXT).GetComponent<PlayerName> ();
		Player_2_Title_Text.player = player2;

		yield return null;
	}


	/// <summary>
	/// A turn of battle and damage. first_attacking_player attacks first, followed by
	/// second_attacking_player if the monster is still alive.
	/// </summary>
	/// <param name="first_attacking_player">First attacking player.</param>
	/// <param name="second_attacking_player">Second attacking player.</param>
	IEnumerator Attack(Player first_attacking_player, Player second_attacking_player){
		yield return StartCoroutine (DisableButtons (second_attacking_player));
		yield return StartCoroutine (EnableButtons (first_attacking_player));
		if (first_attacking_player.isCPU) {
			yield return StartCoroutine (RandomAttack (first_attacking_player));
		}
		else {//Human player is this one
			yield return StartCoroutine(WaitForPlayerAttack(first_attacking_player));
		}
		yield return ShowDamage (second_attacking_player);
		yield return StartCoroutine (EnableButtons (second_attacking_player));
		yield return StartCoroutine (DisableButtons (first_attacking_player));
		if (!engine.isBattleOver ()) {
			if (second_attacking_player.isCPU) {
				yield return StartCoroutine (RandomAttack (second_attacking_player));
			}
			else {//Human Player is this one
				yield return StartCoroutine(WaitForPlayerAttack(second_attacking_player));
			}

			yield return ShowDamage (first_attacking_player);
		}
		yield return StartCoroutine (EnableButtons (first_attacking_player));
		yield return null;
	}

	public GameObject damage_num;

	IEnumerator ShowDamage(Player p){
		//Debug.Log ("MADE IT!!!!");

		Vector2 point = GameObject.Find ("Canvas").GetComponent<RectTransform> ().anchoredPosition;
		Vector3 point_as_vector3 = new Vector3 (point.x, point.y);
		//Debug.Log ("Canvas Point: " + point);

		if (p == player1) {
			GameObject a = Instantiate (damage_num);
			a.transform.SetParent (GameObject.Find ("Canvas").GetComponent<RectTransform>().transform);
			//Debug.Log("Using position: " + PLAYER_1_DAMAGE_POSITION);
			a.transform.position = PLAYER_1_DAMAGE_POSITION + point_as_vector3;
			a.GetComponentInChildren<DamageIndicator>().damage = engine.getDamageTextForDamageDoneToPlayer (p);
		}
		else {
			GameObject a = Instantiate (damage_num);
			a.transform.SetParent (GameObject.Find ("Canvas").transform);
			//Debug.Log("Using position: " + PLAYER_2_DAMAGE_POSITION);
			a.transform.position = PLAYER_2_DAMAGE_POSITION + point_as_vector3;
			a.GetComponentInChildren<DamageIndicator>().damage = engine.getDamageTextForDamageDoneToPlayer (p);
		}
		yield return null;
	}


	/*public DamageIndicator damage_num;

	IEnumerator ShowDamage(Player p){
		Debug.Log ("MADE IT!!!!");
		if (p == player1) {
			DamageIndicator a = (DamageIndicator) Instantiate (damage_num);
			a.transform.SetParent (GameObject.Find ("Canvas").transform);
			a.transform.position = PLAYER_1_DAMAGE_POSITION;
			a.damage = engine.getDamageTextForDamageDoneToPlayer (p);
		}
		else {
			DamageIndicator a = (DamageIndicator) Instantiate (damage_num);
			a.transform.SetParent (GameObject.Find ("Canvas").transform);
			a.transform.position = PLAYER_2_DAMAGE_POSITION;
			a.damage = engine.getDamageTextForDamageDoneToPlayer (p);
		}
		yield return null;
	}*/

	IEnumerator DisableButtons(Player p){
		if (p == player1) {
			Player_1_Normal_Attack_Button.GetComponentInParent<Button> ().interactable = false;
			Player_1_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = "";
			Player_1_Special_Attack_Button.GetComponentInParent<Button> ().interactable = false;
			Player_1_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = "";
		} else {
			Player_2_Normal_Attack_Button.GetComponentInParent<Button> ().interactable = false;
			Player_2_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = "";
			Player_2_Special_Attack_Button.GetComponentInParent<Button> ().interactable = false;
			Player_2_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = "";
		}
		yield return null;
	}

	IEnumerator EnableButtons(Player p){
		if (p == player1) {
			Player_1_Normal_Attack_Button.GetComponentInParent<Button> ().interactable = true;
			Player_1_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = 
				Player_1_Normal_Attack_Button.spell.getName();
			Player_1_Special_Attack_Button.GetComponentInParent<Button> ().interactable = true;
			Player_1_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = 
				Player_1_Special_Attack_Button.spell.getName();
		} else {
			Player_2_Normal_Attack_Button.GetComponentInParent<Button> ().interactable = true;
			Player_2_Normal_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = 
				Player_2_Normal_Attack_Button.spell.getName();
			Player_2_Special_Attack_Button.GetComponentInParent<Button> ().interactable = true;
			Player_2_Special_Attack_Button.GetComponentInParent<Button> ().GetComponentInChildren<Text> ().text = 
				Player_2_Special_Attack_Button.spell.getName();
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
		Debug.Log ("Wait time: " + CPU_THINK_DELAY);
		yield return new WaitForSeconds (CPU_THINK_DELAY);

		List<PseudoSpellCard> spells = engine.getMonsterControlledByPlayer (attacker).getSpells ();

		int index = Random.Range(0, spells.Count);

		engine.playerAttacks (attacker, spells [index]);

		yield return null;
	}

	/// <summary>
	/// Waits for player attack. Attacks when the player presses a button
	/// Current Bug, allows for clicking opponents buttons (nothing happens, purely visual)
	/// </summary>
	/// <param name="human">Human.</param>
	IEnumerator WaitForPlayerAttack(Player human){
		yield return StartCoroutine (ResetButtonTriggers ());
		while (true) {
			//This button has been pressed
			if (Player_1_Normal_Attack_Button.buttonPressed == true) {
				//The player owns this button, do the attack
				if (Player_1_Normal_Attack_Button.player_that_owns_this_button == human) {
					engine.playerAttacks (human, Player_1_Normal_Attack_Button.spell);
					break;
				}
			}

			if (Player_1_Special_Attack_Button.buttonPressed == true) {
				//The player owns this button, do the attack
				if (Player_1_Special_Attack_Button.player_that_owns_this_button == human) {
					engine.playerAttacks (human, Player_1_Special_Attack_Button.spell);
					break;
				}
			}

			if (Player_2_Normal_Attack_Button.buttonPressed == true) {
				//The player owns this button, do the attack
				if (Player_2_Normal_Attack_Button.player_that_owns_this_button == human) {
					engine.playerAttacks (human, Player_2_Normal_Attack_Button.spell);
					break;
				}
			}

			if (Player_2_Special_Attack_Button.buttonPressed == true) {
				//The player owns this button, do the attack
				if (Player_2_Special_Attack_Button.player_that_owns_this_button == human) {
					engine.playerAttacks (human, Player_2_Special_Attack_Button.spell);
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
		Player_1_Normal_Attack_Button.buttonPressed = false;
		Player_1_Special_Attack_Button.buttonPressed = false;

		Player_2_Normal_Attack_Button.buttonPressed = false;
		Player_2_Special_Attack_Button.buttonPressed = false;

		yield return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
