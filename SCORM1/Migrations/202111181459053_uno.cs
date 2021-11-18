namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "hasClientProfile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "hasClientProfile", c => c.Boolean());
        }
    }
}
