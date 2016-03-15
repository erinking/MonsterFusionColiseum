using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Spell button. I'll probably have to explain this one in person,
/// but it has a listener that sets buttonPressed to true if the button was pressed.
/// The logic to reset buttonPressed is currently in BattleLogic
/// </summary>
public class SpellButton : MonoBehaviour {
	public PseudoSpellCard spell;
	public bool buttonPressed = false;
	public Player player_that_owns_this_button;

	public Button myButton;

	void buttonHasBeenPressed(){
		buttonPressed = true;
	}

	// Use this for initialization
	void Start () {
		myButton = this.gameObject.GetComponent<Button> ();
		myButton.onClick.AddListener (buttonHasBeenPressed);
	}
	
	// Update is called once per frame
	void Update () {
		//this.gameObject.GetComponentInChildren<Text> ().text = spell.getName ();
	}

	public PseudoSpellCard returnSpell(){
		return spell;
	}
}
