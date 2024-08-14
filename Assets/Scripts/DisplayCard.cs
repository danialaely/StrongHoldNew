using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class DisplayCard : MonoBehaviourPunCallbacks, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IOnEventCallback
{
    public List<Card> display = new List<Card>();
    public int displayId;
    public int cardid;

    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text healthText;
    public TMP_Text energyText;
  //  public TMP_Text defenseText;
    public Image crdImage;

    public CardShuffler shuffler;

    private CanvasGroup canvasGroup;
    private Vector3 initialPosition;

    public bool isSelected = false;

    public string tagToSearch = "Player2";
    GameObject[] player2;

    public List<GameObject> adjacentCards = new List<GameObject>();

    public List<DisplayCard> allDisplayCards; // Reference to all DisplayCard instances
    UnityEngine.UI.Image outerBorder;

    public Image dice1;
    public Image dice2;

    public static bool Discard;

    public static int P1Power;
    public static int P2Power;

    public List<BoardSlot> BoSlots; //Reference to all BoardSlots

    public GameManager gm;

    public bool canMove;

    public GameObject PopUpCardP1;
    public TMP_Text popNameTxt;
    public TMP_Text popAttackTxt;
    public TMP_Text popHealthTxt;
    public TMP_Text popEnergyTxt;
    public Image popCardImg;
    UnityEngine.UI.Image popOuterBdr;

    public Animator popupAnim;

    public List<GameObject> movementAdjacentCards = new List<GameObject>();
    public string Movtag = "Player1";
    GameObject[] player1;

    public List<Transform> MoveBoardSlots = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCardInformation();

        player2 = GameObject.FindGameObjectsWithTag(tagToSearch);
        player1 = GameObject.FindGameObjectsWithTag(Movtag);

        // Populate allDisplayCards with all instances of DisplayCard
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());
        outerBorder = this.transform.Find("OuterBorder").GetComponent<Image>();
        popOuterBdr = PopUpCardP1.transform.Find("OuterBorder").GetComponent<Image>();

        BoSlots = new List<BoardSlot>(FindObjectsOfType<BoardSlot>());

        dice1.enabled = false;
        dice2.enabled = false;

        Discard = false;
        canMove = true;

        cardid = this.GetComponent<PhotonView>().ViewID;
        PhotonNetwork.AddCallbackTarget(this);
    }

   
    public List<Transform> AdjacentBSlotsAvailable() 
    {
        return MoveBoardSlots;
    }
    
    public IEnumerator CanMoveNow(float delay) 
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    public void UpdateCardInformation()
    {
        Scene cs = SceneManager.GetActiveScene();
        if (cs.name == "AI") 
        {
        Card card = display.Find(c => c.cardId == displayId);
        
        if (card != null)
        {
            nameText.text = card.cardName;
            attackText.text = card.cardAttack.ToString();
            healthText.text = card.cardHealth.ToString();
            energyText.text = card.cardEnergy.ToString(); 
         //   defenseText.text = card.cardDefence.ToString();
           crdImage.sprite = card.cardImage;
        }
        else
        {
            nameText.text = "Card Not Found";
            attackText.text = " ";
            healthText.text = " ";
            energyText.text = " ";
          //  defenseText.text = " ";
            crdImage.sprite = null;
        }
        }
        
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 4)
        {
            object[] data = (object[])photonEvent.CustomData;
            int receivedDisplayId = (int)data[0];
            int receivedCardId = (int)data[1];

            if (this.cardid == receivedCardId)
            {
                this.displayId = receivedDisplayId;
                UpdateClientInfo(receivedDisplayId);
            }
        }
    }

    public void UpdateClientInfo(int dispID) 
    {
        displayId = dispID;
        Card card = display.Find(c => c.cardId == displayId);

        if (card != null)
        {
            nameText.text = card.cardName;
            attackText.text = card.cardAttack.ToString();
            healthText.text = card.cardHealth.ToString();
            energyText.text = card.cardEnergy.ToString();
            //   defenseText.text = card.cardDefence.ToString();
            crdImage.sprite = card.cardImage;
        }
        else
        {
            nameText.text = "Card Not Found";
            attackText.text = " ";
            healthText.text = " ";
            energyText.text = " ";
            //  defenseText.text = " ";
            crdImage.sprite = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        foreach (BoardSlot bslot in BoSlots)
        {
            //  bslot.UpdatePreviousCardsList();
           // bslot.UpdateMovementPhaseList();
            //PLAY HIGHLIGHT
            if (gm.currentPhase == GamePhase.Play && this.transform.parent.name == "Hand")
            { 
                if (bslot.PreviouslyPlacedAvailable().Contains(bslot.transform))
                {
                    // Highlight the slot
                    bslot.GetComponent<Image>().color = Color.green;
                }
            }

            if (gm.currentPhase == GamePhase.Move && this.transform.parent.tag == "BSlot")
            {
                // Cache components and avoid redundant calculations
                List<GameObject> cachedMovementAdjacentCards = new List<GameObject>();
                Dictionary<GameObject, DisplayCard> displayCardCache = new Dictionary<GameObject, DisplayCard>();
                Dictionary<Transform, Image> imageCache = new Dictionary<Transform, Image>();

                // Cache the current position to avoid accessing it multiple times in the loop
                Vector3 currentPosition = transform.position;

                foreach (GameObject p1 in player1)
                {
                    float distance = Vector3.Distance(p1.transform.position, currentPosition);
                    if (p1.gameObject.name == "SHCardP1")
                    {
                        if (distance < 210f && p1 != this.gameObject)
                        {
                            cachedMovementAdjacentCards.Add(p1);
                        }
                    }
                    else
                    {
                        if (!displayCardCache.TryGetValue(p1, out DisplayCard displayCard))
                        {
                            displayCard = p1.GetComponent<DisplayCard>();
                            displayCardCache[p1] = displayCard;
                        }

                        if (displayCard != null && displayCard.canMove)
                        {
                            if (distance < 210f && p1 != this.gameObject)
                            {
                                cachedMovementAdjacentCards.Add(p1);
                            }
                        }
                    }
                }

                HashSet<Transform> processedSlots = new HashSet<Transform>();

                foreach (GameObject gb in cachedMovementAdjacentCards)
                {
                    float dist = Vector3.Distance(bslot.transform.position, gb.transform.position);

                    if (dist < 210f && !processedSlots.Contains(bslot.transform))
                    {
                        if (!imageCache.TryGetValue(bslot.transform, out Image image))
                        {
                            image = bslot.GetComponent<Image>();
                            imageCache[bslot.transform] = image;
                        }

                        if (image != null)
                        {
                            image.color = Color.green;
                            MoveBoardSlots.Add(bslot.transform);
                            processedSlots.Add(bslot.transform);
                        }
                    }
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        int dragging = BoardSlot.GetCurrentEnergy();
        int dragging2 = BoardSlot.GetCurrentEnergyP2();
        bool isP1Turn = ButtonTurn.GetPlayerTurn();
        if (transform.parent!=null && transform.parent.name == "Hand"  && isP1Turn && gm.currentPhase == GamePhase.Play) 
        {
           if (dragging >= 0) 
           {
                 //transform.position = Input.mousePosition;

                 RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),Input.mousePosition, Camera.main,out Vector2 localPos);
                 transform.localPosition = localPos;

           }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Card card = display.Find(c => c.cardId == displayId);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        foreach (BoardSlot bslot in BoSlots)
        {
            for (int i = 0; i < bslot.transform.parent.childCount; i++)
            {
                Transform slot = bslot.transform.parent.GetChild(i);
                slot.GetComponent<Image>().color = Color.white;
            }
        }
        MoveBoardSlots.Clear();

        // If the card is not dropped on a slot, return it to the initial position.
        if (gm.currentPhase == GamePhase.Play  || gm.currentPhase == GamePhase.Draw) 
        {
            if (transform.parent == null || transform.parent.CompareTag("Hand") )
            {
                transform.position = initialPosition;
                int cardEnergy = GetCardEnergy();
            }
        }

        foreach (GameObject p1 in player1)
        {
            float distance = Vector3.Distance(p1.transform.position, transform.position);

            if (distance < 210f)
            {
                UnityEngine.UI.Image p1outerborder = p1.transform.Find("OuterBorder").GetComponent<Image>();
                p1outerborder.color = Color.black;

                //adjacentCards.Add(p1);
                movementAdjacentCards.Clear();
            }
        }

    }

    public int GetCardEnergy()
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardEnergy;
        }
        return 0; // Return 0 if the card is not found.
    }

    public int GetCardAttack() 
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardAttack;
        }
        return 0; // Return 0 if the card is not found.
    }

    public int GetCardHealth()
    {
        Card card = display.Find(c => c.cardId == displayId);
        if (card != null)
        {
            return card.cardHealth;
        }
        return 0; // Return 0 if the card is not found.
    }

    public void OnPtcClk() 
    {
        bool isP1Turn = ButtonTurn.GetPlayerTurn();    // Debug.Log("DC P1 Turn:"+isTurn);
        Debug.Log("P1TURN IN DISPLAYCARD:"+isP1Turn);
        /* //bool isZoom = Zoom.GetBool();
         if (isZoom)
         {
              Vector3 offt1 = new Vector3(-400f, 0, 0);
              dice1.transform.position = this.transform.position + offt1;
              dice2.transform.position = this.transform.position - offt1;
         } */
        Scene s = SceneManager.GetActiveScene();
        
        if (isP1Turn)
        {

            isSelected = !isSelected;
            if (isSelected)
            {
                //PopUpCardP1.SetActive(true);
                popNameTxt.text = nameText.text;
                popAttackTxt.text = attackText.text;
                popEnergyTxt.text = energyText.text;
                popHealthTxt.text = healthText.text;
                popCardImg.sprite = crdImage.sprite;
                popOuterBdr.color = Color.yellow;
                popupAnim.SetBool("Select",true);
                outerBorder.color = Color.white;

                foreach (GameObject p2 in player2)
                {
                    float distance = Vector3.Distance(p2.transform.position, transform.position);

                    if (distance < 210f)
                    {
                        UnityEngine.UI.Image p2outerborder = p2.transform.Find("OuterBorder").GetComponent<Image>();
                        p2outerborder.color = Color.blue;

                        adjacentCards.Add(p2);

                    }
                }

                // Unselect other DisplayCard instances
                foreach (DisplayCard otherCard in allDisplayCards)
                {
                    if (otherCard != this && otherCard.isSelected)
                    {
                        //otherCard.isSelected = false;
                        //otherCard.PopUpCardP1.SetActive(false);
                        otherCard.OnPtcClk();
                        otherCard.adjacentCards.Clear();
                        otherCard.outerBorder.color = Color.black;

                    }
                }

            }
            if (!isSelected)
            {
                outerBorder.color = Color.black;
                popupAnim.SetBool("Select",false);
              //  StartCoroutine(PopUpActiveFalse(0.6f));
                //PopUpCardP1.SetActive(false); //HERE

                // DisplayCard2.dice1.enabled = false;
                // Reset the orthographic camera's size when the card is deselected


                foreach (GameObject p2 in player2)
                {
                    float distance = Vector3.Distance(p2.transform.position, transform.position);

                    if (distance < 210f)
                    {
                        UnityEngine.UI.Image p2outerborder = p2.transform.Find("OuterBorder").GetComponent<Image>();
                        p2outerborder.color = Color.yellow; //FFFF00
                        adjacentCards.Clear();

                        // Check if DisplayCard2 component is attached
                        DisplayCard2 displayCard2 = p2.GetComponent<DisplayCard2>();
                        if (displayCard2 != null)
                        {
                            bool selected = displayCard2.isSelected;
                            if (selected)
                            {
                                Debug.Log("IT WAS SELECTED BEFORE: " + p2.name);
                                selected = false;
                                displayCard2.SetSelected(selected);
                                displayCard2.OnPtClc();
                                dice1.enabled = false;
                                dice2.enabled = false;
                                //dice1.gameObject.SetActive(false);
                                //dice2.gameObject.SetActive(false);
                                //displayCard2.OnPtClc();
                            }
                        }

                        // Check if ShP2Card component is attached
                        ShP2Card shP2Card = p2.GetComponent<ShP2Card>();
                        if (shP2Card != null)
                        {
                            bool selection = shP2Card.isSelected;
                            if (selection)
                            {
                                Debug.Log("IT WAS SELECTED BEFORE: " + p2.name);
                                //selection = false;
                               // shP2Card.setSelection(selection);
                                shP2Card.OnptcClick();
                                dice1.enabled = false;
                                dice2.enabled = false;
                            }
                        }
                    }
                }
            }
        }
        if (!isP1Turn)
        {
            foreach (GameObject displayCardObject in player2)
            {
                DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                if (dp != null && dp.adjCards.Contains(gameObject))
                {
                    foreach (DisplayCard lastselected in allDisplayCards)
                    {

                        if (lastselected.isSelected)
                        {
                            lastselected.isSelected = false;
                            lastselected.outerBorder.color = Color.blue;
                        }
                        else
                        {
                            isSelected = !isSelected;
                        }
                    }
                    isSelected = !isSelected;
                }
            }


            if (isSelected)  //(3):ATTACKING PHASE
            {
                if (gm.currentPhase == GamePhase.Attack) 
                {
                   // PopUpCardP1.SetActive(true);
                    popNameTxt.text = nameText.text;
                    popAttackTxt.text = attackText.text;
                    popEnergyTxt.text = energyText.text;
                    popHealthTxt.text = healthText.text;
                    popCardImg.sprite = crdImage.sprite;
                    popOuterBdr.color = Color.red;
                    popupAnim.SetBool("Select",true);
                }

                foreach (GameObject displayCardObject in player2)
                {
                    DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                    if (dp != null && dp.adjCards.Contains(gameObject) && BoardSlot.GetCurrentEnergyP2() >= 2 && gm.currentPhase == GamePhase.Attack)
                    {
                        outerBorder.color = Color.red;
                        Debug.Log("Player2's Card Attack:" + dp.GetCardAttack());
                        dice1.enabled = true;
                        dice2.enabled = true;

                        shuffler.AttackSound();

                        P1Power = this.GetCardHealth();
                        P2Power = dp.GetCardAttack();
                        /* if (this.GetCardAttack() < dp.GetCardAttack()) 
                         {
                             Discard = true;
                         }*/
                    }
                }
            }
            if (!isSelected)
            {
                //  PopUpCardP1.SetActive(false);
                popupAnim.SetBool("Select",false);
              //  StartCoroutine(PopUpActiveFalse(0.6f));

                foreach (GameObject displayCardObject in player2)
                {
                    DisplayCard2 dp = displayCardObject.GetComponent<DisplayCard2>();
                    if (dp != null && dp.adjCards.Contains(gameObject))
                    {
                        outerBorder.color = Color.blue;
                        dice1.enabled = false;
                        dice2.enabled = false;
                    }
                }
            }
        }

        Debug.Log(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPtcClk();
    }

    public bool GetDiscard() 
    {
        return Discard;
    }

    public bool GetSelected() 
    {
        return isSelected;
    }

    public int Getp1Power() 
    {
        return P1Power;
    }

    public int Getp2Power() 
    {
        return P2Power;
    }

    public void SetSelected(bool selection) 
    {
        isSelected = selection;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     //   Debug.Log("Hovering over:"+gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    IEnumerator PopUpActiveFalse(float delay) 
    {
        yield return new WaitForSeconds(delay);
        PopUpCardP1.SetActive(false);
    }

    [PunRPC]
    public void DisableCardBack2RPC()
    {
        Image cardBackImage = transform.Find("Back").GetComponent<Image>();
        if (cardBackImage != null)
        {
            cardBackImage.enabled = false;
        }
    }


}
