namespace P.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Layers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MeasurementID = c.Int(nullable: false),
                        NLayer = c.Int(nullable: false),
                        ABSposX11 = c.Double(nullable: false),
                        ABSposY11 = c.Double(nullable: false),
                        ABSposX12 = c.Double(nullable: false),
                        ABSposY12 = c.Double(nullable: false),
                        ABSposX21 = c.Double(nullable: false),
                        ABSposY21 = c.Double(nullable: false),
                        ABSposX22 = c.Double(nullable: false),
                        ABSposY22 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Measurements", t => t.MeasurementID, cascadeDelete: true)
                .Index(t => t.MeasurementID);
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogID = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Logs", t => t.LogID, cascadeDelete: true)
                .Index(t => t.LogID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SpecificationID = c.Int(nullable: false),
                        LogDate = c.DateTime(nullable: false),
                        JobNumber = c.String(nullable: false),
                        Operator = c.String(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        Comments = c.String(),
                        UploadName = c.String(nullable: false, maxLength: 256),
                        UploadData = c.Binary(),
                        UploadGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Specifications", t => t.SpecificationID, cascadeDelete: true)
                .Index(t => t.SpecificationID)
                .Index(t => t.UploadName, unique: true);
            
            CreateTable(
                "dbo.SkippedSerialNumbers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LogID = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Logs", t => t.LogID, cascadeDelete: true)
                .Index(t => t.LogID);
            
            CreateTable(
                "dbo.Specifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartNumber = c.String(nullable: false, maxLength: 255),
                        LayerOne = c.Int(),
                        LayerTwo = c.Int(),
                        Tolerance = c.Double(),
                        Offset = c.Double(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.PartNumber, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Layers", "MeasurementID", "dbo.Measurements");
            DropForeignKey("dbo.Measurements", "LogID", "dbo.Logs");
            DropForeignKey("dbo.Logs", "SpecificationID", "dbo.Specifications");
            DropForeignKey("dbo.SkippedSerialNumbers", "LogID", "dbo.Logs");
            DropIndex("dbo.Specifications", new[] { "PartNumber" });
            DropIndex("dbo.SkippedSerialNumbers", new[] { "LogID" });
            DropIndex("dbo.Logs", new[] { "UploadName" });
            DropIndex("dbo.Logs", new[] { "SpecificationID" });
            DropIndex("dbo.Measurements", new[] { "LogID" });
            DropIndex("dbo.Layers", new[] { "MeasurementID" });
            DropTable("dbo.Specifications");
            DropTable("dbo.SkippedSerialNumbers");
            DropTable("dbo.Logs");
            DropTable("dbo.Measurements");
            DropTable("dbo.Layers");
        }
    }
}
