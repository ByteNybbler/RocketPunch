// Author(s): Paul Calande
// Class that collects input every frame for use in FixedUpdate.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputData inputData = new InputData();
    HashSet<IPlayable> subscribers = new HashSet<IPlayable>();

    private void Update()
    {
        inputData.PopulateKeys();
    }

    private void FixedUpdate()
    {
        inputData.PopulateAxes();
        SendInputToSubscribers();
        inputData.Clear();
    }

    public void AddSubscriber(IPlayable subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(IPlayable subscriber)
    {
        subscribers.Remove(subscriber);
    }

    private void SendInputToSubscribers()
    {
        foreach (IPlayable sub in subscribers)
        {
            sub.ReceiveInput(inputData);
        }
    }
}