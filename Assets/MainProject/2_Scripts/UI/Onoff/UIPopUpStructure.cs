using UnityEngine;

public class UIPopUpStructure : MonoBehaviour
{
    
    [SerializeField] public Canvas canvas;
    [System.Serializable]
    public class Canvas
    {
        public GameObject popUp;
        public PopUpGroup popUpGroup;
    }

    [System.Serializable]
    public class PopUpGroup
    {
        public GameObject windowPopUp;
        public WindowPopUpGroup windowPopUpGroup;
    }


    [System.Serializable]
    public class WindowPopUpGroup
    {
        public GameObject defaultPopUp;
        public DefaultPopUpGroup defaultPopUpGroup;
        public GameObject questPopUp;
    }

    [System.Serializable]
    public class DefaultPopUpGroup
    {
        public GameObject dialogueGroup;
        public GameObject imageGroup;
    }

}
