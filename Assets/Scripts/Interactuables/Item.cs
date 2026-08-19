using System;
using UnityEngine;

public class Item : Interactuable
{
    protected ItemOwner owner;
	private ItemOwner playerItemOwner;
	private Item myItem;

	public override void Interactuar()
	{
		ChangeOwner(playerItemOwner);
	}

	public override void Interactuar(Item heldItem)
	{
		//Aqui va la logica de usar item de la mano sobre item interactuado
	}

	public virtual void ChangeOwner(ItemOwner newOwner)
	{
		if(owner != null)
		{
			owner.SetItem(null); //Remove item from current owner
		}

		owner = newOwner;
		
		GameObject socketObject = owner.SetItem(myItem);
		//Set parent
		transform.parent = owner.transform;
		transform.localPosition = socketObject.transform.localPosition;
		transform.localRotation = socketObject.transform.localRotation;
		//Set position in socket & asign holded comida to new owner
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
    {
		base.Start();
		owner = null;
		playerItemOwner = GameObject.Find("Player").GetComponent<ItemOwner>();
		myItem = this;
	}

    // Update is called once per frame
    void Update()
    {
        
    }


}
