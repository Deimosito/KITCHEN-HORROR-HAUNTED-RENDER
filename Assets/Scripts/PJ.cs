using System;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PJ : MonoBehaviour
{
	public enum GameStates
	{
		Invalid = -1,
		Gameplay,
		Menu,
		Dialogue,
	}

	GameStates currentGameState = GameStates.Invalid;

    private Transform myTransform;
	private ItemOwner myItemOwner;
	private Vector2 _mousePosition;
	private Vector2 _previousPosition;
	private float xRotation = 0f;
	private float yRotation = 0f;

	public void SetCurrentGameState(GameStates newGameState)
	{
		currentGameState = newGameState;

		switch(currentGameState)
		{
			case GameStates.Gameplay:
				Cursor.lockState = CursorLockMode.Locked;
				break;
			case GameStates.Menu:
				break;
			case GameStates.Dialogue:
				//Cursor.lockState = CursorLockMode.Locked;
				Cursor.lockState = CursorLockMode.None; //Just for testing purposes, remove or comment, also remember to uncomment the line above
				break;
			case GameStates.Invalid:
				Console.WriteLine("PJ: Estado de juego no valido");
				break;
			default:
				break;
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		myTransform = GetComponent<Transform>();
		myItemOwner = GetComponent<ItemOwner>();
		SetCurrentGameState(GameStates.Gameplay);
	}

	private void GameplayUpdate()
	{
		Vector3 forward_limpio = myTransform.forward;
		forward_limpio.y = 0.0f;

		Vector3 right_limpio = myTransform.right;
		right_limpio.y = 0.0f;

		if (Input.GetKey(KeyCode.W))
		{
			myTransform.Translate(forward_limpio * Time.deltaTime * 10f, Space.World);
		}
		if (Input.GetKey(KeyCode.S))
		{
			myTransform.Translate(-forward_limpio * Time.deltaTime * 10f, Space.World);
		}
		if (Input.GetKey(KeyCode.A))
		{
			myTransform.Translate(-right_limpio * Time.deltaTime * 10f, Space.World);
		}
		if (Input.GetKey(KeyCode.D))
		{
			myTransform.Translate(right_limpio * Time.deltaTime * 10f, Space.World);
		}

		//_mousePosition = Mouse.current.position.value;
		//Vector2 calculatedDelta = _mousePosition - _previousPosition;
		//_previousPosition = _mousePosition;
		//
		//myTransform.Rotate(Vector3.up * calculatedDelta.x *0.1f);
		//Vector3 calculatedDelta3D = new Vector3(calculatedDelta.y, -calculatedDelta.x, 0.0f);
		//myTransform.localRotation = Quaternion.Euler(calculatedDelta3D)
		//myTransform.Rotate(calculatedDelta3D*0.1f);

		float mouseX = Input.GetAxis("Mouse X") * 500f * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * 500f * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		yRotation -= mouseX;
		//yRotation = Mathf.Clamp(yRotation, -90f, 90f);

		myTransform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
		//myTransform.Rotate(Vector3.up * mouseX);



		if (Input.GetKeyDown(KeyCode.E))
		{
			Debug.DrawLine(myTransform.position, myTransform.position + myTransform.forward * 100f, Color.red, 100.0f);
			Debug.Log($"PJ::GameplayUpdate myTransform.position = {myTransform.position}, myTransform.position + myTransform.forward * 100f = {myTransform.position + myTransform.forward * 100f}");
			if (Physics.Raycast(myTransform.position, myTransform.forward, out RaycastHit hitInfo, 100f))
			{
				if (hitInfo.collider.gameObject.CompareTag("Interactuable"))
				{
					Interactuable interactuable = hitInfo.collider.gameObject.GetComponent<Interactuable>();
					if (myItemOwner.item != null)
					{
						interactuable.Interactuar(myItemOwner.item);
					}
					else
					{
						interactuable.Interactuar();
					}
				}
			}
		}
	}

	private void DialogueUpdate()
	{
		if (Input.GetKeyDown(KeyCode.E)) //CHANGE TO ACTIONS ASAP!!!
		{
			//DialogueManager.instance.DialogueNextLine();
			DialogueManager.instance.PlayerInput();
		}
	}


	// Update is called once per frame
	void Update()
    {
		switch(currentGameState)
		{
			case GameStates.Gameplay:
				GameplayUpdate();
				break;
			case GameStates.Menu:
				break;
			case GameStates.Dialogue:
				DialogueUpdate();
				break;
			case GameStates.Invalid:
				break;
			default:
				break;
		}
	}

}
