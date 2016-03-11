using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameMainControl : MonoBehaviour {

    
    public class Player{
		public string name;
		public string myCharacter;
		public string myWeapon;
    	public bool isCPU;
		public List<string> myCards;
		public int STR;
		public int DEF;
		public int AGI;
		public int INT;
        public Player(string name, bool isCPU){
			this.name = name;
            this.isCPU = isCPU;
			this.myCards = new List<string>();
			this.STR = 0;
			this.DEF = 0;
			this.AGI = 0;
        }
    }

	private enum CardType {Monster, Weapon, Element};
	class Kard{
		public string name;
		public CardType category;
		public int STR;
		public int DEF;
		public int AGI;
		public int INT;
		public Kard(string name, CardType category, int STR, int DEF, int AGI){
			this.name = name;
			this.category = category;
			this.STR = STR;
			this.DEF = DEF;
			this.AGI = AGI;
		}
	}

	List<string> allBaseElementNames = new List<string>(){"fire","water","earth","air"};
	List<string> allBaseMonsterNames = new List<string>(){"skink","fairy","undead","blobfish","human"};
	List<string> allBaseWeaponNames = new List<string>(){"dagger","meatstick","staff","bow"};

	List<string> otherCards = new List<string>(){"+STR","+DEF","+AGI","+INT","++STR","++DEF","++AGI","++INT","+++STR","+++DEF","+++AGI","+++INT"};

    List<Player> playerList = new List<Player>(){};
	List<Player> playerOrder = new List<Player>(){};
	List<string> draftingDeck = new List<string>(){};
	Player currentPlayer; // Which player's turn is it?

	List<string> dealtCards = new List<string>(){};

	public GameObject cardPrefab;
	public GameObject playerPrefab;
	public GameObject dealtCardsDisplay;
	public GameObject playersDisplay;

	public int dealAmount = 2;
	public int dealAmountOffset = 1;

	public int NUMBER_OF_DRAFTING_ROUNDS = 10;
	public int DECK_SIZE = 160;

	public bool PICK_ONCE_PER_ROUND = true;
	public bool DEALT_CARDS_AMOUNT_CONSTANT = true;
	public bool acceptInput = false;
	public bool isCardSelected = false;

	public float CPU_THINK_DELAY = 1.5f;
	public float TIME_BETWEEN_TURNS = 0.5f;
	public float TIME_BETWEEN_ROUNDS = 1.0f;

	public Color CANT_INTERACT_COLOR = new Color (1.0f, 1.0f, 1.0f, 0.5f);
	public Color CAN_INTERACT_COLOR = new Color (0.6f, 0.6f, 0.6f, 0.1f);
	public Color ACTIVE_PLAYER_COLOR = new Color (0.1f, 0.9f, 0.2f, 0.8f);
	public Color WINNING_PLAYER_COLOR = new Color(0.8f, 0.9f, 0.8f, 0.8f);
	public Color LOSING_PLAYER_COLOR = new Color (0.9f, 0.1f, 0.2f, 0.8f);
	public float LOSER_ALPHA = 0.6f;

	public float TURN_FADE_IN_TIME = 0.2f;
	public float STATUS_FADE_TIME = 0.8f;
	public float DISCARD_DURATION = 0.5f;


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
				yield return StartCoroutine (InitialCharacterSelect());
			}
			else if (round == 1) 
			{
				// Players select their weapons in reverse order
				yield return StartCoroutine (InitialWeaponSelect());
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
		playerOrder = new List<Player>(playerList);
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
		return otherCards[Random.Range(0,otherCards.Count)];
		//return "Im a Card!" + Random.Range(0,100).ToString();
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
		yield return StartCoroutine(SetPickingOrder(simpleSet:true));
		yield return StartCoroutine(DealCards(characterSelect:true));
		yield return StartCoroutine(TakeTurnsPicking());
		yield return StartCoroutine(AddBackRemainingCards());
        yield return null;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator InitialWeaponSelect()
    {
		yield return StartCoroutine(SetPickingOrder(reversed: true));
		// Select your weapon.
		yield return StartCoroutine(DealCards(weaponSelect:true));
		yield return StartCoroutine (TakeTurnsPicking());
		yield return StartCoroutine(AddBackRemainingCards());
		// Reverse order back at the end.
		yield return StartCoroutine(SetPickingOrder(reversed: true));
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
			dealtCards = new List<string>(allBaseMonsterNames);
		} else if (weaponSelect) {
			//show only base weapons
			dealtCards = new List<string>(allBaseWeaponNames);
		} else {
			dealtCards = draftingDeck.GetRange(0, dealAmount);
			draftingDeck.RemoveRange(0, dealAmount);
		}
		yield return StartCoroutine(UpdateCardsDisplay(newRound:true));
		yield return null;

	}

	/// <summary>
	/// Adds the back the remaining dealt cards to the deck.
	/// </summary>
	IEnumerator AddBackRemainingCards()
	{
		draftingDeck.AddRange(dealtCards);
		dealtCards.Clear();
		yield return StartCoroutine(ShuffleDeck());
	}

    /// <summary>
    /// Set who picks first and what order the other players pick.
    /// </summary>
	IEnumerator SetPickingOrder(bool reversed = false, bool simpleSet = false)
    {
		//simpleSet sets only who is to pick first, does not change the playerOrder. Used for initialization.
		if (!simpleSet) {
			if (reversed) {
				playerOrder.Reverse ();
			} else {
				playerOrder.Add (playerOrder [0]);
				playerOrder.RemoveAt (0);
			}
		}
		//This prevents it from being null on the UpdateCardsDisplay method.
		currentPlayer = playerOrder [0]; 
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
			yield return StartCoroutine(UpdatePlayersDisplay());
			yield return StartCoroutine(UpdateCardsDisplay());
			if (!player.isCPU) {
				yield return StartCoroutine(PlayerPicksCard(player));
			} else {
				yield return new WaitForSeconds(CPU_THINK_DELAY);
				yield return StartCoroutine(PickCardRandomly(player));
			}

			currentPlayer = null;
			yield return StartCoroutine(EndTurn());
		}
        yield return null;
    }

	/// <summary>
	/// This waits until the player has a picked a card before advancing.
	/// </summary>
	IEnumerator PlayerPicksCard(Player player)
	{
		acceptInput = true;
		while (!isCardSelected) {
			// wait until player selects card.
			yield return null;
		}
		acceptInput = false;
		//stuff
		isCardSelected = false;
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
		StartCoroutine(UpdateCardsDisplay ());
		FusePick(pickedCard);
		yield return null;
	}

	/// <summary>
	/// Select a card in play for the current player. This is called from a card button.
	/// </summary>
	public void PickCard(string card)
	{
		if (acceptInput) {
			int cardIndex = dealtCards.IndexOf (card);
			print ("index: " + cardIndex + ", card: " + card +", thing in list there: "+dealtCards[cardIndex]);
			currentPlayer.myCards.Add (card);
			dealtCards.RemoveAt (cardIndex);
			StartCoroutine(UpdateCardsDisplay ());
			FusePick (card);
			isCardSelected = true;
		}
	}

	/// <summary>
	/// End of turn upkeep.
	/// </summary>
	IEnumerator EndTurn(){
		yield return StartCoroutine(UpdatePlayersDisplay());
		yield return new WaitForSeconds(TIME_BETWEEN_TURNS);
	}

    /// <summary>
    /// End of Drafting upkeep
    /// </summary>
    IEnumerator EndOfDrafting()
    {
		dealtCards.Clear();
		yield return StartCoroutine(UpdateCardsDisplay());
		foreach (Player player in playerList) {
			foreach (string card in player.myCards) {
				//print (player.name + ":" + card);
			}
		}
        yield return null;
    }

    /// <summary>
    /// After Drafting, the players battle each other.
    /// </summary>
    IEnumerator Battle()
    {
		Dictionary<Player,int> scores = new Dictionary<Player,int>();
		foreach (Player player in playerList){
			int score = 0; // Against how many other players do they win?
			foreach (Player opponent in playerList) {
				// Determine win condition here.
				if((player.STR > opponent.STR && player.DEF > opponent.DEF)||
					(player.STR > opponent.STR && player.AGI > opponent.AGI)||
					(player.DEF > opponent.DEF && player.AGI > opponent.AGI))
				{
					score++;
				}
			}
			scores [player] = score;
			print (player.name + " : " + score.ToString ());


        yield return null;
   		}
		// Determine winners, calculation required because there might be multiple winners. 
		List<string> winners = new List<string>();
		int currentBestScore = 0;
		foreach (Player player in scores.Keys) {
			if (scores [player] == currentBestScore) {
				winners.Add (player.name);
			} else if (scores [player] > currentBestScore) {
				winners.Clear ();
				winners.Add(player.name);
				currentBestScore = scores [player];
			}
		}

		foreach (Transform player in playersDisplay.transform){
			//Visually display winners.
			if (winners.Contains (player.gameObject.name)) {
				yield return StartCoroutine (DisplayWinner (player.gameObject.name));
			} else {
				//They are a loser. Sorry.
				yield return StartCoroutine (DisplayLoser (player.gameObject.name));
			}
		}
	}

	/// <summary>
	/// Once a card is chosen, it must be integrated into the player's character/weapon/stats.
	/// </summary>
	/// <param name="card">Card.</param>
	void FusePick(string card){
		string statType = card.TrimStart(new char[]{'+'});
		int statValue = card.LastIndexOf ("+") + 1;
		switch (statType) {
		case"STR":
			currentPlayer.STR += statValue;
			break;
		case"DEF":
			currentPlayer.DEF += statValue;
			break;
		case"AGI":
			currentPlayer.AGI += statValue;
			break;
		case"INT":
			currentPlayer.INT += statValue;
			break;
		}

	}

	/// <summary>
	/// Adds the back the remaining dealt cards to the deck.
	/// </summary>
	IEnumerator ShuffleDeck()
	{
		string tempSwapHolder = "";
		for (int i = 0; i < draftingDeck.Count; i++) {
			// Swap two cards
			tempSwapHolder = draftingDeck[i];
			int randomIndex = Random.Range(0, draftingDeck.Count);
			string randomPick = draftingDeck[randomIndex];
			draftingDeck[i] = randomPick;
			draftingDeck[randomIndex] = tempSwapHolder;
		}
		yield return null;
	}

	/// <summary>
	/// Updates the cards displayed.
	/// </summary>
	IEnumerator UpdateCardsDisplay(bool newRound = false)
	{
		if (newRound) {
			//fade out all current cards.
			yield return StartCoroutine (FadeGroupOverTime (dealtCardsDisplay, 0.0f, TURN_FADE_IN_TIME));
		} else {
			//show cards with no delay.
			yield return StartCoroutine (FadeGroupOverTime (dealtCardsDisplay, 1.0f, 0.0f));
		}
		foreach (Transform oldCard in dealtCardsDisplay.transform)
		{
			oldCard.gameObject.SetActive (false);
			StartCoroutine(DiscardVisual(oldCard.gameObject));
		}
		for (int cardIndex = 0; cardIndex < dealtCards.Count; cardIndex++)
		{
			string card = dealtCards[cardIndex];
			GameObject newCard = Instantiate(cardPrefab);
			newCard.GetComponentInChildren<Text> ().text = card;
			newCard.name = card;
			newCard.transform.SetParent(dealtCardsDisplay.transform);
			if (currentPlayer.isCPU) {
				newCard.GetComponent<Button> ().interactable = false;
				if (newRound) {
					StartCoroutine (LerpOverTime (newCard, CANT_INTERACT_COLOR, TURN_FADE_IN_TIME));
				}
			} else {
				newCard.GetComponent<Button> ().interactable = true;
				if (newRound) {
					StartCoroutine (LerpOverTime (newCard, CAN_INTERACT_COLOR, TURN_FADE_IN_TIME));
				}
			}
		}
		yield return null;
	}

	/// <summary>
	/// Updates the players displayed.
	/// </summary>
	IEnumerator UpdatePlayersDisplay()
	{
		foreach (Transform oldPlayer in playersDisplay.transform)
		{
			Destroy(oldPlayer.gameObject);
		}
		foreach (Player player in playerList)
		{
			GameObject newPlayer = Instantiate(playerPrefab);
			newPlayer.name = player.name;
			newPlayer.GetComponentInChildren<Text>().text = player.name;
			newPlayer.transform.SetParent(playersDisplay.transform);
			newPlayer.transform.FindChild ("STR/Level").GetComponent<Text> ().text = player.STR.ToString ();
			newPlayer.transform.FindChild ("DEF/Level").GetComponent<Text> ().text = player.DEF.ToString ();
			newPlayer.transform.FindChild ("AGI/Level").GetComponent<Text> ().text = player.AGI.ToString ();
			newPlayer.transform.FindChild ("INT/Level").GetComponent<Text> ().text = player.INT.ToString ();
			try{
			newPlayer.transform.FindChild ("Character/Name").GetComponent<Text> ().text = player.myCards[0];
			newPlayer.transform.FindChild ("Weapon/Name").GetComponent<Text> ().text = player.myCards[1];
			}catch{

			}
			if (player == currentPlayer) {
				StartCoroutine(LerpOverTime(newPlayer, ACTIVE_PLAYER_COLOR, TURN_FADE_IN_TIME));
			}
		}
		yield return null;
	}

	IEnumerator DiscardVisual(GameObject cardVisual){
		if (cardVisual.activeInHierarchy) {
			yield return StartCoroutine (FadeGroupOverTime(cardVisual, 0, DISCARD_DURATION));
		}
		Destroy (cardVisual);
		yield return null;
	}

	IEnumerator DisplayWinner(string playerName){
		GameObject winningPlayer = playersDisplay.transform.FindChild(playerName).gameObject;
		StartCoroutine (LerpOverTime (winningPlayer, WINNING_PLAYER_COLOR, STATUS_FADE_TIME));
		yield return null;
	}

	IEnumerator DisplayLoser(string playerName){
		GameObject losingPlayer = playersDisplay.transform.FindChild(playerName).gameObject;
		StartCoroutine (LerpOverTime (losingPlayer, LOSING_PLAYER_COLOR, STATUS_FADE_TIME));
		losingPlayer.GetComponent<CanvasGroup> ().alpha = LOSER_ALPHA;
		yield return null;
	}

	IEnumerator LerpOverTime(GameObject UIThing, Color finalColor, float duration){
		
		float initialTime = Time.time;
		Image myImage = UIThing.GetComponent<Image>();
		while (myImage.color != finalColor || (Time.time - initialTime) < duration) {
			myImage.color = Color.Lerp (myImage.color, finalColor, (Time.time - initialTime) / duration);
			yield return null;
		}

		myImage.color = finalColor;
	}

	IEnumerator FadeGroupOverTime(GameObject UIGroup, float finalAlpha, float duration){

		float initialTime = Time.time;
		CanvasGroup myGroup = UIGroup.GetComponent<CanvasGroup>();
		if (duration > 0) {
			while (myGroup.alpha != finalAlpha || (Time.time - initialTime) < duration) {
				myGroup.alpha = Mathf.Lerp (myGroup.alpha, finalAlpha, (Time.time - initialTime) / duration);
				yield return null;
			}
		}
		myGroup.alpha = finalAlpha;
	}
}
