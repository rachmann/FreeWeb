using System;
using System.ComponentModel.DataAnnotations;

namespace FreeModels
{
public class Student
{
    public int StudentId { get; set; }

    [Required]
    public int CityId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string EmailAddress { get; set; }

    [Required]
    public string Password { get; set; }

    // "M" for mail, "F" for female.
    [Required]
    public string Gender { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    public string About { get; set; }

    // 0: Unselected, 1: Primary school, 2: High school 3: University
    [Required]
    public int Education { get; set; }

    //true: Active, false: Passive
    [Required]
    public bool IsActive { get; set; }

    [Required]
    public DateTime RecordDate { get; set; }

    public Student()
    {
        RecordDate = DateTime.Now;
        Password = "123";
        About = "";
    }
}
}