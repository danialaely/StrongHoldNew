using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Dice : MonoBehaviour
{

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
   // private SpriteRenderer rend;
    private Image image;
    public Image otherdice;

    private static int finalSide=0;
    private static int finalSide2 = 0;

    public List<DisplayCard2> allDpCards;
    public List<DisplayCard> allDisplayCards;

    public GameObject discardpile;
    public GameObject discardpile2;

    public Animator discaranimator;
    public Animator animator2;

    public AudioSource src;
    public AudioClip diceClip;
    public AudioClip discardedClip;

    public BoardSlot bslot;

    public bool CanAttackP1;
    public bool CanAttackP2;

    public bool CounterAttackinP1;
    public bool CounterAttackinP2;

    public ShP1Card StrongHoldCardP1;
    public ShP2Card StrongHoldCardP2;

    public ButtonTurn btnTurn;

  //  public TMP_Text SHoldHealthP1;
  //  float healthValStP1;
   
  //  public TMP_Text SHoldHealthP2;
  //  float healthValStP2;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component
       // rend = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        allDpCards = new List<DisplayCard2>(FindObjectsOfType<DisplayCard2>());
        allDisplayCards = new List<DisplayCard>(FindObjectsOfType<DisplayCard>());

        CanAttackP1 = true; 
        CanAttackP2 = true;

        CounterAttackinP1 = false;
        CounterAttackinP2 = false;

      //  healthValStP1 = 100.0f;
      //  SHoldHealthP1.text = healthValStP1.ToString();

      //  healthValStP2 = 100.0f;
      //  SHoldHealthP2.text = healthValStP2.ToString();

 
    }

    // If you left click over the dice then RollTheDice coroutine is started
    public void OnMouseDown()
    {
        DiceSound();
        StartCoroutine(RollAndDiscard());
    }

    private IEnumerator RollAndDiscard() 
    {
        yield return StartCoroutine(RollTheDice());
        yield return StartCoroutine(RollTheDice2());
        Discarded();
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
       // int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            image.sprite = diceSides[randomDiceSide];
            
            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log("Our Dice:"+finalSide);
    }

    private IEnumerator RollTheDice2()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide2 = 0;

        // Final side or value that dice reads in the end of coroutine
        //int finalSide2 = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide2 = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            otherdice.sprite = diceSides[randomDiceSide2];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide2 = randomDiceSide2 + 1;

        // Show final dice value in Console
        Debug.Log("Opponent's Dice:"+finalSide2);
    }

    public int GetDice() 
    {
        return finalSide;
    }

    public int GetDice2() 
    {
        return finalSide2;
    }

    public IEnumerator CanAttackNowP1(float delay) 
    {
        yield return new WaitForSeconds(delay);
        CanAttackP1 = true;
    }
    public IEnumerator CanAttackNowP2(float delay)
    {
        yield return new WaitForSeconds(delay);
        CanAttackP2 = true;
    }
    public IEnumerator CountAttackP1(float del) 
    {
        yield return new WaitForSeconds(del);
        CounterAttackinP1 = false;
    }

    public IEnumerator CountAttackP2(float del)
    {
        yield return new WaitForSeconds(del);
        CounterAttackinP2 = false;
    }

    

    public void Discarded() 
    {
        //if (GetDice() > GetDice2()) {Debug.Log(" Dice Attack True");}
        Scene currentScene = SceneManager.GetActiveScene();

        bool isP1Turn = ButtonTurn.GetPlayerTurn();
            if (isP1Turn && !CounterAttackinP1)
            {//DisplayCard2
             //  bslot.SpendOnAttack();
            int val = BoardSlot.GetCurrentEnergy();
            if (val > 2) 
            {
                val -= 2;
                BoardSlot.SetCurrentEnergy(val);
                bslot.energyText.text = val.ToString();
                Debug.Log("Value:"+val);
                
                if (val == 6)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                }
                if (val == 5)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                }
                if (val == 4)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                    bslot.coinP1img5.SetActive(false);
                }
                if (val == 3)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                    bslot.coinP1img5.SetActive(false);
                    bslot.coinP1img4.SetActive(false);
                }
                if (val == 2)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                    bslot.coinP1img5.SetActive(false);
                    bslot.coinP1img4.SetActive(false);
                    bslot.coinP1img3.SetActive(false);
                }
                if (val == 1)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                    bslot.coinP1img5.SetActive(false);
                    bslot.coinP1img4.SetActive(false);
                    bslot.coinP1img3.SetActive(false);
                    bslot.coinP1img2.SetActive(false);
                }
                if (val == 0)
                {
                    bslot.coinP1img8.SetActive(false);
                    bslot.coinP1img7.SetActive(false);
                    bslot.coinP1img6.SetActive(false);
                    bslot.coinP1img5.SetActive(false);
                    bslot.coinP1img4.SetActive(false);
                    bslot.coinP1img3.SetActive(false);
                    bslot.coinP1img2.SetActive(false);
                    bslot.coinP1img.SetActive(false);
                }

            }
            

            foreach (DisplayCard2 defenderCard in allDpCards)
                {
                    Debug.Log("Selected:"+defenderCard.isSelected);
                    if (defenderCard.isSelected)
                    {
                    if (CanAttackP1) 
                    {
                        if ((GetDice() + defenderCard.GetP1Power()) > 2 * ((GetDice2()) + defenderCard.GetP2Power()))
                        {
                            Debug.Log("Attack More Than Twice");
                            //  Debug.Log("Discard Value:"+ defenderCard.GetDiscard());
                            //Destroy(defenderCard.gameObject);
                            //defenderCard.transform.position += new Vector3(600f,-300f,0f);
                            discaranimator.SetBool("isDiscard", true);
                            Transform discarcard = defenderCard.transform;
                            discarcard.SetParent(discardpile.transform);
                            btnTurn.CardPlacedToBoard.Remove(defenderCard.gameObject);
                            defenderCard.OnPtClc(); //MADE CHANGES HERE
                            DiscardSound();

                          //  healthValStP2 -= 2.5f;
                          //  SHoldHealthP2.text = healthValStP2.ToString();

                        }
                        else if ((GetDice() + defenderCard.GetP1Power()) > ((GetDice2()) + defenderCard.GetP2Power())) 
                        {
                            Debug.Log("ATTACKER Dice:" + GetDice() + "+" + "Attack:" + defenderCard.GetP1Power() + "=" + (GetDice() + defenderCard.GetP1Power()));
                            Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + defenderCard.GetP2Power() + "=" + (GetDice2() + defenderCard.GetP2Power()));

                            discaranimator.SetBool("isDiscard", true);
                            Transform discarcard = defenderCard.transform;
                            discarcard.SetParent(discardpile.transform);
                            if (currentScene.name == "AI")
                            {
                                btnTurn.CardPlacedToBoard.Remove(defenderCard.gameObject);
                            }
                            defenderCard.OnPtClc(); //MADE CHANGES HERE
                            DiscardSound();

                           // healthValStP2 -= 2.5f;
                           // SHoldHealthP2.text = healthValStP2.ToString();

                            CanAttackP1 = false;
                            StartCoroutine(CanAttackNowP1(10.0f));
                        }
                        else if ((GetDice() + defenderCard.GetP1Power()) * 2 < ((GetDice2()) + defenderCard.GetP2Power()))   //APPLY DEFENSE MECHANISM HERE
                        {
                            Debug.Log("Defense More Than Twice");
                            Debug.Log("Hit");

                            int value = BoardSlot.GetCurrentEnergyP2();
                            value -= 1;
                            BoardSlot.SetCurrentEnergyP2(value); //czczczczczczc

                            if (value == 6)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                            }
                            if (value == 5)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                            }
                            if (value == 4)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                                bslot.coinP2img5.SetActive(false);
                            }
                            if (value == 3)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                                bslot.coinP2img5.SetActive(false);
                                bslot.coinP2img4.SetActive(false);
                            }
                            if (value == 2)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                                bslot.coinP2img5.SetActive(false);
                                bslot.coinP2img4.SetActive(false);
                                bslot.coinP2img3.SetActive(false);
                            }
                            if (value == 1)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                                bslot.coinP2img5.SetActive(false);
                                bslot.coinP2img4.SetActive(false);
                                bslot.coinP2img3.SetActive(false);
                                bslot.coinP2img2.SetActive(false);
                            }
                            if (value == 0)
                            {
                                bslot.coinP2img8.SetActive(false);
                                bslot.coinP2img7.SetActive(false);
                                bslot.coinP2img6.SetActive(false);
                                bslot.coinP2img5.SetActive(false);
                                bslot.coinP2img4.SetActive(false);
                                bslot.coinP2img3.SetActive(false);
                                bslot.coinP2img2.SetActive(false);
                                bslot.coinP2img.SetActive(false);
                            }

                            DiceSound();
                            StartCoroutine(RollAndDiscard());
                            CounterAttackinP1 = true;
                            CanAttackP1 = false;
                            StartCoroutine(CanAttackNowP1(10.0f));
                            foreach (DisplayCard atcCard in allDisplayCards) 
                            {
                                if (atcCard.isSelected) 
                                {
                                    StartCoroutine(delay());
                                }
                                IEnumerator delay() 
                                {
                                    yield return new WaitForSeconds(3.0f);
                                    StartCoroutine(CountAttackP1(1.0f));
                                    Debug.Log("COUNTER ATTACK false hona chaahiye:"+CounterAttackinP1);
                                    if ((GetDice() + defenderCard.GetP1Power()) < ((GetDice2()) + defenderCard.GetP2Power()))
                                    {
                                        Debug.Log("Counter Attacked Succeeded");
                                        animator2.SetBool("isDiscarded",true);
                                        Transform discarded = atcCard.transform;
                                        discarded.SetParent(discardpile2.transform);
                                        atcCard.OnPtcClk();
                                        DiscardSound();

                                     //   healthValStP1 -= 2.5f;
                                     //   SHoldHealthP1.text = healthValStP1.ToString();
                                    }
                                    else { Debug.Log("Counter Attacked Failed"); }
                                }
                            }
                        }
                    }
                    }
                }
            if (StrongHoldCardP2.isSelected) 
            {
                if (CanAttackP1) 
                {
                    if ((GetDice() + StrongHoldCardP2.GetP1Power()) > ((GetDice2()) + StrongHoldCardP2.GetP2Power()))
                    {
                        Debug.Log("Attack Attack Attack");

                        Debug.Log("ATTACKER Dice:" + GetDice() + "+" + "Attack:" + StrongHoldCardP2.GetP1Power() + "=" + (GetDice() + StrongHoldCardP2.GetP1Power()));
                        Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + StrongHoldCardP2.GetP2Power() + "=" + (GetDice2() + StrongHoldCardP2.GetP2Power()));

                        int newHealth = StrongHoldCardP2.GetCardHealth() - StrongHoldCardP2.GetP1Power();
                        StrongHoldCardP2.SetSHealth(newHealth);
                        StrongHoldCardP2.healthText.text = newHealth.ToString();
                        Debug.Log("New StrongHealth:"+newHealth);

                        CanAttackP1 = false;
                        StartCoroutine(CanAttackNowP1(10.0f));

                        if (StrongHoldCardP2.GetCardHealth() <=0 ) 
                        {
                            Debug.Log("GAME OVER!, P1 Wins");
                        //discaranimator.SetBool("isDiscard", true);
                        // Transform discarcard = defenderCard.transform;
                        //  discarcard.SetParent(discardpile.transform);
                        //  DiscardSound();
                        }

                        //  healthValStP2 -= 2.5f;
                        //  SHoldHealthP2.text = healthValStP2.ToString();

                    }
                }
            }
                // discaranimator.SetBool("isDiscard", false);  make IEnumerator
                StartCoroutine(DiscardAnim(2.0f));
            }
            // isP1Turn = ButtonTurn.GetPlayerTurn() 
            // if(isP1Turn) then if(dp.isSelected == true) { Destroy(dp.gameobject) } 
            //defe.GetDiscard

            if (!isP1Turn && !CounterAttackinP2) 
            {
            //  bslot.SpendOnAttack2();
            int value = BoardSlot.GetCurrentEnergyP2();
            if (value > 2)
            {
                value -= 2;
                BoardSlot.SetCurrentEnergyP2(value);
                bslot.energyTextP2.text = value.ToString();
                Debug.Log("Value:" + value);

                if (value == 6)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                }
                if (value == 5)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                }
                if (value == 4)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                    bslot.coinP2img5.SetActive(false);
                }
                if (value == 3)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                    bslot.coinP2img5.SetActive(false);
                    bslot.coinP2img4.SetActive(false);
                }
                if (value == 2)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                    bslot.coinP2img5.SetActive(false);
                    bslot.coinP2img4.SetActive(false);
                    bslot.coinP2img3.SetActive(false);
                }
                if (value == 1)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                    bslot.coinP2img5.SetActive(false);
                    bslot.coinP2img4.SetActive(false);
                    bslot.coinP2img3.SetActive(false);
                    bslot.coinP2img2.SetActive(false);
                }
                if (value == 0)
                {
                    bslot.coinP2img8.SetActive(false);
                    bslot.coinP2img7.SetActive(false);
                    bslot.coinP2img6.SetActive(false);
                    bslot.coinP2img5.SetActive(false);
                    bslot.coinP2img4.SetActive(false);
                    bslot.coinP2img3.SetActive(false);
                    bslot.coinP2img2.SetActive(false);
                    bslot.coinP2img.SetActive(false);
                }

            }



            Debug.Log("in dice attack in isp1turn false");
                //DisplayCard
                foreach (DisplayCard defcard in allDisplayCards)
                {
                if (defcard.isSelected)
                {
                    if (CanAttackP2) 
                    {
                        if ((GetDice() + defcard.Getp2Power()) > 2 * ((GetDice2()) + defcard.Getp1Power()))
                        {
                            Debug.Log("Attack is more than twice the defence");
                            animator2.SetBool("isDiscarded", true);
                            Transform discardCard = defcard.transform;
                            discardCard.SetParent(discardpile2.transform);
                            if (currentScene.name == "SampleScene") 
                            {
                                defcard.OnPtcClk(); //MADE CHANGES HERE
                            }
                            // discardCard.GetComponent<DisplayCard>().SetSelected(false);
                            DiscardSound();

                        //    healthValStP1 -= 2.5f;
                        //    SHoldHealthP1.text = healthValStP1.ToString();
                        }
                        else if ((GetDice() + defcard.Getp2Power()) > ((GetDice2()) + defcard.Getp1Power()))
                        {
                            Debug.Log("ATTACKER Dice:" + GetDice() + "+" + "Attack:" + defcard.Getp2Power() + "=" + (GetDice() + defcard.Getp2Power()));
                            Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + defcard.Getp1Power() + "=" + (GetDice2() + defcard.Getp1Power()));
                            // Debug.Log("Discard Value:" +defcard.GetDiscard());
                            // Destroy(defcard.gameObject);
                            animator2.SetBool("isDiscarded", true);
                            Transform discardCard = defcard.transform;
                            discardCard.SetParent(discardpile2.transform);
                            if (currentScene.name == "SampleScene")
                            { 
                                defcard.OnPtcClk(); //MADE CHANGES HERE
                            }
                            //discardCard.GetComponent<DisplayCard>().SetSelected(false);
                            DiscardSound();

                        //    healthValStP1 -= 2.5f;
                        //    SHoldHealthP1.text = healthValStP1.ToString();
                            CanAttackP2 = false;
                            StartCoroutine(CanAttackNowP2(10.0f));
                        }
                        else if ((GetDice() + defcard.Getp2Power()) * 2 < ((GetDice2()) + defcard.Getp1Power())) 
                        {
                            Debug.Log("Defense More Than Twice");
                            Debug.Log("Hit");
                            DiceSound();

                            int val = BoardSlot.GetCurrentEnergy();
                            val -= 1;
                            BoardSlot.SetCurrentEnergy(val); //czczczczczczc

                            if (val == 6)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                            }
                            if (val == 5)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                            }
                            if (val == 4)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                                bslot.coinP1img5.SetActive(false);
                            }
                            if (val == 3)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                                bslot.coinP1img5.SetActive(false);
                                bslot.coinP1img4.SetActive(false);
                            }
                            if (val == 2)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                                bslot.coinP1img5.SetActive(false);
                                bslot.coinP1img4.SetActive(false);
                                bslot.coinP1img3.SetActive(false);
                            }
                            if (val == 1)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                                bslot.coinP1img5.SetActive(false);
                                bslot.coinP1img4.SetActive(false);
                                bslot.coinP1img3.SetActive(false);
                                bslot.coinP1img2.SetActive(false);
                            }
                            if (val == 0)
                            {
                                bslot.coinP1img8.SetActive(false);
                                bslot.coinP1img7.SetActive(false);
                                bslot.coinP1img6.SetActive(false);
                                bslot.coinP1img5.SetActive(false);
                                bslot.coinP1img4.SetActive(false);
                                bslot.coinP1img3.SetActive(false);
                                bslot.coinP1img2.SetActive(false);
                                bslot.coinP1img.SetActive(false);
                            }

                            StartCoroutine(RollAndDiscard());
                            CounterAttackinP2 = true;
                            CanAttackP2 = false;
                            StartCoroutine(CanAttackNowP2(10.0f));
                            foreach (DisplayCard2 atcCard in allDpCards) 
                            {
                                if (atcCard.isSelected)
                                {
                                    StartCoroutine(delayed());
                                }
                                IEnumerator delayed()
                                {
                                    yield return new WaitForSeconds(3.0f);
                                    StartCoroutine(CountAttackP2(1.0f));
                                    Debug.Log("COUNTER ATTACK false hona chaahiye:" + CounterAttackinP2);
                                    if ((GetDice() + defcard.Getp2Power()) < ((GetDice2()) + defcard.Getp1Power()))
                                    {
                                        Debug.Log("Counter Attacked Succeeded");
                                        discaranimator.SetBool("isDiscard", true);
                                        Transform discards = atcCard.transform;
                                        discards.SetParent(discardpile.transform);
                                        if (currentScene.name == "SampleScene")
                                        { 
                                            atcCard.OnPtClc();
                                        }
                                        Debug.Log("Discard Pile Name:"+discardpile.name);
                                        DiscardSound();

                                     //   healthValStP2 -= 2.5f;
                                     //   SHoldHealthP2.text = healthValStP2.ToString();
                                    }
                                    else { Debug.Log("Counter Attacked Failed"); }
                                }
                            }
                        }
                    }
                    }
                }
            if (StrongHoldCardP1.isSelected) 
            {
                if (CanAttackP2) 
                {
                    if ((GetDice() + StrongHoldCardP1.GetP2Power()) > ((GetDice2()) + StrongHoldCardP1.GetP1Power()))
                    {
                        Debug.Log("ATTACKER Dice:" + GetDice() + "+" + "Attack:" + StrongHoldCardP1.GetP2Power() + "=" + (GetDice() + StrongHoldCardP1.GetP2Power()));
                        Debug.Log("DEFENSE Dice:" + GetDice2() + "+" + "Attack:" + StrongHoldCardP1.GetP1Power() + "=" + (GetDice2() + StrongHoldCardP1.GetP1Power()));

                        int newHealthP1 = StrongHoldCardP1.GetCardHealth() - StrongHoldCardP1.GetP2Power();
                        StrongHoldCardP2.SetSHealth(newHealthP1);
                        StrongHoldCardP2.healthText.text = newHealthP1.ToString();
                        Debug.Log("New StrongHealth:" + newHealthP1);

                        CanAttackP2 = false;
                        StartCoroutine(CanAttackNowP2(10.0f));

                        if (StrongHoldCardP1.GetCardHealth() <=0) 
                        {
                            Debug.Log("GAME OVER, P2 WINS");
                        // Debug.Log("Discard Value:" +defcard.GetDiscard());
                        // Destroy(defcard.gameObject);
                        // animator2.SetBool("isDiscarded", true);
                        //Transform discardCard = defcard.transform;
                        //discardCard.SetParent(discardpile2.transform);
                        //DiscardSound();
                        }

                        //    healthValStP1 -= 2.5f;
                        //    SHoldHealthP1.text = healthValStP1.ToString();
                       
                    }
                }
            }

                StartCoroutine(DiscardAnim2(2.0f));
            }
    }

    public void DiceSound() 
    {
        src.clip = diceClip;
        src.Play();
    }

    public void DiscardSound()
    {
        src.clip = discardedClip;
        src.Play();
    }

    private IEnumerator DiscardAnim(float delay) 
    {
        yield return new WaitForSeconds(delay);
        discaranimator.SetBool("isDiscard", false);
    }

    private IEnumerator DiscardAnim2(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator2.SetBool("isDiscarded", false);
    }
}
