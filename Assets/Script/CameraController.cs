using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ตัวละครที่กล้องจะติดตาม
    [SerializeField] private Vector3 offset; // ระยะห่างระหว่างกล้องกับผู้เล่น
    [SerializeField] private float smoothSpeed; // ความเร็วในการปรับตำแหน่งกล้อง (ความนุ่มนวล)
    //[SerializeField] private Transform background; // BG ที่ต้องการให้ติดตามกล้อง

    /*// ข้อมูลสำหรับแต่ละแผนที่
    [SerializeField] private Vector3 offsetMap1, offsetMap2, offsetMap3; // offset สำหรับแต่ละแผนที่

    [Header("Map 1")] [SerializeField] private float minXMap1, maxXMap1, minYMap1, maxYMap1; // ขอบเขตสำหรับ Map 1

    [Header("Map 2")] [SerializeField] private float minXMap2, maxXMap2, minYMap2, maxYMap2; // ขอบเขตสำหรับ Map 2

    [Header("Map 3")] [SerializeField] private float minXMap3, maxXMap3, minYMap3, maxYMap3; // ขอบเขตสำหรับ Map 3*/

    [SerializeField] private Transform[] backgrounds; // Array สำหรับพื้นหลังของแต่ละแผนที่

    private int currentMap = 0; // ตัวแปรที่เก็บแผนที่ปัจจุบัน (เริ่มต้นที่แผนที่ 0)
    
    private void LateUpdate()
    {
        FollowPlayer();
        FollowBackground();
    }

    private void FollowPlayer()
    {
        /*Vector3 offset = Vector3.zero;
        float minX = 0, maxX = 0, minY = 0, maxY = 0;

        // เลือกค่า offset และขอบเขตของกล้องตามแผนที่ที่กำลังใช้งาน
        switch (currentMap)
        {
            case 0:
                offset = offsetMap1;
                minX = minXMap1;
                maxX = maxXMap1;
                minY = minYMap1;
                maxY = maxYMap1;
                break;
            case 1:
                offset = offsetMap2;
                minX = minXMap2;
                maxX = maxXMap2;
                minY = minYMap2;
                maxY = maxYMap2;
                break;
            case 2:
                offset = offsetMap3;
                minX = minXMap3;
                maxX = maxXMap3;
                minY = minYMap3;
                maxY = maxYMap3;
                break;
        }*/

        // คำนวณตำแหน่งกล้องเป้าหมายตามตำแหน่งผู้เล่น
        Vector3 targetPosition = player.position + offset;

        // ใช้ Smooth Damping เพื่อปรับตำแหน่งกล้องให้นุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        /*// จำกัดตำแหน่งกล้องให้อยู่ในขอบเขตที่กำหนด
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);*/

        // อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPosition;
    }

    private void FollowBackground()
    {
        if (backgrounds != null && backgrounds.Length > currentMap)
        {
            // เปลี่ยนพื้นหลังของแผนที่ตาม currentMap
            Transform currentBackground = backgrounds[currentMap];
            if (currentBackground != null)
            {
                // ใช้ตำแหน่งของกล้องเป็นพื้นฐานในการปรับตำแหน่งพื้นหลัง
                currentBackground.position = new Vector3(transform.position.x, transform.position.y, currentBackground.position.z);
            }
        }
    }

    // ฟังก์ชันที่จะเรียกเพื่อเปลี่ยนแผนที่
    public void ChangeMap(int mapIndex)
    {
        if (mapIndex >= 0 && mapIndex < backgrounds.Length)
        {
            currentMap = mapIndex;
        }
    }
}



/*using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ตัวละครที่กล้องจะติดตาม
    [SerializeField] private Vector3 offset; // ระยะห่างระหว่างกล้องกับผู้เล่น
    [SerializeField] private float smoothSpeed; // ความเร็วในการปรับตำแหน่งกล้อง (ความนุ่มนวล)
    [SerializeField] private Transform background; // BG ที่ต้องการให้ติดตามกล้อง

    [SerializeField] private float minX, maxX, minY, maxY; // ขอบเขตของกล้อง

    private void LateUpdate()
    {
        FollowPlayer();
        FollowBackground();
    }

    private void FollowPlayer()
    {
        // คำนวณตำแหน่งกล้องเป้าหมายตามตำแหน่งผู้เล่น
        Vector3 targetPosition = player.position + offset;

        // ใช้ Smooth Damping เพื่อปรับตำแหน่งกล้องให้นุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // จำกัดตำแหน่งกล้องให้อยู่ในขอบเขตที่กำหนด
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        // อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPosition;
    }

    private void FollowBackground()
    {
        if (background != null)
        {
            background.position = new Vector3(transform.position.x, transform.position.y, background.position.z);
        }
    }
}*/




