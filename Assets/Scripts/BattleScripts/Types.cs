using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Elemental
{
	FIRE, WATER, EARTH, WIND, DARK,

	//Tier 2

	//Wind
	GALE, MIST, SANDSTORM, SMOKE, DUSK,

	//Water
	SNOWSTORM, FLOOD, MUD, STEAM, BOG,

	//Earth
	DUST, STREAM, MOUNTAIN, COAL, ROT,

	//Fire
	HEAT, BOILING, METEOR, LAVA, ICE,

	//Dark
	VACUUM, DROUGHT, SKY, WISP, NECRO,

	//Misc
	NONE
}

public enum PseudoCardType
{
	MONSTER, SPELL, ENHANCEMENT
}
public enum SpellType
{
	PHYSICAL_ATTACK, MAGIC_ATTACK, DEBUFF, HEAL
}

public enum StatType
{
	ATTACK, DEFENSE, AGILITY, HEALTH, INTELLECT
}

public static class ElementCheck{

	public static List<Elemental> baseElements = 
		new List<Elemental>(){
		Elemental.FIRE, Elemental.WATER, Elemental.WIND, Elemental.EARTH, Elemental.DARK
	};

	public static List<Elemental> getElementalMakeup(Elemental element){
		List<Elemental> elements = new List<Elemental> ();

		if (baseElements.Contains (element)) {
			elements.Add (element);
			elements.Add (element);
		}

		else if(element == Elemental.GALE) {
			elements.Add (Elemental.WIND);
			elements.Add (Elemental.WIND);
		}
		else if(element == Elemental.MIST) {
			elements.Add (Elemental.WIND);
			elements.Add (Elemental.WATER);
		}
		else if(element == Elemental.SANDSTORM) {
			elements.Add (Elemental.WIND);
			elements.Add (Elemental.EARTH);
		}
		else if(element == Elemental.SMOKE) {
			elements.Add (Elemental.WIND);
			elements.Add (Elemental.FIRE);
		}
		else if(element == Elemental.DUSK) {
			elements.Add (Elemental.WIND);
			elements.Add (Elemental.DARK);
		}


		else if(element == Elemental.SNOWSTORM) {
			elements.Add (Elemental.WATER);
			elements.Add (Elemental.WIND);
		}
		else if(element == Elemental.FLOOD) {
			elements.Add (Elemental.WATER);
			elements.Add (Elemental.WATER);
		}
		else if(element == Elemental.MUD) {
			elements.Add (Elemental.WATER);
			elements.Add (Elemental.EARTH);
		}
		else if(element == Elemental.STEAM) {
			elements.Add (Elemental.WATER);
			elements.Add (Elemental.FIRE);
		}
		else if(element == Elemental.BOG) {
			elements.Add (Elemental.WATER);
			elements.Add (Elemental.DARK);
		}


		else if(element == Elemental.DUST) {
			elements.Add (Elemental.EARTH);
			elements.Add (Elemental.WIND);
		}
		else if(element == Elemental.STREAM) {
			elements.Add (Elemental.EARTH);
			elements.Add (Elemental.WATER);
		}
		else if(element == Elemental.MOUNTAIN) {
			elements.Add (Elemental.EARTH);
			elements.Add (Elemental.EARTH);
		}
		else if(element == Elemental.COAL) {
			elements.Add (Elemental.EARTH);
			elements.Add (Elemental.FIRE);
		}
		else if(element == Elemental.ROT) {
			elements.Add (Elemental.EARTH);
			elements.Add (Elemental.DARK);
		}


		else if(element == Elemental.HEAT) {
			elements.Add (Elemental.FIRE);
			elements.Add (Elemental.WIND);
		}
		else if(element == Elemental.BOILING) {
			elements.Add (Elemental.FIRE);
			elements.Add (Elemental.WATER);
		}
		else if(element == Elemental.METEOR) {
			elements.Add (Elemental.FIRE);
			elements.Add (Elemental.EARTH);
		}
		else if(element == Elemental.LAVA) {
			elements.Add (Elemental.FIRE);
			elements.Add (Elemental.FIRE);
		}
		else if(element == Elemental.ICE) {
			elements.Add (Elemental.FIRE);
			elements.Add (Elemental.DARK);
		}


		else if(element == Elemental.VACUUM) {
			elements.Add (Elemental.DARK);
			elements.Add (Elemental.WIND);
		}
		else if(element == Elemental.DROUGHT) {
			elements.Add (Elemental.DARK);
			elements.Add (Elemental.WATER);
		}
		else if(element == Elemental.SKY) {
			elements.Add (Elemental.DARK);
			elements.Add (Elemental.EARTH);
		}
		else if(element == Elemental.WISP) {
			elements.Add (Elemental.DARK);
			elements.Add (Elemental.FIRE);
		}
		else if(element == Elemental.NECRO) {
			elements.Add (Elemental.DARK);
			elements.Add (Elemental.DARK);
		}


		else{
			Debug.Log ("Messed up: Elements to Two");
			elements.Add (Elemental.NONE);
			elements.Add (Elemental.NONE);
		}

		return elements;
	}

