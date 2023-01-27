namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantProfiles", "ResidentialAddress", c => c.String(nullable: false));
            DropColumn("dbo.ApplicantProfiles", "PermanentResident");
            DropColumn("dbo.ApplicantProfiles", "ResidentialCountry");
            DropColumn("dbo.ApplicantProfiles", "Nationality");
            DropColumn("dbo.StateBranches", "BranchAdminID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StateBranches", "BranchAdminID", c => c.String(nullable: false));
            AddColumn("dbo.ApplicantProfiles", "Nationality", c => c.String(nullable: false));
            AddColumn("dbo.ApplicantProfiles", "ResidentialCountry", c => c.String(nullable: false));
            AddColumn("dbo.ApplicantProfiles", "PermanentResident", c => c.String(nullable: false));
            DropColumn("dbo.ApplicantProfiles", "ResidentialAddress");
        }
    }
}