/*using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ตัวละครที่กล้องจะติดตาม
    [SerializeField] private Vector3 offset; // ระยะห่างระหว่างกล้องกับผู้เล่น
    [SerializeField] private float smoothSpeed; // ความเร็วในการปรับตำแหน่งกล้อง (ความนุ่มนวล)
    [SerializeField] private Transform background; // BG ที่ต้องการให้ติดตามกล้อง

    [SerializeField] private float minX, maxX, minY, maxY; // ขอบเขตของกล้อง

    // ข้อมูลสำหรับแต่ละแผนที่ (ระยะห่างระหว่างกล้องและผู้เล่น) 
    [SerializeField] private Vector3 offsetMap1, offsetMap2, offsetMap3;
    [SerializeField] private float minXMap1, maxXMap1, minYMap1, maxYMap1;
    [SerializeField] private float minXMap2, maxXMap2, minYMap2, maxYMap2;
    [SerializeField] private float minXMap3, maxXMap3, minYMap3, maxYMap3; 

    private int currentMap = 0; // ตัวแปรสำหรับเก็บแผนที่ปัจจุบัน (0 - แผนที่ 1, 1 - แผนที่ 2, 2 - แผนที่ 3)

    private void Start()
    {
        // ตรวจสอบว่าอยู่ในแผนที่ไหน (สามารถใช้ข้อมูลจาก Checkpoint หรือการวาร์ป)
        // ตัวอย่างการตรวจสอบแผนที่จาก Checkpoint
        if (Checkpoint.checkpointReached[0])
            currentMap = 0; // แผนที่ 1
        else if (Checkpoint.checkpointReached[1])
            currentMap = 1; // แผนที่ 2
        else if (Checkpoint.checkpointReached[2])
            currentMap = 2; // แผนที่ 3
    }

    private void LateUpdate()
    {
        FollowPlayer();
        FollowBackground();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = player.position + GetOffsetForCurrentMap();

        // ใช้ Smooth Damping เพื่อปรับตำแหน่งกล้องให้นุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        // จำกัดตำแหน่งกล้องให้อยู่ในขอบเขตที่กำหนด
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, GetMinXForCurrentMap(), GetMaxXForCurrentMap());
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, GetMinYForCurrentMap(), GetMaxYForCurrentMap());

        // อัปเดตตำแหน่งกล้อง
        transform.position = smoothedPosition;
    }

    private void FollowBackground()
    {
        if (background != null)
        {
            background.position = new Vector3(transform.position.x, transform.position.y, background.position.z);
        }
    }

    // ฟังก์ชันสำหรับดึง offset ที่ใช้สำหรับแผนที่ปัจจุบัน
    private Vector3 GetOffsetForCurrentMap()
    {
        switch (currentMap)
        {
            case 0: return offsetMap1;
            case 1: return offsetMap2;
            case 2: return offsetMap3;
            default: return Vector3.zero;
        }
    }

    // ฟังก์ชันสำหรับดึงค่าขอบเขตที่ใช้สำหรับแผนที่ปัจจุบัน
    private float GetMinXForCurrentMap()
    {
        switch (currentMap)
        {
            case 0: return minXMap1;
            case 1: return minXMap2;
            case 2: return minXMap3;
            default: return -Mathf.Infinity;
        }
    }

    private float GetMaxXForCurrentMap()
    {
        switch (currentMap)
        {
            case 0: return maxXMap1;
            case 1: return maxXMap2;
            case 2: return maxXMap3;
            default: return Mathf.Infinity;
        }
    }

    private float GetMinYForCurrentMap()
    {
        switch (currentMap)
        {
            case 0: return minYMap1;
            case 1: return minYMap2;
            case 2: return minYMap3;
            default: return -Mathf.Infinity;
        }
    }

    private float GetMaxYForCurrentMap()
    {
        switch (currentMap)
        {
            case 0: return maxYMap1;
            case 1: return maxYMap2;
            case 2: return maxYMap3;
            default: return Mathf.Infinity;
        }
    }
}*/

