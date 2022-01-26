namespace SCORM1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uno11 : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => new { t.user_id, t.Edutuber_id })
                .ForeignKey("dbo.EdutuberSession", t => t.Edutuber_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.Edutuber_id);
            
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
                .PrimaryKey(t => t.comment_id)
                .ForeignKey("dbo.EdutuberEnrollment", t => new { t.user_id, t.Edutuber_id })
                .Index(t => new { t.user_id, t.Edutuber_id });
            
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
                .PrimaryKey(t => t.Edutuber_file_id)
                .ForeignKey("dbo.EdutuberEnrollment", t => new { t.user_id, t.Edutuber_id })
                .Index(t => new { t.user_id, t.Edutuber_id });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment");
            DropForeignKey("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" }, "dbo.EdutuberEnrollment");
            DropForeignKey("dbo.EdutuberEnrollment", "user_id", "dbo.Users");
            DropForeignKey("dbo.EdutuberEnrollment", "Edutuber_id", "dbo.EdutuberSession");
            DropIndex("dbo.EdutuberUserFile", new[] { "user_id", "Edutuber_id" });
            DropIndex("dbo.EdutuberTeacherComment", new[] { "user_id", "Edutuber_id" });
            DropIndex("dbo.EdutuberEnrollment", new[] { "Edutuber_id" });
            DropIndex("dbo.EdutuberEnrollment", new[] { "user_id" });
            DropTable("dbo.EdutuberUserFile");
            DropTable("dbo.EdutuberTeacherComment");
            DropTable("dbo.EdutuberSession");
            DropTable("dbo.EdutuberEnrollment");
        }
    }
}
