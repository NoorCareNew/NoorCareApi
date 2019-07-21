namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblDoctormigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctor", "DoctorId", c => c.String(maxLength: 50));
            AddColumn("dbo.Doctor", "HospitalId", c => c.String(maxLength: 50));
            AddColumn("dbo.Doctor", "jobType", c => c.Int(nullable: false));
            AddColumn("dbo.Doctor", "CountryCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Doctor", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctor", "Gender", c => c.String(maxLength: 50));
            DropColumn("dbo.Doctor", "CountryCode");
            DropColumn("dbo.Doctor", "jobType");
            DropColumn("dbo.Doctor", "HospitalId");
            DropColumn("dbo.Doctor", "DoctorId");
        }
    }
}
