using System;

namespace FreeModels
{
    public class Person
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime RecordDate { get; set; }

        public Person()
        {
            RecordDate = DateTime.Now;
        }
    }
}