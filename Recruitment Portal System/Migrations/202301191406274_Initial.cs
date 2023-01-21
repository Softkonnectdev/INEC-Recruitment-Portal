namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantProfiles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        SurName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 11),
                        DOB = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        PermanentResident = c.String(nullable: false),
                        LGAOrigin = c.String(nullable: false),
                        StateOrigin = c.String(nullable: false),
                        ResidentialCountry = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                        Skills = c.String(),
                        Passport = c.Binary(nullable: false),
                        PreferredJobLocation = c.String(nullable: false),
                        EmailNotification = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ApplicantWorkExperience_ApplicantID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicantWorkExperiences", t => t.ApplicantWorkExperience_ApplicantID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ApplicantWorkExperience_ApplicantID);
            
            CreateTable(
                "dbo.ApplicantWorkExperiences",
                c => new
                    {
                        ApplicantID = c.String(nullable: false, maxLength: 128),
                        Position = c.String(nullable: false),
                        YearofExperience = c.Int(nullable: false),
                        StartedDate = c.DateTime(nullable: false),
                        TerminationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicantID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.CVs",
                c => new
                    {
                        ApplicantID = c.String(nullable: false, maxLength: 128),
                        CVMerits = c.String(nullable: false),
                        CVPath = c.String(nullable: false),
                        CoverPage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantID)
                .ForeignKey("dbo.ApplicantProfiles", t => t.ApplicantID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicantID = c.String(nullable: false, maxLength: 128),
                        JobID = c.String(nullable: false, maxLength: 128),
                        ApplicantCoverPage = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicantProfiles", t => t.ApplicantID, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.ApplicantID)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.ApplicationHistories",
                c => new
                    {
                        JobAppID = c.String(nullable: false, maxLength: 128),
                        AppliedOn = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.JobAppID)
                .ForeignKey("dbo.JobApplications", t => t.JobAppID)
                .Index(t => t.JobAppID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        JobCategoryID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        JobState = c.String(nullable: false, maxLength: 128),
                        JobType = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        Salary = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StateBranches", t => t.JobState, cascadeDelete: true)
                .ForeignKey("dbo.JobCategories", t => t.JobCategoryID, cascadeDelete: true)
                .Index(t => t.JobCategoryID)
                .Index(t => t.JobState);
            
            CreateTable(
                "dbo.StateBranches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        BranchAdminID = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Description = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobCategories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShortListedCandidates",
                c => new
                    {
                        JobApplicationID = c.String(nullable: false, maxLength: 128),
                        ApplicantEmail = c.String(nullable: false),
                        ShortListedOn = c.DateTime(nullable: false),
                        ApplicantMerit = c.String(nullable: false),
                        Job_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.JobApplications", t => t.JobApplicationID)
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .Index(t => t.JobApplicationID)
                .Index(t => t.Job_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ApplicantProfiles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobApplications", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.ShortListedCandidates", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.ShortListedCandidates", "JobApplicationID", "dbo.JobApplications");
            DropForeignKey("dbo.Jobs", "JobCategoryID", "dbo.JobCategories");
            DropForeignKey("dbo.Jobs", "JobState", "dbo.StateBranches");
            DropForeignKey("dbo.ApplicationHistories", "JobAppID", "dbo.JobApplications");
            DropForeignKey("dbo.JobApplications", "ApplicantID", "dbo.ApplicantProfiles");
            DropForeignKey("dbo.CVs", "ApplicantID", "dbo.ApplicantProfiles");
            DropForeignKey("dbo.ApplicantProfiles", "ApplicantWorkExperience_ApplicantID", "dbo.ApplicantWorkExperiences");
            DropForeignKey("dbo.ApplicantWorkExperiences", "ApplicantID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ShortListedCandidates", new[] { "Job_Id" });
            DropIndex("dbo.ShortListedCandidates", new[] { "JobApplicationID" });
            DropIndex("dbo.Jobs", new[] { "JobState" });
            DropIndex("dbo.Jobs", new[] { "JobCategoryID" });
            DropIndex("dbo.ApplicationHistories", new[] { "JobAppID" });
            DropIndex("dbo.JobApplications", new[] { "JobID" });
            DropIndex("dbo.JobApplications", new[] { "ApplicantID" });
            DropIndex("dbo.CVs", new[] { "ApplicantID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ApplicantWorkExperiences", new[] { "ApplicantID" });
            DropIndex("dbo.ApplicantProfiles", new[] { "ApplicantWorkExperience_ApplicantID" });
            DropIndex("dbo.ApplicantProfiles", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ShortListedCandidates");
            DropTable("dbo.JobCategories");
            DropTable("dbo.StateBranches");
            DropTable("dbo.Jobs");
            DropTable("dbo.ApplicationHistories");
            DropTable("dbo.JobApplications");
            DropTable("dbo.CVs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ApplicantWorkExperiences");
            DropTable("dbo.ApplicantProfiles");
        }
    }
}
