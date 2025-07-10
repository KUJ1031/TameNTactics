using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    // Inspector에 노출되는 직렬화 리스트
    [SerializeField]
    private List<SerializableKeyValuePair<TKey, TValue>> pairs = new List<SerializableKeyValuePair<TKey, TValue>>();

    // 실제 사용시 내부에서 키-값을 빠르게 찾기 위한 딕셔너리
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    // 프로퍼티로 리스트 공개 (확장 메서드 등에서 사용)
    public List<SerializableKeyValuePair<TKey, TValue>> Pairs => pairs;

    // 딕셔너리 기능 - get/set
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            // 리스트 동기화
            int index = pairs.FindIndex(p => EqualityComparer<TKey>.Default.Equals(p.key, key));
            if (index >= 0)
                pairs[index].value = value;
            else
                pairs.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
        }
    }

    public int Count => dictionary.Count;

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        pairs.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
    }

    public bool Remove(TKey key)
    {
        bool removedFromDict = dictionary.Remove(key);
        int index = pairs.FindIndex(p => EqualityComparer<TKey>.Default.Equals(p.key, key));
        if (index >= 0)
        {
            pairs.RemoveAt(index);
        }
        return removedFromDict;
    }

    public void Clear()
    {
        dictionary.Clear();
        pairs.Clear();
    }

    // Unity 직렬화 콜백 - 직렬화 직전 리스트에 데이터 동기화
    public void OnBeforeSerialize()
    {
        pairs.Clear();
        foreach (var kvp in dictionary)
        {
            pairs.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }

    // Unity 직렬화 콜백 - 직렬화 후 딕셔너리에 데이터 로드
    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        foreach (var pair in pairs)
        {
            if (!dictionary.ContainsKey(pair.key))
                dictionary.Add(pair.key, pair.value);
        }
    }
}
