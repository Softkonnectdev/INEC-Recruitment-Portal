namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Jobs", name: "JobState", newName: "JobStateBranchID");
            RenameIndex(table: "dbo.Jobs", name: "IX_JobState", newName: "IX_JobStateBranchID");
            AlterColumn("dbo.CVs", "CoverPage", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CVs", "CoverPage", c => c.String(nullable: false));
            RenameIndex(table: "dbo.Jobs", name: "IX_JobStateBranchID", newName: "IX_JobState");
            RenameColumn(table: "dbo.Jobs", name: "JobStateBranchID", newName: "JobState");
        }
    }
}
