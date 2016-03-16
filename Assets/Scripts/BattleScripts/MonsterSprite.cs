using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterSprite : MonoBehaviour {

	public Player player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.GetComponent<Image>().sprite = player.myCharacter.image;
	}
}
