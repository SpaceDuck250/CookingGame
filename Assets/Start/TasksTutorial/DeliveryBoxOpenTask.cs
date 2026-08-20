using UnityEngine;

public class DeliveryBoxOpenTask : TutorialTask
{
    public DeliveryBoxScript deliveryBoxScript;

    private void Start()
    {
        deliveryBoxScript.OnBoxOpen += OnBoxOpen;
    }

    private void OnDestroy()
    {
        deliveryBoxScript.OnBoxOpen -= OnBoxOpen;

    }

    private void OnBoxOpen()
    {
        CompleteTask();
    }
}
