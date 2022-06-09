CREATE TABLE [dbo].[InvitationEvents]
(
	[Id] BIGINT NOT NULL IDENTITY,
	[InvitationId] BIGINT NOT NULL,    
    [InvitationReSentDate] DATETIME NULL,
    [AccountCreationStartedDate] DATETIME NULL, 
	[PayeSchemeAddedDate] DATETIME NULL,
    [AgreementAcceptedDate] DATETIME NULL,    
    CONSTRAINT [PK_InvitationEvents] PRIMARY KEY ([Id])
)
GO

ALTER TABLE [dbo].[InvitationEvents] ADD  CONSTRAINT [FK_InvitationEvents_Invitations] FOREIGN KEY([InvitationId])
REFERENCES [dbo].[Invitations] ([Id])
GO