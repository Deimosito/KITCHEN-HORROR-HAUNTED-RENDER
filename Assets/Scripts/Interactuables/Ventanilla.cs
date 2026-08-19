using System;
using UnityEngine;

public class Ventanilla : Interactuable
{
    private Dialogue myDialogue;

	public override void Interactuar()
	{
		//base.Interactuar();
		Console.WriteLine("Interactuar con Ventanilla en:  " + name);
		DialogueManager.instance.StartNewDialogue(myDialogue);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
	{
		base.Start();
		myDialogue = GetComponent<Dialogue>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
