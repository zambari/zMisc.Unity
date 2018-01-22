using UnityEngine;
[ExecuteInEditMode]
public class KeepDisabled : MonoBehaviour
{

    void Awake()
    {
        if (Time.time < 1)
            gameObject.SetActive(false);
    }
}
