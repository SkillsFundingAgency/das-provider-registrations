using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.Common.Domain.Types;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.TestHarness
{
    public class TestHarness
    {
        private readonly IMessageSession _publisher;

        public TestHarness(IMessageSession publisher)
        {
            _publisher = publisher;
        }

        public async Task Run()
        {
            long accountId = 1001;
            long agreementId = 2001;
            long legalEntityId = 3001;

            ConsoleKey key = ConsoleKey.Escape;

            while (key != ConsoleKey.X)
            {
                Console.Clear();
                Console.WriteLine("Test Options");
                Console.WriteLine("------------");
                Console.WriteLine("A - AddedPayeSchemeEvent");
                Console.WriteLine("B - SignedAgreementEvent");
                Console.WriteLine("C - UpsertedUserEvent");
                Console.WriteLine("X - Exit");
                Console.WriteLine("Press [Key] for Test Option");
                key = Console.ReadKey().Key;

                try
                {
                    switch (key)
                    {
                        case ConsoleKey.A:
                            await _publisher.Publish(new AddedPayeSchemeEvent
                            {
                                AccountId = accountId, 
                                Created = DateTime.Now, 
                                PayeRef = "000/1234567", 
                                CorrelationId = Guid.NewGuid().ToString(), 
                                UserName = "Tester", 
                                UserRef = Guid.NewGuid(), 
                                Aorn = "AORN"
                            });

                            Console.WriteLine();
                            Console.WriteLine($"Published AddedPayeSchemeEvent");
                            break;

                        case ConsoleKey.B:
                            await _publisher.Publish(new SignedAgreementEvent 
                            { 
                                AccountId = accountId, 
                                Created = DateTime.Now,
                                AgreementId = agreementId,
                                AgreementType = AgreementType.Levy, 
                                CohortCreated = false,
                                CorrelationId = Guid.NewGuid().ToString(),
                                LegalEntityId = legalEntityId,
                                SignedAgreementVersion = 1,
                                OrganisationName = "ORG",
                                UserName = "Tester",
                                UserRef = Guid.NewGuid()
                            });

                            Console.WriteLine();
                            Console.WriteLine($"Published SignedAgreementEvent");
                            break;

                        case ConsoleKey.C:
                            await _publisher.Publish(new UpsertedUserEvent()
                            {
                                Created = DateTime.Now,
                                CorrelationId = Guid.NewGuid().ToString(),
                                UserRef = Guid.NewGuid().ToString()
                            });
                            
                            Console.WriteLine();
                            Console.WriteLine($"Published UpsertedUserEvent");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine();
                }

                if (key == ConsoleKey.X) break;

                Console.WriteLine();
                Console.WriteLine("Press any key to return to menu");
                Console.ReadKey();
            }
        }
    }
}
