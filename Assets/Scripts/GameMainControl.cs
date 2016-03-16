using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMainControl : MonoBehaviour {
	
	List<string> allBaseElementNames = new List<string>(){"Fire","Water","Earth","Air"};
	
	List<string> allMonsterNames = new List<string>(){
		"Sprite", "Blobfish", "Giant", "Gecko", "Ghost",
		"Fairy Queen", "Nymph", "Spriggan", "Djinn", "Spectre",
		"Flying Fish", "Crocodilus", "Kraken", "Naga", "Anglerfish",
		"Elf", "Merp", "Golem", "Pyromancer", "Zombie",
		"Drake", "Mosasaur", "Crag Dragon", "Lava Dragon", "Bone Dragon",
		"Imp", "Depth Horror", "Skeleton", "Demon", "Night Lord"
		};
	List<string> allBaseMonsterNames = new List<string>(){
		"Sprite", "Blobfish", "Giant", "Gecko", "Ghost"
	};
	
	List<List<float>> allMonsterStats = new List<List<float>>{
		new List<float> {50,4,3,6,7}, new List<float> {55,5,4,7,3}, new List<float> {45,3,7,5,6}, new List<float>{40,7,6,4,5}, new List<float>{60,6,5,3,4},
		new List<float>{60,5,1,7,10}, new List<float>{60,5,3,9,6}, new List<float>{50,3,6,7,9}, new List<float>{45,7,5,6,8}, new List<float>{65,6,4,5,7},
		new List<float>{60,5,3,9,6}, new List<float>{70,5,5,10,1}, new List<float>{55,4,7,8,5}, new List<float>{50,7,6,7,5}, new List<float>{65,7,5,7,3},
		new List<float>{50,3,6,7,9}, new List<float>{55,4,7,8,5}, new List<float>{60,1,10,5,7}, new List<float>{50,4,9,4,8}, new List<float>{55,5,9,4,6},
		new List<float>{45,8,6,5,7}, new List<float>{50,9,6,7,3}, new List<float>{50,6,8,4,7}, new List<float>{35,10,9,3,6}, new List<float>{50,9,7,4,5},
		new List<float>{55,6,4,5,7}, new List<float>{70,7,5,6,3}, new List<float>{60,5,8,4,6}, new List<float>{55,9,7,3,5}, new List<float>{75,8,7,1,4}
	};
	
	List<string> allBaseWeaponNames = new List<string>(){"dagger","meatstick","staff","bow"};
	
	// List<string> otherCardNames = new List<string>(){"+STR","+DEF","+AGI","+INT","++STR","++DEF","++AGI","++INT","+++STR","+++DEF","+++AGI","+++INT"};
	List<CardType> randomGenerationRatioList = new List<CardType>(){CardType.Monster,CardType.Weapon,CardType.Element};
	public int amountOfMonstersInGenerationList = 1;
	public int amountOfWeaponsInGenerationList = 1;
	public int amountOfElementsInGenerationList = 1;

	public Sprite[] monsterImageList = new Sprite[]{};
	public Sprite[] weaponImageList = new Sprite[]{};
	public Sprite[] elementImageList = new Sprite[]{};

	public Sprite[] backdropImageList = new Sprite[]{};

    List<Player> playerList = new List<Player>(){};
	List<Player> playerOrder = new List<Player>(){};
	List<Kard> draftingDeck = new List<Kard>(){};
	Player currentPlayer; // Which player's turn is it?

	List<Kard> dealtCards = new List<Kard>(){};
	
	/*Creating a deck of all monsters in the game using the base constructors defined in Kard.cs 
	Shawn's code */
	List<Kard> monsterDeck = new List<Kard>(){}; //name, category, image, stats (HP,STR,DEF,AGI,INT)
	/* */

	public GameObject cardPrefab;
	public GameObject playerPrefab;
	public GameObject dealtCardsDisplay;
	public GameObject playersDisplay;

	public Canvas battleSimCanvas;
	private BattleLogic battleLogicScript = null;

	public GameObject header;
	public GameObject replayButton;

	public AudioSource mainAudioSource;
	public AudioClip cardClickSound;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame(){
		StartCoroutine(GameLoop());
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
		yield return StartCoroutine(EndOfGame());
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
		playerList.Clear ();
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
		draftingDeck.Clear ();
		InitializeRandomGenerationRatioList();
		for (int size = DECK_SIZE; size > 0 ; size--)
		{
			draftingDeck.Add(GenerateRandomCard());
		}
		yield return null;
	}

	/// <summary>
	/// Generates a random card. (Monster, Element, Weapon)
	/// </summary>
	Kard GenerateRandomCard()
	{
		string cardName = "---DEFAULT---";
		CardType category;
		Sprite image = null;
		float HP;
		float STR;
		float DEF;
		float AGI;
		float INT;



		int typeSelectorIndex = Random.Range(0,randomGenerationRatioList.Count);
		CardType typeSelector = randomGenerationRatioList [typeSelectorIndex];
		switch (typeSelector) {
		case CardType.Monster:
			cardName = allBaseMonsterNames [Random.Range (0, allBaseMonsterNames.Count)];
			image = monsterImageList [Random.Range (0, monsterImageList.Length)];
			break;
		case CardType.Element:
			cardName = allBaseElementNames [Random.Range (0, allBaseElementNames.Count)];
			image = elementImageList [Random.Range (0, elementImageList.Length)];
			break;
		case CardType.Weapon:
			cardName = allBaseWeaponNames [Random.Range (0, allBaseWeaponNames.Count)];
			image = weaponImageList [Random.Range (0, weaponImageList.Length)];
			break;
		}
			HP = Random.Range (25, 100);
			STR = Random.Range (0, 3);
			DEF = Random.Range (0, 3);
			AGI = Random.Range (0, 3);
			INT = Random.Range (0, 3);

		Kard createdCard = new Kard (cardName,typeSelector,image,HP,STR,DEF,AGI,INT);
		return createdCard;
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
			List<Kard> baseMonsterCards = new List<Kard>();
			foreach(string name in allBaseMonsterNames){ baseMonsterCards.Add (new Kard (name, CardType.Monster, monsterImageList[Random.Range(0,monsterImageList.Length)]));}
			dealtCards = new List<Kard>(baseMonsterCards);
		} else if (weaponSelect) {
			//show only base weapons
			List<Kard> baseWeaponCards = new List<Kard>();
			foreach(string name in allBaseWeaponNames){ baseWeaponCards.Add (new Kard (name, CardType.Weapon, weaponImageList[Random.Range(0,weaponImageList.Length)]));}
			dealtCards = new List<Kard>(baseWeaponCards);
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
		Kard pickedCard = dealtCards[randomIndex];
		player.myCards.Add(pickedCard);
		dealtCards.RemoveAt(randomIndex);
		StartCoroutine(UpdateCardsDisplay ());
		FusePick(pickedCard);
		yield return null;
	}

	/// <summary>
	/// Select a card in play for the current player. This is called from a card button.
	/// </summary>
	public void PickCard(string cardName)
	{
		if (acceptInput) {
			Kard card = dealtCards.Find(p => p.name == cardName);
			int cardIndex = dealtCards.IndexOf(card);
			currentPlayer.myCards.Add(card);
			dealtCards.RemoveAt (cardIndex);
			StartCoroutine(UpdateCardsDisplay ());
			mainAudioSource.PlayOneShot(cardClickSound);
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
			foreach (Kard card in player.myCards) {
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
		// Get battle script from canvas object.
		battleLogicScript = battleSimCanvas.GetComponent<BattleLogic>();

		Dictionary<Player,int> scores = new Dictionary<Player,int>();
		foreach (Player player in playerList){
			int score = 0; // Against how many other players do they win?
			foreach (Player opponent in playerList) {
				if (opponent == player) {
					//Debug.Log ("Skipping a bad time.");
					continue;
				}

				// Determine win condition here.
				//Debug.Log("Starting battle");
				bool playerOneWins;
				if (battleLogicScript != null) {
					//Debug.Log ("Script is not null!");
					battleSimCanvas.gameObject.SetActive(true);
					yield return StartCoroutine (battleLogicScript.StartBattleFromMain (player, opponent));
					//Debug.Log ("Made it out");
					playerOneWins = battleLogicScript.getWinner ();
					battleSimCanvas.gameObject.SetActive(false);
				}else{
					playerOneWins = (player.STR > opponent.STR && player.DEF > opponent.DEF) ||
					(player.STR > opponent.STR && player.AGI > opponent.AGI) ||
					(player.DEF > opponent.DEF && player.AGI > opponent.AGI);
				}
				if(playerOneWins)
				{
					score++;
				}
				//Debug.Log ("Finished battle");
			}
			//Debug.Log
			scores [player] = score;
			print (player.name + " : " + score.ToString ());


        yield return null;
   		}
		//Debug.Log("FINISHED ALL THE BATTLES!!!!");
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
	void FusePick(Kard card){
		switch (card.category) {
		case CardType.Monster:
			if (currentPlayer.myCharacter != null) {
				currentPlayer.myCharacter = CardFusion (currentPlayer.myCharacter, card);
			} else {
				currentPlayer.myCharacter = card;
			}
			break;

		case CardType.Weapon:
			if (currentPlayer.myWeapon != null) {
				currentPlayer.myWeapon = CardFusion (currentPlayer.myWeapon, card);
			} else {
				currentPlayer.myWeapon = card;
			}
			break;

		case CardType.Element:
			if (currentPlayer.myElement != null) {
				currentPlayer.myElement = CardFusion (currentPlayer.myElement, card);
			} else {
				currentPlayer.myElement = card;
			}
			break;
		}
		SetNewPlayerStats(card);
	}


	Kard CardFusion(Kard card1, Kard card2){
		Debug.Log (card1);
		Debug.Log (card2);
		string fusedName = card1.name +"-"+ card2.name;
		CardType fusedCategory = card1.category;
		Sprite fusedImage = card1.image;
		float fusedHP = card1.HP + card2.HP;
		float fusedSTR = card1.STR + card2.STR;
		float fusedDEF = card1.DEF + card2.DEF;
		float fusedAGI = card1.AGI + card2.AGI;
		float fusedINT = card1.INT + card2.INT;

		Kard fusionCard = new Kard (fusedName, fusedCategory, fusedImage, fusedHP, fusedSTR, fusedDEF, fusedAGI, fusedINT);
		return fusionCard;
	}

	void SetNewPlayerStats(Kard card){
		currentPlayer.HP += card.HP;
		currentPlayer.STR += card.STR;
		currentPlayer.DEF += card.DEF;
		currentPlayer.AGI += card.AGI;
		currentPlayer.INT += card.INT;
	}


	/// <summary>
	/// Adds the back the remaining dealt cards to the deck.
	/// </summary>
	IEnumerator ShuffleDeck()
	{
		Kard tempSwapHolder = null;
		for (int i = 0; i < draftingDeck.Count; i++) {
			// Swap two cards
			tempSwapHolder = draftingDeck[i];
			int randomIndex = Random.Range(0, draftingDeck.Count);
			Kard randomPick = draftingDeck[randomIndex];
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
			if (newRound) {
				StartCoroutine (DiscardVisual (oldCard.gameObject));
			}
		}
		for (int cardIndex = 0; cardIndex < dealtCards.Count; cardIndex++)
		{
			Kard card = dealtCards[cardIndex];
			GameObject newCard = Instantiate(cardPrefab);
			newCard.GetComponentInChildren<Text> ().text = card.name;
			newCard.name = card.name;
			newCard.transform.FindChild ("Image").GetComponent<Image>().sprite = card.image;
			newCard.GetComponent<Image>().sprite = backdropImageList[(int)card.category];
			newCard.transform.SetParent(dealtCardsDisplay.transform);
			if (currentPlayer.isCPU) {
				newCard.GetComponent<Button> ().interactable = false;
				if (newRound) {
					StartCoroutine (FadeGroupOverTime (newCard, CANT_INTERACT_COLOR.a, TURN_FADE_IN_TIME));
				}
			} else {
				newCard.GetComponent<Button> ().interactable = true;
				if (newRound) {
					StartCoroutine (FadeGroupOverTime (newCard, CAN_INTERACT_COLOR.a, TURN_FADE_IN_TIME));
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
			newPlayer.transform.FindChild ("HP/Amount").GetComponent<Text> ().text = player.HP.ToString ();
			newPlayer.transform.FindChild ("STR/Level").GetComponent<Text> ().text = player.STR.ToString ();
			newPlayer.transform.FindChild ("DEF/Level").GetComponent<Text> ().text = player.DEF.ToString ();
			newPlayer.transform.FindChild ("AGI/Level").GetComponent<Text> ().text = player.AGI.ToString ();
			newPlayer.transform.FindChild ("INT/Level").GetComponent<Text> ().text = player.INT.ToString ();
			try{
				//Order is important here.
				//first a character is chosen.
				newPlayer.transform.FindChild ("Character/Name").GetComponent<Text> ().text = player.myCharacter.name;
				newPlayer.transform.FindChild ("Character/Image").GetComponent<Image> ().sprite = player.myCharacter.image;
				//then a weapon is chosen.
				newPlayer.transform.FindChild ("Weapon/Name").GetComponent<Text> ().text = player.myWeapon.name;
				newPlayer.transform.FindChild ("Weapon/Image").GetComponent<Image> ().sprite = player.myWeapon.image;
				//then an element.
				newPlayer.transform.FindChild ("Character").GetComponent<Image> ().sprite = player.myElement.image;
			}catch{

			}
			if (player == currentPlayer) {
				StartCoroutine(LerpOverTime(newPlayer, ACTIVE_PLAYER_COLOR, TURN_FADE_IN_TIME));
			}
		}
		//Header visuals.
		if (currentPlayer != null) {
			header.transform.FindChild ("Header").GetComponent<Text> ().text = currentPlayer.name + "\'s turn";
			header.GetComponent<Image> ().color = Color.clear;
			if (!currentPlayer.isCPU) {
				StartCoroutine (LerpOverTime (header, ACTIVE_PLAYER_COLOR, TURN_FADE_IN_TIME));
			}
		}
		yield return null;
	}

	IEnumerator DiscardVisual(GameObject cardVisual){
		if (cardVisual.activeInHierarchy) {
			yield return StartCoroutine (LerpOverTime(cardVisual, Color.clear, DISCARD_DURATION));
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

	void InitializeRandomGenerationRatioList(){
		randomGenerationRatioList.Clear ();
		for (int i = 0; i < amountOfMonstersInGenerationList; i++) {
			randomGenerationRatioList.Add(CardType.Monster);
		}
		for (int i = 0; i < amountOfWeaponsInGenerationList; i++) {
			randomGenerationRatioList.Add(CardType.Weapon);
		}
		for (int i = 0; i < amountOfElementsInGenerationList; i++) {
			randomGenerationRatioList.Add(CardType.Element);
		}
	}

	IEnumerator EndOfGame(){
		replayButton.SetActive (true);
		yield return null;
	}
}
