using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour {
	public Monster monster;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<Text> ().text = monster.getHealth () + "/" + monster.getMaxHealth ();
	}
}
