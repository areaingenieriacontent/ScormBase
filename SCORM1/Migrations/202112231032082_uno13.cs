namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Company", "ServVideoteca", c => c.Boolean());
            AlterColumn("dbo.Company", "ServJuegos", c => c.Boolean());
            AlterColumn("dbo.Company", "ServRevista", c => c.Boolean());
            AlterColumn("dbo.Company", "ServBiblioteca", c => c.Boolean());
            AlterColumn("dbo.Company", "ServABE", c => c.Boolean());
            AlterColumn("dbo.Company", "ServVSDR", c => c.Boolean());
            AlterColumn("dbo.Company", "ServEdutuber", c => c.Boolean());
            AlterColumn("dbo.Company", "ServCafeteria", c => c.Boolean());
            AlterColumn("dbo.Company", "hasClientProfile", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Company", "hasClientProfile", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServCafeteria", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServEdutuber", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServVSDR", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServABE", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServBiblioteca", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServRevista", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServJuegos", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Company", "ServVideoteca", c => c.Boolean(nullable: false));
        }
    }
}
