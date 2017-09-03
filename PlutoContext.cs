using EntityFrameworkCodeFirst.EntityConfigurations;
using System.Data.Entity;

namespace EntityFrameworkCodeFirst
{
    public class PlutoContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public PlutoContext() : base("name=FluentApiConnection")
        {
            this.Configuration.LazyLoadingEnabled = false; //good practise in web applications
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CourseConfiguration());
        }
    }
}
