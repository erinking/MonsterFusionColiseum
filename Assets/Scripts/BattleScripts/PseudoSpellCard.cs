using UnityEngine;
using System.Collections;

public class PseudoSpellCard : PseudoCards {

	string spellName;
	SpellType type;
	float attack;
	Elemental element;
	PseudoCardType cardType = PseudoCardType.SPELL;
	float accuracy;

	public PseudoSpellCard(string spell_name, SpellType spell_type, float atk, Elemental element_type, float accuracy=100){
		spellName = spell_name;
		type = spell_type;
		attack = atk;
		element = element_type;
		this.accuracy = accuracy;
	}

	public string getName(){
		return spellName;
	}

	public SpellType getSpellType(){
		return type;
	}

	public float getAttack(){
		return attack;
	}

	public Elemental getElement(){
		return element;
	}

	public PseudoCardType getPseudoCardType(){
		return cardType;
	}

	public float getAccuracy(){
		return accuracy;
	}
}
