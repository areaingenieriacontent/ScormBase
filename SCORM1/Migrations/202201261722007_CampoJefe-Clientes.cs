namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoJefeClientes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "SalesManager", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cliente", "SalesManager");
        }
    }
}
