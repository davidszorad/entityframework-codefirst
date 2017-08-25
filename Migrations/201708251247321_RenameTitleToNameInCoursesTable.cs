namespace EntityFrameworkCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTitleToNameInCoursesTable : DbMigration
    {
        public override void Up()
        {
            /*
            // generated :
            AddColumn("dbo.Courses", "Name", c => c.String(nullable: false)); //nullable: false -> name is now required
            DropColumn("dbo.Courses", "Title");

            // better :
            RenameColumn("dbo.Courses", "Title", "Name");
            */

            // or :
            AddColumn("dbo.Courses", "Name", c => c.String(nullable: false)); //nullable: false -> name is now required
            Sql("UPDATE Courses SET Name = Title");
            DropColumn("dbo.Courses", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Title", c => c.String(nullable: false));
            Sql("UPDATE Courses SET Title = Name"); // added to keep content of Name column
            DropColumn("dbo.Courses", "Name");
        }
    }
}
