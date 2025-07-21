using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }

    //기본은 씬 이동시 삭제
    //DontDistroy가 필요하다면 아래 한줄 복붙
    //protected override bool IsDontDestroy => true;

    /* Awake에 추가 내용이 필요할 경우
    protected override void Awake()
    {
        base.Awake();
        //이후 추가 내용작성
    }
     */

    protected virtual bool IsDontDestroy => false;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (IsDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
