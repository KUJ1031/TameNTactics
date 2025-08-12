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

            // 기존 line.Split(',') 대신 큰따옴표 처리 가능한 파서 사용
            List<string> values = ParseCSVLine(line);

            if (values.Count < 13) // 필드 11개 이상인지 체크
            {
                Debug.LogWarning("CSV 데이터 부족: " + line);
                continue;
            }

            string currentTreeId = values[0];

            var node = new DialogueNode
            {
                ID = ParseIntOrDefault(values[1]),
                Speaker = values[2],
                Text = values[3],
                Choice1 = values[4],
                Choice1Next = ParseIntOrDefault(values[5]),
                Choice2 = values[6],
                Choice2Next = ParseIntOrDefault(values[7]),
                Choice3 = values[8],
                Choice3Next = ParseIntOrDefault(values[9]),
                Next = ParseIntOrDefault(values[10]),
                EventKey = values.Count > 11 ? values[11] : null,
                LateEventKey = values.Count > 12 ? values[12] : null
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

    // 큰따옴표 안 쉼표 무시하며 한 줄을 필드 리스트로 분리하는 메서드
    private static List<string> ParseCSVLine(string line)
    {
        var values = new List<string>();
        bool inQuotes = false;
        var current = new System.Text.StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // 큰따옴표 안/밖 토글, 두 개 연속 큰따옴표는 큰따옴표 문자로 처리
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; // 두번째 큰따옴표 건너뛰기
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        values.Add(current.ToString());

        return values;
    }
}

