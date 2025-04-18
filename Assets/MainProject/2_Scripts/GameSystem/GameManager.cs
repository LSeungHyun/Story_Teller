using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private AbsctractGameSession currentSession;

    [SerializeField] public bool isType = false;
    // 에디터나 다른 로직에서 싱글/멀티 여부 지정

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 1) 런타임에 따라 Session 구현체 선택
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
