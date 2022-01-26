namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "hasClientProfile", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "hasClientProfile");
        }
    }
}
