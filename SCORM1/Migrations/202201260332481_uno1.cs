namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "Sales_Manager", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cliente", "Sales_Manager");
        }
    }
}
