using UnityEngine;

public class ItemOwner : MonoBehaviour
{
	public Item Item;
	[SerializeField]
	private GameObject ItemSocket;
	public GameObject SetItem(Item newItem)
	{
		Item = newItem;
		return ItemSocket;
	}

	public void DropItem()
	{
		Item.ChangeOwner(null);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		ItemSocket.SetActive(false); //Hide the item while in game because we dont want it to interact with anything is just a position socket for holded items
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
