using UnityEngine;
using System.Collections;

public class UICardBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		print ("hi");
		//This is a workaround because delegate onClick.AddListener assignments aren't working.
		GameObject.Find ("EventSystem").GetComponent<GameMainControl>().PickCard(gameObject.name);
	}
}