	private static Elemental getElementFromTwoElements(Elemental element_1, Elemental element_2){
		if (element_1 == Elemental.FIRE) {
			if (element_1 == Elemental.FIRE) {
				return Elemental.LAVA;
			}

			else if (element_1 == Elemental.WATER) {
				return Elemental.BOILING;
			}

			else if (element_1 == Elemental.EARTH) {
				return Elemental.METEOR;
			}

			else if (element_1 == Elemental.WIND) {
				return Elemental.HEAT;
			}

			else if (element_1 == Elemental.DARK) {
				return Elemental.ICE;
			}

			else {
				return Elemental.NONE;
			}
		} 

		else if (element_1 == Elemental.WATER) {
			if (element_1 == Elemental.FIRE) {
				return Elemental.STEAM;
			} 

			else if (element_1 == Elemental.WATER) {
				return Elemental.FLOOD;
			}

			else if (element_1 == Elemental.EARTH) {
				return Elemental.MUD;
			}

			else if (element_1 == Elemental.WIND) {
				return Elemental.SNOWSTORM;
			}

			else if (element_1 == Elemental.DARK) {
				return Elemental.BOG;
			}

			else {
				return Elemental.NONE;
			}
		}

		else if (element_1 == Elemental.EARTH) {
			if (element_1 == Elemental.FIRE) {
				return Elemental.COAL;
			}

			else if (element_1 == Elemental.WATER) {
				return Elemental.STREAM;
			}

			else if (element_1 == Elemental.EARTH) {
				return Elemental.MOUNTAIN;
			} 

			else if (element_1 == Elemental.WIND) {
				return Elemental.DUST;
			}

			else if (element_1 == Elemental.DARK) {
				return Elemental.ROT;
			}

			else {
				return Elemental.NONE;
			}
		} 

		else if (element_1 == Elemental.WIND) {
			if (element_1 == Elemental.FIRE) {
				return Elemental.SMOKE;
			} 

			else if (element_1 == Elemental.WATER) {
				return Elemental.MIST;
			}

			else if (element_1 == Elemental.EARTH) {
				return Elemental.SANDSTORM;
			}

			else if (element_1 == Elemental.WIND) {
				return Elemental.GALE;
			}

			else if (element_1 == Elemental.DARK) {
				return Elemental.DUSK;
			}

			else {
				return Elemental.NONE;
			}
		} 

		else if (element_1 == Elemental.DARK) {
			if (element_1 == Elemental.FIRE) {
				return Elemental.WISP;
			} 

			else if (element_1 == Elemental.WATER) {
				return Elemental.DROUGHT;
			}

			else if (element_1 == Elemental.EARTH) {
				return Elemental.SKY;
			}

			else if (element_1 == Elemental.WIND) {
				return Elemental.VACUUM;
			}

			else if (element_1 == Elemental.DARK) {
				return Elemental.NECRO;
			}
			else {
				return Elemental.NONE;
			}
		}

		else {
			return Elemental.NONE;
		}
	
	}

	private static Elemental getElementFromTwoMonsterKards(Kard card_1, Kard card_2){
		Elemental element_1;
		Elemental element_2;

		Debug.Log ("Monster Formation: " + card_1.name + " " + card_2.name);

		if (card_1.name == "Sprite") {
			element_1 = Elemental.WIND;
		}
		else if (card_1.name == "Blobfish") {
			element_1 = Elemental.WATER;
		}
		else if (card_1.name == "Giant") {
			element_1 = Elemental.EARTH;
		}
		else if (card_1.name == "Gecko") {
			element_1 = Elemental.FIRE;
		}
		else if (card_1.name == "Ghost") {
			element_1 = Elemental.DARK;
		}
		else {//Messed up somewhere
			Debug.Log("We messed up getting elements from cards.");
			element_1 = Elemental.NONE;
		}


		if (card_2.name == "Sprite") {
			element_2 = Elemental.WIND;
		}
		else if (card_2.name == "Blobfish") {
			element_2 = Elemental.WATER;
		}
		else if (card_2.name == "Giant") {
			element_2 = Elemental.EARTH;
		}
		else if (card_2.name == "Gecko") {
			element_2 = Elemental.FIRE;
		}
		else if (card_2.name == "Ghost") {
			element_2 = Elemental.DARK;
		}
		else {//Messed up somewhere
			Debug.Log("We messed up getting elements from cards.");
			element_2 = Elemental.NONE;
		}

		return getElementFromTwoElements (element_1, element_2);
	}

