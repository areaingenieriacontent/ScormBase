namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personalizacionfooter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StylesLogos", "titulofooter", c => c.String());
            AddColumn("dbo.StylesLogos", "colorTituloIndex", c => c.String());
            AddColumn("dbo.StylesLogos", "UrlImgMesaServicio", c => c.String());
            AddColumn("dbo.StylesLogos", "UrlLogoHeader", c => c.String());
            AddColumn("dbo.StylesLogos", "LinkSitioWeb", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StylesLogos", "LinkSitioWeb");
            DropColumn("dbo.StylesLogos", "UrlLogoHeader");
            DropColumn("dbo.StylesLogos", "UrlImgMesaServicio");
            DropColumn("dbo.StylesLogos", "colorTituloIndex");
            DropColumn("dbo.StylesLogos", "titulofooter");
        }
    }
}
