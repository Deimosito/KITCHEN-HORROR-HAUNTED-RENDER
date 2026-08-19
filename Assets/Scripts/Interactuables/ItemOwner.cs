using UnityEngine;

public class ItemOwner : MonoBehaviour
{
	public Item item;
	[SerializeField]
	private GameObject itemSocket;
	public GameObject SetItem(Item newItem)
	{
		item = newItem;
		return itemSocket;
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		itemSocket.SetActive(false); //Hide the item while in game because we dont want it to interact with anything is just a position socket for holded items
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
