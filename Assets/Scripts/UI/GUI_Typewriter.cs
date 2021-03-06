﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Typewriter effect for UI Text component.
///
/// Originally from Time Cannon, authored by @mcfunkypants
/// Modified to use TMP component rather than vanilla text component.
/// </summary>

public class GUI_Typewriter : MonoBehaviour {

    // public Text animateText;
    public TextMeshProUGUI animateText;
    public float holdTime = 4.0f;
    public float fadeTime = 2.0f;

    public UnityEvent typewriterComplete;
    public bool skipped = false;
    public bool eventInvoked = false;

    public float age = 0.0f;
    private string fullText;

	[Header("Audio")]
	[SerializeField] private AudioClip typeSound;

	private int oldLen = 0;

	public void Start() {
        fullText = animateText.text;
        // Debug.Log("TYPEWRITER will type out: "+fullText);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            skipped = true;
        }
        
        // Use bool to invoke the event only once.
        if (age > holdTime && !eventInvoked)
        {
            typewriterComplete.Invoke();
            eventInvoked = true;
        }

        // Typewriter & fade effect have completed.
        if (age>holdTime+fadeTime) {
            typewriterComplete.Invoke();
            gameObject.active = false; // go away forever
            return;
        }

        // Skip to end of typewriter effect.
        if (skipped && age <= holdTime)
        {
            age = holdTime;
            animateText.text = fullText;
        }
        
        age +=  Time.deltaTime;
        float percent = 1.0f;

        // Type in text over holdTime duration.
        if (age<holdTime) {
            percent = age/holdTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            int len = (int)(fullText.Length*percent);
            string temp = fullText.Substring(0,len+1);
            // Debug.Log("TYPEWRITER"+len+": "+temp);
            animateText.text = temp;

			if (oldLen < len - 1)
			{
				AudioManager.Instance.PlaySoundSFX(typeSound, gameObject, Random.Range(0.6f, 0.8f), Random.Range(0.95f, 1f), 0f);
				oldLen = len;
			}

		}

        // Fade out full text.
        if (age>holdTime) {
            
            percent = (age-holdTime)/fadeTime;
            if (percent<0) percent = 0;
            if (percent>1) percent = 1;

            // Debug.Log("Typewriter percent:"+percent+" age:"+age);

            var animateTextColor = animateText.color;
            animateText.color = new Color(
                animateTextColor.r,
                animateTextColor.g,
                animateTextColor.b,
                1 - percent);
        }

    }

}