	public static Elemental getMonsterElementFromCards(List<Kard> cards){
		Kard dummy = new Kard ("dummy", CardType.Element);
		Kard first_card = dummy;
		Kard second_card = dummy;
		foreach (Kard card in cards) {
			if (card.category != CardType.Monster) {
				continue;		
			}
			else {
				if (first_card == dummy) {
					first_card = card;
				} 
				else {
					second_card = card;
					break;
				}
			}
		}

		return getElementFromTwoMonsterKards (first_card, second_card);
	}

	/// <summary>
	/// We will need to come back to this method, currently only does
	/// fusion in one way
	/// </summary>
	/// <returns>The spell from cards.</returns>
	/// <param name="cards">Cards.</param>
	public static PseudoSpellCard getSpellFromCards(List<Kard> cards){
		Dictionary<Elemental, int> frequencies = new Dictionary<Elemental, int> ();

		foreach (Elemental element in baseElements) {
			frequencies.Add (element, 0);
		}

		foreach (Kard card in cards) {
			if (card.category != CardType.Element) {
				continue;
			} 
			else {
				if (card.name == "Fire") {
					frequencies [Elemental.FIRE] += 1;
				}
				if (card.name == "Water") {
					frequencies [Elemental.WATER] += 1;
				}
				if (card.name == "Earth") {
					frequencies [Elemental.EARTH] += 1;
				}
				if (card.name == "Air") {
					frequencies [Elemental.WIND] += 1;
				}
			}

		}

		int max = -100;

		foreach (int a in frequencies.Values) {
			if (a > max) {
				max = a;
			}
		}

		int second_max = -100;

		foreach (int b in frequencies.Values) {
			if (b > second_max && b != max) {
				second_max = b;
			}
		}

		Elemental element_1 = Elemental.NONE;
		Elemental element_2 = Elemental.NONE;

		foreach (Elemental element in baseElements) {
			if (frequencies [element] == max) {
				if (element_1 == Elemental.NONE) {
					element_1 = element;
				}
				else {
					element_2 = element;
					break;
				}
			}
			else if(frequencies[element] == second_max){
				if(element_1 == Elemental.NONE){
					element_1 = element;
				}
				else{
					element_2 = element;
					break;
				}
			}

		}

		Elemental spell_element = getElementFromTwoElements(element_1, element_2);

		string spell_name = spell_element.ToString ().Substring (0, 1) + spell_element.ToString ().ToLower ().Substring (1);

		return new PseudoSpellCard(spell_name + " Spell", SpellType.DEBUFF, max, spell_element);
	}


	private static bool isWeakAgainstBases(Elemental ele1, Elemental ele2){
		if (ele1 == Elemental.FIRE) {
			return ele2 == Elemental.WATER;
		}
		if (ele1 == Elemental.WATER) {
			return ele2 == Elemental.WIND;
		}
		if (ele1 == Elemental.WIND) {
			return ele2 == Elemental.EARTH;
		}
		if (ele1 == Elemental.EARTH) {
			return ele2 == Elemental.FIRE;
		}
		if (ele1 == Elemental.DARK) {
			return false;
		}
		return false;
	}

	//Returns true is ele1 is weak against ele2
	public static bool isWeakAgainst(Elemental ele1, Elemental ele2){

		Debug.Log ("Weak check: " + ele1.ToString () + "  " + ele2.ToString ());

		List<Elemental> elemental_composition_1 = getElementalMakeup (ele1);
		List<Elemental> elemental_composition_2 = getElementalMakeup (ele2);



		return isWeakAgainstBases(elemental_composition_1[0], elemental_composition_2[0]) &&
			isWeakAgainstBases(elemental_composition_1[1], elemental_composition_2[1]);
	}

	private static bool isStrongAgainstBases(Elemental ele1, Elemental ele2){
		if (ele1 == Elemental.FIRE) {
			return ele2 == Elemental.EARTH;
		}
		if (ele1 == Elemental.WATER) {
			return ele2 == Elemental.FIRE;
		}
		if (ele1 == Elemental.WIND) {
			return ele2 == Elemental.WATER;
		}
		if (ele1 == Elemental.EARTH) {
			return ele2 == Elemental.WIND;
		}
		if (ele1 == Elemental.DARK) {
			return false;
		}
		return false;
	}

	public static bool isStrongAgainst(Elemental ele1, Elemental ele2){
		Debug.Log ("Strong check: " + ele1.ToString () + "  " + ele2.ToString ());


		List<Elemental> elemental_composition_1 = getElementalMakeup (ele1);
		List<Elemental> elemental_composition_2 = getElementalMakeup (ele2);



		return isStrongAgainstBases(elemental_composition_1[0], elemental_composition_2[0]) &&
			isStrongAgainstBases(elemental_composition_1[1], elemental_composition_2[1]);
	}
}