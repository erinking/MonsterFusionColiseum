using UnityEngine;
using System.Collections;


public enum Elemental
{
	FIRE, WATER, EARTH, WIND, DARK, NONE
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

	//Returns true is ele1 is weak against ele2
	public static bool isWeakAgainst(Elemental ele1, Elemental ele2){
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

	public static bool isStrongAgainst(Elemental ele1, Elemental ele2){
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
}