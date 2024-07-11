using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

public class ButtonTurn : MonoBehaviour
{

    private static bool isPlayer1Turn = true;
    public TMP_Text turnText;
    private Coroutine turnCoroutine;

    public HealthBar turnBar;
    private int turnCount;
    private int turnCount2;
    // private Coroutine healthtimerCoroutine;

    public Camera mainCamera; // Reference to your main camera

    private float originalOrthographicSize;
    Vector3 originalCamPos;

    public Button deckP1;
    public Button deckP2;

    public GameManager gmm;
    public BoardSlot boardSlot;

    public GridLayoutGroup handGrid;
    GameObject selectedCard;

    List<Transform> availableSlots;
    Transform randomSlot;

    public GridLayoutGroup boardGrid;
    GameObject selectedCARD;

    List<Transform> availableSlotsMove;
  //  Transform randomSlotMove;

    public List<GameObject> CardPlacedToBoard = new List<GameObject>();

    public CCardShuffler CShufflerP2;
    public CardShuffler CShufflerP1;

    List<Transform> adjacentSlotsP1;
    Transform randomAdjSlotP1;

    public List<GameObject> AiAttackCards = new List<GameObject>();
    GameObject randomAiAtcCard;

    GameObject defenseCard;
    public Dice dice;

    public List<DisplayCard> allDisplayCards; // Reference to all DisplayCard instances
    public List<DisplayCard2> allDisplayCardsP2; // Reference to all DisplayCard instances
    //  Scene currentScene;
    
    public GameToggleManager gameToggleManager;

    public string Movtag = "Player2";
    GameObject[] player2;

    public List<GameObject> adjacentp2List;
    Transform[] BSlots;

    public List<Transform> adjacentBSlots;
    Transform BoardSlt;

    private void Start()
    {
        boardSlot = FindAnyObjectByType<BoardSlot>();
        availableSlots = boardSlot.Available();
        //availableSlotsMove = boardSlot.MoveAvailable();
        adjacentSlotsP1 = boardSlot.AdjacentP1Available();
        boardSlot.AnotherMethod();
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());
        allDisplayCardsP2 = new List<DisplayCard2>(FindObjectsOfType<DisplayCard2>());

        turnCoroutine = StartCoroutine(ChangeTurn(60.0f));
        TurnStarter(isPlayer1Turn);
        // currentScene = SceneManager.GetActiveScene();
        player2 = GameObject.FindGameObjectsWithTag(Movtag);
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("BSlot");
        BSlots = new Transform[gameObjectsWithTag.Length];
        for (int i = 0; i < gameObjectsWithTag.Length; i++)
        {
            BSlots[i] = gameObjectsWithTag[i].transform;
        }
        //BSlots =;

        if (mainCamera == null)
        {
            // If the mainCamera reference is not set, try to find the main camera in the scene
            mainCamera = Camera.main;
        }

