using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public Sprite card;
    public string blockName;
    public abstract void activate();
}
