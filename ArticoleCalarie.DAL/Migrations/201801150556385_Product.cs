namespace ArticoleCalarie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HexValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(nullable: false, maxLength: 50),
                        ProductName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        CategoryId = c.String(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Brand = c.String(),
                        MaterialDetails = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ProductCode);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsSizeChart = c.Boolean(nullable: false),
                        IsFirstImage = c.Boolean(nullable: false),
                        ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductColors",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Color_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Color_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Colors", t => t.Color_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Color_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductColors", "Color_Id", "dbo.Colors");
            DropForeignKey("dbo.ProductColors", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductColors", new[] { "Color_Id" });
            DropIndex("dbo.ProductColors", new[] { "Product_Id" });
            DropIndex("dbo.Images", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "ProductCode" });
            DropTable("dbo.ProductColors");
            DropTable("dbo.Images");
            DropTable("dbo.Products");
            DropTable("dbo.Colors");
        }
    }
}
