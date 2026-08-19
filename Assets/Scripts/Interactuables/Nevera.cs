using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nevera : Interactuable
{
    private Animator animator;

    public override void Interactuar()
	{
        Interactuar(null);
	}

	public override void Interactuar(Item heldItem)
	{
        animator.SetTrigger("OPEN");
	}

    public override void ReproducirAnimacion()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