        // Store the original orthographic size for resetting
        if (mainCamera != null)
        {
            originalOrthographicSize = mainCamera.orthographicSize;
            originalCamPos = mainCamera.transform.position;
        }
        
    }

    private void Update()
    {
        if (selectedCard.transform.parent.name == "DiscardPile") 
        {
            CardPlacedToBoard.Remove(selectedCard);
        }
     //   Debug.Log("Adjacent cards P2:"+boardSlot.MoveAvailable().Count);
        Debug.Log("Adjacent cards P1:"+boardSlot.AdjacentP1Available().Count);
    }

    public IEnumerator PlaceToBoard(float delayed)
    {
        yield return new WaitForSeconds(delayed);
        SelectedRandomSetupSlot();
        SelectRandomCard();
        //selectedCard.transform.position = randomSlot.position;
        //CardShufflerP2.remove(selectedCard);
       // CShufflerP2.handList.Remove(selectedCard.gameObject);

        selectedCard.transform.SetParent(randomSlot);
        selectedCard.transform.localPosition = Vector3.zero;
        Image carddBackImage = selectedCard.transform.Find("Back").GetComponent<Image>();
        carddBackImage.enabled = false;
        boardSlot.GetPlacementSound();  //hand.OrderByDescending(card => card.AttackPower + card.DefensePower).First();

        // boardSlot.UpdateMoveListP2();
        CardPlacedToBoard.Add(selectedCard); // FINE TILL HERE   "CARD PLACED FROM HAND TO BOARD HAVE BEEN ADDED TO LIST"
        Debug.Log("Card in new list:"+CardPlacedToBoard.Count);
        int cardE = selectedCard.GetComponent<DisplayCard2>().GetCardEnergy();
        int CoinE =  BoardSlot.GetCurrentEnergyP2();
        int newCoinE = CoinE - cardE;
        BoardSlot.SetCurrentEnergyP2(newCoinE);
        Debug.Log("Card Energy:" + cardE);
        Debug.Log("Old CEnergy:" + CoinE);
        Debug.Log("New CEnergy:"+newCoinE);
        if (newCoinE == 7) 
        {
            boardSlot.coinP2img8.SetActive(false);
        }
        if (newCoinE == 6)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
        }
        if (newCoinE == 5)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
        }
        if (newCoinE == 4)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
        }
        if (newCoinE == 3)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
            boardSlot.coinP2img4.SetActive(false);
        }
        if (newCoinE == 2)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
            boardSlot.coinP2img4.SetActive(false);
            boardSlot.coinP2img3.SetActive(false);
        }
        if (newCoinE == 1)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
            boardSlot.coinP2img4.SetActive(false);
            boardSlot.coinP2img3.SetActive(false);
            boardSlot.coinP2img2.SetActive(false);
        }
        if (newCoinE == 0)
        {
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
            boardSlot.coinP2img4.SetActive(false);
            boardSlot.coinP2img3.SetActive(false);
            boardSlot.coinP2img2.SetActive(false);
            boardSlot.coinP2img.SetActive(false);
        } 
        if (newCoinE == -1)
        {
            boardSlot.energyTextP2.text = "0";
            boardSlot.coinP2img8.SetActive(false);
            boardSlot.coinP2img7.SetActive(false);
            boardSlot.coinP2img6.SetActive(false);
            boardSlot.coinP2img5.SetActive(false);
            boardSlot.coinP2img4.SetActive(false);
            boardSlot.coinP2img3.SetActive(false);
            boardSlot.coinP2img2.SetActive(false);
            boardSlot.coinP2img.SetActive(false);
        }
        //if newCoinE ==7 { coinP2img8.SetActive(false); }
        // boardSlot.UpdateMoveListP2();
    }

    public IEnumerator MoveInBoard(float del) 
    {
        yield return new WaitForSeconds(del);
       // SelectRandomMoveSlot();
        //SelectCardToMove();
        GetAdjacentBslotCards();
        GetRandomMoveBSlot();
        DisplayCard2 dpcrd2 = selectedCARD.GetComponent<DisplayCard2>();
        if (dpcrd2.canMove) //selectedCARD.canMove == true
        {
            Debug.Log("This is th card been selected to Move:" + selectedCARD.name);
            Debug.Log("This is th Board SLot the card will move to:" + BoardSlt.name);
            selectedCARD.transform.SetParent(BoardSlt);
            selectedCARD.transform.localPosition = Vector3.zero;
            selectedCARD.GetComponent<DisplayCard2>().canMove = false;

            // SelectRandomMoveSlot();                            // MOVE IN BOARD
            // selectedCARD.transform.SetParent(randomSlotMove);  // MOVE IN BOARD
            //  selectedCARD.transform.localPosition = Vector3.zero;
            //  boardSlot.GetPlacementSound();
            // selectedCARD.GetComponent<DisplayCard2>().outerBorder.color = Color.white;
            int coinEnergy = BoardSlot.GetCurrentEnergyP2();
            int newCE = coinEnergy - 1;
            BoardSlot.SetCurrentEnergyP2(newCE);
            if (newCE == 7)
            {
                boardSlot.coinP2img8.SetActive(false);
            }
            if (newCE == 6)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
            }
            if (newCE == 5)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
            }
            if (newCE == 4)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
            }
            if (newCE == 3)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
                boardSlot.coinP2img4.SetActive(false);
            }
            if (newCE == 2)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
                boardSlot.coinP2img4.SetActive(false);
                boardSlot.coinP2img3.SetActive(false);
            }
            if (newCE == 1)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
                boardSlot.coinP2img4.SetActive(false);
                boardSlot.coinP2img3.SetActive(false);
                boardSlot.coinP2img2.SetActive(false);
            }
            if (newCE == 0)
            {
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
                boardSlot.coinP2img4.SetActive(false);
                boardSlot.coinP2img3.SetActive(false);
                boardSlot.coinP2img2.SetActive(false);
                boardSlot.coinP2img.SetActive(false);
            }
            if (newCE == -1)
            {
                boardSlot.energyTextP2.text = "0";
                boardSlot.coinP2img8.SetActive(false);
                boardSlot.coinP2img7.SetActive(false);
                boardSlot.coinP2img6.SetActive(false);
                boardSlot.coinP2img5.SetActive(false);
                boardSlot.coinP2img4.SetActive(false);
                boardSlot.coinP2img3.SetActive(false);
                boardSlot.coinP2img2.SetActive(false);
                boardSlot.coinP2img.SetActive(false);
            }
            //SelectAttackCard();
            dpcrd2.canMove = false;
        }
    }

    public void SelectedRandomSetupSlot() //PICKING SETUP SLOT
    {
        // Check if any slot has no child
        bool hasEmptySlot = false;
        foreach (var slot in availableSlots)
        {
            if (slot.childCount == 0)
            {
                hasEmptySlot = true;
                break;
            }
        }

        // If there are no slots without children, exit the function
        if (!hasEmptySlot)
        {
            Debug.Log("All slots have children. Exiting function.");
            return;
        }

        // List to store indices of available slots without children
        List<int> emptyIndices = new List<int>();    //CONTINUE NEXT WORK FROM HERE
        for (int i = 0; i < availableSlots.Count; i++)
        {
            if (availableSlots[i].childCount == 0)
            {
                emptyIndices.Add(i);
            }
        }

        // Select a random empty slot
        int randomIndex = emptyIndices[Random.Range(0, emptyIndices.Count)];
        randomSlot = availableSlots[randomIndex];

        Debug.Log("Random Slot from BSlot:" + randomSlot);
    }

    public void SelectRandomMoveSlot() //Movement of card on board MovePhase
    {
     /*   do 
        {
            randomSlotMove = availableSlotsMove[Random.Range(0, availableSlotsMove.Count)];    //APPLY SAME LOGIC FOR CARD SELECTION
        } while (randomSlotMove.childCount > 0);
        Debug.Log("Move Phase BoardSlot:"+randomSlotMove);
     */
    }

    

    public void SelectRandomCard()
    {
        int cardCount = handGrid.transform.childCount;
        bool attackingCard = false;
        //movableCards.OrderByDescending(card => card.GetComponent<DisplayCard2>().GetCardAttack() + GetComponent<DisplayCard2>().GetCardDefense()).First();

        if (cardCount > 0)
        {
            if (gameToggleManager.EasyToggle.isOn)
            {
                // Generate a random index within the range of cardCount
                int randomIndex = Random.Range(0, cardCount);  //hand.OrderByDescending(card => card.AttackPower + card.DefensePower).First();

                // Get the selected card object
                selectedCard = handGrid.transform.GetChild(randomIndex).gameObject;
                Debug.Log("Selected Card from Hand:" + selectedCard);
                // Now you have the selected card, you can perform actions on it
                // For example, you can disable the card, remove it from the hand, etc.
            }
            else if (gameToggleManager.MediumToggle.isOn)
            {
                Transform childWithMostEnergy = null;
                float maxEnergy = float.MinValue;

                foreach (Transform h in handGrid.transform)
                {
                    DisplayCard2 dp = h.GetComponent<DisplayCard2>();
                    if (dp != null)
                    {
                        if ((dp.GetCardAttack() + dp.GetCardHealth()) > maxEnergy)
                        {
                            maxEnergy = dp.GetCardAttack() + dp.GetCardHealth();
                            childWithMostEnergy = h;
                        }
                    }
                }
                if (childWithMostEnergy != null)
                {
                    selectedCard = childWithMostEnergy.gameObject;
                }
                // selectedCard = handGrid.transform.GetChild(0).gameObject;
            }
            else if (gameToggleManager.HardToggle.isOn) 
            {
                Transform childWithMostAttacking = null;
                float maxAttack = float.MinValue;

                attackingCard = !attackingCard;
                if (attackingCard)
                {
                    foreach (Transform h in handGrid.transform) 
                    {
                        DisplayCard2 dp = h.GetComponent<DisplayCard2>();
                        if (dp != null)
                        {
                            if (dp.GetCardAttack() > maxAttack)
                            {
                                maxAttack = dp.GetCardAttack();
                                childWithMostAttacking = h;
                            }
                        }
                    }
                    if (childWithMostAttacking != null)
                    {
                        selectedCard = childWithMostAttacking.gameObject;
                    }
                }
                else 
                {
                    foreach (Transform h in handGrid.transform)
                    {
                        DisplayCard2 dp = h.GetComponent<DisplayCard2>();
                        if (dp != null)
                        {
                            if (dp.GetCardHealth() > maxAttack)
                            {
                                maxAttack = dp.GetCardHealth();
                                childWithMostAttacking = h;
                            }
                        }
                    }
                    if (childWithMostAttacking != null)
                    {
                        selectedCard = childWithMostAttacking.gameObject;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No cards in the hand.");
        }
    }

    public void SelectCardToMove()  // SELECTED CARD(P2 CARD) FROM BOARD TO MOVE 
    {
        var movableCards = CardPlacedToBoard.Where(card => card.GetComponent<DisplayCard2>().canMove).ToList();
        //movableCards.OrderByDescending(card => card.GetComponent<DisplayCard2>().GetCardAttack() + GetComponent<DisplayCard2>().GetCardDefense()).First();
        if (movableCards.Count > 0)
        {  
            selectedCARD = movableCards[Random.Range(0, movableCards.Count)]; //movableCards[0]
        }
        else 
        {
            Debug.Log("CanMove is False");
        }
       /* do 
        {
            selectedCARD = CardPlacedToBoard[Random.Range(0, CardPlacedToBoard.Count)];   //WORKING HERE
        } while (selectedCARD.GetComponent<DisplayCard2>().canMove); */
    }

    public void GetAdjacentP2Cards()  //Adjacent cards of selected p2 Card. 
    {
        SelectCardToMove();
        adjacentp2List.Clear();

        foreach (GameObject p2 in player2) 
        {
            float distance = Vector3.Distance(p2.transform.position, selectedCARD.transform.position);
            if (p2.gameObject.name == "SHCardP2")
            {
                if (distance < 210f && p2 != selectedCARD.gameObject)
                {
                    adjacentp2List.Add(p2);
                }
            }
            else 
            {
                DisplayCard2 displayCard2 = p2.GetComponent<DisplayCard2>();
                if (distance < 210f && p2 != selectedCARD.gameObject && displayCard2.canMove)
                {
                    adjacentp2List.Add(p2);
                }
            }
        }
    }

    public void GetAdjacentBslotCards() 
    {
        GetAdjacentP2Cards();
        adjacentBSlots.Clear();

        foreach (Transform bslot in BSlots) 
        {
            foreach (GameObject p2 in adjacentp2List)
            {
                float dist = Vector3.Distance(p2.transform.position, bslot.transform.position);
                if (dist < 210f && bslot.childCount == 0) 
                {
                    adjacentBSlots.Add(bslot);
                }
            }
        }
    }

    public void ResetCanMoveP2() //RESETTING CANMOVE TO TRUE
    {
        foreach (GameObject p2 in CardPlacedToBoard) 
        {
            DisplayCard2 dpcard2 = p2.GetComponent<DisplayCard2>();
            if (dpcard2.canMove==false) 
            {
                dpcard2.canMove = true;
            }
        }
    }

    public void GetRandomMoveBSlot() 
    {
        //selectedCARD = CardPlacedToBoard[Random.Range(0, CardPlacedToBoard.Count)];
        adjacentBSlots.Sort((x, y) =>
        {
            int numX = ExtractNumber(x.name);
            int numY = ExtractNumber(y.name);
            return numX.CompareTo(numY);
        });

        if (gameToggleManager.EasyToggle.isOn || gameToggleManager.MediumToggle.isOn) 
        {
            BoardSlt = adjacentBSlots[Random.Range(0, adjacentBSlots.Count)];
        }

        else if (gameToggleManager.HardToggle.isOn)
        {    //if selectedCARD.defense > selectedcardattack
            float cdefense = selectedCARD.GetComponent<DisplayCard2>().GetCardHealth();
            float cattack = selectedCARD.GetComponent<DisplayCard2>().GetCardAttack();
            Debug.Log("ATTACK:"+cattack+"\t"+"DEFENSE:"+cdefense);

            if (cattack > cdefense)
            {
                for (int i = adjacentBSlots.Count - 1; i >= 0; i--)
                {
                    if (adjacentBSlots[i].childCount == 0)
                    {
                        BoardSlt = adjacentBSlots[i];
                        break; // Exit the loop as soon as we find a slot without a child
                    }
                }
            }
            else if (cattack < cdefense)
            {
                for (int i = 0; i <= adjacentBSlots.Count ; i++)
                {
                    if (adjacentBSlots[i].childCount == 0)
                    {
                        BoardSlt = adjacentBSlots[i];
                        break; // Exit the loop as soon as we find a slot without a child
                    }
                }
            }
            //else
        }

        /* foreach (Transform t in adjacentBSlots) 
         {
             Debug.Log("TILE NAME:"+t.name);
         }
         Debug.Log("TOTAL TILES:"+adjacentBSlots.Count); */
    }

    int ExtractNumber(string name)
    {
        // Use regular expression to extract the numeric part of the name
        Match match = Regex.Match(name, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return 0;
    }

    public void Attack() 
    {
        //Same logic random slot, the adjacent would be "player2"
        if (adjacentSlotsP1.Count>0) 
        {
            foreach (Transform slot in adjacentSlotsP1) 
            {
                if (slot.childCount > 0)  // Added check for child count
                {
                    if (slot.GetChild(0).gameObject.CompareTag("Player2"))  // Changed to CompareTag for efficiency
                    {
                        // Make a new list and add them to that list   List name: AiAttackCards
                        AiAttackCards.Add(slot.GetChild(0).gameObject);
                        Debug.Log("Addedto AiAttackCards List");
                    }
                }
            }
        }
    }

    public void SelectAttackCard()
    {
        Attack();
        if (AiAttackCards.Count > 0)
        {
            GameObject cardWithMostAttacking = null;
            float maxEnergy = float.MinValue;

            if (gameToggleManager.EasyToggle.isOn)
            {
                do
                {
                    randomAiAtcCard = AiAttackCards[Random.Range(0, AiAttackCards.Count)];
                } while (randomAiAtcCard.name == "SHCardP2");
            }
            else if (gameToggleManager.HardToggle.isOn || gameToggleManager.MediumToggle.isOn) 
            {
                foreach (GameObject atc in AiAttackCards) 
                {
                    DisplayCard2 dp = atc.GetComponent<DisplayCard2>();
                    if (dp != null) 
                    {
                        if (dp.GetCardAttack() > maxEnergy)
                        {
                            maxEnergy = dp.GetCardAttack();
                            cardWithMostAttacking = atc;
                        }
                    }
                }
                if (cardWithMostAttacking != null) 
                {
                    randomAiAtcCard = cardWithMostAttacking.gameObject;
                }
            }
            
            Debug.Log("Picked card for attack:" + randomAiAtcCard);
            randomAiAtcCard.GetComponent<DisplayCard2>().OnPtClc();
            // Assuming adjCards is a list of adjacent cards of the player being attacked
            List<GameObject> adjCards = randomAiAtcCard.GetComponent<DisplayCard2>().adjCards;

            // Check if there are adjacent cards
            if (adjCards.Count > 0)
            {
                // Select a random adjacent card for defense
                if (gameToggleManager.EasyToggle.isOn)
                {
                    defenseCard = adjCards[Random.Range(0, adjCards.Count)];  //RATHER THAN PICKING RANDOM CARD FOR DEFENSE, CHOOSE WITH ONE LOWER DEFENSE
                }
                else if (gameToggleManager.HardToggle.isOn || gameToggleManager.MediumToggle.isOn) 
                {
                    GameObject childWithLowestEnergy = null;
                    float minEnergy = float.MaxValue;

                    foreach (GameObject def in adjCards) 
                    {
                        DisplayCard d = def.GetComponent<DisplayCard>();

                        if(d != null) 
                        {
                            if (d.GetCardHealth() < minEnergy) 
                            {
                                minEnergy = d.GetCardHealth();
                                childWithLowestEnergy = def;
                            }
                        }
                    }
                    if(childWithLowestEnergy != null) 
                    {
                        defenseCard = childWithLowestEnergy.gameObject;
                    }
                }

                defenseCard.GetComponent<DisplayCard>().OnPtcClk();
                StartCoroutine(RollingDice(2.0f));
                StartCoroutine(DeselectAtcCard(6.0f));
            }
            else
            {
                Debug.Log("No adjacent cards for defense.");
            }
            // randomAiAtcCard.GetComponent<DisplayCard2>().isSelected = true;
            //randomAiAtcCard.GetComponent<DisplayCard2>().outerBorder.color = Color.white;
        }
        else 
        {
            Debug.Log("No Cards for Attack");
        }
    }

    IEnumerator RollingDice(float delay) 
    { 
        yield return new WaitForSeconds(delay);
        dice.OnMouseDown();
    }

    IEnumerator DeselectAtcCard(float del) 
    {
        yield return new WaitForSeconds(del);
        defenseCard.GetComponent<DisplayCard>().OnPtcClk();
        randomAiAtcCard.GetComponent<DisplayCard2>().OnPtClc();
    }

    IEnumerator ChangeAIPhaseToMove(float delayed) 
    {
        yield return new WaitForSeconds(delayed);
        gmm.ChangePhase(GamePhase.Move);
    }

    IEnumerator ChangeAIPhaseToAttack(float delayed)
    {
        yield return new WaitForSeconds(delayed);
        gmm.ChangePhase(GamePhase.Attack);
        //SelectAttackCard();
    }

    IEnumerator Attacking(float del) 
    {
        yield return new WaitForSeconds(del);
        SelectAttackCard();
    }

    IEnumerator ChangeAIPhaseToSetup(float delay)
    {
        yield return new WaitForSeconds(delay);
        gmm.ChangePhase(GamePhase.Play);
        //  SelectRandomCard();
        if (gameToggleManager.EasyToggle.isOn || gameToggleManager.MediumToggle.isOn)
        {
            StartCoroutine(PlaceToBoard(2.0f));
            StartCoroutine(ChangeAIPhaseToMove(3.0f));
            StartCoroutine(MoveInBoard(4.0f));
            //StartCoroutine(MoveInBoard(7.0f));
            StartCoroutine(ChangeAIPhaseToAttack(5.0f));
            StartCoroutine(Attacking(6.0f));
        }
        else if (gameToggleManager.HardToggle.isOn) 
        {
            StartCoroutine(PlaceToBoard(2.0f));
            StartCoroutine(PlaceToBoard(3.0f));
            StartCoroutine(ChangeAIPhaseToMove(4.0f));
            StartCoroutine(MoveInBoard(5.0f));
            //StartCoroutine(MoveInBoard(7.0f));
            StartCoroutine(ChangeAIPhaseToAttack(6.0f));
            StartCoroutine(Attacking(6.0f));
            int val = BoardSlot.GetCurrentEnergyP2();
            if (val > 2) 
            {
                StartCoroutine(Attacking(15.0f));
            }
            //StartCoroutine(SelectAttackCard(7.0f));
        }
    }

    public void OnTurnButtonClick()
    {
        ResetTimer();
        
        //DisplayCard.GetTurn();

        if (isPlayer1Turn)
        {
            gmm.ChangePhase(GamePhase.Draw);
            turnText.text = "P2 Turn";  //Button 
            turnCount = 0;
            turnBar.SetTurnTime(turnCount);
            //StartCoroutine(Turnbar2(1.0f));

            turnCount2 = 60;
            turnBar.SetTurnTime2(turnCount2);

            deckP1.enabled = false;
            deckP2.enabled = true;
            boardSlot.AnotherMethod2();

            Scene currentScene = SceneManager.GetActiveScene();
            //if (currentScene.name == "AI") {}
            if (gmm.currentPhase == GamePhase.Draw) 
            {
                CShufflerP2.ShuffleCards(); 
                ResetCanMoveP2(); //RESET THE CanMove OF DPCARD2
                adjacentBSlots.Clear();
                // boardSlot.UpdateMoveListP2();
                //  boardSlot.AnotherMethod2();
                if (currentScene.name =="AI") //HERE HERE HERE HERE  && gameToggleManager.EasyToggle.isOn
                {
                   StartCoroutine(ChangeAIPhaseToSetup(3.0f));
                }
                }
            foreach (DisplayCard otherCard in allDisplayCards)
            {
                if (otherCard.isSelected)
                {
                    otherCard.OnPtcClk();
                }
            }
             //foreach (DisplayCard2 otherCards in allDisplayCardsP2) {if (otherCards.isSelected){otherCards.OnPtClc();}}
            //  MoveSelectedCardToRandomSlot();
        }
        else
        {
            gmm.ChangePhase(GamePhase.Draw);
            turnText.text = "P1 Turn";
            turnCount = 60;
            turnBar.SetTurnTime(turnCount);

            turnCount2 = 0;
            turnBar.SetTurnTime2(turnCount2);

            deckP1.enabled = true;
            deckP2.enabled = false;
            boardSlot.AnotherMethod();
            CShufflerP1.ShuffleCards();
            Scene currentScene = SceneManager.GetActiveScene();

            if (currentScene.name == "SampleScene")
            { 
                foreach (DisplayCard2 otherCard in allDisplayCardsP2)
                {
                    if (otherCard.isSelected)
                    {
                        otherCard.OnPtClc();
                    }
                }
            }
        }

        // Toggle the turn
        isPlayer1Turn = !isPlayer1Turn;
        Debug.Log("PLAYER 1 TURN:" + isPlayer1Turn);
    }

    public static bool GetPlayerTurn()
    {
        return isPlayer1Turn;
    }

    private IEnumerator ChangeTurn(float delay)
    {
        while (true)
        {
            if (isPlayer1Turn)
            {
                gmm.ChangePhase(GamePhase.Draw);
                turnText.text = "P1 Turn";
                turnCount = 60;
                turnBar.SetTurnTime(turnCount);

                turnCount2 = 0;
                turnBar.SetTurnTime2(turnCount2);

                deckP1.enabled = true;
                deckP2.enabled = false;
              //  boardSlot.AnotherMethod();
            }
            else
            {
                gmm.ChangePhase(GamePhase.Draw);
                turnText.text = "P2 Turn";
                turnCount2 = 60;
                turnBar.SetTurnTime2(turnCount2);
              // StartCoroutine(Turnbar2(1.0f));

                turnCount = 0;
                turnBar.SetTurnTime(turnCount2);

                if (mainCamera != null)
                {
                    mainCamera.orthographicSize = originalOrthographicSize;
                    mainCamera.transform.position = originalCamPos;
                }

                deckP1.enabled = false;
                deckP2.enabled = true;
               // boardSlot.AnotherMethod2();
            }
           // Debug.Log("IS PLAYER TURN:"+isPlayer1Turn);

            yield return new WaitForSeconds(delay);
           // isPlayer1Turn = !isPlayer1Turn;
           OnTurnButtonClick();
        }
    }

    private void ResetTimer()
    {
        StopCoroutine(turnCoroutine);

        turnCoroutine = StartCoroutine(ChangeTurn(60.0f));
    }

    private IEnumerator Turnbar(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (turnCount >= 0)
            {
                turnCount--;
               // Debug.Log("TurnCount:" + turnCount);
                turnBar.SetTurnTime(turnCount);
            }
        }
    }

    private IEnumerator Turnbar2(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (turnCount2 >= 0)
            {
                turnCount2--;
               // Debug.Log("TurnCount2:" + turnCount2);

                turnBar.SetTurnTime2(turnCount2);
            }

        }
    }

    private void TurnStarter(bool isP1)
    {
        
        if (isP1)
        {
         //   turnCount = 30;
          //  turnBar.SetTurnTime(turnCount);
          
            StartCoroutine(Turnbar(1.0f));
            StartCoroutine(Turnbar2(1.0f));
        }
        
    }

}
