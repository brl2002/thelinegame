using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainView : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}