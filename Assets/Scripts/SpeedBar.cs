using UnityEngine;
using System.Collections;

public class SpeedBar : MonoBehaviour {

	static int maxWidth = 2;

	public BattleLogic engine;
	public Monster monster;

	int speedCap;

	// Use this for initialization
	void Start () {
		speedCap = engine.getSpeedCap ();
	}
	
	// Update is called once per frame
	void Update () {
		float newX = (float)(1.0 * maxWidth * monster.attackBuildup/speedCap);
		this.gameObject.transform.localScale = new Vector3 (newX, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
	}
}
