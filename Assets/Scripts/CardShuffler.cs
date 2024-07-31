using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CardShuffler : MonoBehaviour
{
    public List<DisplayCard> displayCards; // List of all display cards
    public Button shuffleButton; // Reference to the shuffle button
    public Animator cardAnimator; // Reference to the Animator component on the object you want to animate

   
    public GameObject deck;
    public GameObject hand;
    
    public BoardSlot boardSlot;
    public DisplayCard dc;

    private Dictionary<int, int> usedDetailsCount = new Dictionary<int, int>(); // Track used detail counts

    public GameManager gm;

    public AudioSource src;
    public AudioClip swordClip;

    public Zoom zm;

    private void Start()
    {
       // boardSlot = FindAnyObjectByType<BoardSlot>();
       //Scene currentS = SceneManager.GetActiveScene();
   
        foreach (var card in displayCards)
        {
            onStartShuffle(card);
        }

        // Add a click listener to the shuffle button
        if (!PhotonNetwork.IsMasterClient) 
        {
       // ShuffleCards();
        }
       // shuffleButton.onClick.AddListener(ShuffleCards);
    }

    private void onStartShuffle(DisplayCard c) 
    {
        int randomDetail;
        int detailCount;

        // Generate a random detail until it doesn't exceed the limit of 2
        do
        {
            randomDetail = Random.Range(0, 40);
            usedDetailsCount.TryGetValue(randomDetail, out detailCount);
        } while (detailCount >= 2);

        // Update the used detail count
        usedDetailsCount[randomDetail] = detailCount + 1;

        // Set the random detail ID and update the card information
        if (c.transform.parent.name == "Deck"  || c.transform.parent.name == "Deck2")
        {
            c.displayId = randomDetail;
            c.UpdateCardInformation();
        }


    }

    public void ShuffleCards()
    {
        if (shuffleButton.gameObject.name == "Shuffle" && gm.currentPhase == GamePhase.Draw) 
        {
         //boardSlot.AnotherMethod();
        }

        if (shuffleButton.gameObject.name == "Shuffle2" && gm.currentPhase == GamePhase.Draw) 
        {
         boardSlot.AnotherMethod2();
        }

        StartCoroutine(CardsDelay(2.1f));

        // Play the shuffle animation
        int handchildCount = hand.transform.childCount;

        if (handchildCount==0 && gm.currentPhase == GamePhase.Draw) 
        {
        cardAnimator.SetTrigger("ShuffleTrigger");
          zm.DeckSound();
        StartCoroutine(BackToDefault(3));
        }
        if (handchildCount == 7 && gm.currentPhase == GamePhase.Draw) 
        {
            cardAnimator.SetTrigger("ShuffleTrigger2");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 6 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger3");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 5 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger4");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 4 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger5");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 3 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger6");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 2 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger7");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        if (handchildCount == 1 && gm.currentPhase == GamePhase.Draw)
        {
            cardAnimator.SetTrigger("ShuffleTrigger8");
            zm.DeckSound();
            StartCoroutine(BackToDefault(2.0f));
        }
        // Reset the used detail count dictionary
        usedDetailsCount.Clear();

        foreach (var card in displayCards)
        {
            ShuffleCard(card);
        }
    }

    private void ShuffleCard(DisplayCard card)
    {
        // Initialize variables to track the random detail and its count
        int randomDetail;
        int detailCount;

        // Generate a random detail until it doesn't exceed the limit of 2
        do
        {
            randomDetail = Random.Range(0, 40);
            usedDetailsCount.TryGetValue(randomDetail, out detailCount);
        } while (detailCount >= 2);

        // Update the used detail count
        usedDetailsCount[randomDetail] = detailCount + 1;

        // Set the random detail ID and update the card information
        if (card.transform.parent.name == "Deck" || card.transform.parent.name == "Deck2") 
        {
        card.displayId = randomDetail;
        card.UpdateCardInformation();
        }


    }

    IEnumerator BackToDefault(float delay)
    {
        yield return new WaitForSeconds(delay);
        cardAnimator.SetTrigger("BackTrigger");
    }

    public void AttackSound() 
    {
        src.clip = swordClip;
        src.Play();
    }

    private IEnumerator CardsDelay(float delay) 
    {
        int childCount = deck.transform.childCount;
        int handchildCount = hand.transform.childCount;
        yield return new WaitForSeconds(delay);

        if (childCount > 0)
        {
            // if(handchildCount == 8){ Debug.log("Hand Already full"); }  if(handchildCount == 7){ lastcard }   if(handchildCount == 6){ lastcard secondLastCard} ....

            // Find the last child (last card) under the Deck
            Transform lastCard = deck.transform.GetChild(childCount - 1);        //deck's upper card
            Transform secondLastCard = deck.transform.GetChild(childCount - 2);    //2nd card
            Transform thirdLastCard = deck.transform.GetChild(childCount - 3);   //3rd card
            Transform fourthLastCard = deck.transform.GetChild(childCount - 4);  //4th card
            Transform fifthLastCard = deck.transform.GetChild(childCount - 5);   //5th card
            Transform sixthLastCard = deck.transform.GetChild(childCount - 6);   //6th card
            Transform seventhLastCard = deck.transform.GetChild(childCount - 7); //7th card
            Transform eightLastCard = deck.transform.GetChild(childCount - 8);   //8th card

            if (handchildCount == 8 && gm.currentPhase == GamePhase.Draw) 
            {
                Debug.Log("Hand Already Fill");
            }
            if (handchildCount == 7 && gm.currentPhase == GamePhase.Draw) 
            {
                lastCard.SetParent(hand.transform);
                //handlist.add(lastcard);
            }
            if(handchildCount == 6 && gm.currentPhase == GamePhase.Draw) 
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                //handlist.add(lastcard);
                //handlist.add(secondLastCard);
            }
            if (handchildCount == 5 && gm.currentPhase == GamePhase.Draw)
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                thirdLastCard.SetParent(hand.transform);
            }
            if (handchildCount == 4 && gm.currentPhase == GamePhase.Draw)
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                thirdLastCard.SetParent(hand.transform);
                fourthLastCard.SetParent(hand.transform);
            }
            if (handchildCount == 3 && gm.currentPhase == GamePhase.Draw)
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                thirdLastCard.SetParent(hand.transform);
                fourthLastCard.SetParent(hand.transform);
                fifthLastCard.SetParent(hand.transform);
            }
            if (handchildCount == 2 && gm.currentPhase == GamePhase.Draw)
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                thirdLastCard.SetParent(hand.transform);
                fourthLastCard.SetParent(hand.transform);
                fifthLastCard.SetParent(hand.transform);
                sixthLastCard.SetParent(hand.transform);
            }
            if (handchildCount == 1 && gm.currentPhase == GamePhase.Draw)
            {
                lastCard.SetParent(hand.transform);
                secondLastCard.SetParent(hand.transform);
                thirdLastCard.SetParent(hand.transform);
                fourthLastCard.SetParent(hand.transform);
                fifthLastCard.SetParent(hand.transform);
                sixthLastCard.SetParent(hand.transform);
                seventhLastCard.SetParent(hand.transform);
            }
            if (handchildCount == 0 && gm.currentPhase == GamePhase.Draw) 
            {
            // Change the parent of the last card to the Hand
            lastCard.SetParent(hand.transform);
            secondLastCard.SetParent(hand.transform);
            thirdLastCard.SetParent(hand.transform);
            fourthLastCard.SetParent(hand.transform);
            fifthLastCard.SetParent(hand.transform);
            sixthLastCard.SetParent(hand.transform);
            seventhLastCard.SetParent(hand.transform);
            eightLastCard.SetParent(hand.transform);
            }
        }
    }
}
