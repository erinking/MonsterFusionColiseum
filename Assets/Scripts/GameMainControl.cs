using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMainControl : MonoBehaviour {

    
    class Player{
	   public string name;
       public bool isCPU;
		public List<string> myCards;
        public Player(string name, bool isCPU){
			this.name = name;
            this.isCPU = isCPU;
			this.myCards = new List<string>();
        }
    }

    List<Player> playerList = new List<Player>(){};
	List<Player> playerOrder = new List<Player>(){};
	List<string> draftingDeck = new List<string>(){};
	Player currentPlayer; // Which player's turn is it?

	List<string> dealtCards = new List<string>(){};

	public GameObject cardPrefab;
	public GameObject dealtCardsDisplay;
	public GameObject playerPrefab;
	public GameObject playersDisplay;

	public int dealAmount = 2;
	public int dealAmountOffset = 1;

	public int NUMBER_OF_DRAFTING_ROUNDS = 6;
	public int DECK_SIZE = 60;

	public bool PICK_ONCE_PER_ROUND = true;
	public bool DEALT_CARDS_AMOUNT_CONSTANT = true;


	// Use this for initialization
	void Start () {
		StartCoroutine(GameLoop());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Lays out the sequence of the game.
	/// </summary>
    IEnumerator GameLoop()
    {
        yield return StartCoroutine(NewGameStarted());
        

        for (int round = 0; round < NUMBER_OF_DRAFTING_ROUNDS; round++)
        {
			yield return StartCoroutine(NewRound());
			if (round == 0)
			{
				// Players select their characters
				yield return StartCoroutine (InitialCharacterSelect ());
			}
			if (round == 1) 
			{
				// Players select their weapons in reverse order
				yield return StartCoroutine (InitialWeaponSelect ());
			} 
			else
			{
				yield return StartCoroutine (SetPickingOrder());
				yield return StartCoroutine (DealCards());
				yield return StartCoroutine (TakeTurnsPicking());
			}
        }
        yield return StartCoroutine(EndOfDrafting());
        yield return StartCoroutine(Battle());
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator NewGameStarted()
    {
		yield return StartCoroutine(DeterminePlayers());
		yield return StartCoroutine(CreateDraftingDeck());
        yield return null;

    }

	/// <summary>
	// Determine how many CPU/Human players
	/// </summary>
	IEnumerator DeterminePlayers()
	{
		playerList.Add(new Player ("Player 1", false));
		playerList.Add(new Player ("AI Opponent 1", true));
		// PlayerOrder is a list used to run the game turns and picking order.
		playerOrder = playerList;
		yield return StartCoroutine(UpdatePlayersDisplay());
		yield return null;
	}

	/// <summary>
	// Creates the deck of cards for the current game.
	/// </summary>
	IEnumerator CreateDraftingDeck()
	{
		for (int size = DECK_SIZE; size > 0 ; size--)
		{
			draftingDeck.Add(GenerateRandomCard());
		}
		yield return null;
	}

	/// <summary>
	/// Generates a random card. (Monster, Element, Weapon)
	/// </summary>
	string GenerateRandomCard()
	{
		return "Im a Card!" + Random.Range(0,100).ToString();
	}

	/// <summary>
	/// Upkeep for the start of a round.
	/// </summary>
	IEnumerator NewRound()
	{
		yield return null;
	}

    /// <summary>
    /// Pick your base character from some starting set.
    /// </summary>
    IEnumerator InitialCharacterSelect()
    {
		
		StartCoroutine(DealCards(characterSelect:true));

        yield return null;
    }

	/// <summary>
	/// Deals the current round's cards.
	/// </summary>
	IEnumerator DealCards(bool characterSelect = false, bool weaponSelect = false)
	{
		dealAmount = playerOrder.Count * 2;
		if (characterSelect) {
			//show only base characters
		} else if (weaponSelect) {
			//show only base weapons
		} else {
			dealtCards = draftingDeck.GetRange(0, dealAmount);
			draftingDeck.RemoveRange(0, dealAmount);
		}
		yield return StartCoroutine(UpdateCardsDisplay());
		yield return null;

	}

    /// <summary>
    /// 
    /// </summary>
    IEnumerator InitialWeaponSelect()
    {
		StartCoroutine(SetPickingOrder(reversed: true));
		// Select your weapon.

		// Reverse it back at the end.
		StartCoroutine(SetPickingOrder(reversed: true));
        yield return null;
    }

    /// <summary>
    /// Set who picks first and what order the other players pick.
    /// </summary>
	IEnumerator SetPickingOrder(bool reversed = false)
    {
		if (reversed) {
			playerOrder.Reverse ();
		} else {
			playerOrder.Add(playerOrder[0]);
			playerOrder.RemoveAt(0);
		}

		yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator TakeTurnsPicking()
    {
		foreach (Player player in playerOrder) 
		{
			currentPlayer = player;
			if (!player.isCPU) {
				yield return StartCoroutine(PlayerPicksCard(player));
			} else {
				yield return StartCoroutine(PickCardRandomly(player));
			}
		}
        yield return null;
    }

	/// <summary>
	/// Select a card in play for the current player at random.
	/// </summary>
	IEnumerator PlayerPicksCard(Player player)
	{
		yield return new WaitForSeconds(1.0f);
		// wait until player selects card.
		yield return null;
	}

	/// <summary>
	/// Select a card in play for the current player at random.
	/// </summary>
	IEnumerator PickCardRandomly(Player player)
	{
		int randomIndex = Random.Range (0, dealtCards.Count);
		string pickedCard = dealtCards[randomIndex];
		player.myCards.Add(pickedCard);
		dealtCards.RemoveAt(randomIndex);
		yield return null;
	}

    /// <summary>
    /// 
    /// </summary>
    IEnumerator EndOfDrafting()
    {
		foreach (Player player in playerList) {
			foreach (string card in player.myCards) {
				print (player.name + ":" + card);
			}
		}
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator Battle()
    {
        yield return null;
    }


	IEnumerator UpdateCardsDisplay()
	{
		foreach (Transform oldCard in dealtCardsDisplay.transform)
		{
			Destroy(oldCard.gameObject);
		}
		foreach (string card in dealtCards)
		{
			GameObject newCard = Instantiate(cardPrefab);
			newCard.GetComponentInChildren<Text> ().text = card;

			//newCard.GetComponentInChildren<Image>().color = Random.ColorHSV();

			newCard.transform.SetParent(dealtCardsDisplay.transform);
		}
		yield return null;
	}


	IEnumerator UpdatePlayersDisplay()
	{
		foreach (Player player in playerOrder)
		{
			GameObject newPlayer = Instantiate(playerPrefab);
			print(player);
			newPlayer.GetComponentInChildren<Text>().text = player.name;
			newPlayer.transform.SetParent(playersDisplay.transform);
		}
		yield return null;
	}

}
