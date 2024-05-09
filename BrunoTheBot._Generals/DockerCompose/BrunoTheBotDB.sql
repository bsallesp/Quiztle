-- Adminer 4.8.1 PostgreSQL 16.2 (Debian 16.2-1.pgdg120+2) dump

DROP TABLE IF EXISTS "AILogs";
CREATE TABLE "public"."AILogs" (
    "Id" integer DEFAULT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    "Name" text NOT NULL,
    "JSON" text NOT NULL,
    "Created" timestamptz NOT NULL,
    CONSTRAINT "PK_AILogs" PRIMARY KEY ("Id")
) WITH (oids = false);


DROP TABLE IF EXISTS "BookTasks";
CREATE TABLE "public"."BookTasks" (
    "Id" uuid NOT NULL,
    "Name" text,
    "UserId" integer,
    "Status" smallint NOT NULL,
    "Created" timestamptz NOT NULL,
    CONSTRAINT "PK_BookTasks" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_BookTasks_UserId" ON "public"."BookTasks" USING btree ("UserId");


DROP TABLE IF EXISTS "Books";
CREATE TABLE "public"."Books" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Created" timestamptz NOT NULL,
    "UserId" integer NOT NULL,
    CONSTRAINT "PK_Books" PRIMARY KEY ("Id")
) WITH (oids = false);


DROP TABLE IF EXISTS "Chapters";
CREATE TABLE "public"."Chapters" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Created" timestamptz NOT NULL,
    "BookId" uuid,
    CONSTRAINT "PK_Chapters" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_Chapters_BookId" ON "public"."Chapters" USING btree ("BookId");


DROP TABLE IF EXISTS "Contents";
CREATE TABLE "public"."Contents" (
    "Id" uuid NOT NULL,
    "Text" text,
    CONSTRAINT "PK_Contents" PRIMARY KEY ("Id")
) WITH (oids = false);


DROP TABLE IF EXISTS "Options";
CREATE TABLE "public"."Options" (
    "Id" integer DEFAULT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    "Name" text NOT NULL,
    "Created" timestamptz NOT NULL,
    "QuestionId" integer,
    CONSTRAINT "PK_Options" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_Options_QuestionId" ON "public"."Options" USING btree ("QuestionId");


DROP TABLE IF EXISTS "Questions";
CREATE TABLE "public"."Questions" (
    "Id" integer DEFAULT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    "Name" text NOT NULL,
    "Answer" text,
    "Hint" text,
    "Created" timestamptz NOT NULL,
    "SectionId" uuid,
    CONSTRAINT "PK_Questions" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_Questions_SectionId" ON "public"."Questions" USING btree ("SectionId");


DROP TABLE IF EXISTS "Sections";
CREATE TABLE "public"."Sections" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Created" timestamptz NOT NULL,
    "ContentId" uuid NOT NULL,
    "ChapterId" uuid,
    CONSTRAINT "PK_Sections" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_Sections_ChapterId" ON "public"."Sections" USING btree ("ChapterId");

CREATE INDEX "IX_Sections_ContentId" ON "public"."Sections" USING btree ("ContentId");


DROP TABLE IF EXISTS "User";
CREATE TABLE "public"."User" (
    "Id" integer DEFAULT GENERATED BY DEFAULT AS IDENTITY NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    "PhoneNumber" integer NOT NULL,
    "Password" text NOT NULL,
    CONSTRAINT "PK_User" PRIMARY KEY ("Id")
) WITH (oids = false);


DROP TABLE IF EXISTS "__EFMigrationsHistory";
CREATE TABLE "public"."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
) WITH (oids = false);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
('20240509113143_001Begin',	'8.0.4');

ALTER TABLE ONLY "public"."BookTasks" ADD CONSTRAINT "FK_BookTasks_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User"("Id") NOT DEFERRABLE;

ALTER TABLE ONLY "public"."Chapters" ADD CONSTRAINT "FK_Chapters_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books"("Id") NOT DEFERRABLE;

ALTER TABLE ONLY "public"."Options" ADD CONSTRAINT "FK_Options_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions"("Id") NOT DEFERRABLE;

ALTER TABLE ONLY "public"."Questions" ADD CONSTRAINT "FK_Questions_Sections_SectionId" FOREIGN KEY ("SectionId") REFERENCES "Sections"("Id") NOT DEFERRABLE;

ALTER TABLE ONLY "public"."Sections" ADD CONSTRAINT "FK_Sections_Chapters_ChapterId" FOREIGN KEY ("ChapterId") REFERENCES "Chapters"("Id") NOT DEFERRABLE;
ALTER TABLE ONLY "public"."Sections" ADD CONSTRAINT "FK_Sections_Contents_ContentId" FOREIGN KEY ("ContentId") REFERENCES "Contents"("Id") ON DELETE CASCADE NOT DEFERRABLE;

-- 2024-05-09 11:39:02.202623+00
