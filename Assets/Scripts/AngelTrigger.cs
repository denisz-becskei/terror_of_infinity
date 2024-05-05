using UnityEngine;

public class AngelTrigger : MonoBehaviour
{
    private RaycastHit hit;

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            if(hit.transform.gameObject.CompareTag("FallenAngel"))
            {
                FallenAngel fa = hit.transform.gameObject.GetComponent<FallenAngel>();
                fa.isActive = true;
            }
        }
    }
}
