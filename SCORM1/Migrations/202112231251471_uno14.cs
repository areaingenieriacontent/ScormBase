namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StylesLogos", "colorTexto", c => c.String());
            AddColumn("dbo.StylesLogos", "colorBoton", c => c.String());
            AddColumn("dbo.StylesLogos", "colorTextoBtn", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StylesLogos", "colorTextoBtn");
            DropColumn("dbo.StylesLogos", "colorBoton");
            DropColumn("dbo.StylesLogos", "colorTexto");
        }
    }
}
