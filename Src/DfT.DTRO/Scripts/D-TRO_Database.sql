CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709213021_InitialCreate') THEN
    CREATE TABLE "Dtros" (
        "Id" uuid NOT NULL,
        "SchemaVersion" text NOT NULL,
        "Created" timestamp with time zone NULL,
        "LastUpdated" timestamp with time zone NULL,
        "CreatedCorrelationId" text NULL,
        "LastUpdatedCorrelationId" text NULL,
        "Deleted" boolean NOT NULL,
        "DeletionTime" timestamp with time zone NULL,
        "Data" jsonb NOT NULL,
        CONSTRAINT "PK_Dtros" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230709213021_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230709213021_InitialCreate', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230710062156_NoAutogenerateId') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230710062156_NoAutogenerateId', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230717231737_SearchOptimisationFields') THEN
    ALTER TABLE "Dtros" ADD "HA" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230717231737_SearchOptimisationFields') THEN
    ALTER TABLE "Dtros" ADD "TroName" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230717231737_SearchOptimisationFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230717231737_SearchOptimisationFields', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230718083322_AggregateSearchOptimisationFields') THEN
    ALTER TABLE "Dtros" ADD "OrderReportingPoints" text[] NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230718083322_AggregateSearchOptimisationFields') THEN
    ALTER TABLE "Dtros" ADD "RegulationTypes" text[] NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230718083322_AggregateSearchOptimisationFields') THEN
    ALTER TABLE "Dtros" ADD "VehicleTypes" text[] NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230718083322_AggregateSearchOptimisationFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230718083322_AggregateSearchOptimisationFields', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230721080815_OtherSearchOptimizationFields') THEN
    ALTER TABLE "Dtros" ADD "Location" box NOT NULL DEFAULT BOX '((0,0),(0,0))';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230721080815_OtherSearchOptimizationFields') THEN
    ALTER TABLE "Dtros" ADD "RegulationEnd" timestamp with time zone NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230721080815_OtherSearchOptimizationFields') THEN
    ALTER TABLE "Dtros" ADD "RegulationStart" timestamp with time zone NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230721080815_OtherSearchOptimizationFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230721080815_OtherSearchOptimizationFields', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230804120231_RenameHaToTa') THEN
    ALTER TABLE "Dtros" RENAME COLUMN "HA" TO "TA";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230804120231_RenameHaToTa') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20230804120231_RenameHaToTa', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240327094926_TemplateTables') THEN
    CREATE TABLE "RuleTemplate" (
        "Id" uuid NOT NULL,
        "SchemaVersion" text NOT NULL,
        "Created" timestamp with time zone NULL,
        "LastUpdated" timestamp with time zone NULL,
        "IsActive" boolean NOT NULL,
        "Template" jsonb NOT NULL,
        CONSTRAINT "PK_RuleTemplate" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240327094926_TemplateTables') THEN
    CREATE TABLE "SchemaTemplate" (
        "Id" uuid NOT NULL,
        "SchemaVersion" text NOT NULL,
        "Created" timestamp with time zone NULL,
        "LastUpdated" timestamp with time zone NULL,
        "IsActive" boolean NOT NULL,
        "Template" jsonb NOT NULL,
        CONSTRAINT "PK_SchemaTemplate" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240327094926_TemplateTables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240327094926_TemplateTables', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240328120635_SchemaRulesCorrelation') THEN
    ALTER TABLE "SchemaTemplate" ADD "CreatedCorrelationId" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240328120635_SchemaRulesCorrelation') THEN
    ALTER TABLE "SchemaTemplate" ADD "LastUpdatedCorrelationId" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240328120635_SchemaRulesCorrelation') THEN
    ALTER TABLE "RuleTemplate" ADD "CreatedCorrelationId" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240328120635_SchemaRulesCorrelation') THEN
    ALTER TABLE "RuleTemplate" ADD "LastUpdatedCorrelationId" text NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240328120635_SchemaRulesCorrelation') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240328120635_SchemaRulesCorrelation', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240425091603_RulesChangedToText') THEN
    ALTER TABLE "RuleTemplate" DROP COLUMN "IsActive";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240425091603_RulesChangedToText') THEN
    ALTER TABLE "RuleTemplate" ALTER COLUMN "Template" TYPE text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240425091603_RulesChangedToText') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240425091603_RulesChangedToText', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240606215903_ActionTypeAndReferenceToSourceAndProvision') THEN
    ALTER TABLE "Dtros" ADD "ProvisionActionType" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240606215903_ActionTypeAndReferenceToSourceAndProvision') THEN
    ALTER TABLE "Dtros" ADD "ProvisionReference" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240606215903_ActionTypeAndReferenceToSourceAndProvision') THEN
    ALTER TABLE "Dtros" ADD "SourceActionType" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240606215903_ActionTypeAndReferenceToSourceAndProvision') THEN
    ALTER TABLE "Dtros" ADD "SourceReference" text NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240606215903_ActionTypeAndReferenceToSourceAndProvision') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240606215903_ActionTypeAndReferenceToSourceAndProvision', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision') THEN
    ALTER TABLE "Dtros" DROP COLUMN "ProvisionActionType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision') THEN
    ALTER TABLE "Dtros" DROP COLUMN "ProvisionReference";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision') THEN
    ALTER TABLE "Dtros" DROP COLUMN "SourceActionType";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision') THEN
    ALTER TABLE "Dtros" DROP COLUMN "SourceReference";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240611134713_RemoveActionTypeAndReferenceFromSourceAndProvision', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240612105620_DTROHistoryTableCreated') THEN
    CREATE TABLE "DtroHistories" (
        "Id" uuid NOT NULL,
        "SchemaVersion" text NOT NULL,
        "Created" timestamp with time zone NULL,
        "LastUpdated" timestamp with time zone NULL,
        "Deleted" boolean NOT NULL,
        "DeletionTime" timestamp with time zone NULL,
        "Data" jsonb NOT NULL,
        CONSTRAINT "PK_DtroHistories" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240612105620_DTROHistoryTableCreated') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240612105620_DTROHistoryTableCreated', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617101245_ChangeTraToTraCreatorAndAddTraOwner') THEN
    ALTER TABLE "Dtros" RENAME COLUMN "TA" TO "TaOwner";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617101245_ChangeTraToTraCreatorAndAddTraOwner') THEN
    ALTER TABLE "Dtros" ADD "TaCreator" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617101245_ChangeTraToTraCreatorAndAddTraOwner') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240617101245_ChangeTraToTraCreatorAndAddTraOwner', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617150331_ChangeTaToTra') THEN
    ALTER TABLE "Dtros" RENAME COLUMN "TaOwner" TO "CurrentTraOwner";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617150331_ChangeTaToTra') THEN
    ALTER TABLE "Dtros" RENAME COLUMN "TaCreator" TO "TraCreator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240617150331_ChangeTaToTra') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240617150331_ChangeTaToTra', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240619134210_AdjustDtroHistoryEntity') THEN
    ALTER TABLE "DtroHistories" ADD "CurrentTraOwner" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240619134210_AdjustDtroHistoryEntity') THEN
    ALTER TABLE "DtroHistories" ADD "TraCreator" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240619134210_AdjustDtroHistoryEntity') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240619134210_AdjustDtroHistoryEntity', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240619152601_AddDtroIdToDtroHistory') THEN
    ALTER TABLE "DtroHistories" ADD "DtroId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240619152601_AddDtroIdToDtroHistory') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240619152601_AddDtroIdToDtroHistory', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240701093628_Metrics') THEN
    CREATE TABLE "Metrics" (
        "Id" uuid NOT NULL,
        "TraId" integer NOT NULL,
        "ForDate" date NOT NULL,
        "SystemFailureCount" integer NOT NULL,
        "SubmissionFailureCount" integer NOT NULL,
        "SubmissionCount" integer NOT NULL,
        "DeletionCount" integer NOT NULL,
        "SearchCount" integer NOT NULL,
        "EventCount" integer NOT NULL,
        "CreatedCorrelationId" text NULL,
        "LastUpdatedCorrelationId" text NULL,
        CONSTRAINT "PK_Metrics" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240701093628_Metrics') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240701093628_Metrics', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240708141047_AddSwaCodesTable') THEN
    CREATE TABLE "SwaCodes" (
        "Id" uuid NOT NULL,
        "Code" integer NOT NULL,
        "Name" text NOT NULL,
        "Prefix" character varying(20) NOT NULL,
        CONSTRAINT "PK_SwaCodes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240708141047_AddSwaCodesTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240708141047_AddSwaCodesTable', '6.0.12');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240708153050_AdjustSwaCodesTable') THEN
    ALTER TABLE "SwaCodes" RENAME COLUMN "Code" TO "TraId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240708153050_AdjustSwaCodesTable') THEN
    ALTER TABLE "SwaCodes" ADD "IsAdmin" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240708153050_AdjustSwaCodesTable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20240708153050_AdjustSwaCodesTable', '6.0.12');
    END IF;
END $EF$;
COMMIT;

