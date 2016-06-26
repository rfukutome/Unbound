using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////
//   Class:      PlayerItemInteraction
//   Purpose:    This class handles all of the interactions that the players
//   have with items. This includes money, guns, experience, etc.
//
//   Notes: Attach onto the player
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class PlayerItemInteraction : MonoBehaviour {

    //EXPERIENCE
    float exp = 0;
    int money = 0;
    int level = 1;

    private float expNeeded = 35;
    private Text moneyText;
    private Slider expSlider;
    private Text lvlText;
	// Use this for initialization
	void Awake() {
        //Set UI sliders and numbers here.
        moneyText = GameObject.Find("moneyText").GetComponent<Text>();
        expSlider = GameObject.Find("expSlider").GetComponent<Slider>();
        lvlText = GameObject.Find("lvlText").GetComponent<Text>();

        expSlider.maxValue = expNeeded;
        expSlider.value = exp;

        lvlText.text = "Lv " + level;
        moneyText.text = "URC " + money;
	}
	
    public void addExperience(float expDrop)
    {
        exp += expDrop;
            
        //If there is enough exp to level up
        if (exp >= expNeeded)
        {
            level++;
            lvlText.text = "Lv " + level;
            exp -= expNeeded;
            expNeeded *= 1.5f;
            expSlider.maxValue = expNeeded;
        }
        expSlider.value = exp;
    }

    public void addMoney(int moneyDrop)
    {
            money += moneyDrop;
            moneyText.text = "URC " + money;
    }
}
