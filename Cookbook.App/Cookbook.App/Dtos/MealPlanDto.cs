using System.Xml;
using System;

namespace Cookbook.App.Dtos;

public class MealPlanDto
{
    public int id { get; set; }
    public string monday { get; set; }
    public string tuesday { get; set; }
    public string wednesday { get;set; }
    public string thursday { get; set; }
    public string friday { get; set; }
    public string saturday { get; set; }
    public string sunday { get; set; }
}
