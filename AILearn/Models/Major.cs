using System.Collections.Generic;

namespace AILearn.Models;

public class Major
{
    public string Id { get; set; }        // e.g., "cs_01"
    public string Title { get; set; }     // e.g., "Computer Science"
    public string Category { get; set; }  // e.g., "Science & Technology"
    public List<string> Courses { get; set; } = new(); // The list of courses
}