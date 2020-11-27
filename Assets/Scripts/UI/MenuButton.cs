using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{

    //From Thomas Brush tutorial https://www.youtube.com/watch?v=vqZjZ6yv1lA

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;

    public UnityEvent buttonActions;
    public bool usingMouse = false;

	[Header("Audio")]
	[SerializeField] private AudioClip hoverSound;
	[SerializeField] private AudioClip selectSound;
	[SerializeField] private AudioClip exitSound;
	private AudioSource currentHoverSound;

	// Start is called before the first frame update
	void Start()
    {
        Physics.queriesHitTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputMethod();
        if (usingMouse) return;

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("Selected", true);
            if(Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("Pressed", true);
            }else if (animator.GetBool("Pressed"))
            {
                animator.SetBool("Pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }

    public void ButtonFunction()
    {
        buttonActions.Invoke();
    }

    public void MouseHover()
    {
        usingMouse = true;
        menuButtonController.index = thisIndex;
        animator.SetBool("Selected", true);
		currentHoverSound = AudioManager.Instance.PlaySoundSFX(hoverSound, gameObject, Random.Range(0.8f, 1f), Random.Range(0.9f, 1.1f), 0f);
    }

    public void MouseExit()
    {
        usingMouse = true;
        animator.SetBool("Selected", false);
		AudioManager.Instance.PlaySoundSFX(exitSound, gameObject, Random.Range(0.8f, 1f), Random.Range(0.9f, 1.1f), 0f);
		currentHoverSound.Stop();
	}

    public void MouseClick()
    {
        usingMouse = true;
        animator.SetBool("Pressed", true);
		AudioManager.Instance.PlaySoundSFX(selectSound, gameObject, Random.Range(0.8f, 1f), Random.Range(0.9f, 1.1f), 0f);
		currentHoverSound.Stop();
	}

    private void CheckInputMethod()
    {
        var vertical = Input.GetAxis("Vertical");
        if (vertical > 0)
        {
            usingMouse = false;
        }
    }

}
