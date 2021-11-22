namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "hasClientProfile", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "hasClientProfile");
        }
    }
}
