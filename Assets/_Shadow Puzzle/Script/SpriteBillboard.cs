using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField]
    bool freezeXZaxis = true;

    void LateUpdate()
    {
        if (freezeXZaxis){
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else{
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
