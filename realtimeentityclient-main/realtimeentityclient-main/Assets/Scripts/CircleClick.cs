using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleClick : MonoBehaviour
{
    public int balloonID;

    void OnMouseDown()
    {
        if (NetworkClientProcessing.IsConnectedToServer())
        {
  
        }
        else Destroy(gameObject);
    }
}
