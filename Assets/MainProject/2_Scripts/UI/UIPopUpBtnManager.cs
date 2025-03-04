using UnityEngine;
using UnityEngine.UI;

public class UIPopUpBtnManager : MonoBehaviour
{
    public Button backBtn;
    public Button nextBtn;

    // UIContentsManager에서 페이지 변경 시 전달받은 currentPage, totalPages 값을 이용해 버튼 활성/비활성 업데이트
    public void UpdateNavigationButtons(int currentPage, int totalPages)
    {
        if (backBtn != null)
            backBtn.gameObject.SetActive(currentPage > 1);
        if (nextBtn != null)
            nextBtn.gameObject.SetActive(currentPage < totalPages);
    }
}
