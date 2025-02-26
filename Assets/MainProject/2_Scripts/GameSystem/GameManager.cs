using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private AbstractGameSession currentSession;

    [SerializeField] private bool isMulti = false;
    // �����ͳ� �ٸ� �������� �̱�/��Ƽ ���� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 1) ��Ÿ�ӿ� ���� Session ����ü ����
            if (isMulti)
                currentSession = new MultiSession();
            else
                currentSession = new SingleSession();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AbstractGameSession Session => currentSession;
}
