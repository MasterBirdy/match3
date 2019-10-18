using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AnimalClass
{
    // Start is called before the first frame update
    void ActivatePower(int level);
    bool HasTimeExtension();
    string ReturnName();
    Sprite ReturnSprite();
}
