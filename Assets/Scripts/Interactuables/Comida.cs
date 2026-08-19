using System;
using System.Collections;
using UnityEngine;

public class Comida : Item
{
	public enum estadoComida  //Estados de la comida
	{
		Raw,
		Cortada,
		Cocinada,
		Quemada
	}

	private estadoComida currentEstadoComida = estadoComida.Raw; //Estado inicial de la comida

	private MeshFilter meshFilter; //MeshFilter del objeto comida

	//Igual esto hay que cambiarlo por gameobjects e ir instanciando y borrando, de momento son meshes
	[SerializeField]
	private Mesh[] meshesComida; //Array de meshes que representan los distintos estados de la comida (entera, cortada, quemada, etc)
	public override void Interactuar(Item heldItem)
	{
		//Aqui va la logica de usar item de la mano sobre item interactuado
		Type heldItemType = heldItem.GetType();

		if (heldItemType == typeof(Cuchillo))
		{
			Debug.Log("Cortando comida");
			currentEstadoComida = estadoComida.Cortada;

			//Aqui va la logica de cortar comida
			meshFilter.mesh = (Mesh)meshesComida[(int)estadoComida.Cortada];
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Start()
    {
        base.Start();
		meshFilter = GetComponent<MeshFilter>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }


}
