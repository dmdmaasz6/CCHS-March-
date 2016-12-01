
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/30/2016 22:41:27
-- Generated from EDMX file: C:\Users\Admin\Documents\GitHub\CCHS[March]]\CCHS[March]]\Models\Data Models\CCH_Entities.edmx
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
IF OBJECT_ID(N'[dbo].[FK_RepresentationAdvisor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advisors] DROP CONSTRAINT [FK_RepresentationAdvisor];
GO
IF OBJECT_ID(N'[dbo].[FK_RepresentationCompliant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Representations] DROP CONSTRAINT [FK_RepresentationCompliant];
GO
IF OBJECT_ID(N'[dbo].[FK_Response_Compliant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customer_Response] DROP CONSTRAINT [FK_Response_Compliant];
GO
IF OBJECT_ID(N'[dbo].[FK_AttachmentsCustomer_Response]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attachments] DROP CONSTRAINT [FK_AttachmentsCustomer_Response];
GO
IF OBJECT_ID(N'[dbo].[FK_AssistantCompliant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Assistants] DROP CONSTRAINT [FK_AssistantCompliant];
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
IF OBJECT_ID(N'[dbo].[Customer_Response]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer_Response];
GO
IF OBJECT_ID(N'[dbo].[Attachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attachments];
GO
IF OBJECT_ID(N'[dbo].[Assistants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Assistants];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Compliants'
CREATE TABLE [dbo].[Compliants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [SubjectMatter] nvarchar(max)  NOT NULL,
    [Details] nvarchar(max)  NOT NULL,
    [MatterEscalation] nvarchar(max)  NOT NULL,
    [Purpose] nvarchar(max)  NOT NULL,
    [PurposedOutcome] nvarchar(max)  NOT NULL,
    [YearExplanation] nvarchar(max)  NOT NULL,
    [ConsentToInvestigation] nvarchar(max)  NOT NULL,
    [DateOfSubmission] datetime  NOT NULL,
    [Heading] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Complainant_Id] int  NOT NULL
);
GO

-- Creating table 'Representations'
CREATE TABLE [dbo].[Representations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Proof_of_Rep] nvarchar(max)  NOT NULL,
    [Compliant_Id] int  NOT NULL
);
GO

-- Creating table 'Complainants'
CREATE TABLE [dbo].[Complainants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountNumber] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [National_Id] nvarchar(max)  NOT NULL,
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

-- Creating table 'Customer_Response'
CREATE TABLE [dbo].[Customer_Response] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(max)  NOT NULL,
    [Message_Body] nvarchar(max)  NOT NULL,
    [Date_Created] datetime  NOT NULL,
    [Parent_Message_Id] bigint  NOT NULL,
    [From] nvarchar(max)  NOT NULL,
    [Compliant_Id] int  NOT NULL
);
GO

-- Creating table 'Attachments'
CREATE TABLE [dbo].[Attachments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Location] nvarchar(max)  NOT NULL,
    [Date_Created] datetime  NOT NULL,
    [Customer_Response_Id] int  NOT NULL
);
GO

-- Creating table 'Assistants'
CREATE TABLE [dbo].[Assistants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Compliant_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Customer_Response'
ALTER TABLE [dbo].[Customer_Response]
ADD CONSTRAINT [PK_Customer_Response]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Attachments'
ALTER TABLE [dbo].[Attachments]
ADD CONSTRAINT [PK_Attachments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Assistants'
ALTER TABLE [dbo].[Assistants]
ADD CONSTRAINT [PK_Assistants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating foreign key on [Compliant_Id] in table 'Representations'
ALTER TABLE [dbo].[Representations]
ADD CONSTRAINT [FK_RepresentationCompliant]
    FOREIGN KEY ([Compliant_Id])
    REFERENCES [dbo].[Compliants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RepresentationCompliant'
CREATE INDEX [IX_FK_RepresentationCompliant]
ON [dbo].[Representations]
    ([Compliant_Id]);
GO

-- Creating foreign key on [Compliant_Id] in table 'Customer_Response'
ALTER TABLE [dbo].[Customer_Response]
ADD CONSTRAINT [FK_Response_Compliant]
    FOREIGN KEY ([Compliant_Id])
    REFERENCES [dbo].[Compliants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Response_Compliant'
CREATE INDEX [IX_FK_Response_Compliant]
ON [dbo].[Customer_Response]
    ([Compliant_Id]);
GO

-- Creating foreign key on [Customer_Response_Id] in table 'Attachments'
ALTER TABLE [dbo].[Attachments]
ADD CONSTRAINT [FK_AttachmentsCustomer_Response]
    FOREIGN KEY ([Customer_Response_Id])
    REFERENCES [dbo].[Customer_Response]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AttachmentsCustomer_Response'
CREATE INDEX [IX_FK_AttachmentsCustomer_Response]
ON [dbo].[Attachments]
    ([Customer_Response_Id]);
GO

-- Creating foreign key on [Compliant_Id] in table 'Assistants'
ALTER TABLE [dbo].[Assistants]
ADD CONSTRAINT [FK_AssistantCompliant]
    FOREIGN KEY ([Compliant_Id])
    REFERENCES [dbo].[Compliants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssistantCompliant'
CREATE INDEX [IX_FK_AssistantCompliant]
ON [dbo].[Assistants]
    ([Compliant_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------