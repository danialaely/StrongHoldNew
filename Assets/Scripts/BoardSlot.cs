using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardSlot : MonoBehaviourPunCallbacks, IDropHandler
{
    public TMP_Text energyText;
    public TMP_Text energyTextP2;
    // public GameObject hand;

    // Make currentEnergy static so that it's shared among all instances.
    private static int currentEnergy = 0;
    private static int currentEnergyP2 = 0;

    public HealthBar healthBar;

    public GameManager gmmm;
    // End Turn Button to move between Turns

    public AudioSource src;
    public AudioClip cardPlacementClip;

    public Color highlightColor;
    private Color originalColor;

    public GameObject coinP1img;
    public GameObject coinP1img2;
    public GameObject coinP1img3;
    public GameObject coinP1img4;
    public GameObject coinP1img5;
    public GameObject coinP1img6;
    public GameObject coinP1img7;
    public GameObject coinP1img8;

    public GameObject coinP2img;
    public GameObject coinP2img2;
    public GameObject coinP2img3;
    public GameObject coinP2img4;
    public GameObject coinP2img5;
    public GameObject coinP2img6;
    public GameObject coinP2img7;
    public GameObject coinP2img8;

    public static List<Transform> availableBSlotsAI = new List<Transform>();
    public static List<Transform> availableBSlotsAIMove = new List<Transform>();

    public static List<Transform> adjacentBSlotsPlayer1 = new List<Transform>(); //Player1 adjacent BoardSlots

    public static List<Transform> cardsPlacedInPreviousTurn = new List<Transform>(); //Keeping track of cards placed in previous turns
    public static List<Transform> cardsPlacedInPreviousTurnP2 = new List<Transform>(); //FOR P2 Keeping track of cards placed in previous turns

    private void Start()
    {
     //   UpdateMoveListP2();
        AdjacentBslotListP1();
        cardsPlacedInPreviousTurn.Clear();

        AiPlay();

        UpdatePreviousCardsList();
        Debug.Log("Previously Placed Cards P1:"+PreviouslyPlacedAvailable().Count);

        UpdatePreviousCardsListP2();

       // UpdateMovementPhaseList();
       // Debug.Log("Available Slots:"+availableBSlotsAI.Count);
    }
    #region Ai Logics
    public void AiPlay() 
    {
        availableBSlotsAI.Clear();

        foreach (Transform slot in transform.parent)
        {
            int rwindex = slot.GetSiblingIndex();

            // Check if any adjacent slot has a card tagged as "Player2"
            if ((rwindex > 0 && transform.parent.GetChild(rwindex - 1).childCount > 0 && transform.parent.GetChild(rwindex - 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 12 && transform.parent.GetChild(rwindex - 13).childCount > 0 && transform.parent.GetChild(rwindex - 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 13 && transform.parent.GetChild(rwindex - 14).childCount > 0 && transform.parent.GetChild(rwindex - 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 14 && transform.parent.GetChild(rwindex - 15).childCount > 0 && transform.parent.GetChild(rwindex - 15).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 1 && transform.parent.GetChild(rwindex + 1).childCount > 0 && transform.parent.GetChild(rwindex + 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 13 && transform.parent.GetChild(rwindex + 13).childCount > 0 && transform.parent.GetChild(rwindex + 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 14 && transform.parent.GetChild(rwindex + 14).childCount > 0 && transform.parent.GetChild(rwindex + 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 15 && transform.parent.GetChild(rwindex + 15).childCount > 0 && transform.parent.GetChild(rwindex + 15).GetChild(0).CompareTag("Player2")))
            {
                // cardsPlacedInPreviousTurn.Add(slot);
                availableBSlotsAI.Add(transform.parent.GetChild(rwindex));
            }
        }
    }

    public List<Transform> Available()
    {
        return availableBSlotsAI;
    }

  /*  public void UpdateMoveListP2() 
    {
        availableBSlotsAIMove.Clear(); // Clear the list before updating it for the new turn

        // Iterate through all slots to find adjacent slots with a card tagged as "Player2"
        foreach (Transform slot in transform.parent)
        {
            int rwindex = slot.GetSiblingIndex();

            // Check if any adjacent slot has a card tagged as "Player2"
            if ((rwindex > 0 && transform.parent.GetChild(rwindex - 1).childCount > 0 && transform.parent.GetChild(rwindex - 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 12 && transform.parent.GetChild(rwindex - 13).childCount > 0 && transform.parent.GetChild(rwindex - 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 13 && transform.parent.GetChild(rwindex - 14).childCount > 0 && transform.parent.GetChild(rwindex - 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 14 && transform.parent.GetChild(rwindex - 15).childCount > 0 && transform.parent.GetChild(rwindex - 15).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 1 && transform.parent.GetChild(rwindex + 1).childCount > 0 && transform.parent.GetChild(rwindex + 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 13 && transform.parent.GetChild(rwindex + 13).childCount > 0 && transform.parent.GetChild(rwindex + 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 14 && transform.parent.GetChild(rwindex + 14).childCount > 0 && transform.parent.GetChild(rwindex + 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 15 && transform.parent.GetChild(rwindex + 15).childCount > 0 && transform.parent.GetChild(rwindex + 15).GetChild(0).CompareTag("Player2")))
            {
               // availableBSlotsAIMove.Add(slot);
            }
        }
        //  else { if (availableBSlotsAIMove.Contains(gameObject.transform)) { availableBSlotsAIMove.Remove(transform.parent.GetChild(rwindex)); }}
    }*/

  /*  public List<Transform> MoveAvailable() 
    {
        return availableBSlotsAIMove;
    }*/

    public void AdjacentBslotListP1()  //Adjacent P1 BSlot List
    {
        adjacentBSlotsPlayer1.Clear(); // Clear the list before updating it for the new turn

        // Iterate through all slots to find adjacent slots with a card tagged as "Player1"
        foreach (Transform slot in transform.parent)
        {
            int rwindex = slot.GetSiblingIndex();

            // Check if any adjacent slot has a card tagged as "Player1"
            if ((rwindex > 0 && transform.parent.GetChild(rwindex - 1).childCount > 0 && transform.parent.GetChild(rwindex - 1).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 12 && transform.parent.GetChild(rwindex - 13).childCount > 0 && transform.parent.GetChild(rwindex - 13).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 13 && transform.parent.GetChild(rwindex - 14).childCount > 0 && transform.parent.GetChild(rwindex - 14).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 14 && transform.parent.GetChild(rwindex - 15).childCount > 0 && transform.parent.GetChild(rwindex - 15).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 1 && transform.parent.GetChild(rwindex + 1).childCount > 0 && transform.parent.GetChild(rwindex + 1).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 13 && transform.parent.GetChild(rwindex + 13).childCount > 0 && transform.parent.GetChild(rwindex + 13).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 14 && transform.parent.GetChild(rwindex + 14).childCount > 0 && transform.parent.GetChild(rwindex + 14).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 15 && transform.parent.GetChild(rwindex + 15).childCount > 0 && transform.parent.GetChild(rwindex + 15).GetChild(0).CompareTag("Player1")))
            {
                adjacentBSlotsPlayer1.Add(slot);
            }
        }
    }

    public List<Transform> AdjacentP1Available()
    {
        return adjacentBSlotsPlayer1;
    }
    #endregion Ai Logics

    public void UpdatePreviousCardsList() 
    {
        cardsPlacedInPreviousTurn.Clear();

        foreach (Transform slot in transform.parent)
        {
            int rwindex = slot.GetSiblingIndex();

            // Check if any adjacent slot has a card tagged as "Player1"
            if ((rwindex > 0 && transform.parent.GetChild(rwindex - 1).childCount > 0 && transform.parent.GetChild(rwindex - 1).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 12 && transform.parent.GetChild(rwindex - 13).childCount > 0 && transform.parent.GetChild(rwindex - 13).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 13 && transform.parent.GetChild(rwindex - 14).childCount > 0 && transform.parent.GetChild(rwindex - 14).GetChild(0).CompareTag("Player1")) ||
                (rwindex > 14 && transform.parent.GetChild(rwindex - 15).childCount > 0 && transform.parent.GetChild(rwindex - 15).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 1 && transform.parent.GetChild(rwindex + 1).childCount > 0 && transform.parent.GetChild(rwindex + 1).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 13 && transform.parent.GetChild(rwindex + 13).childCount > 0 && transform.parent.GetChild(rwindex + 13).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 14 && transform.parent.GetChild(rwindex + 14).childCount > 0 && transform.parent.GetChild(rwindex + 14).GetChild(0).CompareTag("Player1")) ||
                (rwindex < transform.parent.childCount - 15 && transform.parent.GetChild(rwindex + 15).childCount > 0 && transform.parent.GetChild(rwindex + 15).GetChild(0).CompareTag("Player1")))
            {
                cardsPlacedInPreviousTurn.Add(slot);
            }
        }
    }

    public List<Transform> PreviouslyPlacedAvailable() 
    {
        return cardsPlacedInPreviousTurn ;
    }

    public void UpdatePreviousCardsListP2()
    {
        cardsPlacedInPreviousTurnP2.Clear();

        foreach (Transform slot in transform.parent)
        {
            int rwindex = slot.GetSiblingIndex();

            // Check if any adjacent slot has a card tagged as "Player2"
            if ((rwindex > 0 && transform.parent.GetChild(rwindex - 1).childCount > 0 && transform.parent.GetChild(rwindex - 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 12 && transform.parent.GetChild(rwindex - 13).childCount > 0 && transform.parent.GetChild(rwindex - 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 13 && transform.parent.GetChild(rwindex - 14).childCount > 0 && transform.parent.GetChild(rwindex - 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex > 14 && transform.parent.GetChild(rwindex - 15).childCount > 0 && transform.parent.GetChild(rwindex - 15).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 1 && transform.parent.GetChild(rwindex + 1).childCount > 0 && transform.parent.GetChild(rwindex + 1).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 13 && transform.parent.GetChild(rwindex + 13).childCount > 0 && transform.parent.GetChild(rwindex + 13).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 14 && transform.parent.GetChild(rwindex + 14).childCount > 0 && transform.parent.GetChild(rwindex + 14).GetChild(0).CompareTag("Player2")) ||
                (rwindex < transform.parent.childCount - 15 && transform.parent.GetChild(rwindex + 15).childCount > 0 && transform.parent.GetChild(rwindex + 15).GetChild(0).CompareTag("Player2")))
            {
                cardsPlacedInPreviousTurnP2.Add(slot);
            }
        }
    }

    public List<Transform> PreviouslyPlacedAvailableP2()
    {
        return cardsPlacedInPreviousTurnP2;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DisplayCard card = eventData.pointerDrag.GetComponent<DisplayCard>();
        DisplayCard2 cardd = eventData.pointerDrag.GetComponent<DisplayCard2>();
        
        //  if (card.transform.parent.name == "Hand"){ }
        bool isP1Turn = ButtonTurn.GetPlayerTurn();

        // originalColor = GetComponent<Image>().color;

        if (card != null && transform.childCount == 0)  //(2): CARD PLACEMENT PHASE
        {
            int cardEnergy = card.GetCardEnergy();

            if (int.TryParse(energyText.text, out currentEnergy))
            {
                if (currentEnergy >= cardEnergy && isP1Turn)
                {
                    int rowIndex = transform.GetSiblingIndex();
                    //int maxRowIndex = 97;

                    //  int columnIndex = transform.parent.GetSiblingIndex();
                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase == GamePhase.Draw)
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase == GamePhase.Play)
                    {  //(rowIndex >= 84 && rowIndex < maxRowIndex) ||
                        if (PreviouslyPlacedAvailable().Contains(transform))
                        {                                                                 // card.transform.parent.name == "Hand"
                            currentEnergy -= cardEnergy;
                            energyText.text = currentEnergy.ToString();
                            healthBar.SetHealth(currentEnergy); //
                                                                //HERE I HAVE TO DESTROY THE COIN IMAGES
                            if (currentEnergy == 7) 
                            {
                                coinP1img8.SetActive(false);
                            }
                            if (currentEnergy == 6)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                            }
                            if (currentEnergy == 5)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                            }
                            if (currentEnergy == 4)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                            }
                            if (currentEnergy == 3)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                            }
                            if (currentEnergy == 2)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                            }
                            if (currentEnergy == 1)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                            }
                            if (currentEnergy == 0)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                                coinP1img.SetActive(false);
                            }

                            if (currentEnergy == -1)
                            {
                                energyText.text = "0";
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                                coinP1img.SetActive(false);
                            }
                            card.transform.SetParent(transform);
                            card.transform.localPosition = Vector3.zero;
                            card.GetComponent<CanvasGroup>().blocksRaycasts = true;

                            card.photonView.RPC("DisableCardBack2RPC", RpcTarget.All);
                            GetPlacementSound();
                            //GetComponent<Image>().color = highlightColor;
                            //  HighlightValidSlots();
                        }
                        else 
                        {
                            gmmm.ErrorSound();
                        }

                    }

                    if (card.transform.parent.name == "Hand" && gmmm.currentPhase == GamePhase.Move)
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Draw)
                    {
                        gmmm.ErrorSound();
                    }

                    if (card.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Play)  {  if (rowIndex <=84) {/*  gmmm.ErrorSound();*/ }}

                    if (card.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Move && card.canMove)
                    {      //(rowIndex >= 84 && rowIndex < maxRowIndex) ||   card.GetComponent<DisplayCard>().MoveBoardSlots().Contains(transform
                        if (card.AdjacentBSlotsAvailable().Contains(transform))  //MovementAvailable().Contains(transform)
                        {                                                                 // card.transform.parent.name == "Hand"
                            //currentEnergy -= cardEnergy;
                            card.canMove = false;
                            StartCoroutine(card.CanMoveNow(20f));
                            currentEnergy -= 1;
                            energyText.text = currentEnergy.ToString();
                            healthBar.SetHealth(currentEnergy); //
                            if (currentEnergy == 7)
                            {
                                coinP1img8.SetActive(false);
                            }
                            if (currentEnergy == 6)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                            }
                            if (currentEnergy == 5)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                            }
                            if (currentEnergy == 4)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                            }
                            if (currentEnergy == 3)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                            }
                            if (currentEnergy == 2)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                            }
                            if (currentEnergy == 1)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                            }
                            if (currentEnergy == 0)
                            {
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                                coinP1img.SetActive(false);
                            }

                            if (currentEnergy == -1)
                            {
                                energyText.text = "0";
                                coinP1img8.SetActive(false);
                                coinP1img7.SetActive(false);
                                coinP1img6.SetActive(false);
                                coinP1img5.SetActive(false);
                                coinP1img4.SetActive(false);
                                coinP1img3.SetActive(false);
                                coinP1img2.SetActive(false);
                                coinP1img.SetActive(false);
                            }
                            card.transform.SetParent(transform);
                            card.transform.localPosition = Vector3.zero;
                            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            GetPlacementSound();

                        }
                        else 
                        {
                            gmmm.ErrorSound(); // Handle the boolean here     MAKE THE BOOLEAN TO CONTROL CARD MOVEMENT IN BSLOT
                           
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Invalid energyText value: " + energyText.text);
                // Handle the error as needed.
            }

            // Snap the card to the slot.

        }

        //if (card.transform.parent.name == "Hand2"){ }
        else if (cardd.gameObject.tag == "Player2" || !PhotonNetwork.IsMasterClient)   //(2):CARD PLACEMENT PHASE
        {
            //Debug.Log("THIS FUNCTION IS WORKING:"+cardd.transform.parent.name);
            if (cardd != null && transform.childCount == 0)
            {
                int carddEnergy = cardd.GetCardEnergy();
                
                if (int.TryParse(energyTextP2.text, out currentEnergyP2))
                {
                    Debug.Log("CURRENTENERGY P2:"+currentEnergyP2);
                    Debug.Log("CARDD ENERGY:"+carddEnergy);
                    if (currentEnergyP2 >= carddEnergy && !isP1Turn)
                    {
                        Debug.Log("I AM HERE");
                        int rowIndex = transform.GetSiblingIndex();
                       // int maxRowIndex = 14;

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Draw)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Move)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Draw)
                        {
                            gmmm.ErrorSound();
                        }

                        if (cardd.transform.parent.name == "Hand2" && gmmm.currentPhase == GamePhase.Play)
                        {
                            if (PreviouslyPlacedAvailableP2().Contains(transform))
                            {

                                currentEnergyP2 -= carddEnergy;
                                energyTextP2.text = currentEnergyP2.ToString();
                                healthBar.SetHealth2(currentEnergyP2); //
                                                                       //HERE I HAVE TO DESTROY THE IMAGES OF COIN
                                if (currentEnergyP2 == 7)
                                {
                                    coinP2img8.SetActive(false);
                                }
                                if (currentEnergyP2 == 6)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                }
                                if (currentEnergyP2 == 5)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                }
                                if (currentEnergyP2 == 4)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                }
                                if (currentEnergyP2 == 3)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                }
                                if (currentEnergyP2 == 2)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                }
                                if (currentEnergyP2 == 1)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                }
                                if (currentEnergyP2 == 0)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                    coinP2img.SetActive(false);
                                }

                                if (currentEnergyP2 == -1)
                                {
                                    energyTextP2.text = "0";
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                    coinP2img.SetActive(false);
                                }
                                cardd.transform.SetParent(transform);
                                cardd.transform.localPosition = Vector3.zero;
                                cardd.GetComponent<CanvasGroup>().blocksRaycasts = true;

                                Scene cs = SceneManager.GetActiveScene();
                                if (cs.name == "AI") 
                                {
                                    Image carddBackImage = cardd.transform.Find("Back").GetComponent<Image>();
                                    carddBackImage.enabled = false;
                                }

                                // Call the RPC to disable the card back image for all players
                                cardd.photonView.RPC("DisableCardBackRPC", RpcTarget.All);

                                GetPlacementSound();
                                //  Debug.Log("Child:"+ transform.parent.GetChild(rowIndex - 1).GetChild(0));
                            }
                            else 
                            {
                                gmmm.ErrorSound();
                            }
                        }

                        if (cardd.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Play) { if (rowIndex >= 14) { /*   gmmm.ErrorSound(); */ }}


                        if (cardd.transform.parent.tag == "BSlot" && gmmm.currentPhase == GamePhase.Move  && cardd.canMove)
                        {
                            if (cardd.AdjacentBSlotsAvailable().Contains(transform))
                            { 
                                cardd.canMove = false;
                                StartCoroutine(cardd.CanMoveNow(20f));
                                //currentEnergyP2 -= carddEnergy;
                                currentEnergyP2 -= 1;
                                energyTextP2.text = currentEnergyP2.ToString();
                                healthBar.SetHealth2(currentEnergyP2); //
                                if (currentEnergyP2 == 7)
                                {
                                    coinP2img8.SetActive(false);
                                }
                                if (currentEnergyP2 == 6)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                }
                                if (currentEnergyP2 == 5)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                }
                                if (currentEnergyP2 == 4)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                }
                                if (currentEnergyP2 == 3)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                }
                                if (currentEnergyP2 == 2)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                }
                                if (currentEnergyP2 == 1)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                }
                                if (currentEnergyP2 == 0)
                                {
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                    coinP2img.SetActive(false);
                                }

                                if (currentEnergyP2 == -1)
                                {
                                    energyTextP2.text = "0";
                                    coinP2img8.SetActive(false);
                                    coinP2img7.SetActive(false);
                                    coinP2img6.SetActive(false);
                                    coinP2img5.SetActive(false);
                                    coinP2img4.SetActive(false);
                                    coinP2img3.SetActive(false);
                                    coinP2img2.SetActive(false);
                                    coinP2img.SetActive(false);
                                }
                                cardd.transform.SetParent(transform);
                                cardd.transform.localPosition = Vector3.zero;
                                cardd.GetComponent<CanvasGroup>().blocksRaycasts = true;

                                Image carddBackImage = cardd.transform.Find("Back").GetComponent<Image>();
                                carddBackImage.enabled = false;

                                GetPlacementSound();
                                //  Debug.Log("Child:"+ transform.parent.GetChild(rowIndex - 1).GetChild(0));
                            }
                            else 
                            {
                                gmmm.ErrorSound();
                            }
                        }

                    }
                }
                else
                {
                    Debug.LogError("Invalid energyText value: " + energyTextP2.text);
                    // Handle the error as needed.
                }

                // Snap the card to the slot.

            }
        }

        // GetComponent<Image>().color = originalColor;
    }

    public void SpendOnAttack() 
    {
        int val = currentEnergy;
   
        if (val == 7) 
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
        }
        if (val == 6)
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
            coinP1img5.SetActive(false);
        }
        if (val == 5)
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
            coinP1img5.SetActive(false);
            coinP1img4.SetActive(false);
        }
        if (val == 4)
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
            coinP1img5.SetActive(false);
            coinP1img4.SetActive(false);
            coinP1img3.SetActive(false);
        }
        if (val == 3)
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
            coinP1img5.SetActive(false);
            coinP1img4.SetActive(false);
            coinP1img3.SetActive(false);
            coinP1img2.SetActive(false);
        }
        if (val == 2)
        {
            val -= 2;
            energyText.text = val.ToString();
            coinP1img8.SetActive(false);
            coinP1img7.SetActive(false);
            coinP1img6.SetActive(false);
            coinP1img5.SetActive(false);
            coinP1img4.SetActive(false);
            coinP1img3.SetActive(false);
            coinP1img2.SetActive(false);
            coinP1img.SetActive(false);
        }

    }

    public void SpendOnAttack2()
    {
        int val = currentEnergyP2;
        if (val >= 2)
        {
            if (val ==8) 
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
            }

            if (val == 7)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
            }
            if (val == 6)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
                coinP2img5.SetActive(false);
            }
            if (val == 5)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
                coinP2img5.SetActive(false);
                coinP2img4.SetActive(false);
            }
            if (val == 4)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
                coinP2img5.SetActive(false);
                coinP2img4.SetActive(false);
                coinP2img3.SetActive(false);
            }
            if (val == 3)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
                coinP2img5.SetActive(false);
                coinP2img4.SetActive(false);
                coinP2img3.SetActive(false);
                coinP2img2.SetActive(false);
            }
            if (val == 2)
            {
                val -= 2;
                energyTextP2.text = val.ToString();
                coinP2img8.SetActive(false);
                coinP2img7.SetActive(false);
                coinP2img6.SetActive(false);
                coinP2img5.SetActive(false);
                coinP2img4.SetActive(false);
                coinP2img3.SetActive(false);
                coinP2img2.SetActive(false);
                coinP2img.SetActive(false);
            }
        }

    }


    public void AnotherMethod()  // (1):CARD DRAW PHASE
    {
      //  UpdateMoveListP2();

        UpdatePreviousCardsList();
        Debug.Log("Previously Placed Cards P1:" + PreviouslyPlacedAvailable().Count);

       // UpdateMovementPhaseList();

        int value = currentEnergy;
        // Debug.Log("CE: " + value);
        if (value >= 0)
        {
            // value += 8;
            value = 8;
            energyText.text = value.ToString();
            healthBar.SetHealth(value);

            coinP1img.SetActive(true);
            coinP1img2.SetActive(true);
            coinP1img3.SetActive(true);
            coinP1img4.SetActive(true);
            coinP1img5.SetActive(true);
            coinP1img6.SetActive(true);
            coinP1img7.SetActive(true);
            coinP1img8.SetActive(true);

        }
        if (value == -1)
        {
            value = 9;
            energyText.text = value.ToString();
            healthBar.SetHealth(value);

            coinP1img.SetActive(true);
            coinP1img2.SetActive(true);
            coinP1img3.SetActive(true);
            coinP1img4.SetActive(true);
            coinP1img5.SetActive(true);
            coinP1img6.SetActive(true);
            coinP1img7.SetActive(true);
            coinP1img8.SetActive(true);
        }

    }

    public void AnotherMethod2()  // (1):CARD DRAW PHASE
    {
        AdjacentBslotListP1();

        UpdatePreviousCardsListP2();
        AiPlay();
       // UpdateMoveListP2();
        int value = currentEnergyP2;
        // Debug.Log("CE: " + value);
        if (value >= 0)
        {
            // value += 8;
            value = 8;
            energyTextP2.text = value.ToString();
            healthBar.SetHealth2(value);

            coinP2img.SetActive(true);
            coinP2img2.SetActive(true);
            coinP2img3.SetActive(true);
            coinP2img4.SetActive(true);
            coinP2img5.SetActive(true);
            coinP2img6.SetActive(true);
            coinP2img7.SetActive(true);
            coinP2img8.SetActive(true);
        }
        if (value == -1)
        {
            value = 9;
            energyTextP2.text = value.ToString();
            healthBar.SetHealth2(value);

            coinP2img.SetActive(true);
            coinP2img2.SetActive(true);
            coinP2img3.SetActive(true);
            coinP2img4.SetActive(true);
            coinP2img5.SetActive(true);
            coinP2img6.SetActive(true);
            coinP2img7.SetActive(true);
            coinP2img8.SetActive(true);
        }
        SetCurrentEnergyP2(value);
    }

    public static int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public static void SetCurrentEnergy(int energy) 
    {
        currentEnergy = energy;
    }

    public static void SetCurrentEnergyP2(int energy)
    {
        currentEnergyP2 = energy;
    }

    public static int GetCurrentEnergyP2()
    {
        return currentEnergyP2;
    }

    public void GetPlacementSound()
    {
        src.clip = cardPlacementClip;
        src.Play();
    }

   

}






















