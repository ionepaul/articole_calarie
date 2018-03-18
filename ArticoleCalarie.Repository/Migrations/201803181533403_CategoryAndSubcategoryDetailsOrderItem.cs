namespace ArticoleCalarie.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryAndSubcategoryDetailsOrderItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "Category", c => c.String());
            AddColumn("dbo.OrderItems", "Subcategory", c => c.String());
            AddColumn("dbo.OrderItems", "SubcategoryId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "SubcategoryId");
            DropColumn("dbo.OrderItems", "Subcategory");
            DropColumn("dbo.OrderItems", "Category");
        }
    }
}
