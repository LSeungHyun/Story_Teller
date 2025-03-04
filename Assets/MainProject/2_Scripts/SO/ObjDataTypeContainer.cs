using UnityEngine;

[CreateAssetMenu(fileName = "ObjDataTypeContainer", menuName = "Scriptable Objects/ObjDataTypeContainer")]
public class ObjDataTypeContainer : ScriptableObject
{
    public string objCode; 
    public Vector3 position;
    public ObjDataType[] objDataType;

    public void SetTransform(Transform objTransform)
    {
        if (objTransform != null)
        {
            position = objTransform.position;
        }
    }
}
