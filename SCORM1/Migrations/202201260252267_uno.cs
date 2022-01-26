namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ServDCO", c => c.Boolean());
            AddColumn("dbo.Company", "ServDCO", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "ServDCO");
            DropColumn("dbo.Users", "ServDCO");
        }
    }
}
