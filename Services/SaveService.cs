using System.Text.Json;
using System.IO;
using TurnBase.Model;

namespace TurnBase.Services;

public static class SaveService
{
    // ตั้งชื่อไฟล์เซฟและหาตำแหน่งเซฟในเครื่อง (ใช้ได้ทั้ง Android, iOS, Windows)
    private static string saveFilePath = Path.Combine(FileSystem.AppDataDirectory, "turnbase_savedata.json");

    // คำสั่งบันทึกเกม
    public static void SaveGame(SaveData data)
    {
        // แปลงข้อมูล SaveData เป็นข้อความ JSON
        string jsonString = JsonSerializer.Serialize(data);
        // เขียนลงไฟล์
        File.WriteAllText(saveFilePath, jsonString);
    }

    // คำสั่งโหลดเกม
    public static SaveData LoadGame()
    {
        // เช็กก่อนว่ามีไฟล์เซฟเก่าไหม ถ้าไม่มีให้คืนค่าว่าง (null)
        if (!File.Exists(saveFilePath))
            return null;

        // อ่านข้อความจากไฟล์
        string jsonString = File.ReadAllText(saveFilePath);
        // แปลงข้อความกลับมาเป็น SaveData
        return JsonSerializer.Deserialize<SaveData>(jsonString);
    }
}