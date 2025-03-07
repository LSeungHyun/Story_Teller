using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    // �÷��̾� �±װ� "Player"�� �����Ǿ� �ִٰ� �����մϴ�.
    // �� ������Ʈ�� �Ҵ�� PlayableDirector�� �ƾ� Timeline�� �����մϴ�.
    public PlayableDirector cutsceneDirector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ Ʈ���� ������ �����ϸ� �ƾ� ����
            if (cutsceneDirector != null)
            {
                cutsceneDirector.Play();
            }
            else
            {
                Debug.LogError("PlayableDirector�� �Ҵ���� �ʾҽ��ϴ�.");
            }
        }
    }
}
