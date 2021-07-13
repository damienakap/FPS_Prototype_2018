using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupData : MonoBehaviour
{
    /*
     * 0: None
     * 1: weapon
     * 2: 
     * 
     */
    protected int itemType = 0;
    protected string itemName;
    protected bool initialized = false;

    public virtual void initialize()
    {

    }

    public void setItemType(int i) { this.itemType = i; }
    public void setItemName(string s) { this.itemName = s; }

    public int getItemType() { return this.itemType; }
    public string getItemName() { return this.itemName; }

    protected void setAsInitialized() { initialized = true; }

}
