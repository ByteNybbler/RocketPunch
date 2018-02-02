// Author(s): Paul Calande
// Class that collects input every frame for use in FixedUpdate.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputData inputData = new InputData();
    HashSet<IInputSubscriber> subscribers = new HashSet<IInputSubscriber>();

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

    public void AddSubscriber(IInputSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(IInputSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    private void SendInputToSubscribers()
    {
        foreach (IInputSubscriber sub in subscribers)
        {
            sub.ReceiveInput(inputData);
        }
    }
}