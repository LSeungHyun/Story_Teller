using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private AbsctractGameSession currentSession;

    [SerializeField] public bool isType = false;
    // �����ͳ� �ٸ� �������� �̱�/��Ƽ ���� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 1) ��Ÿ�ӿ� ���� Session ����ü ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectGameMode(bool isMulti)
    {
        if (isMulti)
            currentSession = new MultiSession();
        else
            currentSession = new SingleSession();

        isType = isMulti;
    }
    public AbsctractGameSession Session => currentSession;
}
