using UnityEngine;
public class DontDestroyGroup : MonoBehaviour
{
    private static DontDestroyGroup instance;

    private void Awake()
    {
        // 이미 인스턴스가 있다면, 새로 생긴 건 파괴
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 최초 인스턴스면 유지
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}