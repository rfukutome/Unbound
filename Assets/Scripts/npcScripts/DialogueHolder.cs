using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      DialogueHolder
//   Purpose:    Holds the lines of text that a NPC will say when
//   interacted with.
//
//   Notes: Attach onto an NPC
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueHolder : MonoBehaviour {
    [SerializeField]
    private DialogueManager dialogueManager;

    //Dialogue text lines
    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    // Use this for initialization
    void Start () {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        currentLine = 0;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (Input.GetButtonDown("Action") && dialogueManager.ShowBox(textLines[currentLine], transform.name))
            {
                //if not equal to the last line
                if (currentLine != textLines.Length - 1)
                {
                    currentLine++;
                }
                else
                {
                    dialogueManager.ShowBox("EOC", transform.name);
                    currentLine--;
                }
            }
        }
    }
}