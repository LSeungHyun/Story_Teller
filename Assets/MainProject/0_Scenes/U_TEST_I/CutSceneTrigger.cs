using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    // 플레이어 태그가 "Player"로 설정되어 있다고 가정합니다.
    // 이 오브젝트에 할당된 PlayableDirector는 컷씬 Timeline을 제어합니다.
    public PlayableDirector cutsceneDirector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거 영역에 진입하면 컷씬 실행
            if (cutsceneDirector != null)
            {
                cutsceneDirector.Play();
            }
            else
            {
                Debug.LogError("PlayableDirector가 할당되지 않았습니다.");
            }
        }
    }
}
