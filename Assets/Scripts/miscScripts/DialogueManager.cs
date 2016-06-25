using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject dialogueBox;
    public Text dialogueText;
    public Text nameObject;
    


    Player player;
    public bool dialogueActive;

    public bool stopPlayerMovement;

    private bool isTyping = false;
    private bool cancelTyping = false;

    float typeSpeed;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        dialogueActive = false;
        dialogueBox.SetActive(false);
	}

    void Update()
    {
        if (dialogueActive && Input.GetButton("Action") && !cancelTyping)
        {
            typeSpeed = 0.01f;
        }
        else
        {
            typeSpeed = 0.05f;
        }
    }
    public bool ShowBox(string argDialogue, string argName)
    {
        if (isTyping)
        {
            return false;
        }
        if (argDialogue.Substring(0, 3) == "EOC")
        {
            dialogueActive = false;
            dialogueBox.SetActive(false);
            player.isBusy = false;
        }
        else
        {
            player.isBusy = true;
            dialogueActive = true;
            dialogueBox.SetActive(true);
            nameObject.text = argName;
            StartCoroutine(TextScroll(argDialogue));
        }

        return true;
    }

    private IEnumerator TextScroll(string argLineOfText)
    {
        int letter = 0;
        dialogueText.text = "";
        isTyping = true;
        cancelTyping = false;
        
        while(isTyping && !cancelTyping && letter < argLineOfText.Length)
        {
            dialogueText.text += argLineOfText[letter];
            letter++;
            yield return new WaitForSeconds(typeSpeed);
        }
        dialogueText.text = argLineOfText;
        isTyping = false;
        cancelTyping = false;
    }
}
