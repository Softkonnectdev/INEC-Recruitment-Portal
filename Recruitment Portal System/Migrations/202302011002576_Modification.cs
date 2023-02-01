namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShortListedCandidates", "ShortListedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShortListedCandidates", "ShortListedOn", c => c.DateTime(nullable: false));
        }
    }
}
