using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{

    //From Thomas Brush tutorial https://www.youtube.com/watch?v=vqZjZ6yv1lA

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (gameObject.GetComponent<LoadScenes>())
        {
            LoadScenes loadScenes = gameObject.GetComponent<LoadScenes>();
            loadScenes.LoadScene(loadScenes.SceneToLoad);
        }
    }
}
