using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UICardBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//This is a workaround because delegate onClick.AddListener assignments aren't working in the GameMainControl script.
		//Issue is due to delegate assignments in a for loop taking the last value assigned.
		gameObject.GetComponent<Button> ().onClick.AddListener (() => GameObject.Find ("EventSystem").GetComponent<GameMainControl>().PickCard(gameObject.name) );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
