using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PlutoContext();

            // LINQ Query Operators
            var csharp_query =
                from c in context.Courses
                where c.Name.Contains("c#")
                orderby c.Name
                select c;

            foreach (var course in csharp_query)
                Console.WriteLine(course.Name);

            // Extension methods
            var csharp_courses = context.Courses
                .Where(c => c.Name.Contains("c#"))
                .OrderBy(c => c.Name);

            foreach (var course in csharp_courses)
                Console.WriteLine(course.Name);


            #region LINQ Query Operators
            var multiple_conditions_query =
                from c in context.Courses
                where c.Level == 1 && c.Author.Id == 1
                select c;


            var ordering_query =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select c;


            var projection_query =
                from c in context.Courses
                where c.Author.Id == 1
                orderby c.Level descending, c.Name
                select new { Name = c.Name, Author = c.Author.Name };


            var grouping_query =
                from c in context.Courses
                group c by c.Level into g
                select g;

            foreach (var group in grouping_query)
                Console.WriteLine("{0} ({1})", group.Key, group.Count());

            foreach (var group in grouping_query)
            {
                Console.WriteLine(group.Key);

                foreach (var course in group)
                {
                    Console.WriteLine("\t{0}", course.Name);
                }
            }


            var joining_query =
                from c in context.Courses
                select new { CourseName = c.Name, AuthorName = c.Author.Name }; // equivalent with INNER JOIN

            // if Course would not contain navigation property Author or there is no relationship between two entities then we would have to use inner join
            var inner_joining_query =
                from c in context.Courses
                join a in context.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Name, AuthorName = a.Name };


            var group_join_query__left_join__number_of_courses_per_author =
                from a in context.Authors
                join c in context.Courses on a.Id equals c.AuthorId into g
                select new { AuhorName = a.Name, Courses = g.Count() };

            foreach (var x in group_join_query__left_join__number_of_courses_per_author)
                Console.WriteLine("{0} ({1})", x.AuhorName, x.Courses);


            var cross_join_query__combination_of_every_author_and_every_course =
                from a in context.Authors
                from c in context.Courses
                select new { AuthorName = a.Name, CourseName = c.Name };

            foreach (var x in cross_join_query__combination_of_every_author_and_every_course)
                Console.WriteLine("{0} - {1}", x.AuthorName, x.CourseName);
            #endregion

            #region Extension methods
            var courses_that_are_level_1 = context.Courses.Where(c => c.Level == 1);


            var ordering_courses = context.Courses
                .Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Level);

            var ordering_courses_descending = context.Courses
                .Where(c => c.Level == 1)
                .OrderByDescending(c => c.Name)
                .ThenByDescending(c => c.Level);


            var projection_courses = context.Courses
                .Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Level)
                .Select(c => new { CourseName = c.Name, AuthorName = c.Author.Name });

            var projection_courses__contains_list_of_lists = context.Courses
                .Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Level)
                .Select(c => c.Tags);

            foreach (var c in projection_courses__contains_list_of_lists)
            {
                foreach (var tag in c)
                {
                    Console.WriteLine(tag.Name);
                }
            }

            var projection_flat_list_of_tags__improved_previous_query = context.Courses
                .Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Level)
                .SelectMany(c => c.Tags);

            foreach (var tag in projection_flat_list_of_tags__improved_previous_query)
                Console.WriteLine(tag.Name);


            var set_operators_courses__removes_duplicates = context.Courses
                .Where(c => c.Level == 1)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Level)
                .SelectMany(c => c.Tags)
                .Distinct();

            foreach (var tag in set_operators_courses__removes_duplicates)
                Console.WriteLine(tag.Name);


            var grouping_courses = context.Courses.GroupBy(c => c.Level);

            foreach (var group in grouping_courses)
            {
                Console.WriteLine("Key: {0}", group.Key);

                foreach (var course in group)
                    Console.WriteLine(course.Name);
            }


            var inner_join_courses = context.Courses.Join(context.Authors,
                c => c.AuthorId,
                a => a.Id,
                (course, author) => new
                {
                    CourseName = course.Name,
                    AuthorName = author.Name
                });


            var group_join_courses__left_join__number_of_courses_per_author = context.Authors.GroupJoin(context.Courses,
                a => a.Id,
                c => c.AuthorId,
                (author, courses) => new
                {
                    AuthorName = author.Name,
                    Courses = courses
                });
            var group_join_courses__left_join__number_of_courses_per_author__different_result = context.Authors.GroupJoin(context.Courses,
                a => a.Id,
                c => c.AuthorId,
                (author, courses) => new
                {
                    AuthorName = author,
                    Courses = courses.Count()
                });


            var cross_join_courses = context.Authors.SelectMany(a => context.Courses, (author, course) => new
            {
                AuthorName = author.Name,
                CourseName = course.Name
            });


            var partitioning_courses = context.Courses.Skip(10).Take(10);


            /* Although Last / LastOrDefault can be used with e.g. XML it cannot be used with SQL */
            var element_operators_course = context.Courses.OrderBy(c => c.Level).First(); //if empty -> exception --> fix it with FirstOrDefault -> if empty it returns null

            var element_operators_with_condition_course = context.Courses
                .OrderBy(c => c.Level)
                .FirstOrDefault(c => c.FullPrice > 100);

            var element_operators_single_course = context.Courses.SingleOrDefault(c => c.Id == 1);


            var quentifying_courses_all_above_10_dollars__bool = context.Courses.All(c => c.FullPrice > 10);
            var quentifying_courses_do_we_have_courses_in_level_1__bool = context.Courses.Any(c => c.Level == 1);


            var aggregating_count_courses = context.Courses.Count();
            var aggregating_count_with_condition_in_count_courses = context.Courses.Count(c => c.Level == 1);
            var aggregating_max_courses__float = context.Courses.Max(c => c.FullPrice);
            var aggregating_min_courses__float = context.Courses.Min(c => c.FullPrice);
            var aggregating_average_courses__float = context.Courses.Average(c => c.FullPrice);
            #endregion

            #region IQueryable vs IEnumerable
            IEnumerable<Course> queryable_courses_memory_extensive = context.Courses;
            var filtered_not_part_of_sql_query = queryable_courses_memory_extensive.Where(c => c.Level == 1); // not part of SQL query, filtering is done in memory on all courses

            // IQueryable has better impact on memory compared to IEnumerable (with IEnumerable you cannot extend the query)
            // -> here the filtering on next line is part of the SQL query
            IQueryable<Course> queryable_courses = context.Courses;
            var filtered = queryable_courses.Where(c => c.Level == 1);


            foreach (var course in filtered)
                Console.WriteLine(course.Name);
            #endregion

            #region Adding objects
            var new_course = new Course
            {
                Name = "",
                Description = "",
                FullPrice = 0,
                Level = 1,
                Author = new Author { Id = 1, Name = "Already existing user" }  //it would create duplicate user and change the Id to different value
            };
            context.Courses.Add(new_course);
            context.SaveChanges();

            // using an existing object in the context
            var existing_author = context.Authors.Single(a => a.Id == 1);
            var course_wpf = new Course
            {
                Name = "",
                Description = "",
                FullPrice = 0,
                Level = 1,
                Author = existing_author  //most suitable for WPF applications
            };
            context.Courses.Add(course_wpf);
            context.SaveChanges();

            // using foreign key property
            var course_mvc = new Course
            {
                Name = "",
                Description = "",
                FullPrice = 0,
                Level = 1,
                AuthorId = 1  //better for web applications
            };
            context.Courses.Add(course_mvc);
            context.SaveChanges();

            // it exists but you probably won't need to use it
            var new_author = new Author { Id = 1, Name = "Brand new user name" };
            context.Authors.Attach(new_author);
            var course_alternative = new Course
            {
                Name = "",
                Description = "",
                FullPrice = 0,
                Level = 1,
                Author = new_author
            };
            context.Courses.Add(course_alternative);
            context.SaveChanges();
            #endregion

            #region Updating objects
            var updated_course = context.Courses.Find(4); // Single(c => c.Id == 4) /* in case of composite keys we can pass multuple values like Find(1, 3, 5) */
            updated_course.Name = "New name";
            updated_course.AuthorId = 2;
            context.SaveChanges();
            #endregion

            #region Removing objects
            var remove_course = context.Courses.Find(6);
            context.Courses.Remove(remove_course); //with cascade delete enabled all the related tags will be deleted
            context.SaveChanges();


            /* 
             * var remove_author = context.Authors.Find(1);
             * context.Authors.Remove(remove_author); --> it will throw an exception because in CourseConfiguration [WillCascadeOnDelete(false)] there are courses that contain remove_author
             */

            var remove_author = context.Authors.Include(a => a.Courses).Single(a => a.Id == 1);
            context.Courses.RemoveRange(remove_author.Courses);
            context.Authors.Remove(remove_author);
            context.SaveChanges();
            #endregion
        }
    }
}
