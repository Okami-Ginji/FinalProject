using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    
    public float timeRemaining = 600f; // 10 phút (600 giây)
    public TextMeshProUGUI timeUI; // UI hiển thị thời gian

    private bool isRunning = true; // Kiểm soát trạng thái đếm ngược

    void Start()
    {
        UpdateTimeUI(); // Cập nhật UI ngay khi game bắt đầu
    }

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Giảm thời gian mỗi frame
            UpdateTimeUI(); // Cập nhật UI mỗi frame
        }
        else if (isRunning && timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            timeUI.text = "Time's Up!";
            TimeUp();
        }
    }

    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // Lấy số phút
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // Lấy số giây

        timeUI.text = string.Format("{0:00} : {1:00}", minutes, seconds); // Hiển thị dạng MM:SS
    }

    void TimeUp()
    {
        Debug.Log("Hết thời gian!");
        // Thêm logic khi hết giờ, ví dụ: dừng game, hiển thị Game Over...
    }
}
