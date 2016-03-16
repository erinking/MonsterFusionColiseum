using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	const float TIME = 1.5f;
	public string damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<Text> ().text = damage;
		StartCoroutine (WaitToDestroy ());
	}

	IEnumerator WaitToDestroy(){
		yield return new WaitForSeconds (TIME);
		Destroy (this.gameObject);
	}
}
