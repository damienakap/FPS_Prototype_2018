using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Test_PickupData : PickupData
{

    public override void initialize()
    {
        base.initialize();

        this.itemName = "Test Pistol";
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
