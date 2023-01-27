namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicantProfiles", "CreatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicantProfiles", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
