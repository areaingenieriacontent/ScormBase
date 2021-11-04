namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VsdrSession", "session_url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VsdrSession", "session_url");
        }
    }
}
