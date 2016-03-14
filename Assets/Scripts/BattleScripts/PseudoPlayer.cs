using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pseudo player acts as a fake player, holding only a list of cards
/// and a boolean determing if this is a CPU. This is used to create the 
/// Monster Class
/// </summary>
public class PseudoPlayer : MonoBehaviour {

	bool isComputer;
	List<PseudoCards> cards = new List<PseudoCards>();


	/// <summary>
	/// Initializes a new instance of the <see cref="PseudoPlayer"/> class.
	/// </summary>
	/// <param name="isCPU">If set to <c>true</c> is CP.</param>
	public PseudoPlayer(bool isCPU){
		isComputer = isCPU;
	}

	/// <summary>
	/// Adds a PseudoCard to the player hand
	/// </summary>
	/// <param name="card">Card.</param>
	public void addCard(PseudoCards card){
		cards.Add (card);
	}

	public bool isCPU(){
		return isComputer;
	} 

	/// <summary>
	/// Returns a list of cards in the player's hand
	/// </summary>
	/// <returns>The cards.</returns>
	public List<PseudoCards> getCards(){
		List<PseudoCards> returnList = new List<PseudoCards>();
		foreach (PseudoCards c in cards) {
			returnList.Add (c);
		}
		return returnList;
	}
}
