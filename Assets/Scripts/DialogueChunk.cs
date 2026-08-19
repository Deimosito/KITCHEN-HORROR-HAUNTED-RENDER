using System;
using System.Collections;
using UnityEngine;

public class DialogueChunk //: MonoBehaviour
{
    public DialogueCharacter dialogueCharacter;
    public ArrayList dialogueLines;
	public int currentDialogueLineIndex;

	public string GetNextDialogueLine()
	{
		string dialogueLine = null;
		if (currentDialogueLineIndex < dialogueLines.Count)
		{
			dialogueLine = (string)dialogueLines[currentDialogueLineIndex];
			currentDialogueLineIndex++;
		}

		return dialogueLine;
	}

	public DialogueChunk()
    {
		dialogueLines = new ArrayList();
		currentDialogueLineIndex = 0;
	}

	//// Start is called once before the first execution of Update after the MonoBehaviour is created
	//void Start()
    //{
	//
	//}
	//
    //// Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
