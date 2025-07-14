using UnityEngine;

public static class TransformUtil
{
    // 이름으로 자식 객체를 깊게 찾는 재귀 함수
    public static Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            var result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
