using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDManager : MonoBehaviour
{
    //This is a reference to the Image component on the HUD gameobject that will have an empty slot for a Sprite that will indicate the player
    [SerializeField]
    private Image playerHeadImage = null;
    //This is a reference to the Slider component on the HUD gameobject that will deplete and fill up depending on the current health value
    [SerializeField]
    private Slider healthBarAmount = null;
    //This is a reference to the Text component on the HUD gameobject that will display how much time has passed since the level started
    [SerializeField]
    private Text timeText = null;
    //This is the message that will display in the Text slot for the timeText text value
    [SerializeField]
    private string timeTextMessage = null;
    //This is a float value will help speed up or slow down how fast the health bar changes in between different values
    [SerializeField]
    private float sliderFillSpeed = .5f;
    //This sets up the HUD so on start it will fill up the health bar
    private float timeTillFill = 0;
    //This is a float that represents how much of the healthBarAmount is currently filled at
    private float currentHealthValue;
    //This is a float value that represents where the healthBarAmount should be filled at if it isn't currently
    private float newHealthValue = 100;
    //This is a float value that represents how long its been since the level started
    private float timeInLevel;

    private void Start()
    {
        //Sets up the sprite value on the playerHeadImage to the Sprite that is being used as the player head
        playerHeadImage.sprite = FindObjectOfType<HeadSpriteFinder>().GetComponent<SpriteRenderer>().sprite;
        //This adds the delegate event on the Health script to this script so when the current health value has changed, this script can run the ChangeValue() method
        Health.UpdateHealth += ChangeValue;
    }

    // Update is called once per frame
    private void Update()
    {
        //A method that constantly adds to the timeInLevel value as long as player is alive
        AddTime();
        //A method that will change the healthBarAmount if the currentHealthValue and newHealthValue are different
        NewBarValue();
    }

    private void AddTime()
    {
        //Checks to see if the player has a currentHealth value of greater than 0
        if (currentHealthValue > 0)
        {
            //Adds time to the timeInLevel float
            timeInLevel += Time.deltaTime;
            //Sets the text value for the timeText component to whatever the time is and whatever decimal place you want to round to
            timeText.text = timeTextMessage + Math.Round(timeInLevel, 2);
        }
    }

    //A method that is called whenever the Health script either takes damage or heals, is called based on a delegate event system from the Health script
    private void ChangeValue(int amount)
    {
        //Sets the newHealthValue to the amount the player either took damage or healed
        newHealthValue = amount;
        //Resets the timeTillFill value back to 0 to allow the bar to fill up with a lerp
        timeTillFill = 0;
    }

    //Method that udpates the healthBarAmount based on the currentHealthValue and newHealthValue and then smoothly transitions the healthBarAmount between the two values with a lerp
    private void NewBarValue()
    {
        //Checks to see if the currentHealthValue and newHealthValue are different
        if (currentHealthValue != newHealthValue)
        {
            //Smoothly sets the currentHealthValue to the newHealthValue by lerping between the two
            currentHealthValue = Mathf.Lerp(currentHealthValue, newHealthValue, timeTillFill);
            //The 't' value in the lerp, this value transitions between 0 and 1 to allow the smooth transition of the lerp
            timeTillFill += sliderFillSpeed * Time.deltaTime;
        }
        //Sets the healthBarAmount slider value to the currentHealthValue
        healthBarAmount.value = currentHealthValue;
    }
}
