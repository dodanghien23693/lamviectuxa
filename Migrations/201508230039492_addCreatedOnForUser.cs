namespace WebSoftSeo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreatedOnForUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreatedOn");
        }
    }
}
