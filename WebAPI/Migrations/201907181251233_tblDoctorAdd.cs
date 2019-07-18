namespace WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblDoctorAdd : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "dbo.Doctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        AlternatePhoneNumber = c.String(),
                        Gender = c.String(),
                        Experience = c.String(),
                        FeeMoney = c.String(),
                        Language = c.String(),
                        AgeGroupGender = c.String(),
                        Degree = c.String(),
                        Specialization = c.String(),
                        PhotoPath = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        DateEntered = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
           
            DropTable("dbo.Doctor");
            
        }
    }
}
