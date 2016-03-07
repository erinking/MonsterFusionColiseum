using UnityEngine;
using System.Collections;

public abstract class Card {

	protected string name;

	public Card(string name) {
		this.name = name;
	}

	public string getName() {
		return name;
	}
}
