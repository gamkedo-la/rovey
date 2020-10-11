using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemUI : MonoBehaviour
{
    [SerializeField]
    private List<Transform> keyItemSlots;

    private List<GameObject> keyItems;

    private void Start()
    {
        keyItems = new List<GameObject>();
    }

    public void AssignKeyItem(GameObject keyItem)
    {
        keyItems.Add(keyItem);

        if (keyItemSlots.Count >= keyItems.Count)
        {
            var itemIndex = keyItems.Count - 1;
            var itemSlot = keyItemSlots[itemIndex];
            keyItem.transform.SetParent(itemSlot, false);
            keyItem.transform.localPosition = Vector3.zero;
            keyItem.transform.localScale = Vector3.one * keyItem.GetComponent<KeyItem>().UIScale;
        }
        else
        {
            // this shouldn't happen I guess? need more slots!
        }
    }
}
