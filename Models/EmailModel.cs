namespace WebApplication1.Models;

public class EmailModel
{
    public required string ToSomeone { get; set; }
    public required string Header { get; set; }
    public required string Content { get; set; }
}
