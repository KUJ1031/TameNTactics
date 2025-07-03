using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [Serializable]
    public class Pair
    {
        public TKey key;
        public TValue value;
    }

    [SerializeField]
    private List<Pair> pairs = new();

    public void FromDictionary(Dictionary<TKey, TValue> dict)
    {
        pairs = dict.Select(kv => new Pair { key = kv.Key, value = kv.Value }).ToList();
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        return pairs.ToDictionary(p => p.key, p => p.value);
    }

    public TValue this[TKey key]
    {
        get => pairs.First(p => EqualityComparer<TKey>.Default.Equals(p.key, key)).value;
        set
        {
            var pair = pairs.FirstOrDefault(p => EqualityComparer<TKey>.Default.Equals(p.key, key));
            if (pair != null)
            {
                pair.value = value;
            }
            else
            {
                pairs.Add(new Pair { key = key, value = value });
            }
        }
    }

    public List<Pair> Pairs => pairs;
}
