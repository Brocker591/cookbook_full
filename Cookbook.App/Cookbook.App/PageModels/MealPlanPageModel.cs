using Cookbook.App.Models;

namespace Cookbook.App.PageModels;

public class MealPlanPageModel : BaseModel
{
    private string dayName;
    public string DayName
    {
        get => dayName;
        set
        {
            if (dayName == value)
                return;
            dayName = value;
            this.OnPropertyChanged();
        }
    }

    private string mealName;
    public string MealName
    {
        get => mealName;
        set
        {
            if (mealName == value)
                return;
            mealName = value;
            this.OnPropertyChanged();
        }
    }
}
