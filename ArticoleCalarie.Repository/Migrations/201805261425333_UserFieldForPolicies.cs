namespace ArticoleCalarie.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFieldForPolicies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsTermsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsPrivacyPolicyAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsNewsletterSubscription", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsNewsletterSubscription");
            DropColumn("dbo.AspNetUsers", "IsPrivacyPolicyAccepted");
            DropColumn("dbo.AspNetUsers", "IsTermsAccepted");
        }
    }
}
