namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicantProfiles", "Passport", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicantProfiles", "Passport", c => c.Binary(nullable: false));
        }
    }
}
