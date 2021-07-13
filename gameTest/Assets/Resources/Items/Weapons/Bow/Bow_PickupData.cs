using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_PickupData : PickupData
{

    public override void initialize()
    {
        base.initialize();

        this.itemName = "Bow";
        this.itemType = 1;
        this.initialized = true;
    }

    private void Start()
    {
        if (!this.initialized)
        {
            this.initialize();
        }
    }

}
