using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireAnimation : MonoBehaviour
{
    private Light fire;
    private float timer;
    [SerializeField] private float pulseSpeed = 1.0f;

    private void Awake()
    {
        fire = GetComponent<Light>();
    }
    void Update()
    {
        timer += Time.deltaTime* pulseSpeed;

        fire.intensity = Mathf.Lerp(8.0f, 80.0f, (Mathf.Sin(timer) + 1.0f) / 2.0f);
    }
}
