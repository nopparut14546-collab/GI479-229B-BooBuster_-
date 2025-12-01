using UnityEngine;

public class OculusIndexTriggerToGun : MonoBehaviour
{
    [SerializeField] private GunController gun;   // ลาก GunController มาวางใน Inspector
    [SerializeField] private bool isRightHand = true;  // true = มือขวา, false = มือซ้าย

    private float prevValue = 0f;

    void Update()
    {
        if (gun == null) return;

        // อ่านค่าปุ่ม Index Trigger (0–1)
        float value = isRightHand
            ? OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)     // มือขวา
            : OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger); // มือซ้าย

        // เริ่มกดจาก 0 → >0
        if (prevValue <= 0f && value > 0f)
        {
            gun.BeginUse();
        }

        // ส่งแรงกดเข้าไปให้ปืน (GunController จะเช็ค fireThreshold เอง)
        gun.ComputeUseStrength(value);

        // ปล่อยจาก >0 → 0
        if (prevValue > 0f && value <= 0f)
        {
            gun.EndUse();
        }

        prevValue = value;
    }
}
