using UnityEngine;

public class OculusRightTriggerToGun : MonoBehaviour
{
    [SerializeField] private GunController gun;

    void Update()
    {
        if (gun == null) return;

        // ยิงได้เฉพาะตอน "ปืนกำลังถูกใช้/ถูกถืออยู่"
        if (!gun.CanShoot) return;

        // อ่านค่าปุ่ม Index Trigger ของจอยขวา (0–1)
        float value = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

        // ส่งแรงกดเข้าไปให้ GunController
        gun.ComputeUseStrength(value);
    }
}
