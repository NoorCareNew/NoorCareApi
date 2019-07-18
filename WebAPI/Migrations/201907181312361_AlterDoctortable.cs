namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDoctortable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Doctor", "FirstName", c => c.String(maxLength: 300));
            AlterColumn("dbo.Doctor", "LastName", c => c.String(maxLength: 300));
            AlterColumn("dbo.Doctor", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.Doctor", "PhoneNumber", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "AlternatePhoneNumber", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "Gender", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "Experience", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "FeeMoney", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.Doctor", "Language", c => c.String(maxLength: 100));
            AlterColumn("dbo.Doctor", "AgeGroupGender", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "Degree", c => c.String(maxLength: 50));
            AlterColumn("dbo.Doctor", "Specialization", c => c.String(maxLength: 200));
            AlterColumn("dbo.Doctor", "PhotoPath", c => c.String(maxLength: 500));
            AlterColumn("dbo.Doctor", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Doctor", "ModifiedBy", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctor", "ModifiedBy", c => c.String());
            AlterColumn("dbo.Doctor", "CreatedBy", c => c.String());
            AlterColumn("dbo.Doctor", "PhotoPath", c => c.String());
            AlterColumn("dbo.Doctor", "Specialization", c => c.String());
            AlterColumn("dbo.Doctor", "Degree", c => c.String());
            AlterColumn("dbo.Doctor", "AgeGroupGender", c => c.String());
            AlterColumn("dbo.Doctor", "Language", c => c.String());
            AlterColumn("dbo.Doctor", "FeeMoney", c => c.String());
            AlterColumn("dbo.Doctor", "Experience", c => c.String());
            AlterColumn("dbo.Doctor", "Gender", c => c.String());
            AlterColumn("dbo.Doctor", "AlternatePhoneNumber", c => c.String());
            AlterColumn("dbo.Doctor", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Doctor", "Email", c => c.String());
            AlterColumn("dbo.Doctor", "LastName", c => c.String());
            AlterColumn("dbo.Doctor", "FirstName", c => c.String());
        }
    }
}
