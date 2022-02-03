namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edutuberconteolike : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EdutuberVideo", "EduVid_CountLike", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EdutuberVideo", "EduVid_CountLike", c => c.String());
        }
    }
}
