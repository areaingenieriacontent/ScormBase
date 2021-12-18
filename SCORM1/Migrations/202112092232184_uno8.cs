namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "ServVideoteca", c => c.Boolean());
            AddColumn("dbo.Company", "ServJuegos", c => c.Boolean());
            AddColumn("dbo.Company", "ServRevista", c => c.Boolean());
            AddColumn("dbo.Company", "ServBiblioteca", c => c.Boolean());
            AddColumn("dbo.Company", "ServABE", c => c.Boolean());
            AddColumn("dbo.Company", "ServVSDR", c => c.Boolean());
            AddColumn("dbo.Company", "ServCafeteria", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "ServCafeteria");
            DropColumn("dbo.Company", "ServVSDR");
            DropColumn("dbo.Company", "ServABE");
            DropColumn("dbo.Company", "ServBiblioteca");
            DropColumn("dbo.Company", "ServRevista");
            DropColumn("dbo.Company", "ServJuegos");
            DropColumn("dbo.Company", "ServVideoteca");
        }
    }
}
