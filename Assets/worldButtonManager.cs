using UnityEngine;

public class worldButtonManager : MonoBehaviour
{
    [SerializeField] private GameEventSO buttonEvent;
    public void Click(GameObject button)
    {
        buttonEvent.RaiseEvent(this, (object)button);
    }
}
