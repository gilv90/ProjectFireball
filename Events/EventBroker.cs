using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFireball.Events;

public static class EventBroker
{
    private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        if (!_subscribers.ContainsKey(typeof(T)))
            _subscribers[typeof(T)] = new List<Delegate>();
        _subscribers[typeof(T)].Add(handler);
    }
    
    public static void Unsubscribe<T>(Action<T> handler)
    {
        if (_subscribers.TryGetValue(typeof(T), out var handlers))
        {
            handlers.Remove(handler);
            if (handlers.Count == 0)
                _subscribers.Remove(typeof(T));
        }
    }

    public static void Publish<T>(T eventData)
    {
        if (_subscribers.TryGetValue(typeof(T), out var handlers))
        {
            foreach (var handler in handlers.Cast<Action<T>>())
                handler(eventData);
        }
    }
    
    public static void Clear() => _subscribers.Clear();
}