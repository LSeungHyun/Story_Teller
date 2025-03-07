using UnityEngine;
public class DontDestroyGroup : MonoBehaviour
{
    private static DontDestroyGroup instance;

    private void Awake()
    {
        // �̹� �ν��Ͻ��� �ִٸ�, ���� ���� �� �ı�
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // ���� �ν��Ͻ��� ����
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}