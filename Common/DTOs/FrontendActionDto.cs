namespace Common.DTOs;

public class FrontendActionDto
{
    public string? Intent { get; set; } 
    public string? Description { get; set; } 
    public string? TargetPath { get; set; } 
    public Dictionary<string, string>? Parameters { get; set; } 
}
