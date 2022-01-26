namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edutuber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EdutuberEnrollment", "Edutuber_id", "dbo.EdutuberSession");
            DropForeignKey("dbo.EdutuberEnrollment", "user_id", "dbo.Users");
            DropForeignKey("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment");
            DropForeignKey("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment");
            DropIndex("dbo.EdutuberEnrollment", new[] { "user_id" });
            DropIndex("dbo.EdutuberEnrollment", new[] { "Edutuber_id" });
            DropIndex("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" });
            DropIndex("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" });
            CreateTable(
                "dbo.EdutuberLike",
                c => new
                    {
                        EduLike_Id = c.Int(nullable: false, identity: true),
                        EduLike_fecha = c.DateTime(nullable: false),
                        EduLike_Estate = c.Boolean(nullable: false),
                        EduVideo_id = c.Int(nullable: false),
                        user_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EduLike_Id)
                .ForeignKey("dbo.Users", t => t.user_id)
                .ForeignKey("dbo.EdutuberVideo", t => t.EduVideo_id, cascadeDelete: true)
                .Index(t => t.EduVideo_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.EdutuberVideo",
                c => new
                    {
                        EduVideo_id = c.Int(nullable: false, identity: true),
                        EduVid_Titulo = c.String(),
                        EduVid_Descri = c.String(),
                        EduVid_UrlVideo = c.String(),
                        EduVid_UrlImag1 = c.String(),
                        EduVid_UrlImag2 = c.String(),
                        EduVid_UrlExpe = c.String(),
                        EduVid_CountLike = c.String(),
                        company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EduVideo_id)
                .ForeignKey("dbo.Company", t => t.company_Id, cascadeDelete: true)
                .Index(t => t.company_Id);
            
            AddColumn("dbo.Users", "ServDCO", c => c.Boolean());
            AddColumn("dbo.Company", "ServDCO", c => c.Boolean());
            DropTable("dbo.EdutuberEnrollment");
            DropTable("dbo.EdutuberSession");
            DropTable("dbo.EdutuberTeacherComment");
            DropTable("dbo.EdutuberUserFile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EdutuberUserFile",
                c => new
                    {
                        user_id = c.String(maxLength: 128),
                        Edutuber_id = c.Int(nullable: false),
                        Edutuber_file_id = c.Int(nullable: false, identity: true),
                        register_name = c.String(),
                        file_description = c.String(),
                        file_name = c.String(),
                        file_extention = c.String(),
                        registered_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Edutuber_file_id);
            
            CreateTable(
                "dbo.EdutuberTeacherComment",
                c => new
                    {
                        user_id = c.String(maxLength: 128),
                        Edutuber_id = c.Int(nullable: false),
                        comment_id = c.Int(nullable: false, identity: true),
                        teacher_id = c.String(),
                        content = c.String(),
                        commentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.comment_id);
            
            CreateTable(
                "dbo.EdutuberSession",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        case_content = c.String(),
                        start_date = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        session_url = c.String(),
                        resource_url = c.String(),
                        available = c.Boolean(nullable: false),
                        open = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.EdutuberEnrollment",
                c => new
                    {
                        user_id = c.String(nullable: false, maxLength: 128),
                        Edutuber_id = c.Int(nullable: false),
                        Edutuber_enro_init_date = c.DateTime(nullable: false),
                        Edutuber_enro_finish_date = c.DateTime(nullable: false),
                        qualification = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.Edutuber_id });
            
            DropForeignKey("dbo.EdutuberLike", "EduVideo_id", "dbo.EdutuberVideo");
            DropForeignKey("dbo.EdutuberVideo", "company_Id", "dbo.Company");
            DropForeignKey("dbo.EdutuberLike", "user_id", "dbo.Users");
            DropIndex("dbo.EdutuberVideo", new[] { "company_Id" });
            DropIndex("dbo.EdutuberLike", new[] { "user_id" });
            DropIndex("dbo.EdutuberLike", new[] { "EduVideo_id" });
            DropColumn("dbo.Company", "ServDCO");
            DropColumn("dbo.Users", "ServDCO");
            DropTable("dbo.EdutuberVideo");
            DropTable("dbo.EdutuberLike");
            CreateIndex("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" });
            CreateIndex("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" });
            CreateIndex("dbo.EdutuberEnrollment", "Edutuber_id");
            CreateIndex("dbo.EdutuberEnrollment", "user_id");
            AddForeignKey("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment", new[] { "user_id", "Edutuber_id" });
            AddForeignKey("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment", new[] { "user_id", "Edutuber_id" });
            AddForeignKey("dbo.EdutuberEnrollment", "user_id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EdutuberEnrollment", "Edutuber_id", "dbo.EdutuberSession", "id", cascadeDelete: true);
        }
    }
}
