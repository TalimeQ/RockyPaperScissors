using UnityEngine;
public class PickupUi : MonoBehaviour
{
    private System.Action<int> uiCallback;
    private bool isActive = false;

    public bool IsActive { get => isActive; set => isActive = value; }

    public void Activate(System.Action<int> pickupCallback)
    {
        Debug.Log("activated!");
        uiCallback = pickupCallback;
        Debug.Log(gameObject);
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void OnRockButtonPressed()
    {
        uiCallback?.Invoke(1);
        Deactivate();
    }

    public void OnPaperButtonPressed()
    {
        uiCallback?.Invoke(2);
        Deactivate();
    }

    public void OnScissorsButtonPressed()
    {
        uiCallback?.Invoke(3);
        Deactivate();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        uiCallback = null;
        gameObject.SetActive(false);
        IsActive = false;
    }

}
