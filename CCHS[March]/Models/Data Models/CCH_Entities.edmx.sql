
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/10/2016 13:24:02
-- Generated from EDMX file: C:\Users\Admin\documents\visual studio 2013\Projects\CCHS[March]]\CCHS[March]]\Models\Data Models\CCH_Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CCHS_TEST_DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CompliantBodyCorporate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BodyCorporates] DROP CONSTRAINT [FK_CompliantBodyCorporate];
GO
IF OBJECT_ID(N'[dbo].[FK_CompliantComplainant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Compliants] DROP CONSTRAINT [FK_CompliantComplainant];
GO
IF OBJECT_ID(N'[dbo].[FK_ComplainantRepresentation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Representations] DROP CONSTRAINT [FK_ComplainantRepresentation];
GO
IF OBJECT_ID(N'[dbo].[FK_RepresentationAdvisor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advisors] DROP CONSTRAINT [FK_RepresentationAdvisor];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Compliants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Compliants];
GO
IF OBJECT_ID(N'[dbo].[Representations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Representations];
GO
IF OBJECT_ID(N'[dbo].[Complainants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Complainants];
GO
IF OBJECT_ID(N'[dbo].[Advisors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advisors];
GO
IF OBJECT_ID(N'[dbo].[BodyCorporates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BodyCorporates];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Compliants'
CREATE TABLE [dbo].[Compliants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectMatter] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL,
    [MatterEscalation] nvarchar(max)  NOT NULL,
    [Purpose] nvarchar(max)  NOT NULL,
    [PurposedOutcome] nvarchar(max)  NOT NULL,
    [YearExplanation] nvarchar(max)  NOT NULL,
    [ConsentToInvestigation] nvarchar(max)  NOT NULL,
    [DateOfSubmission] datetime  NOT NULL,
    [Complainant_Id] int  NOT NULL
);
GO

-- Creating table 'Representations'
CREATE TABLE [dbo].[Representations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Proof_of_Rep] nvarchar(max)  NOT NULL,
    [Complainant_Id] int  NOT NULL
);
GO

-- Creating table 'Complainants'
CREATE TABLE [dbo].[Complainants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Nationality] nvarchar(max)  NOT NULL,
    [PO_Box] nvarchar(max)  NOT NULL,
    [Tel_Number] nvarchar(max)  NOT NULL,
    [FaxNumber] nvarchar(max)  NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Advisors'
CREATE TABLE [dbo].[Advisors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [WorkTel] nvarchar(max)  NOT NULL,
    [Cell] nvarchar(max)  NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [Representation_Id] int  NOT NULL
);
GO

-- Creating table 'BodyCorporates'
CREATE TABLE [dbo].[BodyCorporates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CountryofOrigin] nvarchar(max)  NOT NULL,
    [Registration_Number] nvarchar(max)  NOT NULL,
    [Body_Name] nvarchar(max)  NOT NULL,
    [Compliant_Id] int  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CountryName] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Compliants'
ALTER TABLE [dbo].[Compliants]
ADD CONSTRAINT [PK_Compliants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Representations'
ALTER TABLE [dbo].[Representations]
ADD CONSTRAINT [PK_Representations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Complainants'
ALTER TABLE [dbo].[Complainants]
ADD CONSTRAINT [PK_Complainants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Advisors'
ALTER TABLE [dbo].[Advisors]
ADD CONSTRAINT [PK_Advisors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BodyCorporates'
ALTER TABLE [dbo].[BodyCorporates]
ADD CONSTRAINT [PK_BodyCorporates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Compliant_Id] in table 'BodyCorporates'
ALTER TABLE [dbo].[BodyCorporates]
ADD CONSTRAINT [FK_CompliantBodyCorporate]
    FOREIGN KEY ([Compliant_Id])
    REFERENCES [dbo].[Compliants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompliantBodyCorporate'
CREATE INDEX [IX_FK_CompliantBodyCorporate]
ON [dbo].[BodyCorporates]
    ([Compliant_Id]);
GO

-- Creating foreign key on [Complainant_Id] in table 'Compliants'
ALTER TABLE [dbo].[Compliants]
ADD CONSTRAINT [FK_CompliantComplainant]
    FOREIGN KEY ([Complainant_Id])
    REFERENCES [dbo].[Complainants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompliantComplainant'
CREATE INDEX [IX_FK_CompliantComplainant]
ON [dbo].[Compliants]
    ([Complainant_Id]);
GO

-- Creating foreign key on [Complainant_Id] in table 'Representations'
ALTER TABLE [dbo].[Representations]
ADD CONSTRAINT [FK_ComplainantRepresentation]
    FOREIGN KEY ([Complainant_Id])
    REFERENCES [dbo].[Complainants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ComplainantRepresentation'
CREATE INDEX [IX_FK_ComplainantRepresentation]
ON [dbo].[Representations]
    ([Complainant_Id]);
GO

-- Creating foreign key on [Representation_Id] in table 'Advisors'
ALTER TABLE [dbo].[Advisors]
ADD CONSTRAINT [FK_RepresentationAdvisor]
    FOREIGN KEY ([Representation_Id])
    REFERENCES [dbo].[Representations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RepresentationAdvisor'
CREATE INDEX [IX_FK_RepresentationAdvisor]
ON [dbo].[Advisors]
    ([Representation_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------