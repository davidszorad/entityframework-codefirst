using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCodeFirst
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public float FullPrice { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public IList<Tag> Tags { get; set; }
        public Cover Cover { get; set; }
    }
}
