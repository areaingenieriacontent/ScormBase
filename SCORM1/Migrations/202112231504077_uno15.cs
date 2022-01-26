namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StylesLogos", "colorIconos", c => c.String());
            AddColumn("dbo.StylesLogos", "colorMenu", c => c.String());
            AddColumn("dbo.StylesLogos", "colorTextMenu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StylesLogos", "colorTextMenu");
            DropColumn("dbo.StylesLogos", "colorMenu");
            DropColumn("dbo.StylesLogos", "colorIconos");
        }
    }
}
