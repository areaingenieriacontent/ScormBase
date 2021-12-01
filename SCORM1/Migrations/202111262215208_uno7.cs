namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cliente", "idClasificacion", "dbo.Clasificacion");
            DropIndex("dbo.Cliente", new[] { "idClasificacion" });
            AlterColumn("dbo.Cliente", "idClasificacion", c => c.Int());
            CreateIndex("dbo.Cliente", "idClasificacion");
            AddForeignKey("dbo.Cliente", "idClasificacion", "dbo.Clasificacion", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cliente", "idClasificacion", "dbo.Clasificacion");
            DropIndex("dbo.Cliente", new[] { "idClasificacion" });
            AlterColumn("dbo.Cliente", "idClasificacion", c => c.Int(nullable: false));
            CreateIndex("dbo.Cliente", "idClasificacion");
            AddForeignKey("dbo.Cliente", "idClasificacion", "dbo.Clasificacion", "id", cascadeDelete: true);
        }
    }
}
