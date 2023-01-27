namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobCategories", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobCategories", "Description", c => c.String(nullable: false));
        }
    }
}
