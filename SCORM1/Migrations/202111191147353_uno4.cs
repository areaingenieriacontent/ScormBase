namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "hasClientProfile", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "hasClientProfile", c => c.Int());
        }
    }
}
