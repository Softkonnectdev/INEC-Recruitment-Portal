namespace Recruitment_Portal_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jobs", "Salary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Salary", c => c.String(nullable: false));
        }
    }
}
