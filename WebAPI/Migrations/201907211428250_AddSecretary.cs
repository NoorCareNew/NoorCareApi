namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecretary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Secretary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 300),
                        LastName = c.String(maxLength: 300),
                        Email = c.String(maxLength: 256),
                        CountryCode = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 50),
                        AlternatePhoneNumber = c.String(maxLength: 50),
                        Gender = c.Int(nullable: false),
                        YearOfExperience = c.String(maxLength: 50),
                        SecretaryId = c.String(maxLength: 50),
                        HospitalId = c.String(maxLength: 50),
                        jobType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        ModifiedBy = c.String(maxLength: 128),
                        DateEntered = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Secretary");
        }
    }
}
