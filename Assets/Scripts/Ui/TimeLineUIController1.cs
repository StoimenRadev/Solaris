using UnityEngine;
using UnityEngine.UI;

public class TimeLineUIController : MonoBehaviour
{
    public SimulationTimeController simController;
    public Text rateText;

    public void SetRateReal()
    {
        simController.SetTimeRate(1f);
        rateText.text = "REAL RATE";
    }

    public void SetRateMinutes(int minutes)
    {
        simController.SetTimeRate(minutes * 60f); // 60 sec per minute
        rateText.text = "+" + minutes + " min/s";
    }

    public void SetRateHours(int hours)
    {
        simController.SetTimeRate(hours * 3600f);
        rateText.text = "+" + hours + " h/s";
    }

    public void SetRateDays(int days)
    {
        simController.SetTimeRate(days * 86400f);
        rateText.text = "+" + days + " d/s";
    }

    public void PauseResume()
    {
        simController.PauseResume();
    }

    public void ResetToNow()
    {
        simController.ResetToRealTime();
        SetRateReal();
    }
}