using System;
using Unity.VisualScripting;
using UnityEngine;

public class Encimera : Interactuable
{
    ItemOwner myItemOwner;
	public override void Interactuar(Item heldItem)
	{
		if ((myItemOwner.Item == null))
		{
			heldItem.ChangeOwner(myItemOwner);
		}
		else 
		{ 
			myItemOwner.Item.Interactuar(heldItem);
		}
		ReproducirAnimacion();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		myItemOwner = GetComponent<ItemOwner>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
