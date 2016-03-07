using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMainControl : MonoBehaviour {

    
    class Player{
       bool isCPU;
        public Player(bool isCPU){
            isCPU = isCPU;
        }
    }

    List<Player> playerList = new List<Player>(){};

	public int NUMBER_OF_DRAFTING_ROUNDS = 6;









	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(NewGameStarted());
        // Players select their characters
        yield return StartCoroutine(InitialCharacterSelect());
        // Players select their weapons in reverse order
        yield return StartCoroutine(InitialWeaponSelect());

        for (int round = 0; round < NUMBER_OF_DRAFTING_ROUNDS; round++)
        {
            yield return StartCoroutine(SetWhoPicksFirst());
            yield return StartCoroutine(TakeTurnsPicking());
        }
        yield return StartCoroutine(EndOfDrafting());
        yield return StartCoroutine(Battle());
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator NewGameStarted()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator InitialCharacterSelect()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator InitialWeaponSelect()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator SetWhoPicksFirst()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator TakeTurnsPicking()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator EndOfDrafting()
    {
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator Battle()
    {
        yield return null;
    }



}
