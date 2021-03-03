# Digital Apprenticeships Service

## Provider Registrations

Licensed under the [MIT license](https://github.com/SkillsFundingAgency/das-assessor-service/blob/master/LICENSE.txt)

|               |               |
| ------------- | ------------- |
|![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png)|Provider Registrations Web|
| Info     | A service to allow Providers to invite Employers into the Apprenticeship Service |
| Web      | https://localhost:5001/  |
|          | https://localhost:5001/[UKPRN]/Registration/StartAccountSetup  |
|          | https://localhost:5001/[UKPRN]/Registration/InvitedEmployers   |
| Database | das-[ENV]-preg-db  |

### Developer Setup

#### Requirements

- Install [.NET Core 2.2 SDK](https://www.microsoft.com/net/download)
- Install [Visual Studio 2019](https://www.visualstudio.com/downloads/) with these workloads:
    - ASP.NET and web development
    - Azure development
- Install [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- Install [SQL Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on atleast v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/) 
- Administrator Access

#### Setup

- Clone this repository
- Open Visual Studio as an administrator

##### Publish Database

- Build the solution SFA.DAS.ProviderRegistrations.sln
- Either use Visual Studio's `Publish Database` tool to publish the database project SFA.DAS.ProviderRegistrations.Database to name {{database name}} on {{local instance name}}

	or

- Create a database manually named {{database name}} on {{local instance name}} and run each of the `.sql` scripts in the SFA.DAS.ProviderRegistrations.Database project.

##### Config

- Get the das-provider-registrations configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-provider-registrations/SFA.DAS.ProviderRegistrations.json); which is a non-public repository.
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.ProviderRegistrations_1.0, Data: {{The contents of the local config json file}}.
- Update Configuration SFA.DAS.ProviderRegistrations_1.0, "DatabaseConnectionString":"Data Source={{Local Instance Name}};Database={{Database Name}};Integrated Security = true;Trusted_Connection=True"

#### Run the solution

The default JSON configuration was created to work with dotnet run:

- Navigate to src/SFA.DAS.ProviderRegistrations.Web/
- run `dotnet restore`
- run `dotnet run`
- Open https://localhost:5001

Alternatively the project can be run within Visual Studio:

- Set SFA.DAS.ProviderRegistrations.Web as the Startup Project
- Select SFA.DAS.ProviderRegistrations.Web in the Run menu (not IIS Express)
- Start the project

#### Getting Started

Provider Registrations does not have a default landing page. There are 2 routes into the solution that are linked via Provider Apprenticeship Service:

- https://localhost:5001/[UKPRN]/Registration/StartAccountSetup 
- https://localhost:5001/[UKPRN]/Registration/InvitedEmployers

The UKPRN must be replaced to match the UKPRN of the provider used to login.
