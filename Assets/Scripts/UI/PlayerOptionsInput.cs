using UnityEngine;

public class PlayerOptionsInput : MonoBehaviour
{
    [SerializeField] private GameObject ToggledMenu;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape) && (!PickupUi.Instance.IsActive|| PickupUi.Instance == null))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        bool activationStatus = ToggledMenu.activeInHierarchy;
        ToggledMenu.SetActive(!activationStatus);
    }
}
