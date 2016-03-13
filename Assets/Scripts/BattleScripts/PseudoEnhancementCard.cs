using UnityEngine;
using System.Collections;

public class PseudoEnhancementCard : PseudoCards {

	StatType statToEnhance;
	float enhancementAmount;
	PseudoCardType type = PseudoCardType.ENHANCEMENT;

	public PseudoEnhancementCard(StatType stat, float value){
		statToEnhance = stat;
		enhancementAmount = value;
	}

	public StatType getStatToEnhance(){
		return statToEnhance;
	}

	public float getEnhancementAmount(){
		return enhancementAmount;
	}

	public PseudoCardType getPseudoCardType(){
		return type;
	}

}
