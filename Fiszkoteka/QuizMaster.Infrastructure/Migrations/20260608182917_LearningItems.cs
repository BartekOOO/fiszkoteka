using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LearningItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningSessionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearningSessionId = table.Column<int>(type: "int", nullable: false),
                    FlashcardId = table.Column<int>(type: "int", nullable: false),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSessionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningSessionItems_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningSessionItems_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessionItems_FlashcardId",
                table: "LearningSessionItems",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessionItems_LearningSessionId_FlashcardId",
                table: "LearningSessionItems",
                columns: new[] { "LearningSessionId", "FlashcardId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningSessionItems");
        }
    }
}
