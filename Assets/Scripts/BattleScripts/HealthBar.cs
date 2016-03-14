using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	public Monster monster;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<Image> ().fillAmount = monster.getHealth () / monster.getMaxHealth ();
	}
}
