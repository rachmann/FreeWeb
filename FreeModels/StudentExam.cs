using System;
using System.ComponentModel.DataAnnotations;

namespace FreeModels
{
    public class StudentExam
    {
        public int StudentExamId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public string Degree { get; set; }
    }
}