namespace WebSoftSeo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update248 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SkillJobs",
                c => new
                    {
                        Skill_Id = c.Int(nullable: false),
                        Job_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_Id, t.Job_Id })
                .ForeignKey("dbo.Skills", t => t.Skill_Id, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .Index(t => t.Skill_Id)
                .Index(t => t.Job_Id);
            
            CreateTable(
                "dbo.SkillApplicationUsers",
                c => new
                    {
                        Skill_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Skill_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Skills", t => t.Skill_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Skill_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddColumn("dbo.AttachedFiles", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Jobs", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Jobs", "ModifiedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "Skills");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Skills", c => c.String());
            DropForeignKey("dbo.SkillApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SkillApplicationUsers", "Skill_Id", "dbo.Skills");
            DropForeignKey("dbo.SkillJobs", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.SkillJobs", "Skill_Id", "dbo.Skills");
            DropIndex("dbo.SkillApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SkillApplicationUsers", new[] { "Skill_Id" });
            DropIndex("dbo.SkillJobs", new[] { "Job_Id" });
            DropIndex("dbo.SkillJobs", new[] { "Skill_Id" });
            DropColumn("dbo.Jobs", "ModifiedOn");
            DropColumn("dbo.Jobs", "CreatedOn");
            DropColumn("dbo.AttachedFiles", "CreatedOn");
            DropTable("dbo.SkillApplicationUsers");
            DropTable("dbo.SkillJobs");
            DropTable("dbo.Skills");
        }
    }
}
