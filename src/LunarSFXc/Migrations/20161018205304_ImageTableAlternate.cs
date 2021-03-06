﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LunarSFXc.Migrations
{
    public partial class ImageTableAlternate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "ImageDescriptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageDescriptions_PostId",
                table: "ImageDescriptions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageDescriptions_Posts_PostId",
                table: "ImageDescriptions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageDescriptions_Posts_PostId",
                table: "ImageDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_ImageDescriptions_PostId",
                table: "ImageDescriptions");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ImageDescriptions");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<byte[]>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: true),
                    Postion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_PostId",
                table: "Images",
                column: "PostId");
        }
    }
}
