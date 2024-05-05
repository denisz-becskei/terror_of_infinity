using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public int doorType;
    private KeyCode openChar;
    public bool isDoorOpen;

    private Ray rayOrigin;
    private RaycastHit hitInfo;

    private void Start()
    {
        if (doorType == 1)
        {
            openChar = WorldWideScripts.keyCodes[transform.parent.GetComponent<KeyDoorScript>().openKey];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.CompareTag("Player") || isDoorOpen)
        {
            return;
        }

        rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayOrigin, out hitInfo, 5f))
        {
            if (hitInfo.collider.transform != transform)
            {
                return;
            }
        }

        switch (doorType)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponent<Animator>().Play("JustDoorOpen");
                    foreach(BoxCollider collider in GetComponents<BoxCollider>())
                    {
                        collider.enabled = false;
                    }
                    isDoorOpen = true;
                }
                break;
            case 1:
                if (Input.GetKeyDown(openChar))
                {
                    GetComponent<Animator>().Play("KeyDoorOpen");
                    foreach (BoxCollider collider in GetComponents<BoxCollider>())
                    {
                        collider.enabled = false;
                    }
                    isDoorOpen = true;
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Animator[] animators = transform.parent.GetComponentsInChildren<Animator>();
                    OpenDoor[] openDoors = transform.parent.GetComponentsInChildren<OpenDoor>();
                    foreach (Animator animator in animators)
                    {
                        if (animator.GetComponent<SlowDoorScript>().side == 0)
                        {
                            animator.Play("SlowDoorLeftOpen");
                        }
                        else
                        {
                            animator.Play("SlowDoorRightOpen");
                        }
                    }
                    foreach (OpenDoor openDoor in openDoors)
                    {
                        openDoor.isDoorOpen = true;
                    }
                }
                break;
            default:
                Debug.LogError("Not supposed to be here");
                break;
        }
    }

}
