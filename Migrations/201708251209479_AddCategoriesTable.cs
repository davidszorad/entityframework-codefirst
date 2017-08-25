namespace EntityFrameworkCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            // to be able to insert Id into DB change identity to false -> Id = c.Int(nullable: false, identity: false)
            Sql("INSERT INTO Categories (Name) VALUES ('Web Development')");
            Sql("INSERT INTO Categories (Name) VALUES ('Programming Languages')");
        }
        
        public override void Down()
        {
            DropTable("dbo.Categories");
        }
    }
}
