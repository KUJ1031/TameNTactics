using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfoPresent : MonoBehaviour
{
    [SerializeField] private FieldInfoView fieldInfoView;

    private float playTime;

    private string currentMapName = "시작의 광장";

    // Start is called before the first frame update
    void Start()
    {
        playTime = 0f;
        fieldInfoView.SetMapName(currentMapName);
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        fieldInfoView.SetPlayTime(playTime);
    }

    public void SetMapName(string mapName)
    {
        currentMapName = mapName;
        fieldInfoView.SetMapName(currentMapName);
    }
}
