using DG.Tweening;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public float distance;
    [Range(.1f, 10f)]
    public float duration;

    private void Start()
    {
        Vector3 start = transform.position;

        transform.DOMoveZ(start.z+ distance, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
