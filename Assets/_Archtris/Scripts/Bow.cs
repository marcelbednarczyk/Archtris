using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    private Notch notch = null;

    protected override void Awake()
    {
        base.Awake();
        notch = GetComponentInChildren<Notch>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(notch.SetReady);
        selectExited.AddListener(notch.SetReady);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(notch.SetReady);
        selectExited.RemoveListener(notch.SetReady);
    }
}
