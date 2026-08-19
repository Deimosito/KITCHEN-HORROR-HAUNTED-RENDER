using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public enum DialogueStates
	{
		Invalid = -1,
		Idle,
		Writing,
		Scrolling,
		WaitingInput,
	}
	public GameObject sectionFrameUI;
	public TMP_Text dialogueText;
	public TMP_Text dialogueHeader;
	public UnityEngine.UI.Image dialoguePortrait;

	public PJ player;

	public DialogueChunk currentDialogueChunk;
	public string currentLine;

	[SerializeField]
	public TextAsset charactersFile;

	private Dialogue currentDialogue;

	[SerializeField]
	private DialogueCharacter[] EditorCreatedCharacters;
	public Dictionary<string, DialogueCharacter> characters;

	[SerializeField]
	public string breakLineString = "<br>";
	[SerializeField]
	private string midLineCutString = "-";

	[SerializeField]
	private float lettersPerSecond = 40.0f;
	[SerializeField]
	public int maxCharactersPerLine = 40;

	[SerializeField]
	private int maxLinesPerDialogueScreen = 3;
	[SerializeField]
	private int scrollLines = 1;
	[SerializeField]
	private int scrollLinesPerSecond = 3;

	private DialogueStates currentDialogueState = DialogueStates.Invalid;

	private int printedCharacters = 0;
	private int linesInScreen = 0;
	private int chunkPrintedLines = 0;
	private int scrolledLines = 0;

	private float stateTimer = 0.0f;
	private float secondsPerLetter = 1.0f;
	private float scrollSecondsPerLine = 1.0f;

	private string rootPath;

	public static DialogueManager instance { get; private set; }

	public void ChangeState(DialogueStates newState)
	{
		currentDialogueState = newState;
		stateTimer = 0.0f;
	}

	public void PlayerInput()
	{
		switch (currentDialogueState)
		{
			case DialogueStates.Idle:
				break;
			case DialogueStates.Writing:
				dialogueText.text += currentLine.Substring(printedCharacters);
				//Mas codigo que se repite
				printedCharacters = 0;
				linesInScreen++;
				chunkPrintedLines++;
				ChangeState(DialogueStates.WaitingInput);
				break;
			case DialogueStates.Scrolling:
				break;
			case DialogueStates.WaitingInput:
				bool bIsLastLine = currentDialogueChunk.dialogueLines.Count == currentDialogueChunk.currentDialogueLineIndex;
				bool bIsScreenOverflow = linesInScreen >= maxLinesPerDialogueScreen;
				if (!bIsLastLine && bIsScreenOverflow)
				{
					//Srcoll
					dialogueText.text = "";
					//Las tengo que pintar desde atras hacia adelante
					for (int i = maxLinesPerDialogueScreen - 1; i > 0; i--)
					{
						dialogueText.text += (string)currentDialogueChunk.dialogueLines[currentDialogueChunk.currentDialogueLineIndex - i];
					}

				}
				DialogueNextLine();
				break;
			case DialogueStates.Invalid:
				Console.WriteLine("DialogueManager: Estado de dialogo no valido");
				break;
			default:
				break;
		}
	}

	public void LoadCharacters()
	{
		characters = new Dictionary<string, DialogueCharacter>();
		foreach (DialogueCharacter character in EditorCreatedCharacters)
		{
			characters.Add(character.characterName, character);
		}
	}

	private void ShowDialogueUI()
	{
		sectionFrameUI.SetActive(true);

	}

	private void HideDialogueUI()
	{
		sectionFrameUI.SetActive(false);
	}

	public void StartNewDialogue(Dialogue dialogue)
	{
		player.SetCurrentGameState(PJ.GameStates.Dialogue);

		dialogue.LoadFromFile();
		currentDialogue = dialogue;
		ShowDialogueUI();
		DialogueNextChunk();
	}

	public void EndDialogue()
	{
		HideDialogueUI();
		player.SetCurrentGameState(PJ.GameStates.Gameplay);
		ChangeState(DialogueStates.Idle);
		currentDialogue = null;
	}

	public void DialogueNextChunk()
	{
		if (currentDialogue == null)
		{
			return;
		}

		currentDialogueChunk = currentDialogue.GetNextDialogueChunk();

		if (currentDialogueChunk == null)
		{
			EndDialogue();
			return;
		}

		dialoguePortrait.sprite = Sprite.Create((Texture2D)currentDialogueChunk.dialogueCharacter.characterPortrait, new Rect(0, 0, currentDialogueChunk.dialogueCharacter.characterPortrait.width, currentDialogueChunk.dialogueCharacter.characterPortrait.height), new Vector2(0.5f, 0.5f));

		dialogueHeader.color = currentDialogueChunk.dialogueCharacter.characterColor;
		dialogueHeader.text = currentDialogueChunk.dialogueCharacter.characterName;

		dialogueText.color = currentDialogueChunk.dialogueCharacter.characterColor;
		dialogueText.text = "";
		printedCharacters = 0;
		linesInScreen = 0;
		scrolledLines = 0;

		DialogueNextLine();
	}

	public void DialogueNextLine()
	{
		if (currentDialogue == null)
		{
			return;
		}

		if (currentDialogueChunk == null)
		{
			return;
		}

		currentLine = currentDialogueChunk.GetNextDialogueLine();

		if (currentLine == null)
		{
			currentDialogueChunk = null;
			DialogueNextChunk();
			return;
		}

		//Si tenemos dialogo, pasa a escribirlo
		ChangeState(DialogueStates.Writing);
	}

	private void Awake()
	{
		//Check if there is another instance of this class, then destroy as we only want one instance
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		ChangeState(DialogueStates.Idle);
		instance = this;
		rootPath = Application.persistentDataPath + "/Prefabs/DialoguePrefabs/";
		stateTimer = 0.0f;
		printedCharacters = 0;

		dialogueText.text = "";

		secondsPerLetter = 1.0f / lettersPerSecond;
		scrollSecondsPerLine = 1.0f / scrollLinesPerSecond;

		LoadCharacters();

		player = GameObject.Find("Player").GetComponent<PJ>();

		HideDialogueUI();
	}

	// Update is called once per frame

	private void IdleUpdate()
	{

	}

	private void WritingUpdate()
	{
		stateTimer += Time.deltaTime;
		if (stateTimer >= secondsPerLetter)
		{
			stateTimer = 0.0f;
			if (printedCharacters < currentLine.Length - breakLineString.Length)
			{
				dialogueText.text += currentLine[printedCharacters];
				printedCharacters++;
			}
			else
			{
				//Hemos terminado de escribir la linea
				dialogueText.text += breakLineString;
				printedCharacters = 0;
				linesInScreen++;
				chunkPrintedLines++;
				ChangeState(DialogueStates.WaitingInput);
				return;
			}

		}
	}

	private void ScrollingUpdate()
	{
		stateTimer += Time.deltaTime;
		if (stateTimer >= scrollSecondsPerLine)
		{
			stateTimer = 0.0f;
			dialogueText.text = "";
			//	////maxLinesPerDialogueScreen - 1; //Lineas que hay que pintar
			//	////Las tengo que pintar desde atras hacia adelante
			//	//for (int i = maxLinesPerDialogueScreen - 1; i > 0; i--)
			//	//{
			//	//	dialogueText.text += (string)currentDialogueChunk.dialogueLines[currentDialogueChunk.currentDialogueLineIndex - i] + DialogueManager.instance.breakLineString;
			//	//}

			//	//scrolledLines += 1;
			//	//if(scrolledLines >= 1)
			//	//{
			//	//	scrolledLines = 0;
			//	//	printedLines = maxLinesPerDialogueScreen - 1;
			//	//	printedCharacters = 0;
			//	//	DialogueNextLine();
			//	//	return;
			//	//}

			//	//printedLines = maxLinesPerDialogueScreen - 1;
			//	//printedCharacters = 0;
			//	//dialogueText.text = "";
			//	//dialogueText.text += currentDialogueChunk.dialogueLines[currentDialogueChunk.currentDialogueLineIndex - 2];
			//	//dialogueText.text += currentDialogueChunk.dialogueLines[currentDialogueChunk.currentDialogueLineIndex - 1];
			//	//dialogueText.text += currentDialogueChunk.dialogueLines[currentDialogueChunk.currentDialogueLineIndex];
			//	//ChangeState(DialogueStates.Idle);
			//	//DialogueNextLine();
		}
	}

	private void WaitingInputUpdate()
	{

	}

	void Update()
	{
		stateTimer += Time.deltaTime;
		switch (currentDialogueState)
		{
			case DialogueStates.Idle:
				IdleUpdate();
				break;
			case DialogueStates.Writing:
				WritingUpdate();
				break;
			case DialogueStates.Scrolling:
				ScrollingUpdate();
				break;
			case DialogueStates.WaitingInput:
				WaitingInputUpdate();
				break;
			case DialogueStates.Invalid:
				Console.WriteLine("DialogueManager: Estado de dialogo no valido");
				break;
			default:
				break;
		}
	}
}
