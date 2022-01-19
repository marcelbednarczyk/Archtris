using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public static EventManager current;
    public bool actionLeft = false;
    public bool actionRight = false;
    public bool actionRotate = false;
    public bool actionHardDrop = false;

    List<UnityEngine.XR.InputDevice> gameControllers;

    public event Action MoveLeftEvent;
    public event Action MoveRightEvent;
    public event Action RotateEvent;
    public event Action HardDropEvent;

    private void Awake()
    {
        current = this;
        gameControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, gameControllers);
    }

    // Update is called once per frame
    private void Update()
    {
        bool triggerValue;
        foreach (var device in gameControllers)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("Pause");
            }
        }
    }

    public void OnHitTarget(int id)
    {
        switch (id)
        {
            case 0:
                OnHitLeftTarget();
                break;
            case 1:
                OnHitMiddleTarget();
                break;
            case 2:
                OnHitRightTarget();
                break;
            case 3:
                OnHitHardDropTarget();
                break;
        }
    }

    private void OnHitLeftTarget()
    {
        MoveLeftEvent?.Invoke();
        actionLeft = true;
    }

    private void OnHitRightTarget()
    {
        MoveRightEvent?.Invoke();
        actionRight = true;
    }

    private void OnHitMiddleTarget()
    {
        RotateEvent?.Invoke();
        actionRotate = true;
    }

    private void OnHitHardDropTarget()
    {
        HardDropEvent?.Invoke();
        actionHardDrop = true;
    }
}
