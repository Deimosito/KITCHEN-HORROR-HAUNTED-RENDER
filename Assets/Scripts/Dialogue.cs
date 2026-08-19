//using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
	[SerializeField]
	private TextAsset dialogueFile;

	private ArrayList dialogueChunks;

	private int currentDialogueChunkIndex;


	private void SplitDialogueLines(string remainingDialogueLine, ArrayList lines)
	{
		remainingDialogueLine = remainingDialogueLine.TrimStart(' ');
		if (remainingDialogueLine.Length <= DialogueManager.instance.maxCharactersPerLine)
		{
			lines.Add(remainingDialogueLine + DialogueManager.instance.breakLineString);
			return;
		}
		int maxCharactersPerLine = DialogueManager.instance.maxCharactersPerLine;
		int lastSpaceIndex = remainingDialogueLine.LastIndexOf(' ');
		int cutIndex = 0;

		//Un espacio antes del maximo de caracteres, partimos por el espacio
		if (lastSpaceIndex <= maxCharactersPerLine && lastSpaceIndex > -1)
		{
			lines.Add(remainingDialogueLine.Substring(0, lastSpaceIndex) + DialogueManager.instance.breakLineString);
			//lines.Add(remainingDialogueLine.Substring(0, lastSpaceIndex) + DialogueManager.instance.breakLineString);
			cutIndex = lastSpaceIndex;
		}
		else
		{
			//No hay espacios en la linea o la linea esta mas alla del limite de la pantalla, parto por el maximo de caracteres
			cutIndex = maxCharactersPerLine + 1;
			lines.Add(remainingDialogueLine.Substring(0, cutIndex) + DialogueManager.instance.breakLineString);
			//lines.Add(remainingDialogueLine.Substring(0, maxCharactersPerLine) + DialogueManager.instance.breakLineString);
		}
		remainingDialogueLine = remainingDialogueLine.Remove(0, cutIndex);
		SplitDialogueLines(remainingDialogueLine, lines);
	}

	public virtual void LoadFromFile()
	{
		//El resultado tiene que ser un array con las lineas de texto en el formato de la pantalla

		ArrayList lines = new ArrayList();
		dialogueChunks = new ArrayList();
		currentDialogueChunkIndex = 0;

		string[] lineSplitFileCharacters = new string[] { "\r\n", "\n\n", "\r", "\n" };
		string[] dialogueFileSplitedInLines = dialogueFile.text.Split(lineSplitFileCharacters, StringSplitOptions.None);
		string breakLineString = DialogueManager.instance.breakLineString;
		int maxCharactersPerLine = DialogueManager.instance.maxCharactersPerLine;
		DialogueChunk dialogueChunk = null;
		for (int i = 0; i < dialogueFileSplitedInLines.Length; i++)
		{
			if (dialogueFileSplitedInLines[i].StartsWith("[CHAR]"))
			{
				if (dialogueChunk != null)
				{
					dialogueChunk.dialogueLines = lines;
					lines = new ArrayList();
					dialogueChunks.Add(dialogueChunk);
				}
				dialogueChunk = new DialogueChunk();
				string characterName = dialogueFileSplitedInLines[i].Replace("[CHAR]", "").Trim();
				dialogueChunk.dialogueCharacter = DialogueManager.instance.characters[characterName];
			}else
			{
				//No es un nuevo personaje, es una linea del personaje actual
				SplitDialogueLines(dialogueFileSplitedInLines[i], lines);
			}
		}
		//Codigo duplicado, un basuron
		dialogueChunk.dialogueLines = lines;
		lines = new ArrayList();
		dialogueChunks.Add(dialogueChunk);

		Debug.Log("Lineas " + lines.ToString());
	}

	public DialogueChunk GetNextDialogueChunk()
	{
		DialogueChunk dialogueChunk = null;
		if (currentDialogueChunkIndex < dialogueChunks.Count)
		{
			dialogueChunk = (DialogueChunk)dialogueChunks[currentDialogueChunkIndex];
			currentDialogueChunkIndex++;
		}

		return dialogueChunk;
	}



	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
}
