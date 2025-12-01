using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using TheDeveloperTrain.SciFiGuns;
using UnityEngine;
using UnityEngine.Events;

public class GunController : MonoBehaviour, IHandGrabUseDelegate
{
    [Header("Fire")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField, Range(0, 1)] private float fireThreshold = 0.7f;
    [SerializeField, Range(0, 1)] private float releaseThreshold = 0.2f;
    [SerializeField] private Bullet bullet;

    [Space]
    [SerializeField] private bool isAutoFireMode = true;
    [SerializeField] private float fireInterval = 0.25f;

    [Header("Trigger Visual")]
    [SerializeField] private Transform triggerPivot;
    [SerializeField] private float triggerSpeed = 20f;
    [SerializeField] private bool rotateXAxis;
    [SerializeField] private float triggerXRotation;
    [SerializeField] private bool moveZAxis;
    [SerializeField] private float triggerZPosition;

    public UnityEvent WhenShoot;

    private float lastUseTime;
    private bool wasFired;
    private float dampedUseStrength;
    private float fireTimer;

    // ใช้เช็คว่าตอนนี้ "ถือปืนอยู่ไหม"
    [SerializeField] private bool canShoot = false;
    public bool CanShoot => canShoot;   // ให้สคริปต์อื่นอ่านได้

    #region IHandGrabUseDelegate
    public void BeginUse()
    {
        Debug.Log($"Begin use: {gameObject.name}");
        dampedUseStrength = 0f;
        lastUseTime = Time.realtimeSinceStartup;
        canShoot = true;        // เริ่มถือปืน / เริ่มใช้
    }

    public float ComputeUseStrength(float strength)
    {
        // ถ้าไม่ได้ถือปืนอยู่ ก็ไม่ต้องประมวลผลอะไร
        if (!canShoot)
            return 0f;

        float delta = Time.realtimeSinceStartup - lastUseTime;
        lastUseTime = Time.realtimeSinceStartup;

        if (strength > 0f)
        {
            dampedUseStrength = Mathf.Lerp(dampedUseStrength, strength, triggerSpeed * delta);
        }
        else
        {
            dampedUseStrength = strength;
        }

        UpdateTriggerProgress(dampedUseStrength);
        return dampedUseStrength;
    }

    public void EndUse()
    {
        Debug.Log($"End use: {gameObject.name}");
        canShoot = false;  // ปล่อยปืนแล้ว หยุดยิงทุกอย่าง
        wasFired = false;
        dampedUseStrength = 0f;
        fireTimer = 0f;
    }
    #endregion

    private void Update()
    {
        // ยิงอัตโนมัติเฉพาะตอนถือปืนอยู่ + เปิด auto fire
        if (!isAutoFireMode || !canShoot)
            return;

        if (wasFired && fireTimer <= 0f)
        {
            ShootBullet();
            fireTimer = fireInterval;
        }

        fireTimer -= Time.deltaTime;
    }

    private void UpdateTriggerProgress(float progress)
    {
        if (!canShoot)
            return;

        if (rotateXAxis)
            UpdateTriggerRotation(progress);

        if (moveZAxis)
            UpdateTriggerPosition(progress);

        if (progress >= fireThreshold && !wasFired)
        {
            wasFired = true;

            if (!isAutoFireMode)
            {
                // โหมดยิงทีละนัด
                ShootBullet();
            }
        }
        else if (progress <= releaseThreshold)
        {
            wasFired = false;
        }
    }

    private void UpdateTriggerRotation(float progress)
    {
        float value = triggerXRotation * progress;
        var angles = triggerPivot.localEulerAngles;
        angles.x = value;
        triggerPivot.localEulerAngles = angles;
    }

    private void UpdateTriggerPosition(float progress)
    {
        float value = triggerZPosition * progress;
        var pos = triggerPivot.localPosition;
        pos.z = value;
        triggerPivot.localPosition = pos;
    }

    private void ShootBullet()
    {
        if (!canShoot) return;   // กันเผื่อ

        Debug.Log($"{gameObject.name} shoot a bullet.");
        Instantiate(bullet, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        WhenShoot?.Invoke();
    }
}
