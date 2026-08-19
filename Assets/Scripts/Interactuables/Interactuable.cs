using System;
using UnityEngine;

public class Interactuable : MonoBehaviour
{
    public virtual void Interactuar(Item heldItem) 
    { 
        Console.WriteLine("Interactuar con objeto en mano no implementado en " + name);
	}

	public virtual void Interactuar()
	{
        Console.WriteLine("Interactuar sin objeto en mano no implementado en " + name);
		//Igual llamamos desde aqui a ReproducirAnimacion y quitamos este mensaje obligando a llamar a Super.Interactuar en las clases hijas
        //Parece que va a ser el caso
	}

	public virtual void ReproducirAnimacion()
    { 
        Console.WriteLine("ReproducirAnimacion no implementado en " + name);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected virtual void Start()
    {
        tag = "Interactuable";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
