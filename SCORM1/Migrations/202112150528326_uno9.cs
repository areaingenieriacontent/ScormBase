namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ServVideoteca", c => c.Boolean());
            AddColumn("dbo.Users", "ServJuegos", c => c.Boolean());
            AddColumn("dbo.Users", "ServRevista", c => c.Boolean());
            AddColumn("dbo.Users", "ServBiblioteca", c => c.Boolean());
            AddColumn("dbo.Users", "ServABE", c => c.Boolean());
            AddColumn("dbo.Users", "ServVSDR", c => c.Boolean());
            AddColumn("dbo.Users", "ServCafeteria", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ServCafeteria");
            DropColumn("dbo.Users", "ServVSDR");
            DropColumn("dbo.Users", "ServABE");
            DropColumn("dbo.Users", "ServBiblioteca");
            DropColumn("dbo.Users", "ServRevista");
            DropColumn("dbo.Users", "ServJuegos");
            DropColumn("dbo.Users", "ServVideoteca");
        }
    }
}
