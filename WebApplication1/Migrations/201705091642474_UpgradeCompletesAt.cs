namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpgradeCompletesAt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mine", "UpgradeCompleteAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mine", "UpgradeCompleteAt");
        }
    }
}
