namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clasificacion", "Descripcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clasificacion", "Descripcion");
        }
    }
}
