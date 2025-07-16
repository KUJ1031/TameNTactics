using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class DialogueCSVParser
{
    public static Dictionary<string, Dictionary<int, DialogueNode>> ParseByTreeID(TextAsset csvFile)
    {
        var result = new Dictionary<string, Dictionary<int, DialogueNode>>();
        var reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if (isFirstLine) { isFirstLine = false; continue; }

            string[] values = line.Split(',');
            if (values.Length < 9) // 트리ID 포함 9개 이상이어야 함
            {
                Debug.LogWarning("CSV 데이터 부족: " + line);
                continue;
            }

            string currentTreeId = values[0]; // 첫 칼럼을 트리ID로 사용

            var node = new DialogueNode
            {
                ID = ParseIntOrDefault(values[1]),
                Speaker = values[2],
                Text = values[3],
                Choice1 = values[4],
                Choice1Next = ParseIntOrDefault(values[5]),
                Choice2 = values[6],
                Choice2Next = ParseIntOrDefault(values[7]),
                Next = ParseIntOrDefault(values[8])
            };

            if (!result.ContainsKey(currentTreeId))
                result[currentTreeId] = new Dictionary<int, DialogueNode>();

            result[currentTreeId][node.ID] = node;
        }

        return result;

        int ParseIntOrDefault(string s, int defaultValue = -1)
        {
            return int.TryParse(s, out var result) ? result : defaultValue;
        }
    }

}
