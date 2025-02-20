using UnityEngine;

public class TeleportChangePOV : TeleportOnTrigger
{
    [SerializeField]
    private Camera mainCamera;
    public override void OnTeleport()
    {
        base.OnTeleport();
        mainCamera.transform.Rotate(0, 0, 180);
    }
}
