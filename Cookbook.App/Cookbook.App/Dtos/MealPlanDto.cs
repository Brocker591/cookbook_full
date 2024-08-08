using System.Xml;
using System;

namespace Cookbook.App.Dtos;

public class MealPlanDto
{
    public int id { get; set; }
    public string monday { get; set; } = default!;
    public string tuesday { get; set; } = default!;
    public string wednesday { get;set; } = default!;
    public string thursday { get; set; } = default!;
    public string friday { get; set; } = default!;
    public string saturday { get; set; } = default!;
    public string sunday { get; set; } = default!;
}
