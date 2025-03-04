using UnityEngine;
using UnityEngine.UI;

public class UIPopUpBtnManager : MonoBehaviour
{
    public Button backBtn;
    public Button nextBtn;

    // UIContentsManager���� ������ ���� �� ���޹��� currentPage, totalPages ���� �̿��� ��ư Ȱ��/��Ȱ�� ������Ʈ
    public void UpdateNavigationButtons(int currentPage, int totalPages)
    {
        if (backBtn != null)
            backBtn.gameObject.SetActive(currentPage > 1);
        if (nextBtn != null)
            nextBtn.gameObject.SetActive(currentPage < totalPages);
    }
}
