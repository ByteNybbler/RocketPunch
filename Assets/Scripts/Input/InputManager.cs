// Author(s): Paul Calande
// Class that collects input every frame for use in FixedUpdate.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputWriter inputWriter;
    InputReader inputReader;
    HashSet<IPlayable> subscribers = new HashSet<IPlayable>();

    private void Awake()
    {
        InputData inputData = new InputData();
        inputWriter = new InputWriter(inputData);
        inputReader = new InputReader(inputData);
    }

    private void Update()
    {
        inputWriter.PopulateKeys();
    }

    private void FixedUpdate()
    {
        inputWriter.PopulateAxes();
        SendInputToSubscribers();
        inputWriter.Clear();
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
            sub.ReceiveInput(inputReader);
        }
    }
}