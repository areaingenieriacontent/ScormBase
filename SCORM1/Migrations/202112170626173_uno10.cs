namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ServEdutuber", c => c.Boolean());
            AddColumn("dbo.Company", "ServEdutuber", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "ServEdutuber");
            DropColumn("dbo.Users", "ServEdutuber");
        }
    }
}
