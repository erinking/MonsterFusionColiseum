using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	static int maxWidth = 2;
	public Monster monster;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//this.gameObject
		float newX = (float) (1.0*maxWidth*monster.health/monster.getMaxHealth()); 
		this.gameObject.transform.localScale = new Vector3 (newX, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
	}
}
